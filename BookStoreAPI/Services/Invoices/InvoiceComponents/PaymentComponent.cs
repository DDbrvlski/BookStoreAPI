using BookStoreDto.Dtos.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookStoreAPI.Services.Invoices.InvoiceComponents
{
    public class PaymentComponent : IComponent
    {
        private decimal TotalNetto { get; }
        private decimal TaxValue { get; }
        private decimal TotalBrutto { get; }

        public PaymentComponent(List<ProductInvoiceDto> orderItems, decimal deliveryPrice)
        {
            TotalNetto = orderItems.Sum(x => x.NettoValue) + deliveryPrice;
            TaxValue = orderItems.Sum(x => x.TaxValue);
            TotalBrutto = orderItems.Sum(x => x.BruttoValue) + deliveryPrice;
        }

        public void Compose(IContainer container)
        {
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial);
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Suma ceny netto:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{TotalNetto:F2}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Podatek:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{TaxValue:F2}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Suma ceny brutto:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{TotalBrutto:F2}").Style(textStyle);
                });
            });
        }
    }
}
