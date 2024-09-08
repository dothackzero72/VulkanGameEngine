#include "Camera.h"

class OrthographicCamera : public Camera
{
public:
	OrthographicCamera();
	OrthographicCamera(float width, float height);
	OrthographicCamera(const vec2& viewScreenSize);
	OrthographicCamera(const vec2& viewScreenSize, const vec2& position);
	virtual ~OrthographicCamera();

	virtual void Update(SceneDataBuffer& sceneProperties);
	virtual void UpdateKeyboard(float deltaTime) override;
	virtual void UpdateMouse() override;
};