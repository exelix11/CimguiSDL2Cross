# Android builds
Building for android, as expected, is annoying as hell.

This is the android app template from SDL2, to build you will need to compile SDL2 with the ndk. The `initialize.sh` script will download and symlink the needed sources.

In practice this is just an example on how to configure everything, to use newer SDL versions you should get the SDL source and make a new android project from scratch with their bootstrapper script and only copy over the following folders:
- `jni/src` : contains the main() wrapper
- `jni/cimgui` : our custom cimgui build
- `jni/example` : our compied C# project

Once SDL sources are in place we use [bflat](https://github.com/bflattened/bflat) to make a shared library version of our example and copy it to the ndk-make folder. Once this is done you just need to run `./gradlew build` to get an APK

Note that the gradle build does not recompile the C# library but just the APK from the precompiled .so libs 
