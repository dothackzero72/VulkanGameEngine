#pragma once
#include "LogEntry.h"

namespace Logger
{
	void f(Entry* entry);
	class LoggerLog : public Entry
	{
		void g() { f(this); };
	};
}