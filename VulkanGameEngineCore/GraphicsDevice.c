#include "GraphicsDevice.h"

LRESULT CALLBACK WindowProc2(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    switch (uMsg) 
    {
        case WM_DESTROY:
            PostQuitMessage(0);
            return 0;
        case WM_PAINT:
            return 0;
    }
    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

HWND __stdcall CreateVulkanWindow(HINSTANCE hInstance, int width, int height, const wchar_t* title)
{
    WNDCLASS wc = { 0 };
    wc.lpfnWndProc = WindowProc2;
    wc.hInstance = hInstance;
    wc.lpszClassName = WINDOW_CLASS_NAME;

    RegisterClass(&wc);
    HWND hwnd = CreateWindowEx(0, WINDOW_CLASS_NAME, title, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, width, height, NULL, NULL, hInstance, NULL);
    if (hwnd == NULL) 
    {
        return NULL;
    }
    ShowWindow(hwnd, SW_SHOW);
    return hwnd;
}