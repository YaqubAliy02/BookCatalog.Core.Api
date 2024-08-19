﻿using Domain.Enums;

namespace Application.DTOs.BookDTO
{
    public class BookCreateDto
    {
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public Guid[] AuthorsId { get; set; }
        public BookCategories Categories { get; set; }
    }
}