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

    for (size_t i = 0; i < pixelCount; ++i)
    {
        const size_t offset = i * 4;

        pixels[offset] = static_cast<uint8_t>(
            std::clamp(static_cast<int>(pixels[offset] * 0.92f), 0, 255));

        pixels[offset + 1] = static_cast<uint8_t>(
            std::clamp(static_cast<int>(pixels[offset + 1] * 0.98f), 0, 255));

        pixels[offset + 2] = static_cast<uint8_t>(
            std::clamp(static_cast<int>(pixels[offset + 2] * 1.08f + 5.0f), 0, 255));

        // Preserve alpha in pixels[offset + 3].
    }
}
