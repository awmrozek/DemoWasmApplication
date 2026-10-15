	em++ grayscale.cpp \
  -O3 \
  -std=c++17 \
  -s STANDALONE_WASM=1 \
  -Wl,--no-entry \
  -Wl,--export=grayscale \
  -o grayscale.wasm

