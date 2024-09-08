#include "LogChannel.h"
#include "LogDriver.h"

namespace Logger
{
	Channel::Channel(std::vector<std::shared_ptr<IDriver>> driverList)
	{
		DriverList = std::move(driverList);
	}

	void Channel::Submit(Entry& entry)
	{
		for (auto& pDriver : DriverList)
		{
			pDriver->Submit(entry);
		}
	}

	void Channel::AttachDriver(std::shared_ptr<IDriver> driver)
	{
		DriverList.emplace_back(std::move(driver));
	}
}

