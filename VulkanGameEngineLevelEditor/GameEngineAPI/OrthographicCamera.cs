using GlmSharp;
using System.Numerics;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using vec2 = GlmSharp.vec2;

public class OrthographicCamera : Camera
{

    public OrthographicCamera()
    {
    }

    public OrthographicCamera(float width, float height)
    {
        Initialize(width, height, new vec3(0.0f));
    }

    public OrthographicCamera(vec2 viewScreenSize)
    {
        Initialize(viewScreenSize.x, viewScreenSize.y, new vec3(0.0f));
    }

    public OrthographicCamera(vec2 viewScreenSize, vec3 position)
    {
        Initialize(viewScreenSize.x, viewScreenSize.y, position);
    }

    private void Initialize(float width, float height, vec3 position)
    {
        Width = width;
        Height = height;
        AspectRatio = width / height;
        Zoom = 1.0f;

        Position = new vec3(position);
        ViewScreenSize = new vec2(width, height);
        ProjectionMatrix = mat4.Ortho(-AspectRatio * Zoom, AspectRatio * Zoom, -1.0f * Zoom, 1.0f * Zoom, -1.0f, 1.0f);
        ViewMatrix = mat4.Identity;
    }

    public override void Update(ref SceneDataBuffer sceneProperties)
    {
        mat4 transform = mat4.Translate(Position) * mat4.Rotate(VMath.DegreesToRadians(0.0f), new vec3(0, 0, 1));
        ViewMatrix = transform.Inverse;

        float Aspect = Width / Height;
        ProjectionMatrix = mat4.Ortho(-Aspect * Zoom, Aspect * Zoom, -1.0f * Zoom, 1.0f * Zoom, -10.0f, 10.0f);

        mat4 modifiedProjectionMatrix = ProjectionMatrix;
        modifiedProjectionMatrix[1, 1] *= -1;

        ViewScreenSize = new vec2((Aspect * Zoom) * 2, (1.0f * Zoom) * 2);

        sceneProperties.CameraPosition = new vec3(Position.x, Position.y, Position.z);
        sceneProperties.View = ViewMatrix;
        sceneProperties.Projection = modifiedProjectionMatrix;
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

        public static Matrix4x4 ToSystemNumerics(mat4 glmMatrix)
        {
            Matrix4x4 systemMatrix = new Matrix4x4(
                glmMatrix[0, 0], glmMatrix[1, 0], glmMatrix[2, 0], glmMatrix[3, 0],
                glmMatrix[0, 1], glmMatrix[1, 1], glmMatrix[2, 1], glmMatrix[3, 1],
                glmMatrix[0, 2], glmMatrix[1, 2], glmMatrix[2, 2], glmMatrix[3, 2],
                glmMatrix[0, 3], glmMatrix[1, 3], glmMatrix[2, 3], glmMatrix[3, 3]
            );

            return systemMatrix;
        }
    }
}