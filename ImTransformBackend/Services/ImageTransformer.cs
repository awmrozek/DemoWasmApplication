using Wasmtime;

namespace ImTransformBackend.Services
{
    public static class ImageTransformer
    {
        public static void TransformImage(
            byte[] pixels,
            int width,
            int height,
            string wasmFile)
        {
            ArgumentNullException.ThrowIfNull(pixels);
            ArgumentException.ThrowIfNullOrWhiteSpace(wasmFile);

            if (width <= 0 || height <= 0)
                return;

            int requiredLength = checked(width * height * 4);
            if (pixels.Length < requiredLength)
                throw new ArgumentException(
                    "Pixel buffer is too small.",
                    nameof(pixels));

            using var engine = new Engine();
            using var module = Module.FromFile(engine, wasmFile);
            using var linker = new Linker(engine);
            using var store = new Store(engine);

            var instance = linker.Instantiate(store, module);

            var memory = instance.GetMemory("memory")
                ?? throw new InvalidOperationException(
                    "Wasm module does not export memory named 'memory'.");

            var alloc = instance.GetFunction<int, int>("alloc")
                ?? throw new InvalidOperationException(
                    "Wasm module does not export alloc(int) -> int.");

            var free = instance.GetAction<int, int>("free_buffer")
                ?? throw new InvalidOperationException(
                    "Wasm module does not export free_buffer(int, int).");

            var filter = instance.GetAction<int, int, int>("filter")
                ?? throw new InvalidOperationException(
                    "Wasm module does not export filter(int, int, int).");

            int address = alloc(requiredLength);
            if (address <= 0)
                throw new InvalidOperationException(
                    "The wasm module could not allocate a pixel buffer.");

            try
            {
                pixels.AsSpan(0, requiredLength)
                    .CopyTo(memory.GetSpan<byte>(address).Slice(0, requiredLength));

                filter(address, width, height);

                memory.GetSpan<byte>(address)
                    .Slice(0, requiredLength)
                    .CopyTo(pixels.AsSpan(0, requiredLength));
            }
            finally
            {
                free(address, requiredLength);
            }
        }
    }
}
