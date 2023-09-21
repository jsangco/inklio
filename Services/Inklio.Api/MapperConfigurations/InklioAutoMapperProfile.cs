
using AutoMapper;

public class InklioAutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of an <see cref="InklioAutoMapperProfile"/> object.
    /// </summary>
    /// <param name="baseUri">The URI prepended to the paths of images. (i.e. http://localhost/images/)</param>
    public InklioAutoMapperProfile(Uri baseUri)
    {
        string imageUrl = baseUri.ToString() + "images";

        this.CreateMap<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>()
            .ForMember(e => e.CreatedBy, e => e.MapFrom(e => e.CreatedByUsername));
        this.CreateMap<Inklio.Api.Domain.AskComment, Inklio.Api.Application.Commands.AskComment>()
            .ForMember(e => e.CreatedBy, e => e.MapFrom(e => e.CreatedByUsername));
        this.CreateMap<Inklio.Api.Domain.AskImage, Inklio.Api.Application.Commands.AskImage>()
            .ForMember(e => e.Url, e => e.MapFrom(e => imageUrl + e.UrlRelative));
        this.CreateMap<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>()
            .ForMember(e => e.CreatedBy, e => e.MapFrom(e => e.CreatedByUsername));
        this.CreateMap<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>()
            .ForMember(e => e.CreatedBy, e => e.MapFrom(e => e.CreatedByUsername));
        this.CreateMap<Inklio.Api.Domain.DeliveryComment, Inklio.Api.Application.Commands.DeliveryComment>()
            .ForMember(e => e.CreatedBy, e => e.MapFrom(e => e.CreatedByUsername));
        this.CreateMap<Inklio.Api.Domain.DeliveryImage, Inklio.Api.Application.Commands.DeliveryImage>()
            .ForMember(e => e.Url, e => e.MapFrom(e => imageUrl + e.UrlRelative));
        this.CreateMap<Inklio.Api.Domain.Image, Inklio.Api.Application.Commands.Image>()
            .ForMember(e => e.Url, e => e.MapFrom(e => imageUrl + e.UrlRelative));
        this.CreateMap<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }
}