#pragma once
#include <windows.h>
#include <vulkan/vulkan.h>

#ifdef Renderer_EXPORTS
#define DLL_EXPORT __declspec(dllexport)
#else
#define DLL_EXPORT __declspec(dllimport)
#endif


const wchar_t* WINDOW_CLASS_NAME = L"VulkanWindowClass"; // Use wide string

DLL_EXPORT LRESULT CALLBACK WindowProc2(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
DLL_EXPORT HWND __stdcall CreateVulkanWindow(HINSTANCE hInstance, int width, int height, const wchar_t* title);
