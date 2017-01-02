using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace System.Drawing
{
    public static class DrawingExtensions
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            BitmapSource bitmapSource = null;

            try
            {
                // Continue only if bitmap object was specified.
                if (bitmap != null)
                {
                    // Get the pointer to the bitmap.
                    IntPtr ip = bitmap.GetHbitmap();

                    // NOTE: Generating this bitmap source will create a GDI pointer; this needs to be destroyed!
                    bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                           ip,
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

                    // IMPORTANT: Destroy GDI pointer!
                    DeleteObject(ip);
                }
            }
            catch { }

            return bitmapSource;
        }
    }
}
