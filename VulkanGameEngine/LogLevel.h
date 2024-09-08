#pragma once
#include <string>

namespace Logger
{
	enum Level
	{
		Trace,
		Debug,
		Info,
		Warning,
		Error,
		Fatal
	};

	std::wstring Logger_GetLevelName(Level logLevel);
}