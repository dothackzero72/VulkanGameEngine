#pragma once
#include "Material.h"
#include "GameTypeDef.h"

class MeshSystem;
class MaterialSystem
{
private:
public:
	UnorderedMap<RenderPassGuid, Material>                        MaterialList;

	MaterialSystem();
	~MaterialSystem();

	VkGuid LoadMaterial(const String& materialPath);
};
extern MaterialSystem materialSystem;
