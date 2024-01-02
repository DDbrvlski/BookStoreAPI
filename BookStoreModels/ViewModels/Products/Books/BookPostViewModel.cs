using BookStoreViewModels.ViewModels.Helpers;
using BookStoreViewModels.ViewModels.Media.Images;
using System.ComponentModel.DataAnnotations;

namespace BookStoreViewModels.ViewModels.Products.Books
{
    public class BookPostViewModel : BaseViewModel
    {
        public new int? Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Oryginalny język jest wymagany.")]
        [Display(Name = "Oryginalny język")]
        public int? OriginalLanguageID { get; set; }

        [Required(ErrorMessage = "Wydawca jest wymagany.")]
        [Display(Name = "Wydawca")]
        public int? PublisherID { get; set; }


        public List<ListOfIds>? ListOfBookAuthors { get; set; }
        public List<ListOfIds>? ListOfBookCategories { get; set; }
        public List<ImageViewModel>? ListOfBookImages { get; set; }
    }
}
