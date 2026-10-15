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

    const size_t pixelCount =
        static_cast<size_t>(width) * static_cast<size_t>(height);
    const std::vector<uint8_t> source(pixels, pixels + pixelCount * 4);

    const int shift = std::max(4, width / 30);

    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x)
        {
            const int redX = std::clamp(x - shift, 0, width - 1);
            const int blueX = std::clamp(x + shift, 0, width - 1);

            const size_t output =
                (static_cast<size_t>(y) * width + x) * 4;
            const size_t redSource =
                (static_cast<size_t>(y) * width + redX) * 4;
            const size_t blueSource =
                (static_cast<size_t>(y) * width + blueX) * 4;

            pixels[output] = source[redSource];         // Red from left
            pixels[output + 1] = source[output + 1];   // Green stays put
            pixels[output + 2] = source[blueSource + 2]; // Blue from right
            // Preserve alpha at the output pixel.
        }
    }
}
