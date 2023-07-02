#!/bin/sh
# This uses bflat to cross-compile the android binary
# Unfortunately bflat doesn't seem to support x64 bionic libc so for now only arm64 is supported
# https://github.com/bflattened/bflat
bflat build --os:linux --arch:arm64 --libc:bionic --no-reflection --no-globalization -d ANDROID_LIB --ldflags "-soname libexample.so"