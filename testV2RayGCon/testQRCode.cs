using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace testV2RayGCon
{
    public partial class testQRCode : Form
    {
        int index, stop;
        List<List<Rectangle>> data;
        Point drawSize;
        IEnumerator<Bitmap> bmp;

        public testQRCode()
        {
            InitializeComponent();
            // picDrawArea.Image= new System.Drawing.Bitmap(picDrawArea.Width, picDrawArea.Height);
            timerDraw.Interval = 1000;
            drawSize = new Point(picDrawArea.Width, picDrawArea.Height);
#if DEBUG
            bmp = V2RayGCon.Lib.QRCode.QRCode.DbgCopyScreen();
#endif
        }

        private void btnScanWinTest_Click(object sender, EventArgs e)
        {
            timerDraw.Stop();
            picDrawArea.Image= new System.Drawing.Bitmap(picDrawArea.Width, picDrawArea.Height);
            data = V2RayGCon.Lib.QRCode.QRCode.GenSquareScanWinList(drawSize, 7, 6);
            stop = data.Count;
            index = 0;
            timerDraw.Tick += DrawSquareScanList;
            timerDraw.Start();
        }

        void DrawSquareScanList(Object sender,EventArgs ev)
        {
            DrawRect(data[index][1]);
            picDrawArea.Invalidate();
            index++;
            if (index >= stop)
            {
                timerDraw.Stop();
                timerDraw.Tick -= DrawSquareScanList;
            }
            Debug.WriteLine("index: " + index + "stop: " + stop);
        }

        void DrawZoomScanList(Object sender, EventArgs ev)
        {
            DrawRect(data[index][0]);
            picDrawArea.Invalidate();
            index++;
            if (index >= stop)
            {
                timerDraw.Stop();
                timerDraw.Tick -= DrawZoomScanList;
            }
            Debug.WriteLine("index: " + index + "stop: " + stop);

        }

        private void btnZoomTest_Click(object sender, EventArgs e)
        {
            timerDraw.Stop();
            picDrawArea.Image = new System.Drawing.Bitmap(picDrawArea.Width, picDrawArea.Height);
            data = V2RayGCon.Lib.QRCode.QRCode.GenZoomScanWinList(drawSize);
            stop = data.Count;
            index = 0;
            timerDraw.Tick += DrawZoomScanList;
            timerDraw.Start();

        }

        private void btnScanQRCode_Click(object sender, EventArgs e)
        {
            void success(string link)
            {
                Debug.WriteLine("Callback got link:" + link);
            }
            void fail()
            {
                Debug.WriteLine("no link found!");
            }

            V2RayGCon.Lib.QRCode.QRCode.ScanQRCode(success,fail);
        }

        private void btnCpoyScreen_Click(object sender, EventArgs e)
        {
#if DEBUG
            if (bmp.MoveNext())
            {
                picDrawArea.Image = bmp.Current;
                picDrawArea.Invalidate();
            }
            else
            {
                Debug.WriteLine("done!");
            }
#endif


        }

        void DrawRect(Rectangle rect) { 
            var bmp = picDrawArea.Image;
            Debug.WriteLine("Rect: " + rect);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                Pen red = new Pen(Color.Red, 3);
                g.DrawRectangle(red, rect);
            }
        }
    }
}
