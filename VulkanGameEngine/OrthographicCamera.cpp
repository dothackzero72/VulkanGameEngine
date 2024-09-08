#include "OrthographicCamera.h"
#include "SceneDataBuffer.h"

OrthographicCamera::OrthographicCamera()
{

}

OrthographicCamera::OrthographicCamera(float width, float height)
{
	Width = width;
	Height = height;
	AspectRatio = width / height;
	Zoom = 1.0f;

	Position = vec3(0.0f);
	ViewScreenSize = vec2(width, height);
	ProjectionMatrix = glm::ortho(-AspectRatio * Zoom, AspectRatio * Zoom, -1.0f * Zoom, 1.0f * Zoom, -1.0f, 1.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera::OrthographicCamera(const vec2& viewScreenSize)
{
	Width = viewScreenSize.x;
	Height = viewScreenSize.y;
	AspectRatio = viewScreenSize.x / viewScreenSize.y;
	Zoom = 1.0f;

	Position = vec3(0.0f);
	ViewScreenSize = viewScreenSize;
	ProjectionMatrix = glm::ortho(-AspectRatio * Zoom, AspectRatio * Zoom, -1.0f * Zoom, 1.0f * Zoom, -1.0f, 1.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera::OrthographicCamera(const vec2& viewScreenSize, const vec2& position)
{
	Width = viewScreenSize.x;
	Height = viewScreenSize.y;
	AspectRatio = viewScreenSize.x / viewScreenSize.y;
	Zoom = 1.0f;

	Position = glm::vec3(position, 0.0f);
	ViewScreenSize = viewScreenSize;
	ProjectionMatrix = glm::ortho(-AspectRatio * Zoom, AspectRatio * Zoom, -1.0f * Zoom, 1.0f * Zoom, -1.0f, 1.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera::~OrthographicCamera()
{

}

void OrthographicCamera::Update(SceneDataBuffer& sceneProperties)
{
	mat4 transform = glm::translate(mat4(1.0f), Position) * rotate(mat4(1.0f), glm::radians(0.0f), vec3(0, 0, 1));
	ViewMatrix = glm::inverse(transform);

	const auto 	Aspect = Width / Height;
	ProjectionMatrix = glm::ortho(-Aspect * Zoom, Aspect * Zoom, -1.0f * Zoom, 1.0f * Zoom, -10.0f, 10.0f);
	ProjectionMatrix[1][1] *= -1;

	ViewScreenSize = vec2((Aspect * Zoom) * 2, (1.0f * Zoom) * 2);

	sceneProperties.CameraPosition = Position;
	sceneProperties.View = ViewMatrix;
	sceneProperties.Projection = ProjectionMatrix;
}

void OrthographicCamera::UpdateKeyboard(float deltaTime)
{

}

void OrthographicCamera::UpdateMouse()
{

}