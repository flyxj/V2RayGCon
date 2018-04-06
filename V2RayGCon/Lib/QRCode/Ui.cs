using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace V2RayGCon.Lib.QRCode
{
    class Ui
    {
        static QRCodeSplashForm qrSplash = null;

        public static void ShowSplash(Rectangle rect)
        {
            if (qrSplash != null)
            {
                Debug.WriteLine("Splash form exist! skip");
                return;
            }
            Point screen_size = GetScreenPhysicalSize();
            
            Screen screen = Screen.PrimaryScreen;

            Debug.WriteLine("Screen: " + screen_size.ToString());

            qrSplash = new QRCodeSplashForm();
            qrSplash.FormClosed += (s, e) =>
            {
                qrSplash = null;
            };

            qrSplash.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
            double dpi = Screen.PrimaryScreen.Bounds.Width / (double)screen_size.X;
            qrSplash.TargetRect = new Rectangle(
                (int)(rect.Left * dpi + screen.Bounds.X),
                (int)(rect.Top * dpi + screen.Bounds.Y),
                (int)(rect.Width * dpi),
                (int)(rect.Height * dpi));

            Debug.WriteLine("target: " + qrSplash.TargetRect);

            qrSplash.Size = new Size(screen_size.X, screen_size.Y);
            qrSplash.Show();
        }

        public enum DeviceCap
        {
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,
        }

        public static Point GetScreenPhysicalSize()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = g.GetHdc();
                int PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPHORZRES);
                int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

                return new Point(PhysicalScreenWidth, PhysicalScreenHeight);
            }
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);       
    }
}
