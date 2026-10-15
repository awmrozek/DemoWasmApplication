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

    // 3 inputs (RGB), 4 hidden neurons, 3 outputs (RGB).
    constexpr float w1[4][3] = {
        { 1.8f, -0.6f,  0.4f},
        {-0.5f,  1.7f, -0.4f},
        { 0.3f, -0.7f,  1.9f},
        { 1.0f,  1.0f, -1.0f}
    };
    constexpr float b1[4] = {-0.3f, 0.1f, -0.2f, 0.0f};

    constexpr float w2[3][4] = {
        { 1.2f, -0.7f,  0.6f,  0.8f},
        {-0.6f,  1.2f,  0.5f, -0.7f},
        { 0.5f, -0.6f,  1.2f,  0.8f}
    };
    constexpr float b2[3] = {0.1f, -0.1f, 0.0f};

    const size_t count =
        static_cast<size_t>(width) * static_cast<size_t>(height);

    for (size_t i = 0; i < count; ++i)
    {
        const size_t p = i * 4;
        float input[3] = {
            pixels[p] / 127.5f - 1.0f,
            pixels[p + 1] / 127.5f - 1.0f,
            pixels[p + 2] / 127.5f - 1.0f
        };

        float hidden[4];
        for (int h = 0; h < 4; ++h)
        {
            float sum = b1[h];
            for (int c = 0; c < 3; ++c)
                sum += w1[h][c] * input[c];
            hidden[h] = std::tanh(sum);
        }

        for (int c = 0; c < 3; ++c)
        {
            float sum = b2[c];
            for (int h = 0; h < 4; ++h)
                sum += w2[c][h] * hidden[h];

            const float value = (std::tanh(sum) + 1.0f) * 127.5f;
            pixels[p + c] = static_cast<uint8_t>(
                std::clamp(static_cast<int>(value), 0, 255));
        }
        // Preserve alpha at pixels[p + 3].
    }
}
