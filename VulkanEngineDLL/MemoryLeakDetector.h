#pragma once
#include "DLL.h"
#include "TypeDef.h"
#include <mutex>

struct MemoryLeakPtr
{
    void*  PtrAddress;
    size_t PtrElements;
    String DanglingPtrMessage;
};

extern "C" 
{
    DLL_EXPORT MemoryLeakPtr MemoryLeakPtr_NewPtr(size_t memorySize, size_t elementCount, const char* danglingPtrMessage);
    DLL_EXPORT void MemoryLeakPtr_DeletePtr(MemoryLeakPtr* ptr);
    DLL_EXPORT void MemoryLeakPtr_DanglingPtrMessage(MemoryLeakPtr* ptr);
}

class MemoryLeakSystem 
{
private:
    UnorderedMap<void*, MemoryLeakPtr> PtrAddressMap;
    std::mutex Mutex;

public:
    MemoryLeakSystem() 
    {
        #ifdef _DEBUG
            _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
        #endif
    }

    template <class T>
    T* AddPtrBuffer(size_t elementCount)
    {
        std::lock_guard<std::mutex> lock(Mutex);
        MemoryLeakPtr memoryLeakPtr = MemoryLeakPtr_NewPtr(sizeof(T) * elementCount, elementCount, "asdfads");
        PtrAddressMap[memoryLeakPtr.PtrAddress] = memoryLeakPtr;
        return reinterpret_cast<T*>(memoryLeakPtr.PtrAddress);
    }

    template <class T>
    void RemovePtrBuffer(T* ptr)
    {
        std::lock_guard<std::mutex> lock(Mutex);
        auto asdf = *ptr;
        MemoryLeakPtr* memoryLeakPtr = &PtrAddressMap[static_cast<void*>(ptr)];
        MemoryLeakPtr_DeletePtr(memoryLeakPtr);
        PtrAddressMap.erase(memoryLeakPtr->PtrAddress);
    }

    void ReportLeaks() 
    {
        std::lock_guard<std::mutex> lock(Mutex);
        if (!PtrAddressMap.empty()) 
        {
            fprintf(stderr, "Memory leaks detected in DLL:\n");
            for (auto& ptr : PtrAddressMap) 
            {
                MemoryLeakPtr_DanglingPtrMessage(&ptr.second);
            }
        }
    }
};
DLL_EXPORT MemoryLeakSystem memoryLeakSystem;