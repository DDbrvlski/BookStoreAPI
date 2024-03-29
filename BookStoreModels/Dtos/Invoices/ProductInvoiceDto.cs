﻿namespace BookStoreDto.Dtos.Invoices
{
    public class ProductInvoiceDto
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Tax { get; set; }
        public decimal TaxValue { get; set; }
        public decimal NettoPrice { get; set; }
        public decimal NettoValue { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal BruttoValue { get; set; }
    }
}
