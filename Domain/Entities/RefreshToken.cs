﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Column("refresh_token_id")]
        public Guid RefreshTokenid { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("refresh_token_value")]
        public string RefreshTokenValue { get; set; }

        [Column("expired_date")]
        public DateTime ExpiredDate { get; set; }
    }
}
