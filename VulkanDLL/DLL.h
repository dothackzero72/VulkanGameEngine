#pragma once
#include <Windows.h>

#ifdef VulkanEngine_DLL
#define DLL_EXPORT extern "C" __declspec(dllexport)
#else
#define DLL_EXPORT __declspec(dllimport)
#endif

typedef void (*TextCallback)(const char*);
typedef void (*RichTextBoxCallback)(const char*);