using BookStoreDto.Dtos.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookStoreAPI.Services.Invoices.InvoiceComponents
{
    public class InvoiceDocument : IDocument
    {
        public InvoiceDataDto Model { get; }

        public InvoiceDocument(InvoiceDataDto model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.DefaultTextStyle(TextStyle.Default.FontFamily(Fonts.Arial));
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }
        void ComposeTable(IContainer container)
        {
            int number = 1;
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(8);
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Lp.").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Kod").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Nazwa towaru").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Ilość").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("J.m.").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Cena jednostkowa netto").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Stawka VAT").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Wartość sprzedaży netto").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Kwota VAT").Style(textStyle);
                    header.Cell().Element(CellStyle).AlignRight().Text("Suma brutto").Style(textStyle);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).BorderLeft(1).BorderRight(1).BorderColor(Colors.White).PaddingVertical(5).Background(Colors.Grey.Lighten1).AlignCenter().AlignMiddle();
                    }
                });

                foreach (var item in Model.InvoiceProducts)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text($"{number++}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.Code}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.Name}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.Quantity}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.UnitOfMeasure}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.NettoPrice:F2}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.Tax}%").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.NettoValue:F2}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.TaxValue:F2}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{item.BruttoValue:F2}").Style(textStyle);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).AlignCenter().AlignMiddle();
                    }
                }
            });
        }
        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontFamily(Fonts.Arial);
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Faktura VAT").Style(titleStyle);
                    column.Item().PaddingBottom(10).Text($"nr {Model.InvoiceNumber}").Style(textStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Data wystawienia: ").SemiBold().Style(textStyle);
                        text.Span($"{Model.IssueDate:d}").Style(textStyle);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Data sprzedaży: ").SemiBold().Style(textStyle);
                        text.Span($"{Model.DueDate:d}").Style(textStyle);
                    });
                });

                //row.ConstantItem(100).Height(50).Placeholder();
            });
        }
        void ComposeComments(IContainer container)
        {
            container.Padding(10).AlignBottom().Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().BorderTop(1).AlignCenter().Text("Podpis osoby uprawnionej do wystawienia dokumentu").FontSize(6);
                });
                row.ConstantItem(20);
                row.RelativeItem().Column(column =>
                {
                    column.Item().BorderTop(1).AlignCenter().Text("Data odbioru").FontSize(6);
                });
                row.ConstantItem(20);
                row.RelativeItem().Column(column =>
                {
                    column.Item().BorderTop(1).AlignCenter().Text("Podpis osoby uprawnionej do odbioru dokumentu").FontSize(6);
                });
            });
        }
        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(10);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new SellerAddressComponent("Sprzedawca", Model.SellerInvoice));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new BuyerAddressComponent("Nabywca", Model.CustomerInvoice));
                });

                column.Item().Element(ComposeTable);

                //var totalPrice = Model.InvoiceProducts.Sum(x => x. * x.Quantity);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new PaymentDetailsComponent(Model.AdditionalInfoInvoice.PaymentMethodName, Model.AdditionalInfoInvoice.PaymentDate, Model.AdditionalInfoInvoice.DeliveryName, Model.AdditionalInfoInvoice.CurrencyName));
                    row.ConstantItem(80);
                    row.RelativeItem().Component(new PaymentComponent(Model.InvoiceProducts));
                });

                column.Item().Height(70).Element(ComposeComments);
            });
        }
    }
}
