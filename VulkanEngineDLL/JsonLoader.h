#pragma once
#include "Typedef.h"
#include "JsonStruct.h"
#include "from_json.h"

RenderPassLoader JsonLoader_LoadRenderPassLoaderInfo(const char* renderPassLoaderJson, const ivec2& defaultRenderPassResoultion);