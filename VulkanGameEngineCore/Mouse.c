//#include "Mouse.h"
//#include "Global.h"
//
//void GameEngine_SDL_MouseMoveEvent(const SDL_Event* event)
//{
//	global.Mouse.X = event->motion.x;
//	global.Mouse.Y = event->motion.y;
//}
//
//void GameEngine_SDL_MouseButtonPressedEvent(const SDL_Event* event)
//{
//    switch (global.Window.Event.button.button)
//    {
//		case SDL_BUTTON_LEFT:   global.Mouse.MouseButtonState[MB_Left-1] = MS_PRESSED;  break;
//		case SDL_BUTTON_MIDDLE: global.Mouse.MouseButtonState[MB_Middle-1] = MS_PRESSED; break;
//		case SDL_BUTTON_RIGHT:  global.Mouse.MouseButtonState[MB_Right-1] = MS_PRESSED; break;
//        default: break;
//    }
//}
//
//void GameEngine_SDL_MouseButtonUnPressedEvent(const SDL_Event* event)
//{
//	switch (global.Window.Event.button.button)
//	{
//		case SDL_BUTTON_LEFT:   global.Mouse.MouseButtonState[MB_Left-1] = MS_UNPRESSED; break;
//		case SDL_BUTTON_MIDDLE: global.Mouse.MouseButtonState[MB_Middle-1] = MS_UNPRESSED; break;
//		case SDL_BUTTON_RIGHT:  global.Mouse.MouseButtonState[MB_Right-1] = MS_UNPRESSED; break;
//		default: break;
//	}
//}
//
//void GameEngine_SDL_MouseWheelEvent(const SDL_Event* event)
//{
//	global.Mouse.WheelOffset += event->wheel.y;
//}
