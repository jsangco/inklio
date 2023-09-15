using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;

namespace Inklio.Api.Client;

public class User : IDisposable
{
    /// <summary>
    /// Gets or sets the default Base URI for the Inklio website.
    /// WARNING: The HttpClient.BaseAddress property pays extra special
    ///          attention to leading and trailing '/' characters:
    ///          https://stackoverflow.com/a/23438417/200363
    /// </summary>
    public static Uri DefaultBaseUri { get; set; } = new Uri("http://localhost/api/");

    /// <summary>
    /// Gets or sets the default user password word to use if no password is specified.
    /// </summary>
    public static string DefaultPassword { get; set; } = "SuperSecret!1";

    /// <summary>
    /// Gets or sets the default email prefix to use on the email field when creating a user without a specified email.
    /// The format of the default email is "{DefaultEmailPrefix}{username}{DefaultEmailSuffix}".
    /// </summary>
    public static string DefaultEmailPrefix { get; set; } = "inklio";

    /// <summary>
    /// Gets or sets the default email suffix to use on the email field when creating a user without a specified email.
    /// The format of the default email is "{DefaultEmailPrefix}{username}{DefaultEmailSuffix}".
    /// </summary>
    public static string DefaultEmailSuffix { get; set; } = "@mailinator.com";

    /// <summary>
    /// Gets the Username for the <see cref="User"/> object.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the Email for the <see cref="User"/> object.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the Password for the <see cref="User"/> object.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Gets the UserId of the <see cref="User"/> object.
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// The <see cref="HttpClient"/> object used to communicate with the Inklio API.
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// A flag indicating if the <see cref="User"/> object is logged in.
    /// </summary>
    private bool isLoggedIn = false;

    /// <summary>
    /// A logger object.
    /// </summary>
    private readonly ILogger<User> logger;

    /// <summary>
    /// Flag indicating if the object is disposed.
    /// </summary>
    private bool isDisposed = false;

    /// <summary>
    /// Initilaizes a new instance of a <see cref="User"/> object.
    /// </summary>
    /// <param name="username">The username</param>
    public User(string username) : this(
        username,
        $"{DefaultEmailPrefix}{username}{DefaultEmailSuffix}",
        DefaultPassword,
        DefaultBaseUri.ToString(),
        ClientLogger.DefaultLoggerFactory.CreateLogger<User>())
    {
    }

    /// <summary>
    /// Initilaizes a new instance of a <see cref="User"/> object.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="baseUrl">The URL of the user.</param>
    /// <param name="loggerFactory">A logger factory.</param>
    public User(string username, string email, string password, string baseUrl, ILogger<User> logger)
    {
        this.Username = username;
        this.Email = email;
        this.Password = password;
        this.UserId = "";

        var cookieContainer = new CookieContainer();
        var httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true };
        this.httpClient = new HttpClient(httpClientHandler);
        this.httpClient.BaseAddress = new Uri(baseUrl);
        this.logger = logger;
    }

    /// <summary>
    /// Logs in to an Inklio account. If the Account does not exist the account is created.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public async Task CreateOrLoginAsync(CancellationToken cancellationToken = default)
    {
        if (this.isLoggedIn)
        {
            return;
        }

        var doCreateOrLogin = async () =>
        {
            this.logger.LogInformation("Logging in user. {Username} | {Email} | {Url}", this.Username, this.Email, this.httpClient?.BaseAddress?.ToString());
            var loginCommand = new AccountLogin() { Username = this.Username, Password = this.Password, IsRememberMe = true };
            var loginResponse = await this.httpClient!.PostAsJsonAsync("v1/accounts/login", loginCommand, cancellationToken);

            if (loginResponse.IsSuccessStatusCode == false)
            {
                this.logger.LogInformation(
                    "Login failed. Attempting to create new user. {Username} | {Email} | {Url}",
                    this.Username,
                    this.Email,
                    this.httpClient?.BaseAddress?.ToString());
                var createCommand = new AccountCreate()
                {
                    Username = this.Username,
                    Password = this.Password,
                    ConfirmPassword = this.Password,
                    Email = this.Email,
                };
                return await this.httpClient!.PostAsJsonAsync("v1/accounts/register", createCommand, cancellationToken);
            }
            return loginResponse;
        };

        var response = await doCreateOrLogin();
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            throw new InklioClientException((int)response.StatusCode, "Unable to create or login user.", e);
        }

        var account = await response.Content.ReadFromJsonAsync<Account>();
        this.UserId = account?.Id ?? throw new InklioClientException("Inklio API did not return expected data format.");
        this.isLoggedIn = true;
        this.logger.LogInformation("User has been logged in. {Username} | {Email} | {Url}", this.Username, this.Email, this.httpClient?.BaseAddress?.ToString());
    }

    /// <summary>
    /// Creates a new ask.
    /// </summary>
    /// <param name="askCreate">The ask to create</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public async Task AddAskAsync(AskCreate askCreate, CancellationToken cancellationToken = default)
    {
        await CreateOrLoginAsync(cancellationToken);
        this.logger.LogInformation("Creating new Ask. {Username} | {Url}", this.Username, this.httpClient?.BaseAddress?.ToString());
        var createResponse = await this.httpClient!.PostAsync("v1/asks", askCreate.ToMultipartFormDataContent());
        try
        {
            createResponse.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            throw new InklioClientException((int)createResponse.StatusCode, $"Unable to create ask.\n{error}", e);
        }
    }

    /// <summary>
    /// Creates a new delivery.
    /// </summary>
    /// <param name="deliveryCreate">The delivery to create.</param>
    /// <param name="askId">The ID of the ask to add the delivery to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public async Task AddDeliveryAsync(DeliveryCreate deliveryCreate, int askId, CancellationToken cancellationToken = default)
    {
        await CreateOrLoginAsync(cancellationToken);
        this.logger.LogInformation("Creating new Delivery. {Username} | {Url}", this.Username, this.httpClient?.BaseAddress?.ToString());
        var createResponse = await this.httpClient!.PostAsync($"v1/asks/{askId}/deliveries", deliveryCreate.ToMultipartFormDataContent());
        try
        {
            createResponse.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            throw new InklioClientException((int)createResponse.StatusCode, $"Unable to create delivery on ask {askId}\n{error}", e);
        }
    }

    /// <summary>
    /// Adds a comment to an ask
    /// </summary>
    /// <param name="commentCreate">The comment to create</param>
    /// <param name="askId">The ID of the ask to add the delivery to.</param>
    /// <param name="deliveryId">The ID of the delivery to add the comment to.</param>
    /// <param name="askId">The ID of the ask with the delivery to add the comment to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public async Task AddCommentAsync(CommentCreate commentCreate, int askId, CancellationToken cancellationToken = default)
    {
        await CreateOrLoginAsync(cancellationToken);
        this.logger.LogInformation("Adding comment to ask. {Username} | {Url}", this.Username, this.httpClient?.BaseAddress?.ToString());
        var createResponse = await this.httpClient!.PostAsync($"v1/asks/{askId}/comments", commentCreate.ToHttpContent());
        try
        {
            createResponse.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            throw new InklioClientException((int)createResponse.StatusCode, $"Unable to add comment to ask {askId}\n{error}", e);
        }
    }

    /// <summary>
    /// Adds a comment to a delivery
    /// </summary>
    /// <param name="commentCreate">The comment to create</param>
    /// <param name="askId">The ID of the ask to add the delivery to.</param>
    /// <param name="deliveryId">The ID of the delivery to add the comment to.</param>
    /// <param name="askId">The ID of the ask with the delivery to add the comment to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public async Task AddCommentAsync(CommentCreate commentCreate, int askId, int deliveryId, CancellationToken cancellationToken = default)
    {
        await CreateOrLoginAsync(cancellationToken);
        this.logger.LogInformation("Adding comment to delivery. {Username} | {Url}", this.Username, this.httpClient?.BaseAddress?.ToString());
        var createResponse = await this.httpClient!.PostAsync($"v1/asks/{askId}/deliveries/{deliveryId}/comments", commentCreate.ToHttpContent());
        try
        {
            createResponse.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var error = await createResponse.Content.ReadAsStringAsync();
            throw new InklioClientException((int)createResponse.StatusCode, $"Unable to add comment to delivery {deliveryId} on ask {askId}\n{error}", e);
        }
    }

    /// <summary>
    /// Adds a comment to an ask
    /// </summary>
    /// <param name="commentBody">The content of the added comment</param>
    /// <param name="askId">The ID of the ask to add the delivery to.</param>
    /// <param name="deliveryId">The ID of the delivery to add the comment to.</param>
    /// <param name="askId">The ID of the ask with the delivery to add the comment to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public Task AddCommentAsync(string commentBody, int askId, CancellationToken cancellationToken = default)
    {
        return AddCommentAsync(new CommentCreate(commentBody), askId, cancellationToken);
    }

    /// <summary>
    /// Adds a comment to a delivery
    /// </summary>
    /// <param name="commentBody">The content of the added comment</param>
    /// <param name="askId">The ID of the ask to add the delivery to.</param>
    /// <param name="deliveryId">The ID of the delivery to add the comment to.</param>
    /// <param name="askId">The ID of the ask with the delivery to add the comment to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task.</returns>
    public Task AddCommentAsync(string commentBody, int askId, int deliveryId, CancellationToken cancellationToken = default)
    {
        return AddCommentAsync(new CommentCreate(commentBody), askId, deliveryId, cancellationToken);
    }

    /// <summary>
    /// Gets the asks.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The asks.</returns>
    public async Task<ODataResponse<Ask>> GetAsksAsync(string? filter = null, CancellationToken cancellationToken = default)
    {
        var query = filter?.Replace(httpClient.BaseAddress!.ToString() + "v1/asks?", "");
        string url = query == null ?  "v1/asks?expand=deliveries(expand=comments,images,tags),comments,images,tags"
            : string.IsNullOrWhiteSpace(query) ?"v1/asks/"
            : $"v1/asks?{query}";
        this.logger.LogInformation("Fetching asks. {Username} | {Url}", this.Username, this.httpClient!.BaseAddress?.ToString() + $"{url}");
        try
        {
            var response = await this.httpClient.GetAsync(url, cancellationToken);
            var asks = await response.Content.ReadFromJsonAsync<ODataResponse<Ask>>(cancellationToken: cancellationToken);
            if (asks is null)
            {
                throw new InklioClientException($"Unable to fetch Asks");
            }

            return asks;
        }
        catch (Exception e)
        {
            throw new InklioClientException($"Unable to fetch Asks", e);
        }
    }

    /// <summary>
    /// Gets the claims of the logged in user.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The claims of the user.</returns>
    public async Task<string> GetClaimsAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Fetching user claims. {Username} | {Url}", this.Username, this.httpClient!.BaseAddress?.ToString());
        var response = await this.httpClient.GetAsync("v1/accounts/claims", cancellationToken);
        var claims = await response.Content.ReadAsStringAsync(cancellationToken);
        return claims;
    }

    /// <summary>
    /// Dispose the User object
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose implementation.
    /// </summary>
    /// <param name="disposing">A falg indicating if the object is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }
        if (disposing)
        {
            httpClient.Dispose();
        }
        this.isDisposed = true;
    }
}