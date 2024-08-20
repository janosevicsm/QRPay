using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using ZXing.QrCode;
using ZXing;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] PaymentDataDTO paymentData)
        {
            var document = CreatePdfDocument(paymentData);

            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var smtpServer = _configuration["SmtpServer"];
                var smtpPort = int.Parse(_configuration["SmtpPort"]);
                var smtpUsername = _configuration["SmtpUsername"];
                var smtpPassword = _configuration["SmtpPassword"];
                var fromEmail = _configuration["FromEmail"];

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(paymentData.Email);
                mailMessage.Subject = "Payment Data PDF";
                mailMessage.Body = "Payment Data PDF.";

                var attachment = new Attachment(memoryStream, "payment-data.pdf", "application/pdf");
                mailMessage.Attachments.Add(attachment);

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    smtpClient.Send(mailMessage);
                }

                return Ok("Email sent successfully.");
            }
        }

        private Document CreatePdfDocument(PaymentDataDTO paymentData)
        {
            var qrCodeImage = GenerateQRCode(paymentData);


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
                        .Column(column =>
                        {
                            column.Item().Table(table =>
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

                            column.Item().AlignCenter().MaxWidth(250).MaxHeight(250).Padding(40).Image(qrCodeImage);

                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generisano: {DateTime.Now:dd.MM.yyyy.}");
                });
            });
        }

        private byte[] GenerateQRCode(PaymentDataDTO paymentData)
        {
            var qrContent = $"K:EK|" +
                            $"V:01|" +
                            $"C:1|" +
                            $"R:{paymentData.RacunPoverioca}|" +
                            $"N:{paymentData.Poverilac}|" +
                            $"I:{paymentData.Valuta}{paymentData.Iznos.ToString("0.00", CultureInfo.InvariantCulture)}|" +
                            $"SF:{paymentData.Sifra}|" +
                            $"S:{paymentData.Svrha}|" +
                            $"RO:{paymentData.Poziv}";

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250,
                    Margin = 0
                }
            };

            var pixelData = writer.Write(qrContent);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            {
                using (var ms = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                            pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }

                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
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
