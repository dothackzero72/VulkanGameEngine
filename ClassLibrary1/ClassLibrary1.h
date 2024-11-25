#pragma once

#ifdef MY_DLL_API
#define MY_DLL_API __declspec(dllexport)
#else
#define MY_DLL_API __declspec(dllimport)
#endif

extern "C" {
    MY_DLL_API void CallSayHello();
    MY_DLL_API int Add(int a, int b);
    MY_DLL_API int Multiply(int a, int b);
}