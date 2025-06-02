#pragma once
#include "Material.h"
#include "GameTypeDef.h"

class MeshSystem;
class MaterialSystem
{
private:
public:
	
	UnorderedMap<RenderPassGuid, Material>                        MaterialMap;

	MaterialSystem();
	~MaterialSystem();

	void Update(const float& deltaTime);
	VkGuid LoadMaterial(const String& materialPath);

	const Material& FindMaterial(const RenderPassGuid& guid);
	const Vector<Material>& MaterialList();
	const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();
};
extern MaterialSystem materialSystem;
