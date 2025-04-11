namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static class Keyboard
    {
        private static KeyboardState keyboardState;
        
        public static void KeyboardKeyPressed(InputKey key, KeyState keyState)
        {
            switch (keyState)
            {
                case KeyState.KS_PRESSED: keyboardState.KeyPressed[(int)key] = KeyState.KS_PRESSED; break;
                case KeyState.KS_HELD: keyboardState.KeyPressed[(int)key] = KeyState.KS_HELD; break;
                case KeyState.KS_RELEASED: keyboardState.KeyPressed[(int)key] = KeyState.KS_RELEASED; break;
            }
        }
    }
}
