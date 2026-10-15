em++ grayscale.cpp \
  -O3 \
  -std=c++17 \
  -s STANDALONE_WASM=1 \
  -Wl,--no-entry \
  -Wl,--export=alloc \
  -Wl,--export=free_buffer \
  -Wl,--export=filter \
  -Wl,--export-memory \
  -o grayscale.wasm
