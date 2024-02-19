using BookStoreDto.Dtos.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BookStoreAPI.Services.Invoices.InvoiceComponents
{
    public class SellerAddressComponent : IComponent
    {
        private string Title { get; }
        private SellerInvoiceDto SellerData { get; }

        public SellerAddressComponent(string title, SellerInvoiceDto sellerData)
        {
            Title = title;
            SellerData = sellerData;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                column.Item().Text($"{SellerData.Name}");
                column.Item().Text($"{SellerData.Street} {SellerData.StreetNumber}{SellerData.HouseNumber}");
                column.Item().Text($"{SellerData.Postcode} {SellerData.City}");
                column.Item().Text($"{SellerData.Country}");
                column.Item().Text($"NIP {SellerData.TaxIdentificationNumber}");
                column.Item().Text($"Telefon {SellerData.Phone}");
                column.Item().Text($"Adres e-mail {SellerData.Email}");
            });
        }
    }
}
