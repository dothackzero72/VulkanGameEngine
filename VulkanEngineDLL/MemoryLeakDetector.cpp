#include "MemoryLeakDetector.h"
#include <iostream>

MemorySystem memorySystem = MemorySystem();

extern "C" 
{
    MemoryLeakPtr MemoryLeakPtr_NewPtr(size_t memorySize, size_t elementCount, int block, const char* file, int line, const char* danglingPtrMessage)
    {
        void* memory = nullptr;
        try
        {
#ifdef _DEBUG
            memory = new(_NORMAL_BLOCK, __FILE__, __LINE__) byte[memorySize];
#else
            memory = new byte[memorySize];
#endif
        }
        catch (const std::bad_alloc&)
        {
            fprintf(stderr, "Allocation failed: %s\n", danglingPtrMessage ? danglingPtrMessage : "Unknown");
            return MemoryLeakPtr
            {
                .PtrAddress = nullptr,
                .PtrElements = 0,
                .isArray = false,
                .DanglingPtrMessage = ""
            };
        }

        return MemoryLeakPtr
        {
            .PtrAddress = memory,
            .PtrElements = elementCount,
            .isArray = elementCount > 1,
            .DanglingPtrMessage = danglingPtrMessage ? danglingPtrMessage : ""
        };
    }

    void MemoryLeakPtr_DeletePtr(MemoryLeakPtr* memoryLeakPtr)
    {
        if (memoryLeakPtr && memoryLeakPtr->PtrAddress)
        {
            if (memoryLeakPtr->isArray && 
                memoryLeakPtr->PtrElements > 1)
            {
                delete[] static_cast<void*>(memoryLeakPtr->PtrAddress);
            }
            else
            {
                delete static_cast<void*>(memoryLeakPtr->PtrAddress);
            }
            memoryLeakPtr->PtrAddress = nullptr;
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
        memorySystem.ReportLeaks();
        #ifdef _DEBUG
        _CrtDumpMemoryLeaks();
        #endif
    }
}
