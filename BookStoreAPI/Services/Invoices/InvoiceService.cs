using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Orders;
using BookStoreData.Data;
using BookStoreData.Models.Customers;
using BookStoreViewModels.ViewModels.Invoices;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;
using System.Net.Http.Headers;

namespace BookStoreAPI.Services.Invoices
{
    public interface IInvoiceService
    {
        Task<InvoiceDocument> CreateInvoice(int orderId);
    }

    public class InvoiceService(BookStoreContext context, IOrderService orderService) : IInvoiceService
    {
        public async Task<InvoiceDocument> CreateInvoice(int orderId)
        {
            var invoiceData = await orderService.GetUserOrderForInvoiceByOrderIdAsync(orderId);
            return new InvoiceDocument(invoiceData);
        }
    }

    public class InvoiceDocument : IDocument
    {
        public InvoiceDataViewModel Model { get; }

        public InvoiceDocument(InvoiceDataViewModel model)
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
                    table.Cell().Element(CellStyle).Text($"{(item.NettoPrice * item.Quantity):F2}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{(item.TaxValue * item.Quantity):F2}").Style(textStyle);
                    table.Cell().Element(CellStyle).Text($"{(item.BruttoPrice * item.Quantity):F2}").Style(textStyle);

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
            //container.Padding(10).AlignBottom().Column(column =>
            //{
            //    column.Spacing(5);
            //    column.Item().AlignLeft().AlignBottom().Text("Dziękujemy za zakupy").FontSize(14);
            //});
        }
        void ComposeContent(IContainer container)
        {
            //container.PaddingVertical(40).Border(1).Table(table =>
            //{

            //    table.ColumnsDefinition(column =>
            //    {
            //        column.ConstantColumn(200);
            //        column.ConstantColumn(95);
            //        column.ConstantColumn(200);
            //    });

            //    table.Cell().Row(1).Column(1).Component(new AddressComponent("Sprzedawca", Model.SellerAddress));
            //    table.Cell().Row(1).Column(2).Width(50);
            //    table.Cell().Row(1).Column(3).Component(new AddressComponent("Odbiorca", Model.CustomerAddress));

            //    table.Cell().Row(2).ColumnSpan(3).Height(10);

            //    table.Cell().Row(3).ColumnSpan(3).Element(ComposeTable);

            //    table.Cell().Row(4).ColumnSpan(3).Height(10);

            //    table.Cell().Row(5).Column(1).Component(new PaymentDetailsComponent("Gotówka", new DateTime(new DateOnly(2023, 12, 2), new TimeOnly()), "Kurier"));
            //    table.Cell().Row(5).Column(2);
            //    table.Cell().Row(5).Column(3).Component(new PaymentComponent(Model.Items));

            //    table.Cell().Row(6).ColumnSpan(3).Height(20);

            //    table.Cell().Row(7).ColumnSpan(3).AlignBottom().Border(1).Element(ComposeComments);
            //});
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
    public class PaymentComponent : IComponent
    {
        private decimal TotalNetto { get; }
        private decimal TaxValue { get; }
        private decimal TotalBrutto { get; }

        public PaymentComponent(List<ProductInvoiceViewModel> orderItems)
        {
            TotalNetto = orderItems.Sum(x => x.NettoPrice * x.Quantity);
            TaxValue = orderItems.Sum(x => x.TaxValue * x.Quantity);
            TotalBrutto = orderItems.Sum(x => x.BruttoPrice * x.Quantity);
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
    public class PaymentDetailsComponent : IComponent
    {
        private string PaymentMethodTitle { get; }
        private string PaymentCurrencyTitle { get; }
        private DateTime PaymentDate { get; }
        private string DeliveryMethodTitle { get; }

        public PaymentDetailsComponent(string paymentMethodTitle, DateTime? paymentDate, string deliveryMethodTitle, string currencyName)
        {
            if (paymentDate == null)
            {
                throw new BadRequestException("Wystąpił błąd podczas generowania faktury");
            }
            PaymentMethodTitle = paymentMethodTitle;
            DeliveryMethodTitle = deliveryMethodTitle;
            PaymentDate = (DateTime)paymentDate;
            PaymentCurrencyTitle = currencyName;
        }

        public void Compose(IContainer container)
        {
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial);
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Forma płatności:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{PaymentMethodTitle}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Waluta:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{PaymentCurrencyTitle}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Data płatności:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{PaymentDate.ToShortDateString()}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Sposób dostawy:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{DeliveryMethodTitle}").Style(textStyle);
                });
            });
        }
    }
    public class SellerAddressComponent : IComponent
    {
        private string Title { get; }
        private SellerInvoiceViewModel SellerData { get; }

        public SellerAddressComponent(string title, SellerInvoiceViewModel sellerData)
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
                column.Item().Text($"{SellerData.Postcode} {SellerData.CityName}");
                column.Item().Text($"{SellerData.CountryName}");
                column.Item().Text($"NIP {SellerData.TaxIdentificationNumber}");
                column.Item().Text($"Telefon {SellerData.Phone}");
                column.Item().Text($"Adres e-mail {SellerData.Email}");
            });
        }
    }
    public class BuyerAddressComponent : IComponent
    {
        private string Title { get; }
        private CustomerInvoiceViewModel BuyerData { get; }

        public BuyerAddressComponent(string title, CustomerInvoiceViewModel buyerData)
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
