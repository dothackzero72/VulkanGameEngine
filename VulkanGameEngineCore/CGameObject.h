//#pragma once
//#include "ctype.h"
//
//#ifdef __cplusplus
//extern "C" {
//#endif
//
//
//typedef struct GameObjectComponent
//{
//	char Name[30];
//	size_t MemorySize;
//};
//
//typedef struct GameObject
//{
//	char Name[30];
//	struct GameObjectComponent* GameObjectComponentList;
//};
//
//void GameObject_AllocateComponentMemory(struct GameObject* gameObject)
//{
//	size_t MemorySize = 0;
//	for (int x = 0; x < sizeof(gameObject->GameObjectComponentList); x++)
//	{
//		MemorySize += gameObject->GameObjectComponentList->MemorySize;
//	}
//	void* memoryLocation = malloc();
//}
//
//#ifdef __cplusplus
//}
//#endif