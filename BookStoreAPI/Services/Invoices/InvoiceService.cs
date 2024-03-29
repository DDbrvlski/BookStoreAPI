﻿using BookStoreAPI.Infrastructure.Exceptions;
using BookStoreAPI.Services.Invoices.InvoiceComponents;
using BookStoreAPI.Services.Orders;
using BookStoreAPI.Services.Users;
using BookStoreData.Data;
using BookStoreDto.Dtos.Invoices;
using Microsoft.EntityFrameworkCore;
using Spire.Doc;
using Spire.Doc.Documents;

namespace BookStoreAPI.Services.Invoices
{
    public interface IInvoiceService
    {
        Task<byte[]> GetCurrentInvoiceTemplateFile();
        Task UploadInvoiceDocxTemplateFile(IFormFile file);
        Task<InvoiceDocument> CreateInvoice(int orderId);
        Task<byte[]> CreateInvoiceByDocxTemplate(int orderId);
        List<PossibleTemplateFieldsDto> GetPossibleFieldsInInvoiceTemplate();
    }

    public class InvoiceService
        (BookStoreContext context,
        IUserContextService userContextService,
        IOrderService orderService,
        IWebHostEnvironment hostEnvironment)
        : IInvoiceService
    {
        public async Task<byte[]> GetCurrentInvoiceTemplateFile()
        {
            var invoiceTemplatePath = GetDocumentTemplateFilePath("FakturaTemplate.docx");

            if (!File.Exists(invoiceTemplatePath))
            {
                throw new NotFoundException("Nie znaleziono żadnego pliku");
            }

            return File.ReadAllBytes(invoiceTemplatePath);
        }
        public async Task UploadInvoiceDocxTemplateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("Nie wysłano pliku.");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".docx")
            {
                throw new BadRequestException("Wysłano niepoprawny format pliku - przyjmowane formaty [.docx].");
            }
                
            var invoiceTemplatePath = GetDocumentTemplateFilePath("FakturaTemplate.docx");

            if (File.Exists(invoiceTemplatePath))
            {
                var guid = Guid.NewGuid();
                var oldInvoiceTemplatePath = GetDocumentTemplateFilePath($"FakturaTemplate_{guid}.docx");
                File.Move(invoiceTemplatePath, oldInvoiceTemplatePath);
            }

            using (var stream = new FileStream(invoiceTemplatePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!File.Exists(invoiceTemplatePath))
            {
                throw new BadRequestException("Wystąpił błąd podczas zapisu pliku.");
            }
        }
        public List<PossibleTemplateFieldsDto> GetPossibleFieldsInInvoiceTemplate()
        {
            List<PossibleTemplateFieldsDto> fields = new List<PossibleTemplateFieldsDto>()
            {
                new PossibleTemplateFieldsDto("{InvoiceNumber}","Numer faktury"),
                new PossibleTemplateFieldsDto("{IssueDate}","Data wystawienia"),
                new PossibleTemplateFieldsDto("{DueDate}","Data sprzedaży"),
                new PossibleTemplateFieldsDto("{SellerName}","Nazwa sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerAddress}","Ulica i dom sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerCity}","Miasto sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerCountry}","Kraj sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerTaxIdentificationNumber}","Numer identyfikacji podatkowej sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerPhone}","Numer telefonu sprzedawcy"),
                new PossibleTemplateFieldsDto("{SellerEmail}","Email sprzedawcy"),
                new PossibleTemplateFieldsDto("{CustomerName}","Nazwa nabywcy"),
                new PossibleTemplateFieldsDto("{CustomerAddress}","Ulica i dom nabywcy"),
                new PossibleTemplateFieldsDto("{CustomerCity}","Miasto nabywcy"),
                new PossibleTemplateFieldsDto("{CustomerCountry}","Kraj nabywcy"),
                new PossibleTemplateFieldsDto("{CustomerEmail}","Email nabywcy"),
                new PossibleTemplateFieldsDto("{CustomerPhone}","Numer telefonu nabywcy"),
                new PossibleTemplateFieldsDto("{PaymentMethod}","Metoda płatności"),
                new PossibleTemplateFieldsDto("{Currency}","Waluta"),
                new PossibleTemplateFieldsDto("{PaymentDate}","Data płatności"),
                new PossibleTemplateFieldsDto("{DeliveryMethod}","Sposób dostawy"),
                new PossibleTemplateFieldsDto("{DeliveryPrice}","Koszt dostawy"),
                new PossibleTemplateFieldsDto("{TotalNetto}","Suma ceny netto"),
                new PossibleTemplateFieldsDto("{TaxValue}","Suma podatku"),
                new PossibleTemplateFieldsDto("{TotalBrutto}","Suma ceny brutto"),
            };
            return fields;
        }
        /// <summary>
        /// Creates pdf invoice using QuestPDF
        /// </summary>
        public async Task<InvoiceDocument> CreateInvoice(int orderId)
        {
            var invoiceData = await GetCustomerInvoiceDataAsync(orderId);
            return new InvoiceDocument(invoiceData);
        }
        /// <summary>
        /// Creates pdf invoice using Spire.Doc
        /// </summary>
        public async Task<byte[]> CreateInvoiceByDocxTemplate(int orderId)
        {
            var invoiceData = await GetCustomerInvoiceDataAsync(orderId);
            var invoiceTemplatePath = GetDocumentTemplateFilePath("FakturaTemplate.docx");

            var pdfFilePath = BindInvoiceDataToDocumentTemplate(invoiceData, invoiceTemplatePath);

            if (pdfFilePath == null || !File.Exists(pdfFilePath))
            {
                throw new NotFoundException("Nie znaleziono podanego pliku");
            }

            return File.ReadAllBytes(pdfFilePath);
        }

        private string BindInvoiceDataToDocumentTemplate(InvoiceDataDto invoiceData, string templateFilePath)
        {
            using (Document document = new Document())
            {
                document.LoadFromFile(templateFilePath);

                FindAndBindDataToDocument(document, "{InvoiceNumber}", invoiceData.InvoiceNumber.ToString());
                FindAndBindDataToDocument(document, "{IssueDate}", invoiceData.IssueDate.ToShortDateString());
                FindAndBindDataToDocument(document, "{DueDate}", invoiceData.DueDate.ToShortDateString());

                FindAndBindDataToDocument(document, "{SellerName}", invoiceData.SellerInvoice.Name);
                FindAndBindDataToDocument(document, "{SellerAddress}", $"{invoiceData.SellerInvoice.Street} {invoiceData.SellerInvoice.StreetNumber}{invoiceData.SellerInvoice.HouseNumber}");
                FindAndBindDataToDocument(document, "{SellerCity}", $"{invoiceData.SellerInvoice.Postcode} {invoiceData.SellerInvoice.City}");
                FindAndBindDataToDocument(document, "{SellerCountry}", invoiceData.SellerInvoice.Country);
                FindAndBindDataToDocument(document, "{SellerTaxIdentificationNumber}", invoiceData.SellerInvoice.TaxIdentificationNumber);
                FindAndBindDataToDocument(document, "{SellerPhone}", invoiceData.SellerInvoice.Phone);
                FindAndBindDataToDocument(document, "{SellerEmail}", invoiceData.SellerInvoice.Email);

                FindAndBindDataToDocument(document, "{CustomerName}", invoiceData.CustomerInvoice.Name);
                FindAndBindDataToDocument(document, "{CustomerAddress}", $"{invoiceData.CustomerInvoice.Address.Street} {invoiceData.CustomerInvoice.Address.StreetNumber}{invoiceData.CustomerInvoice.Address.HouseNumber}");
                FindAndBindDataToDocument(document, "{CustomerCity}", $"{invoiceData.CustomerInvoice.Address.Postcode} {invoiceData.CustomerInvoice.Address.CityName}");
                FindAndBindDataToDocument(document, "{CustomerCountry}", invoiceData.CustomerInvoice.Address.CountryName);
                FindAndBindDataToDocument(document, "{CustomerEmail}", invoiceData.CustomerInvoice.Email ?? "");
                FindAndBindDataToDocument(document, "{CustomerPhone}", invoiceData.CustomerInvoice.Phone ?? "");

                DateTime paymentDate = (DateTime)invoiceData.AdditionalInfoInvoice.PaymentDate;

                FindAndBindDataToDocument(document, "{PaymentMethod}", invoiceData.AdditionalInfoInvoice.PaymentMethodName);
                FindAndBindDataToDocument(document, "{Currency}", invoiceData.AdditionalInfoInvoice.CurrencyName);
                FindAndBindDataToDocument(document, "{PaymentDate}", paymentDate.ToShortDateString());
                FindAndBindDataToDocument(document, "{DeliveryMethod}", invoiceData.AdditionalInfoInvoice.DeliveryName);
                FindAndBindDataToDocument(document, "{DeliveryPrice}", $"{invoiceData.AdditionalInfoInvoice.DeliveryPrice:F2}");

                decimal totalNetto = invoiceData.InvoiceProducts.Sum(x => x.NettoValue) + invoiceData.AdditionalInfoInvoice.DeliveryPrice;
                decimal taxValue = invoiceData.InvoiceProducts.Sum(x => x.TaxValue);
                decimal totalBrutto = invoiceData.InvoiceProducts.Sum(x => x.BruttoValue) + invoiceData.AdditionalInfoInvoice.DeliveryPrice;

                FindAndBindDataToDocument(document, "{TotalNetto}", $"{totalNetto:F2}");
                FindAndBindDataToDocument(document, "{TaxValue}", $"{taxValue:F2}");
                FindAndBindDataToDocument(document, "{TotalBrutto}", $"{totalBrutto:F2}");
                
                Table table = FindTable(document, "Produkty");
                BindDataToTable(table, invoiceData.InvoiceProducts);

                return GeneratePdfFromDocx(document);
            }
        }
        private string GeneratePdfFromDocx(Document document)
        {
            Guid guid = Guid.NewGuid();
            string pdfFilePath = GetDocumentsFilePath($"{guid}.pdf");
            using (MemoryStream documentMemoryStream = new MemoryStream())
            {
                document.SaveToStream(documentMemoryStream, FileFormat.Docx);

                documentMemoryStream.Position = 0;

                using (MemoryStream pdfMemoryStream = new MemoryStream())
                {
                    Document pdfDoc = new Document();
                    pdfDoc.LoadFromStream(documentMemoryStream, FileFormat.Docx);

                    pdfDoc.SaveToStream(pdfMemoryStream, FileFormat.PDF);

                    using (FileStream fileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        pdfMemoryStream.Position = 0;
                        pdfMemoryStream.CopyTo(fileStream);

                        return pdfFilePath;
                    }
                }
            }
        }
        private Table? FindTable(Document document, string tableName)
        {
            foreach (Section section in document.Sections)
            {
                foreach (Table table in section.Tables)
                {
                    if (table.Title == tableName)
                    {
                        return table;
                    }
                }
            }
            return null;
        }
        private void BindDataToTable(Table? table, List<ProductInvoiceDto> invoiceProducts)
        {
            if (table != null)
            {
                if (table.Rows.Count < invoiceProducts.Count + 1)
                {
                    int rowsToAdd = invoiceProducts.Count - table.Rows.Count + 1;
                    for (int i = 0; i < rowsToAdd; i++)
                    {
                        table.Rows.Add(table.Rows[1].Clone());
                    }
                }

                int rowIndex = 1;
                foreach (var product in invoiceProducts)
                {
                    FillTableRow(table.Rows[rowIndex], product, rowIndex);
                    rowIndex++;
                }
            }
        }
        private void FillTableRow(TableRow row, ProductInvoiceDto product, int lp)
        {
            if (row.Cells.Count > 0 && row.Cells[0].Paragraphs.Count > 0)
            {
                row.Cells[0].Paragraphs[0].Text = lp.ToString();
                row.Cells[1].Paragraphs[0].Text = product.Code.ToString();
                row.Cells[2].Paragraphs[0].Text = product.Name;
                row.Cells[3].Paragraphs[0].Text = product.Quantity.ToString();
                row.Cells[4].Paragraphs[0].Text = product.UnitOfMeasure;
                row.Cells[5].Paragraphs[0].Text = $"{product.NettoPrice:F2}" .ToString();
                row.Cells[6].Paragraphs[0].Text = product.Tax.ToString();
                row.Cells[7].Paragraphs[0].Text = $"{product.NettoValue:F2}";
                row.Cells[8].Paragraphs[0].Text = $"{product.TaxValue:F2}";
                row.Cells[9].Paragraphs[0].Text = $"{product.BruttoValue:F2}";
            }
        }
        private void FindAndBindDataToDocument(Document document, string bindVariableName, string dataToBind)
        {
            TextSelection firstNameField = document.FindString(bindVariableName, false, true);
            if (firstNameField != null)
            {
                firstNameField.GetAsOneRange().Text = dataToBind;
            }
        }
        private string GetDocumentTemplateFilePath(string fileName)
        {
            string rootPath = hostEnvironment.ContentRootPath;
            return Path.Combine(rootPath, "Files", "DocumentTemplates", fileName);
        }
        private string GetDocumentsFilePath(string fileName)
        {
            string rootPath = hostEnvironment.ContentRootPath;
            return Path.Combine(rootPath, "Files", "Documents", fileName);
        }
        private async Task<InvoiceDataDto> GetCustomerInvoiceDataAsync(int orderId)
        {
            var customer = await userContextService.GetCustomerByTokenAsync();
            var isOrderBelongsToUser = await orderService.CheckIfOrderBelongsToCustomer(customer.Id, orderId);
            if (!isOrderBelongsToUser)
            {
                throw new BadRequestException("Nie można pobrać faktury zamówienia, które nie należy do zalogowanego użytkownika.");
            }

            var paymentDate = await context.Order
                .Where(x => x.Id == orderId && x.IsActive)
                .Select(x => x.Payment.PaymentDate)
                .FirstOrDefaultAsync();
            if(paymentDate == null)
            {
                throw new BadRequestException("Nie zapłacono za zamówienie, brak możliwości pobrania faktury.");
            }

            return await orderService.GetUserOrderForInvoiceByOrderIdAsync(orderId);
        }
    }
}
