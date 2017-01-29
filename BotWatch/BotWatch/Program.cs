using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BotWatch.Properties;

namespace BotWatch {

    class Program {




        static void Main(string[] args) {
            do {
                try {
                    var images = new List<string>();
                    foreach (var process in Process.GetProcesses().Where(p => p.MainWindowTitle == "RuneScape")) {
                        var fileNameEnd = "/" + process.Id + args[4] + ".jpg";
                        var ftpFileName = args[0] + fileNameEnd;

                        upload(args[0], args[1], args[2], ImageToByte(PrintWindow(process.MainWindowHandle)), ftpFileName);
                        images.Add(args[3] + fileNameEnd);
                    }
                    var webpage = string.Format(Resources.htmlWebpage, args[4], images.Aggregate("", (current, image) => current + ("<img max-width=\"90%\" src=\"" + image + "\"/>" + Environment.NewLine)));
                    upload(args[0], args[1], args[2], Encoding.UTF8.GetBytes(webpage), args[0] + "/" + args[4] + "index.htm");
                    Thread.Sleep(20000);
                } catch (Exception) {
                    // ignored
                }
                
            } while (!Console.KeyAvailable);
        }

        public static void upload(string host, string username, string password, byte[] bytes, string fileName) {
            var request = (FtpWebRequest)WebRequest.Create(fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.ContentLength = bytes.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            var response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine(@"Upload File Complete, status {0}", response.StatusDescription);
            response.Close();
        }


        public static byte[] ImageToByte(Image img) {
            using (var stream = new MemoryStream()) {
                img.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public static Bitmap PrintWindow(IntPtr hwnd) {
            RECT rc;
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;

        public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom) {
        }
        public RECT(int Left, int Top, int Right, int Bottom) {
            _Left = Left;
            _Top = Top;
            _Right = Right;
            _Bottom = Bottom;
        }

        public int X {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Y {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Left {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Top {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Right {
            get { return _Right; }
            set { _Right = value; }
        }
        public int Bottom {
            get { return _Bottom; }
            set { _Bottom = value; }
        }
        public int Height {
            get { return _Bottom - _Top; }
            set { _Bottom = value + _Top; }
        }
        public int Width {
            get { return _Right - _Left; }
            set { _Right = value + _Left; }
        }
        public Point Location {
            get { return new Point(Left, Top); }
            set {
                _Left = value.X;
                _Top = value.Y;
            }
        }
        public Size Size {
            get { return new Size(Width, Height); }
            set {
                _Right = value.Width + _Left;
                _Bottom = value.Height + _Top;
            }
        }

        public static implicit operator Rectangle(RECT Rectangle) {
            return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
        }
        public static implicit operator RECT(Rectangle Rectangle) {
            return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        }
        public static bool operator ==(RECT Rectangle1, RECT Rectangle2) {
            return Rectangle1.Equals(Rectangle2);
        }
        public static bool operator !=(RECT Rectangle1, RECT Rectangle2) {
            return !Rectangle1.Equals(Rectangle2);
        }

        public override string ToString() {
            return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public bool Equals(RECT Rectangle) {
            return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
        }

        public override bool Equals(object Object) {
            if (Object is RECT) {
                return Equals((RECT)Object);
            } else if (Object is Rectangle) {
                return Equals(new RECT((Rectangle)Object));
            }

            return false;
        }
    }
}
