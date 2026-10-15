#include <cstddef>
#include <cstdint>
#include <cstdlib>
#include <algorithm>
#include <cstdint>
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

    for (size_t i = 0; i < pixelCount; ++i)
    {
        const size_t offset = i * 4;
        const int r = source[offset];
        const int g = source[offset + 1];
        const int b = source[offset + 2];

        // Warm amber tone mixed with the original color.
        pixels[offset]     = static_cast<uint8_t>(std::min(255, (110 * r + 20 * g + 10 * b) / 100 + 8));
        pixels[offset + 1] = static_cast<uint8_t>(std::min(255, (10 * r + 105 * g + 5 * b) / 100 + 3));
        pixels[offset + 2] = static_cast<uint8_t>(std::min(255, (5 * r + 15 * g + 95 * b) / 100));

        // Preserve alpha.
        pixels[offset + 3] = source[offset + 3];
    }
}
