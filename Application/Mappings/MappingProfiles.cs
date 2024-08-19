using Application.DTOs.AuthorDTO;
using Application.DTOs.BookDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            BookMappingRules();
            AuthorMappingRules();
        }

        public void BookMappingRules()
        {
            CreateMap<Book, BookGetDto>()
                .ForMember(destination => destination.AuthorsId, option =>
                    option.MapFrom(src => src.Authors.Select(x => x.Id)))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue)));

            CreateMap<BookCreateDto, Book>()
                .ForMember(destination => destination.Authors, option =>
                    option.MapFrom(src => src.AuthorsId.Select(x => new Author { Id = x })))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));

            CreateMap<BookUpdateDTO, Book>()
               .ForMember(destination => destination.PublishedDate, option =>
               option.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));
        }


        public void AuthorMappingRules()
        {
            CreateMap<Author, AuthorGetDTO>()
                .ForMember(destination => destination.BooksId, option =>
                option.MapFrom(src => src.Books.Select(x => x.Id)))

                 .ForMember(destination => destination.BirthDate, option =>
            option.MapFrom(src => src.BirthDate.ToDateTime(TimeOnly.MinValue))); ;

            CreateMap<AuthorCreateDTO, Author>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));

            CreateMap<AuthorUpdateDTO, Author>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        }
    }
}
