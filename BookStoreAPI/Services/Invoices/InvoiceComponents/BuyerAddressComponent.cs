using BookStoreDto.Dtos.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BookStoreAPI.Services.Invoices.InvoiceComponents
{
    public class BuyerAddressComponent : IComponent
    {
        private string Title { get; }
        private CustomerInvoiceDto BuyerData { get; }

        public BuyerAddressComponent(string title, CustomerInvoiceDto buyerData)
        {
            Title = title;
            BuyerData = buyerData;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                column.Item().Text($"{BuyerData.Name} {BuyerData.Surname}");
                column.Item().Text($"{BuyerData.Address.Street} {BuyerData.Address.StreetNumber}{BuyerData.Address.HouseNumber}");
                column.Item().Text($"{BuyerData.Address.Postcode} {BuyerData.Address.CityName}");
                column.Item().Text($"{BuyerData.Address.CountryName}");
                column.Item().Text($"{BuyerData.Email}");
                column.Item().Text($"{BuyerData.Phone}");
            });
        }
    }
}
