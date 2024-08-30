﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDTO
{
    public class UserGetDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public Guid[] RolesId { get; set; }
    }
}