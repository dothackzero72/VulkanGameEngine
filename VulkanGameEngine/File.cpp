#include "File.h"
#include "io.h"

char* File::ReadFile(const String filePath)
{
    return File_Read(filePath.c_str()).Data;
}

nlohmann::json File::ReadJsonFile(const String filePath)
{
    return nlohmann::json::parse(ReadFile(filePath));
}

bool File::WriteFile(void* fileInfo, size_t size, const String filePath)
{
    return File_Write(fileInfo, size, filePath.c_str());
}

String File::GetFileExtention(const String filePath)
{
    return File_GetFileExtention(filePath.c_str());
}

String File::GetFileNameFromPath(const String filePath)
{
    return File_GetFileNameFromPath(filePath.c_str());
}

time_t File::LastModifiedTime(const String filePath)
{
    return File_LastModifiedTime(filePath.c_str());
}

String File::RemoveFileExtention(const String filePath)
{
    return File_RemoveFileExtention(filePath.c_str());
}

bool File::FileExists(const String filePath)
{
    return File_Exists(filePath.c_str());
}
