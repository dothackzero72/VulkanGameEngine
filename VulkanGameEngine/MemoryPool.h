//Probably tried to optimize too early.
//Keeping because I'll probably need it later on.

//#pragma once
//extern "C"
//{
//#include <Macro.h>
//}
//#include <iostream>
//#include "Typedef.h"
//#include <limits>
//
//template <class T>
//class MemoryPool
//{
//private:
//	const uint32 FailedToFind = -1;
//	const uint8_t MemoryBlockUsed = 1;
//	const uint8_t FreeMemoryBlock = 0;
//
//	uint8_t* MemoryBlockPtr = nullptr;
//	size_t ObjectSize = sizeof(T);
//	uint32 ObjectCount = 0;
//	List<uint8_t> MemoryBlockInUse;
//
//	uint32 FindNextFreeMemoryBlockIndex()
//	{
//		auto itr = std::find(MemoryBlockInUse.begin(), MemoryBlockInUse.end(), FreeMemoryBlock);
//		if (itr != MemoryBlockInUse.end()) {
//			return std::distance(MemoryBlockInUse.begin(), itr);
//		}
//		return FailedToFind;
//	}
//
//	//void UpdateMemoryPoolSize(List<SharedPtr<T>>& memoryPoolList)
//	//{
//	//	uint32 newObjectCount =  ObjectCount * 2;
//	//	MemoryBlock* tempMemoryBlock = new MemoryBlock[ObjectSize * newObjectCount];
//	//	memcpy(tempMemoryBlock, MemoryBlockPtr, ObjectSize * ObjectCount);
//
//	//	for (int x = 0; x < ObjectCount; x++)
//	//	{
//	//		memoryPoolList[x].reset();
//
//	//		uint8_t* address = reinterpret_cast<uint8_t*>(MemoryBlockPtr) + (x * sizeof(T));
//	//		T* ptr = reinterpret_cast<T*>(address);
//	//		memoryPoolList[x] = SharedPtr<T>(newObject, [this, memoryIndex](T* ptr)
//	//			{
//	//				ptr->~T();
//	//				MemoryBlockInUse[memoryIndex] = false;
//	//			})
//	//	}
//	//	delete MemoryBlockPtr;
//
//	//	ObjectCount = newObjectCount;
//	//	MemoryBlockPtr = tempMemoryBlock;
//	//}
//
//public:
//
//	MemoryPool()
//	{
//
//	}
//
//	~MemoryPool()
//	{
//	}
//
//	void CreateMemoryPool(uint32 objectCount)
//	{
//		MemoryBlockInUse.resize(objectCount, FreeMemoryBlock);
//		ObjectCount = objectCount;
//		MemoryBlockPtr = new uint8_t[ObjectSize * ObjectCount];
//	}
//
//	SharedPtr<T> AllocateMemoryLocation()
//	{
//		uint32 memoryIndex = FindNextFreeMemoryBlockIndex();
//		if (memoryIndex == FailedToFind)
//		{
//			throw std::runtime_error("No free memory block available.");
//		}
//
//		T* newObject = new (MemoryBlockPtr + (memoryIndex * ObjectSize)) T(); 
//		SharedPtr<T> newPtr(newObject);
//
//		MemoryBlockInUse[memoryIndex] = MemoryBlockUsed;
//		return newPtr;
//	}
//
//	List<T*> ViewMemoryPool()
//	{
//		List<T*> memoryList;
//		for (uint32 x = 0; x < ObjectCount; x++)
//		{
//			T* ptr = reinterpret_cast<T*>(MemoryBlockPtr + (x * ObjectSize));
//			memoryList.emplace_back(ptr);
//		}
//		return memoryList;
//	}
//
//	void ViewMemoryMap()
//	{
//		const auto memory = ViewMemoryPool();
//		std::cout << "Memory Map of the " << typeid(T).name() << " Memory Pool(" << sizeof(T) << " bytes each, "
//			<< std::to_string(sizeof(T) * memory.size()) << " bytes total) : " << std::endl;
//		std::cout << std::setw(20) << "Address" << std::setw(15) << "Value" << std::endl;
//
//		for (size_t x = 0; x < memory.size(); x++)
//		{
//			std::cout << std::hex << "0x" << reinterpret_cast<void*>(memory[x]) << ": "
//				<< (MemoryBlockInUse[x] == MemoryBlockUsed ? memory[x]->Name : "nullptr") << std::endl;
//		}
//
//		std::cout << std::endl << std::endl;
//	}
//
//	const List<uint8_t> ViewMemoryBlockUsage()
//	{
//		return MemoryBlockInUse;
//	}
//
//	void DestroyObject(int memoryIndex)
//	{
//		if (MemoryBlockInUse[memoryIndex])
//		{
//			reinterpret_cast<T*>(MemoryBlockPtr + (memoryIndex * ObjectSize))->Destroy();
//			MemoryBlockInUse = 0;
//		}
//	}
//
//	void Destroy()
//	{
//		for (int x = 0; x < MemoryBlockInUse.size(); x++)
//		{
//			if (MemoryBlockInUse[x])
//			{
//				reinterpret_cast<T*>(MemoryBlockPtr + (x * ObjectSize))->Destroy();
//			}
//		}
//
//		if (MemoryBlockPtr)
//		{
//			delete[] MemoryBlockPtr;
//			MemoryBlockPtr = nullptr;
//		}
//		MemoryBlockInUse.clear();
//	}
//};
