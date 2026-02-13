using BookingService.Api.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookingService.Api.Pdf
{
    public class TicketPdfDocument : IDocument
    {
        private readonly TicketPdfModel _ticket;

        public TicketPdfDocument(TicketPdfModel ticket)
        {
            _ticket = ticket;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.Grey.Lighten4);
                page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Grey.Darken3));

                page.Content()
                    .AlignCenter()
                    .PaddingVertical(40)
                    .Column(column =>
                    {
                        column.Item().Width(450).Element(ComposeTicketCard);
                    });

                page.Footer()
                    .AlignCenter()
                    .PaddingBottom(20)
                    .Text("Thank you for choosing us!")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);
            });
        }

        private void ComposeTicketCard(IContainer container)
        {
            container
                .Background(Colors.White)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Column(column =>
                {
                    column.Item().Element(ComposeHeader);
                    column.Item().Padding(25).Element(ComposeBody);
                    column.Item().LineHorizontal(1);
                    column.Item().Padding(25).Element(ComposeFooter);
                });
        }

        // 🎬 HEADER WITH POSTER
        private void ComposeHeader(IContainer container)
        {
            container
                .Background(Colors.DeepPurple.Darken2)
                .Padding(20)
                .Row(row =>
                {
                    if (_ticket.MoviePoster != null && _ticket.MoviePoster.Length > 0)
                    {
                        row.AutoItem()
                            .Width(90)
                            .Height(130)
                            .Image(_ticket.MoviePoster, ImageScaling.FitArea);
                    }

                    row.RelativeItem()
                        .PaddingLeft(15)
                        .Column(column =>
                        {
                            column.Item().Text("MOVIE TICKET")
                                .FontSize(10)
                                .SemiBold()
                                .FontColor(Colors.Grey.Lighten3);

                            column.Item().Text(_ticket.MovieName)
                                .FontSize(22)
                                .Bold()
                                .FontColor(Colors.White);

                            column.Item().PaddingTop(5)
                                .Text(_ticket.TheatreName)
                                .FontSize(14)
                                .FontColor(Colors.DeepPurple.Lighten4);
                        });
                });
        }

        private void ComposeBody(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(
                        new LabelValueComponent(
                            "Date",
                            _ticket.ShowDate.ToString("dd MMM yyyy")
                        ));

                    row.RelativeItem().Component(
                        new LabelValueComponent("Time", _ticket.ShowTime));

                    row.RelativeItem().Component(
                        new LabelValueComponent("Screen", _ticket.Screen));
                });

                column.Item().PaddingTop(15).Component(
                    new LabelValueComponent("Address", _ticket.Address));

                column.Item().PaddingTop(15).Component(
                   new LabelValueComponent("Seats", _ticket.SeatCount.ToString()));

                column.Item().PaddingTop(15).Component(
                          new LabelValueComponent(
                              "Seats",
                              string.Join(", ", _ticket.SeatNos)
                          ));


            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Total Amount")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);

                    column.Item().Text($"₹{_ticket.Amount}")
                        .FontSize(20)
                        .Bold()
                        .FontColor(Colors.DeepPurple.Darken2);
                });
                if (_ticket.QrCodeImage != null && _ticket.QrCodeImage.Length > 0)
                {
                    row.AutoItem()
                        .AlignMiddle()
                        .PaddingLeft(20)
                        .Width(90)
                        .Height(90)
                        .Image(_ticket.QrCodeImage, ImageScaling.FitArea);
                }
            });
        }

        private class LabelValueComponent : IComponent
        {
            private readonly string _label;
            private readonly string _value;

            public LabelValueComponent(string label, string value)
            {
                _label = label;
                _value = value;
            }

            public void Compose(IContainer container)
            {
                container.Column(column =>
                {
                    column.Item().Text(_label)
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);

                    column.Item().Text(_value)
                        .FontSize(12)
                        .SemiBold()
                        .FontColor(Colors.Black);
                });
            }
        }
    }
}
