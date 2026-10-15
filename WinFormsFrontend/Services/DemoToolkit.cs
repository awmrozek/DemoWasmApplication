using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormsFrontend.Services
{
    public static class DemoToolkit
    {
        public static (byte[] Pixels, int Width, int Height) ToRgba(Bitmap source)
        {
            int width = source.Width;
            int height = source.Height;
            var pixels = new byte[checked(width * height * 4)];

            // Normalize the source to a known 32-bit format first.
            using var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap))
                graphics.DrawImage(source, 0, 0, width, height);

            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                int rowBytes = width * 4;
                byte[] row = new byte[rowBytes];

                for (int y = 0; y < height; y++)
                {
                    IntPtr rowAddress = IntPtr.Add(data.Scan0, y * data.Stride);
                    Marshal.Copy(rowAddress, row, 0, rowBytes);

                    for (int x = 0; x < width; x++)
                    {
                        int i = x * 4;
                        int output = (y * width + x) * 4;

                        // Format32bppArgb is stored as BGRA bytes on Windows.
                        pixels[output] = row[i + 2]; // R
                        pixels[output + 1] = row[i + 1]; // G
                        pixels[output + 2] = row[i];     // B
                        pixels[output + 3] = row[i + 3]; // A
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return (pixels, width, height);
        }

        public static Bitmap ToBitmap(byte[] pixels, int width, int height)
        {
            ArgumentNullException.ThrowIfNull(pixels);

            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(width), "Width and height must be positive.");

            int requiredLength = checked(width * height * 4);
            if (pixels.Length < requiredLength)
                throw new ArgumentException("Pixel buffer is too small.", nameof(pixels));

            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int rowBytes = width * 4;
                byte[] row = new byte[rowBytes];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int input = (y * width + x) * 4;
                        int output = x * 4;

                        row[output] = pixels[input + 2]; // B
                        row[output + 1] = pixels[input + 1]; // G
                        row[output + 2] = pixels[input];     // R
                        row[output + 3] = pixels[input + 3]; // A
                    }

                    IntPtr rowAddress = IntPtr.Add(data.Scan0, y * data.Stride);
                    Marshal.Copy(row, 0, rowAddress, rowBytes);
                }
            }
            catch
            {
                bitmap.Dispose();
                throw;
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return bitmap;
        }
    }
}
