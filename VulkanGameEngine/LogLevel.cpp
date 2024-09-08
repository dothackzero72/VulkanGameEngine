#include "LogLevel.h"

namespace Logger
{
	std::wstring Logger_GetLevelName(Level loggerLevel)
	{

		switch (loggerLevel)
		{
		case Level::Trace: return L"Trace";
		case Level::Debug: return L"Debug";
		case Level::Info: return L"Info";
		case Level::Warning: return L"Warning";
		case Level::Error: return L"Error";
		case Level::Fatal: return L"Fatal";
		default: return L"Unknown";
		}
	}
}
