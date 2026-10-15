#include <algorithm>
#include <cmath>
#include <cstddef>
#include <cstdint>
#include <cstdlib>
#include <vector>

extern "C" __attribute__((visibility("default")))
uint8_t* alloc(size_t size)
{
    return static_cast<uint8_t*>(std::malloc(size));
}

extern "C" __attribute__((visibility("default")))
void free_buffer(uint8_t* ptr, size_t /* size */)
{
    std::free(ptr);
}

extern "C" __attribute__((visibility("default")))
void filter(uint8_t* pixels, int width, int height)
{
    if (!pixels || width <= 0 || height <= 0)
        return;

    constexpr int blockSize = 16;
    const size_t pixelCount =
        static_cast<size_t>(width) * static_cast<size_t>(height);
    const std::vector<uint8_t> source(pixels, pixels + pixelCount * 4);

    for (int blockY = 0; blockY < height; blockY += blockSize)
    {
        for (int blockX = 0; blockX < width; blockX += blockSize)
        {
            const int endX = std::min(blockX + blockSize, width);
            const int endY = std::min(blockY + blockSize, height);

            uint64_t red = 0, green = 0, blue = 0, count = 0;

            for (int y = blockY; y < endY; ++y)
            {
                for (int x = blockX; x < endX; ++x)
                {
                    const size_t offset =
                        (static_cast<size_t>(y) * width + x) * 4;

                    red += source[offset];
                    green += source[offset + 1];
                    blue += source[offset + 2];
                    ++count;
                }
            }

            const uint8_t avgRed = static_cast<uint8_t>(red / count);
            const uint8_t avgGreen = static_cast<uint8_t>(green / count);
            const uint8_t avgBlue = static_cast<uint8_t>(blue / count);

            for (int y = blockY; y < endY; ++y)
            {
                for (int x = blockX; x < endX; ++x)
                {
                    const size_t offset =
                        (static_cast<size_t>(y) * width + x) * 4;

                    pixels[offset] = avgRed;
                    pixels[offset + 1] = avgGreen;
                    pixels[offset + 2] = avgBlue;
                    // Preserve this pixel's alpha.
                }
            }
        }
    }
}
