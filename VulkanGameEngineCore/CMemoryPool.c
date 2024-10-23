#include "CMemoryPool.h"
#include <corecrt_malloc.h>

CMemoryPool* MemoryPool_CreateMemoryPool(uint32 objectSize, uint32 objectCount)
{
	CMemoryPool* memoryPool = (CMemoryPool*)malloc(sizeof(CMemoryPool));
	memoryPool->MemoryBlock = malloc(objectSize * objectCount);
	memoryPool->NextIndex = 0;
	memoryPool->ObjectSize = objectSize;
	memoryPool->ObjectCount = objectCount;
	return memoryPool;
}