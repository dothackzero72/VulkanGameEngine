#include "LogBuilder.h"
#include "LogChannel.h"
namespace Logger
{
	EntryBuilder::EntryBuilder(const wchar_t* sourceFile, const wchar_t* sourceFunctionName, int sourceLine)
		:
		Entry{
			.level_ = Level::Error,
			.sourceFile_ = sourceFile,
			.sourceFunctionName_ = sourceFunctionName,
			.sourceLine_ = sourceLine,
			.timestamp_ = std::chrono::system_clock::now()
		}
	{}
	EntryBuilder& EntryBuilder::note(std::wstring note)
	{
		note_ = std::move(note);
		return *this;
	}
	EntryBuilder& EntryBuilder::level(Level level)
	{
		level_ = level;
		return *this;
	}
	EntryBuilder& EntryBuilder::chan(IChannel* pChan)
	{
		pDest_ = pChan;
		return *this;
	}
	EntryBuilder& EntryBuilder::Trace(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Trace;
		return *this;
	}
	EntryBuilder& EntryBuilder::Debug(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Debug;
		return *this;
	}
	EntryBuilder& EntryBuilder::Info(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Info;
		return *this;
	}
	EntryBuilder& EntryBuilder::Warning(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Warning;
		return *this;
	}
	EntryBuilder& EntryBuilder::Error(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Error;
		return *this;
	}
	EntryBuilder& EntryBuilder::Fatal(std::wstring body)
	{
		note_ = std::move(body);
		level_ = Level::Fatal;
		return *this;
	}
	EntryBuilder::~EntryBuilder()
	{
		if (pDest_) {
			pDest_->Submit(*this);
		}
	}
}