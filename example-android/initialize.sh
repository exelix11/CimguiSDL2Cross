#!/bin/sh

# Symlink cimgui to the build folder
ln -s $(pwd)/../Android.mk ./com.test/app/jni/CimguiSDL2Cross/
ln -s $(pwd)/../cimgui ./com.test/app/jni/CimguiSDL2Cross/

# Symlink the example code library
ln -s $(pwd)/../example ./com.test/app/jni/
# And compile it
pushd ./com.test/app/jni/example
chmod +x build_aot_android.sh.sh
# note that you need bflat in PATH for this
./build_aot_android.sh.sh
popd

# Fix gradlew permissions if needed
chmod +x com.test/gradlew

# Download reference SDL2 version
wget https://github.com/libsdl-org/SDL/releases/download/release-2.28.1/SDL2-2.28.1.tar.gz
tar xf SDL2-2.28.1.tar.gz

# Symlink SDL2 folders
ln -s $(pwd)/SDL2-2.28.1/src ./com.test/app/jni/SDL/
ln -s $(pwd)/SDL2-2.28.1/include ./com.test/app/jni/SDL/

# Example to integrate more libraries: Download SDL_Image
wget https://github.com/libsdl-org/SDL_image/releases/download/release-2.6.3/SDL2_image-2.6.3.tar.gz
tar xf SDL2_image-2.6.3.tar.gz

ln -s $(pwd)/SDL2_image-2.6.3 ./com.test/app/jni/SDL_Image