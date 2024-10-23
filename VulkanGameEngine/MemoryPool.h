#pragma once
extern "C"
{
#include <CMemoryPool.h>
#include <Macro.h>
}
#include "Typedef.h"
#include <limits>

template <class T>
class MemoryPool
{
private:
	const uint32 FailedToFind = -1;
	const uint8_t MemoryBlockUsed = 1;
	const uint8_t FreeMemoryBlock = 0;

	MemoryBlock* MemoryBlockPtr = nullptr;
	size_t ObjectSize = 0;
	uint32 ObjectCount = 0;
	List<uint8_t> MemoryBlockInUse;

	uint32 FindNextFreeMemoryBlockIndex() 
	{
		auto itr = std::find(MemoryBlockInUse.begin(), MemoryBlockInUse.end(), FreeMemoryBlock);
		if (itr != MemoryBlockInUse.end()) {
			return std::distance(MemoryBlockInUse.begin(), itr);
		}
		return FailedToFind;
	}

	uint32 FindBlockIndexFromMemory(void* memoryLocation) 
	{
		void* baseAddress = MemoryBlockPtr;
		void* targetAddress = memoryLocation;

		if (targetAddress < baseAddress || 
			targetAddress >= baseAddress + (ObjectSize * ObjectCount)) {
			return FailedToFind;
		}

		uint32 blockIndex = (targetAddress - baseAddress) / ObjectSize;
		return blockIndex < ObjectCount ? blockIndex : FailedToFind;
	}

	//void UpdateMemoryPoolSize(List<std::shared_ptr<T>>& memoryPoolList)
	//{
	//	uint32 newObjectCount =  ObjectCount * 2;
	//	MemoryBlock* tempMemoryBlock = new MemoryBlock[ObjectSize * newObjectCount];
	//	memcpy(tempMemoryBlock, MemoryBlockPtr, ObjectSize * ObjectCount);

	//	for (int x = 0; x < ObjectCount; x++)
	//	{
	//		memoryPoolList[x].reset();

	//		uint8_t* address = reinterpret_cast<uint8_t*>(MemoryBlockPtr) + (x * sizeof(T));
	//		T* ptr = reinterpret_cast<T*>(address);
	//		memoryPoolList[x] = std::shared_ptr<T>(newObject, [this, memoryIndex](T* ptr)
	//			{
	//				ptr->~T();
	//				MemoryBlockInUse[memoryIndex] = false;
	//			})
	//	}
	//	delete MemoryBlockPtr;

	//	ObjectCount = newObjectCount;
	//	MemoryBlockPtr = tempMemoryBlock;
	//}

public:
	MemoryPool()
	{

	}

	~MemoryPool()
	{
	}

	void CreateMemoryPool(uint objectCount)
	{
		MemoryBlockInUse.resize(objectCount, FreeMemoryBlock);
		ObjectSize = sizeof(T);
		ObjectCount = objectCount;
		MemoryBlockPtr = new uint8_t[ObjectSize * ObjectCount];
	}

	std::shared_ptr<T> AllocateMemoryLocation()
	{
		MemoryBlock* memoryAddress = MemoryBlockPtr;

		uint32 memoryIndex = FindNextFreeMemoryBlockIndex();
		if (memoryIndex == FailedToFind)
		{
			/*UpdateMemoryPoolSize();
			memoryIndex = FindNextFreeMemoryBlockIndex();*/
		}

		size_t offset = memoryIndex * sizeof(T);
		T* newObject = new (memoryAddress + offset) T();

		MemoryBlockInUse[memoryIndex] = MemoryBlockUsed;
		return std::shared_ptr<T>(newObject, [this, memoryIndex](T* ptr)
			{
				ptr->~T();
				MemoryBlockInUse[memoryIndex] = FreeMemoryBlock;
			});
	}

	std::vector<T*> ViewMemoryPool()
	{
		std::vector<T*> memoryList;
		for (int x = 0; x < ObjectCount; x++)
		{
			uint8_t* address = MemoryBlockPtr + (x * sizeof(T));
			T* ptr = reinterpret_cast<T*>(address);
			memoryList.emplace_back(ptr);
		}
		return memoryList;
	}

	void Destroy()
	{
		delete[] MemoryBlockPtr;
		MemoryBlockPtr = nullptr;
		MemoryBlockInUse.clear();
	}
};

