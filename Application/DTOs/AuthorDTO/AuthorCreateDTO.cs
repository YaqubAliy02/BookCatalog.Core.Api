﻿using Domain.Enums;

namespace Application.DTOs.AuthorDTO
{
    public class AuthorCreateDTO
    {
        public string FullName { get; set; }
        public string AuthorPhoto { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public string AboutAuthor { get; set; }
    }
}