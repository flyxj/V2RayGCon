using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.QrCode;

namespace V2RayGCon.Lib.QRCode
{
    class Generator
    {
        static QrCodeEncodingOptions options = new QrCodeEncodingOptions
        {
            DisableECI = true,
            CharacterSet = "UTF-8",
            Width = 512,
            Height = 512,
        };

        public static Bitmap GenQRCode(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            IBarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options,
            };

            return new Bitmap(writer.Write(content));
        }
    }
}
