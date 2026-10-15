#include <cstdint>

// pixels points to width * height * 4 bytes of RGBA data.
extern "C" __attribute__((visibility("default")))
void grayscale(uint8_t* pixels, int width, int height)
{
    if (!pixels || width <= 0 || height <= 0)
        return;

    const int pixelCount = width * height;

    for (int i = 0; i < pixelCount; ++i)
    {
        uint8_t* p = pixels + i * 4;

        // Integer approximation of luminance.
        const uint8_t gray =
            static_cast<uint8_t>((77 * p[0] + 150 * p[1] + 29 * p[2]) >> 8);

        p[0] = gray;
        p[1] = gray;
        p[2] = gray;
        // Leave p[3] (alpha) unchanged.
    }
}

