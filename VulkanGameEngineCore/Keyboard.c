#include <stdio.h>
#include <ctype.h>
#include "Keyboard.h"

static int ConvertToAscii(SDL_Keycode key)
{
    switch (key)
    {
        case SDLK_UNKNOWN: return -1;
        case SDLK_ESCAPE: return 256;
        case SDLK_RETURN: return 257;
        case SDLK_TAB: return 258;
        case SDLK_BACKSPACE: return 259;
        case SDLK_INSERT: return 260;
        case SDLK_DELETE: return 261;
        case SDLK_RIGHT: return 262;
        case SDLK_LEFT: return 263;
        case SDLK_DOWN: return 264;
        case SDLK_UP: return 265;
        case SDLK_PAGEUP: return 266;
        case SDLK_PAGEDOWN: return 267;
        case SDLK_HOME: return 268;
        case SDLK_END: return 269;
        case SDLK_CAPSLOCK: return 280;
        case SDLK_SCROLLLOCK: return 281;
        case SDLK_NUMLOCKCLEAR: return 282;
        case SDLK_PRINTSCREEN: return 283;
        case SDLK_PAUSE: return 284;
        case SDLK_F1: return 290;
        case SDLK_F2: return 291;
        case SDLK_F3: return 292;
        case SDLK_F4: return 293;
        case SDLK_F5: return 294;
        case SDLK_F6: return 295;
        case SDLK_F7: return 296;
        case SDLK_F8: return 297;
        case SDLK_F9: return 298;
        case SDLK_F10: return 299;
        case SDLK_F11: return 300;
        case SDLK_F12: return 301;
        case SDLK_F13: return 302;
        case SDLK_F14: return 303;
        case SDLK_F15: return 304;
        case SDLK_F16: return 305;
        case SDLK_F17: return 306;
        case SDLK_F18: return 307;
        case SDLK_F19: return 308;
        case SDLK_F20: return 309;
        case SDLK_F21: return 310;
        case SDLK_F22: return 311;
        case SDLK_F23: return 312;
        case SDLK_F24: return 313;
        case SDLK_KP_1: return 320;
        case SDLK_KP_2: return 321;
        case SDLK_KP_3: return 322;
        case SDLK_KP_4: return 323;
        case SDLK_KP_5: return 324;
        case SDLK_KP_6: return 325;
        case SDLK_KP_7: return 326;
        case SDLK_KP_8: return 327;
        case SDLK_KP_9: return 328;
        case SDLK_KP_0: return 329;
        case SDLK_KP_PERIOD: return 330;
        case SDLK_KP_DIVIDE: return 331;
        case SDLK_KP_MULTIPLY: return 332;
        case SDLK_KP_MINUS: return 333;
        case SDLK_KP_PLUS: return 334;
        case SDLK_KP_ENTER: return 335;
        case SDLK_KP_EQUALS: return 336;
        case SDLK_LSHIFT: return 340;
        case SDLK_LCTRL: return 341;
        case SDLK_LALT: return 342;
        case SDLK_RSHIFT: return 344;
        case SDLK_RCTRL: return 345;
        case SDLK_RALT: return 346;
        case SDLK_MENU: return 348;
        default: return key;
    }
}

static void UpdateKeyState(VulkanWindow* self, const SDL_Event* event, KeyState keyState)
{
    KeyCode unifiedKey = ConvertSDLKeyToUnified(event->key.keysym.sym);

    if (keyState == KS_Pressed ||
        keyState == KS_Held)
    {
        if (self->keyboard.KeyPressed[unifiedKey] == KS_Pressed)
        {
            self->keyboard.KeyPressed[unifiedKey] = KS_Held;
        }
        else
        {
            self->keyboard.KeyPressed[unifiedKey] = KS_Pressed;
        }
    }
    else
    {
        self->keyboard.KeyPressed[unifiedKey] = KS_Unpressed;
    }
}

void GameEngine_SDL_KeyboardKeyPressed(VulkanWindow* self, const SDL_Event* event)
{
    UpdateKeyState(self, event, KS_Pressed);
}

void GameEngine_GLFW_KeyboardKeyPressed(VulkanWindow* self, int key, int action)
{
    KeyCode unifiedKey = ConvertGLFWKeyToUnified(key);

    if (action == GLFW_PRESS)
    {
        self->keyboard.KeyPressed[unifiedKey] = KS_Pressed;
    }
    else if (action == GLFW_RELEASE)
    {
        self->keyboard.KeyPressed[unifiedKey] = KS_Unpressed;
    }
}