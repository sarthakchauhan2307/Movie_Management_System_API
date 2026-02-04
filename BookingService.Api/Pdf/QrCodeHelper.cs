using QRCoder;
using System.Drawing.Imaging;

namespace BookingService.Api.Pdf
{
    public class QrCodeHelper
    {
        public static byte[] GenerateQrCode(string qrText)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var qrCodeImage = qrCode.GetGraphic(20);
            using var stream = new MemoryStream();
            qrCodeImage.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
