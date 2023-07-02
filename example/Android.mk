LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE := example-prebuilt

LOCAL_SRC_FILES := libexample.so

LOCAL_SHARED_LIBRARIES := SDL2 SDL2_image cimgui

include $(PREBUILT_SHARED_LIBRARY)
