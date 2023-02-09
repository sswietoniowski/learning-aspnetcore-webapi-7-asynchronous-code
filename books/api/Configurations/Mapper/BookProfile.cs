using AutoMapper;
using Books.Api.Dtos;
using Books.Api.Dtos.External;
using Books.Api.Entities;

namespace Books.Api.Configurations.Mapper;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Author, string>()
            .ConstructUsing(src => src.Name);
        CreateMap<Book, BookDto>();

        // FIXME: not working as expected, requires further investigation
        // CreateMap<Book, BookDto>()
        //     .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name));

        CreateMap<BookForCreationDto, Book>().ReverseMap();        
        CreateMap<BookForUpdateDto, Book>();

        CreateMap<BookForBulkUpdateDto, Book>();

        CreateMap<BookDto, BookWithCoversDto>()
            .ConstructUsing(src => new BookWithCoversDto(src.Id, src.Title, src.Description, src.AuthorId, src.Author));
        CreateMap<CoverDto, BookCoverDto>();
        CreateMap<IEnumerable<CoverDto>, BookWithCoversDto>()
            .ForMember(dest => dest.Covers, opt => opt.MapFrom(src => src));
    }
}