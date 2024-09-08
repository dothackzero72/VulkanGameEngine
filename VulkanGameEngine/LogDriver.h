#pragma once
#include <memory>

namespace Logger
{
	struct Entry;
	class ITextFormatter;

	class IDriver
	{
		public:
			virtual ~IDriver() = default;
			virtual void Submit(const Entry& entry) = 0;
	};

	class ITextDriver : public IDriver
	{
		public:
			virtual ~ITextDriver() = default;
			virtual void Submit(const Entry& entry) = 0;
			virtual void SetFormatter(std::unique_ptr<ITextFormatter> ptrFormatter) = 0;
	};
}

