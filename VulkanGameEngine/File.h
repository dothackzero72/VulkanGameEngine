#pragma once
extern "C"
{
	#include <io.h>
}
#include "Typedef.h"
#include <nlohmann/json.hpp>

class File
{
public:
	static char*  ReadFile2(const String filePath);
	static nlohmann::json ReadJsonFile2(const String filePath);
	static bool   WriteFile2(void* fileInfo, size_t size, const String filePath);
	static String GetFileExtention2(const String filePath);
	static String GetFileNameFromPath2(const String filePath);
	static time_t LastModifiedTime2(const String filePath);
	static String RemoveFileExtention2(const String filePath);
	static bool FileExists2(const String filePath);
};

