using BookStoreAPI.Services.Orders;
using BookStoreData.Data;
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
        Task CreateInvoice();
    }

    public class InvoiceService(BookStoreContext context, IOrderService orderService) : IInvoiceService
    {
        public async Task CreateInvoice()
        {
            var order = await orderService.GetUserOrderByIdAsync(1);


        }
    }

    public class InvoiceDocument : IDocument
    {
        public InvoiceModel Model { get; }

        public InvoiceDocument(InvoiceModel model)
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
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial);
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn();
                    columns.RelativeColumn(4);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignLeft().Text("Lp.").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Kod").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Nazwa towaru").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Ilość").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("J.m.").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("VAT").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Cena Brutto").Style(textStyle);
                    header.Cell().Element(CellStyle).Text("Rabat Brutto").Style(textStyle);
                    header.Cell().Element(CellStyle).AlignRight().Text("Wartość brutto").Style(textStyle);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).BorderLeft(1).BorderRight(1).BorderColor(Colors.White).PaddingVertical(5).Background(Colors.Grey.Lighten1).AlignCenter().AlignMiddle();
                    }
                });

                // step 3
                foreach (var item in Model.Items)
                {
                    table.Cell().Element(CellStyle).AlignLeft().Text("1").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("1241").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("Asus rog super monitor ultra").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("2").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("szt.").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("23%").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("200,00").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("11,30").Style(textStyle);
                    table.Cell().Element(CellStyle).Text("22150,00").Style(textStyle);

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
                    row.RelativeItem().Component(new AddressComponent("Sprzedawca", Model.SellerAddress));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new AddressComponent("Odbiorca", Model.CustomerAddress));
                });

                column.Item().Element(ComposeTable);

                var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new PaymentDetailsComponent("Gotówka", new DateTime(new DateOnly(2023, 12, 2), new TimeOnly()), "Kurier"));
                    row.ConstantItem(80);
                    row.RelativeItem().Component(new PaymentComponent(Model.Items));
                });


                if (!string.IsNullOrWhiteSpace(Model.Comments))
                    column.Item().Height(70).Element(ComposeComments);
            });
        }
    }
    public class PaymentComponent : IComponent
    {
        private decimal Discount { get; }
        private decimal Subtotal { get; }
        private decimal Total { get; }

        public PaymentComponent(List<OrderItem> orderItems)
        {

        }

        public void Compose(IContainer container)
        {
            var textStyle = TextStyle.Default.FontFamily(Fonts.Arial);
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Suma częściowa:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{Subtotal}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Rabat:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{Discount}").Style(textStyle);
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("Całkowita cena:").Style(textStyle);
                    row.ConstantItem(5);
                    row.RelativeItem().Text($"{Total}").Style(textStyle);
                });
            });
        }
    }
    public class PaymentDetailsComponent : IComponent
    {
        private string PaymentMethodTitle { get; }
        private string PaymentCurrencyTitle { get; } = "PLN";
        private DateTime PaymentDate { get; }
        private string DeliveryMethodTitle { get; }

        public PaymentDetailsComponent(string paymentMethodTitle, DateTime paymentDate, string deliveryMethodTitle)
        {
            PaymentMethodTitle = paymentMethodTitle;
            DeliveryMethodTitle = deliveryMethodTitle;
            PaymentDate = paymentDate;
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
    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private Address Address { get; }

        public AddressComponent(string title, Address address)
        {
            Title = title;
            Address = address;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                column.Item().Text(Address.CompanyName);
                column.Item().Text(Address.Street);
                column.Item().Text($"{Address.City}, {Address.State}");
                column.Item().Text(Address.Email);
                column.Item().Text(Address.Phone);
            });
        }
    }
    public static class InvoiceDocumentDataSource
    {
        private static Random Random = new Random();

        public static InvoiceModel GetInvoiceDetails()
        {
            var items = Enumerable
                .Range(1, 10)
                .Select(i => GenerateRandomOrderItem())
                .ToList();

            return new InvoiceModel
            {
                InvoiceNumber = Random.Next(1_000, 10_000),
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now + TimeSpan.FromDays(14),

                SellerAddress = GenerateRandomAddress(),
                CustomerAddress = GenerateRandomAddress(),

                Items = items,
                Comments = Placeholders.Paragraph()
            };
        }

        private static OrderItem GenerateRandomOrderItem()
        {
            return new OrderItem
            {
                Name = Placeholders.Label(),
                Price = (decimal)Math.Round(Random.NextDouble() * 100, 2),
                Quantity = Random.Next(1, 10)
            };
        }

        private static Address GenerateRandomAddress()
        {
            return new Address
            {
                CompanyName = Placeholders.Name(),
                Street = Placeholders.Label(),
                City = Placeholders.Label(),
                State = Placeholders.Label(),
                Email = Placeholders.Email(),
                Phone = Placeholders.PhoneNumber()
            };
        }
    }

    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public Address SellerAddress { get; set; }
        public Address CustomerAddress { get; set; }

        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Address
    {
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public object Email { get; set; }
        public string Phone { get; set; }
    }
}
