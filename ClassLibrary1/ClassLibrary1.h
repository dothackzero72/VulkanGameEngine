#pragma once

extern "C" __declspec(dllexport) void CallSayHello();
extern "C" __declspec(dllimport) int Add(int a, int b);
extern "C" __declspec(dllimport) int Multiply(int a, int b);