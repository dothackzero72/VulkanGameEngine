#pragma once
#include <cstdint>
#include <cstring>
#include <functional>
#include <CTypedef.h>
#include <objbase.h>
#include <stdexcept>
#include <array>
#include <iostream>
#include "TypeDef.h"

class VkGuid
{
private:
    uint32 Data1;
    uint16 Data2;
    uint16 Data3;
    std::array<uint8_t, 8> Data4;

public:
    VkGuid() : Data1(0), Data2(0), Data3(0), Data4{ 0 } {}

    VkGuid(const char* guidchar)
    {
        GUID guid = {};
        String guidString = String(guidchar);
        if (guidString.front() != '{')
        {
            guidString = "{" + guidString + "}";
        }

        std::wstring wGuidStr(guidString.begin(), guidString.end());
        HRESULT result = CLSIDFromString(wGuidStr.c_str(), &guid);

        if (FAILED(result))
        {
            std::cerr << "Failed to convert string to GUID. HRESULT: " << std::hex << result << std::endl;
            throw std::runtime_error("Invalid GUID format.");
        }

        Data1 = guid.Data1;
        Data2 = guid.Data2;
        Data3 = guid.Data3;
        std::copy(std::begin(guid.Data4), std::end(guid.Data4), Data4.begin());
    }

    VkGuid(const GUID& guid) {
        Data1 = guid.Data1;
        Data2 = guid.Data2;
        Data3 = guid.Data3;
        std::copy(std::begin(guid.Data4), std::end(guid.Data4), Data4.begin());
    }

    static VkGuid GenerateGUID()
    {
        GUID guid;
        HRESULT result = CoCreateGuid(&guid);
        if (FAILED(result)) {
            throw std::runtime_error("Failed to create GUID");
        }
        return VkGuid(guid);
    }

    bool operator==(const VkGuid& rhs) const {
        return std::memcmp(this, &rhs, sizeof(VkGuid)) == 0;
    }

    bool operator==(const GUID& rhs) const {
        return std::memcmp(this, &rhs, sizeof(GUID)) == 0;
    }

    friend struct std::hash<VkGuid>;
};

namespace std {
    template <>
    struct hash<VkGuid> {
        size_t operator()(const VkGuid& guid) const {
            size_t h1 = hash<uint32_t>{}(guid.Data1);
            size_t h2 = hash<uint16_t>{}(guid.Data2);
            size_t h3 = hash<uint16_t>{}(guid.Data3);
            size_t h4 = 0;
            for (auto byte : guid.Data4) {
                h4 = (h4 << 8) | byte;
            }
            return h1 ^ (h2 << 1) ^ (h3 << 2) ^ h4;
        }
    };
}