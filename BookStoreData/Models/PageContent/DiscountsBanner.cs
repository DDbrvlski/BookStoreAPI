﻿using BookStoreData.Models.Helpers;
using BookStoreData.Models.Media;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStoreData.Models.PageContent
{
    public class DiscountsBanner : BaseEntity
    {
        public string Header { get; set; }
        public string ButtonTitle { get; set; }

        #region Foreign Keys
        //Image
        [Display(Name = "Zdjęcie")]
        public int? ImageID { get; set; }

        [ForeignKey("ImageID")]
        [JsonIgnore]
        public virtual Images Image { get; set; }
        #endregion
    }
}
