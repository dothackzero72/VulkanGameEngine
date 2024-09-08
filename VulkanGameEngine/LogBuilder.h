#pragma once
#include "LogEntry.h"
namespace Logger
{
	class IChannel;

	class EntryBuilder : private Entry
	{
	public:
		EntryBuilder(const wchar_t* sourceFile, const wchar_t* sourceFunctionName, int sourceLine);
		EntryBuilder& note(std::wstring note);
		EntryBuilder& level(Level);
		EntryBuilder& chan(IChannel*);
		EntryBuilder& Trace(std::wstring body = L"");
		EntryBuilder& Debug(std::wstring body = L"");
		EntryBuilder& Info(std::wstring body = L"");
		EntryBuilder& Warning(std::wstring body = L"");
		EntryBuilder& Error(std::wstring body = L"");
		EntryBuilder& Fatal(std::wstring body = L"");
		~EntryBuilder();
	private:
		IChannel* pDest_ = nullptr;
	};
}