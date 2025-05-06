//#pragma once
//#include "SDLWindow.h"
//#include <stdbool.h>
//
//typedef enum gameControllerButtons
//{
//	GCB_DPadLeft,
//	GCB_DPadRight,
//	GCB_DPadUp,
//	GCB_DPadDown
//}GameControllerButtons;
//
//typedef enum gameControllerEventState
//{
//	GCS_UNPRESSED,
//	GCS_PRESSED,
//	GCS_HELD
//}GameControllerEventState;
//
//typedef struct gameControllerState
//{
//	GameControllerEventState ButtonPressed[16];
//	GameControllerEventState AxisMoved[16];
//}GameControllerState;
//
//void GameEngine_CreateGameController();
//void GameEngine_ControllerMoveAxis(const SDL_Event* event);
//void GameEngine_ControllerButtonDown(const SDL_Event* event);
//void GameEngine_ControllerButtonUp(const SDL_Event* event);