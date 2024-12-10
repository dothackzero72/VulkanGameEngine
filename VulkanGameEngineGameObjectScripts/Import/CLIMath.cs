using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineGameObjectScripts.Import
{
    public class CLIMath
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
