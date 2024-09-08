#pragma once
#include "LogDriver.h"
#include <memory>

namespace Logger
{
	class MSVCDebugDriver : ITextDriver
	{
		private:
			std::unique_ptr<ITextFormatter> pFormatter;
		public:
			MSVCDebugDriver(std::unique_ptr<ITextFormatter> ptrFormatter = {});
			void Submit(const Entry& entry) override;
			void SetFormatter(std::unique_ptr<ITextFormatter>  ptrFormatter) override;
	};
};