#include "MemoryLeakDetector.h"
#include <iostream>

MemoryLeakSystem memoryLeakSystem = MemoryLeakSystem();

extern "C" 
{
    MemoryLeakPtr MemoryLeakPtr_NewPtr(size_t memorySize, size_t elementCount, const char* danglingPtrMessage) {
        byte* memory = nullptr;
        try 
        {
#ifdef _DEBUG
            memory = new(_NORMAL_BLOCK, __FILE__, __LINE__) byte[memorySize];
#else
            memory = new byte[memorySize];
#endif
        }
        catch (const std::bad_alloc&) {
            fprintf(stderr, "Allocation failed: %s\n", danglingPtrMessage ? danglingPtrMessage : "Unknown");
            return { nullptr, 0, "" };
        }
        MemoryLeakPtr ptr = { memory, elementCount, danglingPtrMessage ? danglingPtrMessage : "" };
        return ptr;
    }

    void MemoryLeakPtr_DeletePtr(MemoryLeakPtr* ptr)
    {
        if (ptr) 
        {
            if (ptr->PtrElements > 1)
            {
                delete[] static_cast<byte*>(ptr->PtrAddress);
                ptr = nullptr;
            }
            else
            {
                delete static_cast<byte*>(ptr->PtrAddress);
                ptr = nullptr;
            }
        }
    }

    void MemoryLeakPtr_DanglingPtrMessage(MemoryLeakPtr* ptr) 
    {
        if (ptr) 
        {
            HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hConsole != INVALID_HANDLE_VALUE) 
            {
                CONSOLE_SCREEN_BUFFER_INFO consoleInfo;
                GetConsoleScreenBufferInfo(hConsole, &consoleInfo);
                WORD originalAttributes = consoleInfo.wAttributes;

                SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
                fprintf(stdout, "ERROR: ");
                SetConsoleTextAttribute(hConsole, originalAttributes);
                fprintf(stdout, "%s\n", ptr->DanglingPtrMessage);
            }
        }
    }

    void MemoryLeakPtr_ReportLeaks() 
    {
        memoryLeakSystem.ReportLeaks();
        #ifdef _DEBUG
        _CrtDumpMemoryLeaks();
        #endif
    }
}
