#pragma once
#include "Typedef.h"

static struct ScenePropertiesBuffer
{
	alignas(4)  uint32  MeshBufferIndex = -1;
	alignas(16) mat4 Projection = mat4(1.0f);
	alignas(16) mat4 View = mat4(1.0f);
	alignas(16) vec3 CameraPosition = vec3(0.0f);
} scenePropertiesBuffer;

enum ComponentTypeEnum
{
	kUndefined,
	kRenderMesh2DComponent,
	kTransform2DComponent,
	kInputComponent,
	kSpriteComponent,
	kTransform3DComponent
};
