#include <algorithm>
#include <cmath>
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
    const size_t byteCount = pixelCount * 4;

    const std::vector<uint8_t> source(pixels, pixels + byteCount);

    const float centerX = (width - 1) * 0.5f;
    const float centerY = (height - 1) * 0.5f;
    const float radius = std::min(centerX, centerY);

    if (radius <= 0.0f)
        return;

    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x)
        {
            const float dx = x - centerX;
            const float dy = y - centerY;
            const float distance = std::sqrt(dx * dx + dy * dy);
            const float normalizedDistance = distance / radius;

            int sourceX = x;
            int sourceY = y;

            if (normalizedDistance < 1.0f && distance > 0.0f)
            {
                // Map output radius to a more compressed source radius.
                const float sourceDistance =
                    radius * std::sin(normalizedDistance * 1.57079632679f);

                const float scale = sourceDistance / distance;
                sourceX = std::clamp(
                    static_cast<int>(std::lround(centerX + dx * scale)),
                    0, width - 1);
                sourceY = std::clamp(
                    static_cast<int>(std::lround(centerY + dy * scale)),
                    0, height - 1);
            }

            const size_t sourceOffset =
                (static_cast<size_t>(sourceY) * width + sourceX) * 4;
            const size_t outputOffset =
                (static_cast<size_t>(y) * width + x) * 4;

            const uint8_t gray = static_cast<uint8_t>(
                (77 * source[sourceOffset] +
                 150 * source[sourceOffset + 1] +
                 29 * source[sourceOffset + 2]) >> 8);

            pixels[outputOffset] = source[sourceOffset];
            pixels[outputOffset + 1] = source[sourceOffset + 1];
            pixels[outputOffset + 2] = source[sourceOffset + 2];
            pixels[outputOffset + 3] = source[sourceOffset + 3];
        }
    }
}