namespace ImTransformBackend.Services
{
    public static class ImageTransformer
    {
        // pixels must contain width * height * 4 bytes in RGBA order.
        public static void TransformImage(byte[] pixels, int width, int height)
        {
            ArgumentNullException.ThrowIfNull(pixels);

            if (width <= 0 || height <= 0)
                return;

            int requiredLength = checked(width * height * 4);
            if (pixels.Length < requiredLength)
                throw new ArgumentException("Pixel buffer is too small.", nameof(pixels));

            for (int i = 0; i < width * height; i++)
            {
                int offset = i * 4;

                byte gray = (byte)(
                    (77 * pixels[offset] +
                     150 * pixels[offset + 1] +
                     29 * pixels[offset + 2]) >> 8);

                pixels[offset] = gray;
                pixels[offset + 1] = gray;
                pixels[offset + 2] = gray;
                // Keep pixels[offset + 3] (alpha) unchanged.
            }
        }
    }
}
