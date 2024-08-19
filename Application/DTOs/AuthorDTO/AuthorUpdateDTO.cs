﻿using Domain.Enums;

namespace Application.DTOs.AuthorDTO
{
    public class AuthorUpdateDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
    }
}

