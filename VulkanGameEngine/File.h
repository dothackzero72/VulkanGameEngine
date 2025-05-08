#pragma once
extern "C"
{
	#include <CFile.h>
}
#include "Typedef.h"
#include <nlohmann/json.hpp>

class FileSystem
{
private:
public:
	const char* ReadFile(const String& filePath);
	nlohmann::json ReadJsonFile(const String& filePath);
	bool   WriteFile(void* fileInfo, size_t size, const String& filePath);
	String GetFileExtention(const String& filePath);
	String GetFileNameFromPath(const String& filePath);
	time_t LastModifiedTime(const String& filePath);
	String RemoveFileExtention(const String& filePath);
	bool FileExists(const String& filePath);
}
extern fileSystem;

