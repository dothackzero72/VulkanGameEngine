#include "OrthographicCamera2D.h"
#include "SceneDataBuffer.h"

OrthographicCamera2D::OrthographicCamera2D()
{

}

OrthographicCamera2D::OrthographicCamera2D(float width, float height)
{
	Width = width;
	Height = height;
	AspectRatio = width / height;
	Zoom = 1.0f;

	Position = vec3(0.0f);
	ViewScreenSize = vec2(width, height);
	ProjectionMatrix = glm::ortho(0.0f, Width, Height, 0.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera2D::OrthographicCamera2D(const vec2& viewScreenSize)
{
	Width = viewScreenSize.x;
	Height = viewScreenSize.y;
	AspectRatio = viewScreenSize.x / viewScreenSize.y;
	Zoom = 1.0f;

	Position = vec3(0.0f);
	ViewScreenSize = viewScreenSize;
	ProjectionMatrix = glm::ortho(0.0f, Width, Height, 0.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera2D::OrthographicCamera2D(const vec2& viewScreenSize, const vec2& position)
{
	Width = viewScreenSize.x;
	Height = viewScreenSize.y;
	AspectRatio = viewScreenSize.x / viewScreenSize.y;
	Zoom = 1.0f;

	Position = glm::vec3(position, 0.0f);
	ViewScreenSize = viewScreenSize;
	ProjectionMatrix = glm::ortho(0.0f, Width, Height, 0.0f);
	ViewMatrix = mat4(1.0f);
}

OrthographicCamera2D::~OrthographicCamera2D()
{

}

void OrthographicCamera2D::Update(SceneDataBuffer& sceneProperties)
{
	ProjectionMatrix = glm::ortho(0.0f, Width, Height, 0.0f);

	sceneProperties.CameraPosition = Position;
	sceneProperties.View = mat4(1.0f);
	sceneProperties.Projection = ProjectionMatrix;
}

void OrthographicCamera2D::UpdateKeyboard(float deltaTime)
{

}

void OrthographicCamera2D::UpdateMouse()
{

}