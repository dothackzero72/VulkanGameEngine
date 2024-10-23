#include "EngineManager.h"

//ComponentMemoryPoolManager EngineManager::componentMemoryPoolManager;
//GameObjectMemoryPoolManager EngineManager::gameObjectMemoryPoolManager;
//List<std::shared_ptr<GameObject>>  EngineManager::GameObjectList;
//
//void EngineManager::StartUp()
//{
//	componentMemoryPoolManager.StartUp(30);
//	gameObjectMemoryPoolManager.StartUp(30);
//}
//
//void EngineManager::CreateGameObject(List<ConponentEnum> conponentList)
//{
//	std::shared_ptr<GameObject> gameObject = gameObjectMemoryPoolManager.CreateGameObject();
//	for (ConponentEnum conponent : conponentList)
//	{
//		std::shared_ptr<GameObjectComponent> gameObjectConponent = nullptr;
//		switch (conponent)
//		{
//		case kRenderMesh2DComponent: gameObjectConponent = componentMemoryPoolManager.CreateRenderMesh2DRendererComponent(); break;
//		}
//		gameObject->AddComponent(gameObjectConponent);
//	}
//	GameObjectList.emplace_back(gameObject);
//}
