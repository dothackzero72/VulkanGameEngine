#pragma once
#include <vulkan/vulkan_core.h>
#include <stdbool.h>
#include "Macro.h"
#include"SDL.h"
#include <SDL2/SDL_keycode.h>
#include <glfw/include/GLFW/glfw3.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef enum Window_Type
{
    Win32 = 0,
    SDL = 1,
    GLFW = 2
} Window_Type;

typedef enum mouseButtons
{
    MB_Left = 1,
    MB_Middle = 2,
    MB_Right = 3
}MouseButtons;

typedef enum mouseButtonEventState
{
    MS_UNPRESSED,
    MS_PRESSED,
    MS_HELD
}MouseButtonEventState;

typedef struct mouseState
{
    int X;
    int Y;
    int WheelOffset;
    MouseButtonEventState MouseButtonState[3];
}MouseState;

typedef enum KeyCode {
    KEY_UNKNOWN = -1,
    KEY_A = 0, KEY_B, KEY_C, KEY_D, KEY_E, KEY_F, KEY_G, KEY_H, KEY_I,
    KEY_J, KEY_K, KEY_L, KEY_M, KEY_N, KEY_O, KEY_P, KEY_Q, KEY_R,
    KEY_S, KEY_T, KEY_U, KEY_V, KEY_W, KEY_X, KEY_Y, KEY_Z,
    KEY_0, KEY_1, KEY_2, KEY_3, KEY_4, KEY_5, KEY_6, KEY_7, KEY_8, KEY_9,
    KEY_SPACE,
    KEY_ENTER,
    KEY_ESCAPE,
    KEY_TAB,
    KEY_BACKSPACE,
    KEY_DELETE,
    KEY_LEFT, KEY_RIGHT, KEY_UP, KEY_DOWN,
    KEY_SHIFT,
    KEY_CTRL,
    KEY_ALT,
    KEY_CAPS_LOCK,
    KEY_NUM_LOCK,
    KEY_SCROLL_LOCK,
    KEY_F1, KEY_F2, KEY_F3, KEY_F4, KEY_F5, KEY_F6,
    KEY_F7, KEY_F8, KEY_F9, KEY_F10, KEY_F11, KEY_F12,
    KEY_HOME,
    KEY_END,
    KEY_PAGE_UP,
    KEY_PAGE_DOWN,
    KEY_PRINT_SCREEN,
    KEY_INSERT,
    KEY_MENU,
    KEY_LAST
} KeyCode;

KeyCode ConvertGLFWKeyToUnified(int glfwKey)
{
    switch (glfwKey)
    {
    case GLFW_KEY_A: return KEY_A;
    case GLFW_KEY_B: return KEY_B;
    case GLFW_KEY_C: return KEY_C;
    case GLFW_KEY_D: return KEY_D;
    case GLFW_KEY_E: return KEY_E;
    case GLFW_KEY_F: return KEY_F;
    case GLFW_KEY_G: return KEY_G;
    case GLFW_KEY_H: return KEY_H;
    case GLFW_KEY_I: return KEY_I;
    case GLFW_KEY_J: return KEY_J;
    case GLFW_KEY_K: return KEY_K;
    case GLFW_KEY_L: return KEY_L;
    case GLFW_KEY_M: return KEY_M;
    case GLFW_KEY_N: return KEY_N;
    case GLFW_KEY_O: return KEY_O;
    case GLFW_KEY_P: return KEY_P;
    case GLFW_KEY_Q: return KEY_Q;
    case GLFW_KEY_R: return KEY_R;
    case GLFW_KEY_S: return KEY_S;
    case GLFW_KEY_T: return KEY_T;
    case GLFW_KEY_U: return KEY_U;
    case GLFW_KEY_V: return KEY_V;
    case GLFW_KEY_W: return KEY_W;
    case GLFW_KEY_X: return KEY_X;
    case GLFW_KEY_Y: return KEY_Y;
    case GLFW_KEY_Z: return KEY_Z;
    case GLFW_KEY_0: return KEY_0;
    case GLFW_KEY_1: return KEY_1;
    case GLFW_KEY_2: return KEY_2;
    case GLFW_KEY_3: return KEY_3;
    case GLFW_KEY_4: return KEY_4;
    case GLFW_KEY_5: return KEY_5;
    case GLFW_KEY_6: return KEY_6;
    case GLFW_KEY_7: return KEY_7;
    case GLFW_KEY_8: return KEY_8;
    case GLFW_KEY_9: return KEY_9;
    case GLFW_KEY_SPACE: return KEY_SPACE;
    case GLFW_KEY_ENTER: return KEY_ENTER;
    case GLFW_KEY_ESCAPE: return KEY_ESCAPE;
    case GLFW_KEY_TAB: return KEY_TAB;
    case GLFW_KEY_BACKSPACE: return KEY_BACKSPACE;
    case GLFW_KEY_DELETE: return KEY_DELETE;
    case GLFW_KEY_LEFT: return KEY_LEFT;
    case GLFW_KEY_RIGHT: return KEY_RIGHT;
    case GLFW_KEY_UP: return KEY_UP;
    case GLFW_KEY_DOWN: return KEY_DOWN;
    case GLFW_KEY_LEFT_SHIFT: return KEY_SHIFT;
    case GLFW_KEY_RIGHT_SHIFT: return KEY_SHIFT;
    case GLFW_KEY_LEFT_CONTROL: return KEY_CTRL;
    case GLFW_KEY_RIGHT_CONTROL: return KEY_CTRL;
    case GLFW_KEY_LEFT_ALT: return KEY_ALT;
    case GLFW_KEY_RIGHT_ALT: return KEY_ALT;
    case GLFW_KEY_CAPS_LOCK: return KEY_CAPS_LOCK;
    case GLFW_KEY_NUM_LOCK: return KEY_NUM_LOCK;
    case GLFW_KEY_SCROLL_LOCK: return KEY_SCROLL_LOCK;
    case GLFW_KEY_F1: return KEY_F1;
    case GLFW_KEY_F2: return KEY_F2;
    case GLFW_KEY_F3: return KEY_F3;
    case GLFW_KEY_F4: return KEY_F4;
    case GLFW_KEY_F5: return KEY_F5;
    case GLFW_KEY_F6: return KEY_F6;
    case GLFW_KEY_F7: return KEY_F7;
    case GLFW_KEY_F8: return KEY_F8;
    case GLFW_KEY_F9: return KEY_F9;
    case GLFW_KEY_F10: return KEY_F10;
    case GLFW_KEY_F11: return KEY_F11;
    case GLFW_KEY_F12: return KEY_F12;
    case GLFW_KEY_HOME: return KEY_HOME;
    case GLFW_KEY_END: return KEY_END;
    case GLFW_KEY_PAGE_UP: return KEY_PAGE_UP;
    case GLFW_KEY_PAGE_DOWN: return KEY_PAGE_DOWN;
    case GLFW_KEY_PRINT_SCREEN: return KEY_PRINT_SCREEN;
    case GLFW_KEY_INSERT: return KEY_INSERT;
    case GLFW_KEY_MENU: return KEY_MENU;
    default: return KEY_UNKNOWN;
    }
}

KeyCode ConvertSDLKeyToUnified(SDL_Keycode sdlKey)
{
    switch (sdlKey)
    {
    case SDLK_a: return KEY_A;
    case SDLK_b: return KEY_B;
    case SDLK_c: return KEY_C;
    case SDLK_d: return KEY_D;
    case SDLK_e: return KEY_E;
    case SDLK_f: return KEY_F;
    case SDLK_g: return KEY_G;
    case SDLK_h: return KEY_H;
    case SDLK_i: return KEY_I;
    case SDLK_j: return KEY_J;
    case SDLK_k: return KEY_K;
    case SDLK_l: return KEY_L;
    case SDLK_m: return KEY_M;
    case SDLK_n: return KEY_N;
    case SDLK_o: return KEY_O;
    case SDLK_p: return KEY_P;
    case SDLK_q: return KEY_Q;
    case SDLK_r: return KEY_R;
    case SDLK_s: return KEY_S;
    case SDLK_t: return KEY_T;
    case SDLK_u: return KEY_U;
    case SDLK_v: return KEY_V;
    case SDLK_w: return KEY_W;
    case SDLK_x: return KEY_X;
    case SDLK_y: return KEY_Y;
    case SDLK_z: return KEY_Z;
    case SDLK_0: return KEY_0;
    case SDLK_1: return KEY_1;
    case SDLK_2: return KEY_2;
    case SDLK_3: return KEY_3;
    case SDLK_4: return KEY_4;
    case SDLK_5: return KEY_5;
    case SDLK_6: return KEY_6;
    case SDLK_7: return KEY_7;
    case SDLK_8: return KEY_8;
    case SDLK_9: return KEY_9;
    case SDLK_SPACE: return KEY_SPACE;
    case SDLK_RETURN: return KEY_ENTER;
    case SDLK_ESCAPE: return KEY_ESCAPE;
    case SDLK_TAB: return KEY_TAB;
    case SDLK_BACKSPACE: return KEY_BACKSPACE;
    case SDLK_DELETE: return KEY_DELETE;
    case SDLK_LEFT: return KEY_LEFT;
    case SDLK_RIGHT: return KEY_RIGHT;
    case SDLK_UP: return KEY_UP;
    case SDLK_DOWN: return KEY_DOWN;
    case SDLK_LSHIFT: return KEY_SHIFT;
    case SDLK_RSHIFT: return KEY_SHIFT;
    case SDLK_LCTRL: return KEY_CTRL;
    case SDLK_RCTRL: return KEY_CTRL;
    case SDLK_LALT: return KEY_ALT;
    case SDLK_RALT: return KEY_ALT;
    case SDLK_CAPSLOCK: return KEY_CAPS_LOCK;
    case SDLK_NUMLOCKCLEAR: return KEY_NUM_LOCK;
    case SDLK_F1: return KEY_F1;
    case SDLK_F2: return KEY_F2;
    case SDLK_F3: return KEY_F3;
    case SDLK_F4: return KEY_F4;
    case SDLK_F5: return KEY_F5;
    case SDLK_F6: return KEY_F6;
    case SDLK_F7: return KEY_F7;
    case SDLK_F8: return KEY_F8;
    case SDLK_F9: return KEY_F9;
    case SDLK_F10: return KEY_F10;
    case SDLK_F11: return KEY_F11;
    case SDLK_F12: return KEY_F12;
    case SDLK_HOME: return KEY_HOME;
    case SDLK_END: return KEY_END;
    case SDLK_PAGEUP: return KEY_PAGE_UP;
    case SDLK_PAGEDOWN: return KEY_PAGE_DOWN;
    case SDLK_PRINTSCREEN: return KEY_PRINT_SCREEN;
    case SDLK_INSERT: return KEY_INSERT;
    case SDLK_MENU: return KEY_MENU;
    default: return KEY_UNKNOWN;
    }
}

typedef enum inputKey
{
    INPUTKEY_UNKNOWN = -1,
    INPUTKEY_RETURN = '\r',
    INPUTKEY_ESCAPE = '\x1B',
    INPUTKEY_BACKSPACE = '\b',
    INPUTKEY_TAB = '\t',
    INPUTKEY_SPACE = ' ',
    INPUTKEY_EXCLAIM = '!',
    INPUTKEY_QUOTEDBL = '"',
    INPUTKEY_HASH = '#',
    INPUTKEY_PERCENT = '%',
    INPUTKEY_DOLLAR = '$',
    INPUTKEY_AMPERSAND = '&',
    INPUTKEY_QUOTE = '\'',
    INPUTKEY_LEFTPAREN = '(',
    INPUTKEY_RIGHTPAREN = ')',
    INPUTKEY_ASTERISK = '*',
    INPUTKEY_PLUS = '+',
    INPUTKEY_COMMA = ',',
    INPUTKEY_MINUS = '-',
    INPUTKEY_PERIOD = '.',
    INPUTKEY_SLASH = '/',
    INPUTKEY_0 = '0',
    INPUTKEY_1 = '1',
    INPUTKEY_2 = '2',
    INPUTKEY_3 = '3',
    INPUTKEY_4 = '4',
    INPUTKEY_5 = '5',
    INPUTKEY_6 = '6',
    INPUTKEY_7 = '7',
    INPUTKEY_8 = '8',
    INPUTKEY_9 = '9',
    INPUTKEY_COLON = ':',
    INPUTKEY_SEMICOLON = ';',
    INPUTKEY_LESS = '<',
    INPUTKEY_EQUALS = '=',
    INPUTKEY_GREATER = '>',
    INPUTKEY_QUESTION = '?',
    INPUTKEY_AT = '@',
    INPUTKEY_LEFTBRACKET = '[',
    INPUTKEY_BACKSLASH = '\\',
    INPUTKEY_RIGHTBRACKET = ']',
    INPUTKEY_CARET = '^',
    INPUTKEY_UNDERSCORE = '_',
    INPUTKEY_BACKQUOTE = '`',
    INPUTKEY_A = 'a',
    INPUTKEY_B = 'b',
    INPUTKEY_C = 'c',
    INPUTKEY_D = 'd',
    INPUTKEY_E = 'e',
    INPUTKEY_F = 'f',
    INPUTKEY_G = 'g',
    INPUTKEY_H = 'h',
    INPUTKEY_I = 'i',
    INPUTKEY_J = 'j',
    INPUTKEY_K = 'k',
    INPUTKEY_L = 'l',
    INPUTKEY_M = 'm',
    INPUTKEY_N = 'n',
    INPUTKEY_O = 'o',
    INPUTKEY_P = 'p',
    INPUTKEY_Q = 'q',
    INPUTKEY_R = 'r',
    INPUTKEY_S = 's',
    INPUTKEY_T = 't',
    INPUTKEY_U = 'u',
    INPUTKEY_V = 'v',
    INPUTKEY_W = 'w',
    INPUTKEY_X = 'x',
    INPUTKEY_Y = 'y',
    INPUTKEY_Z = 'z',
    INPUTKEY_INSERT = 260,
    INPUTKEY_DELETE = 261,
    INPUTKEY_RIGHT = 262,
    INPUTKEY_LEFT = 263,
    INPUTKEY_DOWN = 264,
    INPUTKEY_UP = 265,
    INPUTKEY_PAGEUP = 266,
    INPUTKEY_PAGEDOWN = 267,
    INPUTKEY_HOME = 268,
    INPUTKEY_END = 269,
    INPUTKEY_CAPSLOCK = 280,
    INPUTKEY_SCROLLLOCK = 281,
    INPUTKEY_NUMLOCKCLEAR = 282,
    INPUTKEY_PRINTSCREEN = 283,
    INPUTKEY_PAUSE = 284,
    INPUTKEY_F1 = 290,
    INPUTKEY_F2 = 291,
    INPUTKEY_F3 = 292,
    INPUTKEY_F4 = 293,
    INPUTKEY_F5 = 294,
    INPUTKEY_F6 = 295,
    INPUTKEY_F7 = 296,
    INPUTKEY_F8 = 297,
    INPUTKEY_F9 = 298,
    INPUTKEY_F10 = 299,
    INPUTKEY_F11 = 300,
    INPUTKEY_F12 = 301,
    INPUTKEY_F13 = 302,
    INPUTKEY_F14 = 303,
    INPUTKEY_F15 = 304,
    INPUTKEY_F16 = 305,
    INPUTKEY_F17 = 306,
    INPUTKEY_F18 = 307,
    INPUTKEY_F19 = 308,
    INPUTKEY_F20 = 309,
    INPUTKEY_F21 = 310,
    INPUTKEY_F22 = 311,
    INPUTKEY_F23 = 312,
    INPUTKEY_F24 = 313,
    INPUTKEY_KP_1 = 320,
    INPUTKEY_KP_2 = 321,
    INPUTKEY_KP_3 = 322,
    INPUTKEY_KP_4 = 323,
    INPUTKEY_KP_5 = 324,
    INPUTKEY_KP_6 = 325,
    INPUTKEY_KP_7 = 326,
    INPUTKEY_KP_8 = 327,
    INPUTKEY_KP_9 = 328,
    INPUTKEY_KP_0 = 329,
    INPUTKEY_KP_PERIOD = 330,
    INPUTKEY_KP_DIVIDE = 331,
    INPUTKEY_KP_MULTIPLY = 332,
    INPUTKEY_KP_MINUS = 333,
    INPUTKEY_KP_PLUS = 334,
    INPUTKEY_KP_ENTER = 335,
    INPUTKEY_KP_EQUALS = 336,
    INPUTKEY_LSHIFT = 340,
    INPUTKEY_LCTRL = 341,
    INPUTKEY_LALT = 342,
    INPUTKEY_RSHIFT = 344,
    INPUTKEY_RCTRL = 345,
    INPUTKEY_RALT = 346,
    INPUTKEY_MENU = 348
}InputKey;

typedef enum keyState
{
    KS_Unpressed,
    KS_Pressed,
    KS_Held
}KeyState;

typedef struct keyboardState
{
    KeyState KeyPressed[350];
}KeyboardState;

typedef struct VulkanWindow 
{
    Window_Type WindowType;
    void* WindowHandle;
    uint32_t    Width;
    uint32_t    Height;
    bool        FrameBufferResized;
    bool        ShouldClose;
    MouseState  mouse;
    KeyboardState keyboard;

    struct VulkanWindow* (*CreateGraphicsWindow)(struct VulkanWindow* self, const char* WindowName, uint32_t width, uint32_t height);
    void (*PollEventHandler)(struct VulkanWindow* self);
    void (*SwapBuffer)(struct VulkanWindow* self);
    const char** (*GetInstanceExtensions)(struct VulkanWindow* self, uint32_t* outExtensionCount, bool enableValidationLayers);
    void (*CreateSurface)(struct VulkanWindow* self, VkInstance* instance, VkSurfaceKHR* surface);
    void (*GetFrameBufferSize)(struct VulkanWindow* self, int* width, int* height);
    void (*DestroyWindow)(struct VulkanWindow* self);
    bool (*WindowShouldClose)(struct VulkanWindow* self);
} VulkanWindow;
extern VulkanWindow* vulkanWindow;

VulkanWindow* Window_CreateWindow(Window_Type windowType, const char* WindowName, uint32_t width, uint32_t height);
#ifdef __cplusplus
}
#endif