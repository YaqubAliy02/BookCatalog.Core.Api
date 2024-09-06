using Application.DTOs.AuthorDTO;
using Application.DTOs.BookDTO;
using Application.DTOs.PermissionDTO;
using Application.DTOs.RoleDTO;
using Application.DTOs.UserDTO;
using Application.UseCases.Accounts.Command;
using Application.UseCases.Authors.Command;
using Application.UseCases.Books.Command;
using Application.UseCases.Permissions.Commands;
using Application.UseCases.Roles.Command;
using Application.UseCases.Users.Command;
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
            
            CreateMap<CreateRoleCommand, Role>()
                .ForMember(desination => desination.Permissions,
                    options => options.MapFrom(src => src.PermissionId
                        .Select(x => new Permission() { PermissionId = x })));

            CreateMap<Role, RoleGetDTO>()
                .ForMember(destination => destination.PermissionsId,
                    options => options.MapFrom(src => src.Permissions
                        .Select(x => x.PermissionId)));

            CreateMap<Role, CreateRoleCommandResult>()
              .ForMember(destination => destination.PermissionId,
                  options => options.MapFrom(src => src.Permissions
                      .Select(x => x.PermissionId)));

            CreateMap<RoleUpdateDTO, Role>()
                .ForMember(destination => destination.Permissions,
                    options => options.MapFrom(src => src.PermissionsId
                        .Select(x => new Permission() { PermissionId = x })));
            
            CreateMap<UpdateRoleCommand, Role>()
                .ForMember(destination => destination.Permissions,
                    options => options.MapFrom(src => src.PermissionsId
                        .Select(x => new Permission() { PermissionId = x })));

            
        }

        private void PermissionMappingRules()
        {
            CreateMap<CreatePermissionDTO, Permission>();
            CreateMap<CreatePermissionCommand, Permission>();
            CreateMap<UpdatePermissionQuery, Permission>();
        }

        private void UserMappingRules()
        {
            CreateMap<UserCreateDTO, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                        .Select(x => new Role() { RoleId = x }))); 
            
            CreateMap<CreateUserCommand, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                        .Select(x => new Role() { RoleId = x })));

            CreateMap<RegisterUserCommand, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                        .Select(x => new Role() { RoleId = x })));


            CreateMap<User, CreateUserCommandHandlerResult>()
                .ForMember(destination => destination.RolesId,
                    options => options.MapFrom(src => src.Roles
                        .Select(x => x.RoleId))); 
            
            CreateMap<User, RegisterUserCommandResult>()
                .ForMember(destination => destination.RolesId,
                    options => options.MapFrom(src => src.Roles
                        .Select(x => x.RoleId)));

            CreateMap<User, UserGetDTO>()
            .ForMember(destination => destination.RolesId,
                options => options.MapFrom(src => src.Roles
                    .Select(x => x.RoleId)));

            CreateMap<UserUpdateDTO, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                    .Select(x => new Role() { RoleId = x })));
            
            CreateMap<UpdateUsersCommand, User>()
                .ForMember(destination => destination.Roles,
                    options => options.MapFrom(src => src.RolesId
                    .Select(x => new Role() { RoleId = x })));

            CreateMap<UpdateUserCommand, User>()
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
            
            CreateMap<UpdateBookCommand, Book>()
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
            
            CreateMap<CreateAuthorCommand, Author>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
            
            CreateMap<Author, CreateAuthorCommandHandlerResult>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => src.BirthDate.ToDateTime(TimeOnly.MinValue)));

            CreateMap<AuthorUpdateDTO, Author>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
            
            CreateMap<UpdateAuthorCommand, Author>()
                .ForMember(destination => destination.BirthDate, option =>
                option.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
        }
    }
}
