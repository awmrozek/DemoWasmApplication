#include <cstddef>
#include <cstdint>
#include <cstdlib>

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

    const int pixelCount = width * height;

    for (int i = 0; i < pixelCount; ++i)
    {
        uint8_t* p = pixels + i * 4;

        const uint8_t gray =
            static_cast<uint8_t>((77 * p[0] + 150 * p[1] + 29 * p[2]) >> 8);

        p[0] = gray;
        p[1] = gray;
        p[2] = gray;
        // Preserve alpha in p[3].
    }
}
