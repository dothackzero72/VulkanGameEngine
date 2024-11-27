#pragma once
#include <Windows.h>
#define DLL_EXPORT_UNMANAGED extern "C" __declspec(dllexport)
#define DLL_EXPORT_MANAGED __declspec(dllexport)