#pragma once
#include <CTypedef.h>

typedef struct
{
	void* MemoryBlock;
	uint32 NextIndex;
	uint32 ObjectSize;
	uint32 ObjectCount;
}CMemoryPool;

CMemoryPool* MemoryPool_CreateMemoryPool(uint32 objectSize, uint32 objectCount);