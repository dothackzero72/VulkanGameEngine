#include "File.h"
#include "io.h"

char* File::ReadFile2(const String filePath)
{
    return File_Read(filePath.c_str()).Data;
}

nlohmann::json File::ReadJsonFile2(const String filePath)
{
    return nlohmann::json::parse(ReadFile2(filePath));
}

bool File::WriteFile2(void* fileInfo, size_t size, const String filePath)
{
    return File_Write(fileInfo, size, filePath.c_str());
}

String File::GetFileExtention2(const String filePath)
{
    return File_GetFileExtention(filePath.c_str());
}

String File::GetFileNameFromPath2(const String filePath)
{
    return File_GetFileNameFromPath(filePath.c_str());
}

time_t File::LastModifiedTime2(const String filePath)
{
    return File_LastModifiedTime(filePath.c_str());
}

String File::RemoveFileExtention2(const String filePath)
{
    return File_RemoveFileExtention(filePath.c_str());
}

bool File::FileExists2(const String filePath)
{
    return File_Exists(filePath.c_str());
}
