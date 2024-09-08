#pragma once
#include "Typedef.h"

struct SceneDataBuffer
{
	alignas(16) mat4 Projection = mat4(1.0f);
	alignas(16) mat4 View = mat4(1.0f);
	alignas(16) vec3 CameraPosition = vec3(0.0f);
};
