using GlmSharp;
using System.Numerics;
using VulkanGameEngineLevelEditor.GameEngineAPI;

public class OrthographicCamera : Camera
{

    public OrthographicCamera()
    {
        // Default constructor
    }

    public OrthographicCamera(float width, float height)
    {
        Initialize(width, height, new Vector2(0.0f));
    }

    public OrthographicCamera(Vector2 viewScreenSize)
    {
        Initialize(viewScreenSize.X, viewScreenSize.Y, new Vector2(0.0f));
    }

    public OrthographicCamera(Vector2 viewScreenSize, Vector2 position)
    {
        Initialize(viewScreenSize.X, viewScreenSize.Y, position);
    }

    private void Initialize(float width, float height, Vector2 position)
    {
        Width = width;
        Height = height;
        AspectRatio = width / height;
        Zoom = 1.0f;

        Position = new Vector3(position, 0.0f);
        ViewScreenSize = new Vector2(width, height);
        UpdateProjectionMatrix();
        ViewMatrix = Matrix4x4.Identity;
    }

    public override void Update(SceneDataBuffer sceneProperties)
    {
        var transform = Matrix4x4.CreateTranslation(Position) * Matrix4x4.CreateRotationZ(0.0f);
        Matrix4x4.Invert(transform, out Matrix4x4 inverseTransform);
        ViewMatrix = inverseTransform;

        UpdateProjectionMatrix();

        sceneProperties.CameraPosition = new vec3(Position.X, Position.Y, Position.Z);
        sceneProperties.View = MatrixConverter.ToGLM(ViewMatrix);
        sceneProperties.Projection = MatrixConverter.ToGLM(ProjectionMatrix);
    }

    private void UpdateProjectionMatrix()
    {
        var projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(
                   -AspectRatio * Zoom,
                   AspectRatio * Zoom,
                   -1.0f * Zoom,
                   1.0f * Zoom,
                   -10.0f,
                   10.0f);

        projectionMatrix.M21 *= -1;
        ProjectionMatrix = projectionMatrix;
        ViewScreenSize = new Vector2((AspectRatio * Zoom) * 2, (1.0f * Zoom) * 2);
    }

    public override void UpdateKeyboard(float deltaTime)
    {
    }

    public override void UpdateMouse()
    {
    }

    public static class MatrixConverter
    {
        public static mat4 ToGLM(Matrix4x4 systemMatrix)
        {
            mat4 glmMatrix = new mat4();

            glmMatrix[0, 0] = systemMatrix.M11;
            glmMatrix[0, 1] = systemMatrix.M12;
            glmMatrix[0, 2] = systemMatrix.M13;
            glmMatrix[0, 3] = systemMatrix.M14;

            glmMatrix[1, 0] = systemMatrix.M21;
            glmMatrix[1, 1] = systemMatrix.M22;
            glmMatrix[1, 2] = systemMatrix.M23;
            glmMatrix[1, 3] = systemMatrix.M24;

            glmMatrix[2, 0] = systemMatrix.M31;
            glmMatrix[2, 1] = systemMatrix.M32;
            glmMatrix[2, 2] = systemMatrix.M33;
            glmMatrix[2, 3] = systemMatrix.M34;

            glmMatrix[3, 0] = systemMatrix.M41;
            glmMatrix[3, 1] = systemMatrix.M42;
            glmMatrix[3, 2] = systemMatrix.M43;
            glmMatrix[3, 3] = systemMatrix.M44;

            return glmMatrix;
        }
    }
}