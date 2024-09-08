#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "VulkanRenderer.h"
#include "Typedef.h"
#include "SceneDataBuffer.h"

class Camera
{
protected:
	float Width;
	float Height;
	float AspectRatio;
	float Zoom;

	vec2 ViewScreenSize;
	mat4 ProjectionMatrix;
	mat4 ViewMatrix;

public:
	vec3 Position;

	Camera();
	virtual ~Camera();

	virtual void Update(SceneDataBuffer& sceneProperties) = 0;
	virtual void UpdateKeyboard(float deltaTime) = 0;
	virtual void UpdateMouse() = 0;

	const vec2& GetViewScreenSize() { return ViewScreenSize; }
	const mat4& GetProjectionMatrix() { return ProjectionMatrix; }
	const mat4& GetViewMatrix() { return ViewMatrix; }
	const vec3& GetPosition() { return Position; }
	const float GetZoom() { return Zoom; }
};