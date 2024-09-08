#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <time.h>
#include <string.h>
#include <stdbool.h>

typedef struct fileState
{
	char* Data;
	size_t Size;
	bool Valid;
}FileState;

bool	  File_Exists(const char* fileName);
time_t    File_LastModifiedTime(const char* fileName);
char*	  File_RemoveFileExtention(const char* fileName);
char*	  File_GetFileExtention(const char* fileName);
FileState File_Read(const char* path);
int       File_Write(void* buffer, size_t size, const char* path);