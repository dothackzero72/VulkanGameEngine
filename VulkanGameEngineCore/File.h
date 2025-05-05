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
	static char* ReadFile(const String filePath);
	static nlohmann::json ReadJsonFile(const String filePath);
	static bool   WriteFile(void* fileInfo, size_t size, const String filePath);
	static String GetFileExtention(const String filePath);
	static String GetFileNameFromPath(const String filePath);
	static time_t LastModifiedTime(const String filePath);
	static String RemoveFileExtention(const String filePath);
	static bool FileExists(const String filePath);
};

