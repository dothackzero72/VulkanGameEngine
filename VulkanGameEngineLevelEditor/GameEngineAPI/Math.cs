namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class VMath
    {
        private const float PI = 3.14159265358979323846f;

        public static float DegreesToRadians(float degrees)
        {
            return degrees * (PI / 180.0f);
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * (180.0f / PI);
        }
    }
}
