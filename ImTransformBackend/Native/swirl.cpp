#include <cstddef>
#include <cstdint>
#include <cstdlib>
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

    // Keep the original image intact while remapping pixels.
    const std::vector<uint8_t> source(pixels, pixels + byteCount);

    const float centerX = (width - 1) * 0.5f;
    const float centerY = (height - 1) * 0.5f;
    const float radius = std::min(centerX, centerY);

    if (radius <= 0.0f)
        return;

    constexpr float maxAngle = 4.0f; // Radians; increase for a stronger swirl.

    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x)
        {
            const float dx = x - centerX;
            const float dy = y - centerY;
            const float distance = std::sqrt(dx * dx + dy * dy);

            int sourceX = x;
            int sourceY = y;

            if (distance < radius)
            {
                const float falloff = 1.0f - distance / radius;
                const float angle = maxAngle * falloff * falloff;

                const float cosAngle = std::cos(angle);
                const float sinAngle = std::sin(angle);

                // Inverse-map the output coordinate into the source image.
                const float sx = centerX + dx * cosAngle - dy * sinAngle;
                const float sy = centerY + dx * sinAngle + dy * cosAngle;

                sourceX = std::clamp(
                    static_cast<int>(std::lround(sx)), 0, width - 1);
                sourceY = std::clamp(
                    static_cast<int>(std::lround(sy)), 0, height - 1);
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

