using AutoMapper;
using Books.Api.Dtos;
using Books.Api.Entities;

namespace Books.Api.Configurations.Mapper;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Name));
        CreateMap<BookForCreationDto, Book>().ReverseMap();
        CreateMap<BookForUpdateDto, Book>();
        CreateMap<BookForBulkUpdateDto, Book>();
    }
}