using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Command handler to create a <see cref="DomainDelivery"/>
/// </summary>
public class DeliveryCreateCommandHandler : IRequestHandler<DeliveryCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IBlobRepository blobRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initializes an instance of a <see cref="DeliveryCreateCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for <see cref="DomainAsk"/> objects</param>
    /// <param name="userRepository">A repository for <see cref="DomainUser"/> objects</param>
    /// <param name="tagRepository">A repository for <see cref="DomainTag"/> objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DeliveryCreateCommandHandler(
        IAskRepository askRepository,
        IBlobRepository blobRepository,
        IUserRepository userRepository,
        ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.blobRepository = blobRepository ?? throw new ArgumentNullException(nameof(blobRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(DeliveryCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        DomainDelivery delivery = ask.AddDelivery(request.Body, user, request.IsAi, request.IsNsfl, request.IsNsfw, request.IsSpoiler, request.Title);

        // The creator automatically upvotes their post.
        delivery.AddUpvote((int)UpvoteType.Basic, user);

        // Get and add tags to the new Ask
        IEnumerable<DomainTag> tags = request.IncludeAskTags ?
            this.GetOrCreateTags(request.Tags, user).Concat(ask.Tags).Distinct(new TagEqualityComparer())
            : this.GetOrCreateTags(request.Tags, user);
        foreach (var tag in tags)
        {
            ask.AddTag(false, user, tag);
        }

        // Upload relevant images to storage
        IEnumerable<IFormFile> forms = request?.Images ?? new IFormFile[] { };
        IEnumerable<DomainDeliveryImage> deliveryImages = await this.CreateImageAsync(delivery, forms, user, cancellationToken);

        try
        {
            // Add images to the ask
            foreach (var image in deliveryImages)
            {
                delivery.AddImage(image, user);
            }
            await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            // If a delivery image was created, then there was an image uploaded to storage.
            // We need to delete uploads as part of a compensary transaction.
            var deleteTasks = deliveryImages
                .Select(deliveryImages => this.blobRepository.DeleteDeliveryBlobAsync(deliveryImages.Name, cancellationToken))
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
        foreach (var deliveryTag in tags)
        {
            this.askRepository.TryGetTagByName(deliveryTag.Type, deliveryTag.Value, out DomainTag? tag);
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
    /// Creates new DeliveryImages and uploads their data to storage.
    /// </summary>
    /// <param name="delivery">The associated delivery.</param>
    /// <param name="forms">The image to upload</param>
    /// <param name="user">The user uploading the image</param>
    /// <param name="cancellationToken">A cancelation token</param>
    /// <returns>The DeliveryImage if it was created. Null if no DeliveryImage was created.</returns>
    private async Task<IEnumerable<DomainDeliveryImage>> CreateImageAsync(DomainDelivery delivery, IEnumerable<IFormFile> forms, User user, CancellationToken cancellationToken)
    {
        delivery.ValidateCanAddImages(forms.Count(), user); // Validate we can add the images before we start uploading forms.

        // Create the DeliveryImages. This is done first so that domain validation can be done.
        var images = forms.Select(form => DomainDeliveryImage.Create(user, delivery, form.Length)).ToArray();

        // Upload each image to storage then set it on the blob
        var imagesAndForms = images.Zip(forms);
        foreach (var imageAndForm in imagesAndForms)
        {
            var image = imageAndForm.First;
            var form = imageAndForm.Second;
            Blob blob = await this.blobRepository.AddDeliveryBlobAsync(form, image.Name, cancellationToken);
            image.SetBlob(blob);
        }

        return images;
    }
}