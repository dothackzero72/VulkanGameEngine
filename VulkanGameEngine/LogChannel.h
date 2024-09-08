#pragma once
#include "LogEntry.h"
#include <vector>

namespace Logger
{
	struct Entry;
	class IDriver;

	class IChannel
	{
	public:
		virtual ~IChannel() = default;
		virtual void Submit(Entry&) = 0;
		virtual void AttachDriver(std::shared_ptr<IDriver>) = 0;
	};

	class Channel : public IChannel
	{
	private:
		std::vector<std::shared_ptr<IDriver>> DriverList;
	public:
		Channel(std::vector<std::shared_ptr<IDriver>> driverList = {});
		virtual void Submit(Entry&) override;
		virtual void AttachDriver(std::shared_ptr<IDriver>) override;
	};
}