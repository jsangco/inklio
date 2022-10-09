using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;
using CommandAsk = Inklio.Api.Application.Commands.Ask;
using CommandTag = Inklio.Api.Application.Commands.Tag;
using DomainAsk = Inklio.Api.Domain.Ask;
using DomainAskImage = Inklio.Api.Domain.AskImage;
using DomainTag = Inklio.Api.Domain.Tag;

public class AskCreateCommandHandler : IRequestHandler<AskCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IBlobRepository blobRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initializes an instance of a new <see cref="AskCreateCommandHandler"/>
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="blobRepository">A repository for blob objects</param>
    /// <param name="tagRepository">A repository for tag objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AskCreateCommandHandler(
        IAskRepository askRepository,
        IBlobRepository blobRepository,
        ITagRepository tagRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.blobRepository = blobRepository ?? throw new ArgumentNullException(nameof(blobRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    /// <summary>
    /// Creates a new Ask.
    /// </summary>
    /// <param name="request">The command to create the Ask.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the Ask creation was a success.</returns>
    public async Task<bool> Handle(AskCreateCommand request, CancellationToken cancellationToken)
    {
        // Get the user creating the tag
        User user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);

        // Create the ask
        DomainAsk ask = DomainAsk.Create(request.Body, user, request.IsNsfl, request.IsNswf, request.Title);

        // Get and add tags to the new Ask
        IEnumerable<DomainTag> tags = this.GetOrCreateTags(request.Tags, user);
        foreach (var tag in tags)
        {
            ask.AddTag(false, user, tag);
        }

        // Upload relevant images to storage
        IEnumerable<IFormFile> forms = request?.Images ?? new IFormFile[] { };
        IEnumerable<DomainAskImage> askImages = await this.CreateImageAsync(ask, forms, user, cancellationToken);

        try
        {
            // Add images to the ask
            foreach (var image in askImages)
            {
                ask.AddImage(image, user);
            }

            // Add the ask in the repository
            await this.askRepository.AddAskAsync(ask, cancellationToken);

            // Save changes
            await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            // If an ask image was created, then there was an image uploaded to storage.
            // We need to delete uploads as part of a compensary transaction.
            var deleteTasks = askImages
                .Select(askImages => this.blobRepository.DeleteAskBlobAsync(askImages.Name, cancellationToken))
                .ToArray();
            await Task.WhenAll(deleteTasks);

            throw;
        }

        return true;
    }

    /// <summary>
    /// Gets all tags from the tag repository. If a tag does not exist, it creates a new one.
    /// </summary>
    /// <param name="tags">The tags to get.</param>
    /// <param name="user">The user creating getting or creating the tags</param>
    /// <returns>A collection of all relevant Tags that were retrieved from the repository.</returns>
    private IEnumerable<DomainTag> GetOrCreateTags(IEnumerable<CommandTag> tags, User user)
    {
        // Get all existing tags
        var existingTags = new List<DomainTag>();
        foreach (var askTag in tags)
        {
            this.askRepository.TryGetTagByName(askTag.Type, askTag.Value, out DomainTag? tag);
            if (tag is not null)
            {
                existingTags.Add(tag);
            }
        }
        
        // Create any tags that don't exist
        var newTags = tags.Select(t => t.ToString()).Except(existingTags.Select(t => t.ToString()));
        foreach (var newTag in newTags)
        {
            var tagSplit = newTag.Split(':');
            var tagType = string.IsNullOrWhiteSpace(tagSplit[0]) ? DomainTag.DefaultTagType : tagSplit[0];
            var tagValue = tagSplit[1];
            var tag = DomainTag.Create(user, tagType, tagValue);

            // Add to the list of existing tags to be returned later
            existingTags.Add(tag);
        }

        return existingTags;
    }

    /// <summary>
    /// Creates new AskImages and uploads their data to storage.
    /// </summary>
    /// <param name="ask">The associated ask.</param>
    /// <param name="forms">The image to upload</param>
    /// <param name="user">The user uploading the image</param>
    /// <param name="cancellationToken">A cancelation token</param>
    /// <returns>The AskImage if it was created. Null if no AskImage was created.</returns>
    private async Task<IEnumerable<DomainAskImage>> CreateImageAsync(DomainAsk ask, IEnumerable<IFormFile> forms, User user, CancellationToken cancellationToken)
    {
        ask.ValidateCanAddImages(forms.Count(), user); // Validate we can add the images before we start uploading forms.

        // Create the AskImages. This is done first so that domain validation can be done.
        var images = forms.Select(form => DomainAskImage.Create(ask, user, form.Length)).ToArray();

        // Upload each image to storage then set it on the blob
        var imagesAndForms = images.Zip(forms);
        foreach (var imageAndForm in imagesAndForms)
        {
            var image = imageAndForm.First;
            var form = imageAndForm.Second;
            var blob = await this.blobRepository.AddAskBlobAsync(form, image.Name, cancellationToken);
            image.SetBlob(blob);
        }

        return images;
    }
}