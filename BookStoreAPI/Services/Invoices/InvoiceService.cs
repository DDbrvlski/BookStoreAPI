﻿namespace BookStoreAPI.Services.Invoices
{
    public class InvoiceService
    {
        public void CreateInvoice()
        {
            var htmlContent = @"<div class=""page-container"">
                              Page
                              <span class=""page""></span>
                              of
                              <span class=""pages""></span>
                            </div>

                            <div class=""logo-container"">
                              <img
                                style=""height: 18px""
                                src=""""
                              >
                            </div>

                            <table class=""invoice-info-container"">
                              <tr>
                                <td rowspan=""2"" class=""client-name"">
                                  Client Name
                                </td>
                                <td>
                                  Anvil Co
                                </td>
                              </tr>
                              <tr>
                                <td>
                                  123 Main Street
                                </td>
                              </tr>
                              <tr>
                                <td>
                                  Invoice Date: <strong>May 24th, 2024</strong>
                                </td>
                                <td>
                                  San Francisco CA, 94103
                                </td>
                              </tr>
                              <tr>
                                <td>
                                  Invoice No: <strong>12345</strong>
                                </td>
                                <td>
                                  hello@useanvil.com
                                </td>
                              </tr>
                            </table>


                            <table class=""line-items-container"">
                              <thead>
                                <tr>
                                  <th class=""heading-quantity"">Qty</th>
                                  <th class=""heading-description"">Description</th>
                                  <th class=""heading-price"">Price</th>
                                  <th class=""heading-subtotal"">Subtotal</th>
                                </tr>
                              </thead>
                              <tbody>
                                <tr>
                                  <td>2</td>
                                  <td>Blue large widgets</td>
                                  <td class=""right"">$15.00</td>
                                  <td class=""bold"">$30.00</td>
                                </tr>
                                <tr>
                                  <td>4</td>
                                  <td>Green medium widgets</td>
                                  <td class=""right"">$10.00</td>
                                  <td class=""bold"">$40.00</td>
                                </tr>
                                <tr>
                                  <td>5</td>
                                  <td>Red small widgets with logo</td>
                                  <td class=""right"">$7.00</td>
                                  <td class=""bold"">$35.00</td>
                                </tr>
                              </tbody>
                            </table>


                            <table class=""line-items-container has-bottom-border"">
                              <thead>
                                <tr>
                                  <th>Payment Info</th>
                                  <th>Due By</th>
                                  <th>Total Due</th>
                                </tr>
                              </thead>
                              <tbody>
                                <tr>
                                  <td class=""payment-info"">
                                    <div>
                                      Account No: <strong>123567744</strong>
                                    </div>
                                    <div>
                                      Routing No: <strong>120000547</strong>
                                    </div>
                                  </td>
                                  <td class=""large"">May 30th, 2024</td>
                                  <td class=""large total"">$105.00</td>
                                </tr>
                              </tbody>
                            </table>

                            <div class=""footer"">
                              <div class=""footer-info"">
                                <span>hello@useanvil.com</span> |
                                <span>555 444 6666</span> |
                                <span>useanvil.com</span>
                              </div>
                              <div class=""footer-thanks"">
                                <img src=""https://github.com/anvilco/html-pdf-invoice-template/raw/main/img/heart.png"" alt=""heart"">
                                <span>Thank you!</span>
                              </div>
                            </div>";
        }
    }
}
