using System;
using System.Collections.Generic;
using System.Text;
using ImTransformBackend.Services;

namespace ImTransformBackend.Tests
{
    public class ImageTransformerTests
    {
        [Fact]
        public void Grayscale_ConvertsRgbToGrayAndKeepsAlpha()
        {
            byte[] pixels = [255, 0, 0, 123];

            ImageTransformer.TransformImage(pixels, width: 1, height: 1);

            byte expectedGray = (byte)((77 * 255) >> 8);

            Assert.Equal(
                new byte[] { expectedGray, expectedGray, expectedGray, 123 },
                pixels);
        }

        [Fact]
        public void Grayscale_ThrowsWhenBufferIsTooSmall()
        {
            byte[] pixels = [1, 2, 3];

            Assert.Throws<ArgumentException>(
                () => ImageTransformer.TransformImage(pixels, width: 1, height: 1));
        }

        [Fact]
        public void Grayscale_DoesNothingForNonPositiveDimensions()
        {
            byte[] pixels = [1, 2, 3, 4];
            byte[] original = (byte[])pixels.Clone();

            ImageTransformer.TransformImage(pixels, width: 0, height: 1);

            Assert.Equal(original, pixels);
        }
    }

}
