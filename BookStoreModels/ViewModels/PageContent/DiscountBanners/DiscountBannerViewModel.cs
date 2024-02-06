﻿using BookStoreViewModels.ViewModels.Helpers;

namespace BookStoreViewModels.ViewModels.PageContent.DiscountBanners
{
    public class DiscountBannerViewModel : BaseViewModel
    {
        public string Header { get; set; }
        public string ButtonTitle { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageURL { get; set; }
    }
}
