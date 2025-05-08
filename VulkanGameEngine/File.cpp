#include "File.h"

FileSystem fileSystem = FileSystem();

const char* FileSystem::ReadFile(const String& filePath)
{
    return File_Read(filePath.c_str()).Data;
}

nlohmann::json FileSystem::ReadJsonFile(const String& filePath)
{
    return nlohmann::json::parse(ReadFile(filePath));
}

bool FileSystem::WriteFile(void* fileInfo, size_t size, const String& filePath)
{
    return File_Write(fileInfo, size, filePath.c_str());
}

String FileSystem::GetFileExtention(const String& filePath)
{
    return File_GetFileExtention(filePath.c_str());
}

String FileSystem::GetFileNameFromPath(const String& filePath)
{
    return File_GetFileNameFromPath(filePath.c_str());
}

time_t FileSystem::LastModifiedTime(const String& filePath)
{
    return File_LastModifiedTime(filePath.c_str());
}

String FileSystem::RemoveFileExtention(const String& filePath)
{
    return File_RemoveFileExtention(filePath.c_str());
}

bool FileSystem::FileExists(const String& filePath)
{
    return File_Exists(filePath.c_str());
}
