﻿using BookStoreDto.Dtos.Helpers;

namespace BookStoreDto.Dtos.PageContent.FooterLinks
{
    public class FooterLinkDto : BaseDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
        public int Position { get; set; }
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public int ColumnPosition { get; set; }
        public string HTMLObject { get; set; }

    }
}
