#include "ChiliContainer.h"

namespace chil::ioc
{
	Container& Get() noexcept
	{
		static Container container;
		return container;
	}
}