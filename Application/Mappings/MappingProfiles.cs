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
        }

        public void BookMappingRules()
        {
            CreateMap<BookCreateDto, Book>()
               .ForMember(destination => destination.Authors, option =>
                   option.MapFrom(src => src.AuthorsId.Select(x => new Author() { Id = x })));

            CreateMap<BookCreateDto, Book>()
              .ForMember(destination => destination.Authors, option =>
                  option.MapFrom(src => src.AuthorsId.Select(x => new Author() { Id = x })));
        }

        public void AuthorMappingRules()
        {

        }
    }
}
