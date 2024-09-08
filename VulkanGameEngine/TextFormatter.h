#pragma once
#include <string>

namespace Logger
{
	class ITextFormatter
	{
	public:
	//	virtual std::wstring Format(const Entry& entry) const = 0;
	};

	class TextFormatter : ITextFormatter
	{
	public:
	//	std::wstring Format(const Entry& entry) const override;
	};
};