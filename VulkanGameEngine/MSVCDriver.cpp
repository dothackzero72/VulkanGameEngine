#include "MSVCDriver.h"
#include <Windows.h>
#include "TextFormatter.h"

//Logger::MSVCDebugDriver::MSVCDebugDriver(std::unique_ptr<ITextFormatter> ptrFormatter) : pFormatter{ std::move(ptrFormatter) }
//{
//}
//
//void Logger::MSVCDebugDriver::Submit(const Entry& entry)
//{
//	if (pFormatter)
//	{
//		//OutputDebugStringW(pFormatter->Format(entry).c_str());
//	}
//}
//
//void Logger::MSVCDebugDriver::SetFormatter(std::unique_ptr<ITextFormatter> ptrFormatter)
//{
//	pFormatter = std::move(ptrFormatter);
//}
