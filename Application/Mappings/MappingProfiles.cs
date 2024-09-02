using Application.DTOs.AuthorDTO;
using Application.DTOs.BookDTO;
using Application.DTOs.PermissionDTO;
using Application.DTOs.RoleDTO;
using Application.DTOs.UserDTO;
using Application.UseCases.Books.Command;
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
            UserMappingRules();
            PermissionMappingRules();
            RoleMappingRules();
        }

        private void RoleMappingRules()
        {
            CreateMap<RoleCreateDTO, Role>()
                .ForMember(desination => desination.Permissions,
                    options => options.MapFrom(src => src.PermissionId
                        .Select(x => new Permission() { PermissionId = x })));

            CreateMap<Role, RoleGetDTO>()
                .ForMember(destination => destination.PermissionsId,
                    options => options.MapFrom(src => src.Permissions
                        .Select(x => x.PermissionId)));

            CreateMap<RoleUpdateDTO, Role>()
                .ForMember(destination => destination.Permissions,
                    options => options.MapFrom(src => src.PermissionsId
                        .Select(x => new Permission() { PermissionId = x })));
        }

        private void PermissionMappingRules()
        {
            CreateMap<CreatePermissionDTO, Permission>();
        }

        private void UserMappingRules()
        {
            CreateMap<UserCreateDTO, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                        .Select(x => new Role() { RoleId = x })));

            CreateMap<User, UserGetDTO>()
            .ForMember(destination => destination.RolesId,
                options => options.MapFrom(src => src.Roles
                    .Select(x => x.RoleId)));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                    .Select(x => new Role() { RoleId = x })));
        }

        public void BookMappingRules()
        {
            CreateMap<BookCreateDto, Book>()
                .ForMember(destination => destination.Authors, option =>
                    option.MapFrom(src => src.AuthorsId.Select(x => new Author { Id = x })))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));

            CreateMap<BookUpdateDTO, Book>()
             .ForMember(destination => destination.PublishedDate, option =>
             option.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));

            CreateMap<Book, BookGetDto>()
                .ForMember(destination => destination.AuthorsId, option =>
                    option.MapFrom(src => src.Authors.Select(x => x.Id)))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue)));


            CreateMap<CreateBookCommand, Book>()
                .ForMember(destination => destination.Authors, option =>
                    option.MapFrom(src => src.AuthorsId.Select(x => new Author { Id = x })))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => DateOnly.FromDateTime(src.PublishedDate)));

            /*           CreateMap<Book, CreateBookCommandResult>()
                          .ForMember(destination => destination.AuthorsId, option =>
                              option.MapFrom(src => src.Authors.Select(x => x.Id)));*/

            CreateMap<Book, CreateBookCommandResult>()
                .ForMember(destination => destination.AuthorsId, option =>
                    option.MapFrom(src => src.Authors.Select(x => x.Id)))

                .ForMember(destination => destination.PublishedDate, option =>
                    option.MapFrom(src => src.PublishedDate.ToDateTime(TimeOnly.MinValue)));
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
