using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail([FromBody] PaymentDataDTO paymentData)
        {
            var document = CreatePdfDocument(paymentData);

            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);

                return File(memoryStream.ToArray(), "application/pdf", "payment-data.pdf");
            }
        }

        private Document CreatePdfDocument(PaymentDataDTO paymentData)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .AlignLeft()
                                    .AlignMiddle()
                                    .Height(100)
                                    .Image(Path.Combine("wwwroot/images", "QRPay.png"));

                                row.RelativeItem()
                                    .AlignRight()
                                    .AlignMiddle()
                                    .Height(100)
                                    .MaxWidth(100)
                                    .Image(Path.Combine("wwwroot/images", "Plativoo.png"));
                            });

                            column.Item().Text("NALOG ZA PRENOS")
                                .FontSize(20)
                                .Bold()
                                .AlignCenter();
                               
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(200);
                                columns.RelativeColumn();
                            });


                            table.Cell().Element(CellStyle).Text("Dužnik/Nalogodavac");
                            table.Cell().Element(CellStyle).Text(paymentData.Duznik);

                            table.Cell().Element(CellStyle).Text("Račun dužnika/nalogodavca");
                            table.Cell().Element(CellStyle).Text(paymentData.RacunDuznika);

                            table.Cell().Element(CellStyle).Text("Poverilac/Primalac");
                            table.Cell().Element(CellStyle).Text(paymentData.Poverilac);

                            table.Cell().Element(CellStyle).Text("Račun poverioca/primaoca");
                            table.Cell().Element(CellStyle).Text(paymentData.RacunPoverioca);

                            table.Cell().Element(CellStyle).Text("Šifra plaćanja");
                            table.Cell().Element(CellStyle).Text(paymentData.Sifra);

                            table.Cell().Element(CellStyle).Text("Valuta");
                            table.Cell().Element(CellStyle).Text(paymentData.Valuta);

                            table.Cell().Element(CellStyle).Text("Iznos");
                            table.Cell().Element(CellStyle).Text(paymentData.Iznos.ToString());

                            table.Cell().Element(CellStyle).Text("Svrha plaćanja");
                            table.Cell().Element(CellStyle).Text(paymentData.Svrha);

                            table.Cell().Element(CellStyle).Text("Model");
                            table.Cell().Element(CellStyle).Text(paymentData.Model);

                            table.Cell().Element(CellStyle).Text("Poziv na broj");
                            table.Cell().Element(CellStyle).Text(paymentData.Poziv);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generisano: {DateTime.Now:dd.MM.yyyy}");
                });
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container
                .Padding(5)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .AlignLeft()
                .AlignMiddle();
        }
    }
}
