#pragma once
extern "C"
{
#include <Macro.h>
}
#include <iostream>
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

	//void UpdateMemoryPoolSize(List<SharedPtr<T>>& memoryPoolList)
	//{
	//	uint32 newObjectCount =  ObjectCount * 2;
	//	MemoryBlock* tempMemoryBlock = new MemoryBlock[ObjectSize * newObjectCount];
	//	memcpy(tempMemoryBlock, MemoryBlockPtr, ObjectSize * ObjectCount);

	//	for (int x = 0; x < ObjectCount; x++)
	//	{
	//		memoryPoolList[x].reset();

	//		uint8_t* address = reinterpret_cast<uint8_t*>(MemoryBlockPtr) + (x * sizeof(T));
	//		T* ptr = reinterpret_cast<T*>(address);
	//		memoryPoolList[x] = SharedPtr<T>(newObject, [this, memoryIndex](T* ptr)
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
		MemoryBlockPtr = new MemoryAddress[ObjectSize * ObjectCount];
	}

	SharedPtr<T> AllocateMemoryLocation()
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
		return SharedPtr<T>(newObject, [this, memoryIndex](T* ptr)
			{
				if (MemoryBlockInUse.size() > 0 &&
					MemoryBlockInUse[memoryIndex] == MemoryBlockUsed)
				{
					ptr->Destroy();
					MemoryBlockInUse[memoryIndex] = FreeMemoryBlock;
				}
			});
	}

	std::vector<T*> ViewMemoryPool()
	{
		std::vector<T*> memoryList;
		for (int x = 0; x < ObjectCount; x++)
		{
			MemoryAddress* address = MemoryBlockPtr + (x * sizeof(T));
			T* ptr = reinterpret_cast<T*>(address);
			memoryList.emplace_back(ptr);
		}
		return memoryList;
	}

	void ViewMemoryMap()
	{
		const auto memory = ViewMemoryPool();

		std::cout << "Memory Map of the " << typeid(T).name()  << " Memory Pool(" << sizeof(T) << " bytes each, " << std::to_string(sizeof(T) * memory.size()) << " bytes total) : " << std::endl;
		std::cout << std::setw(20) << "Address" << std::setw(15) << "Value" << std::endl;
		for (size_t x = 0; x < memory.size(); x++)
		{
			if (ViewMemoryBlockUsage()[x] == 1)
			{
				std::cout << std::setw(10) << std::hex << "0x" << &memory[x] << ": " << std::setw(15) << memory[x]->Name << std::endl;
			}
			else
			{
				std::cout << std::hex << "0x" << &memory[x] << ": " << "nullptr" << std::endl;
			}
		}
		std::cout << "" << std::endl << std::endl;
	}

	List<uint8_t> ViewMemoryBlockUsage()
	{
		return MemoryBlockInUse;
	}

	void Destroy()
	{
		for (int x = 0; x < ObjectCount; x++)
		{
			MemoryAddress* address = MemoryBlockPtr + (x * sizeof(T));
			T* ptr = reinterpret_cast<T*>(address);
			if (MemoryBlockInUse[x] == MemoryBlockUsed)
			{
				ptr->Destroy();
				ptr = nullptr;
				MemoryBlockInUse[x] = FreeMemoryBlock;
			}
		}
		delete[] MemoryBlockPtr;
		MemoryBlockPtr = nullptr;
		MemoryBlockInUse.clear();
	}
};
