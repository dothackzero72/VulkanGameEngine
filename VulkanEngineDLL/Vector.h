#pragma once
#include <vector>
#include <algorithm>
#include <stdexcept>
#include "VkGuid.h"

template<typename T>
class Vector2
{
private:
	std::vector<T> stdVector;

public:

    T& operator[](size_t index) 
    {
        if (index >= this->size()) 
        {
            throw std::out_of_range("Index out of range");
        }
        return stdVector[index]; 
    }

    const T& operator[](size_t index) const 
    {
        if (index >= this->size()) 
        {
            throw std::out_of_range("Index out of range");
        }
        return stdVector[index];
    }

    typename T Find(int targetId) 
    {
        auto it = std::find_if(this->begin(), this->end(), [targetId](const T& obj) { return obj.id == targetId; });
        return *it;
    }

    typename T Find(int targetId) const 
    {
        auto it = std::find_if(this->begin(), this->end(), [targetId](const T& obj) { return obj.id == targetId; });
        return *it;
    }

    typename T Find(const VkGuid& targetId)
    {
        auto it = std::find_if(this->begin(), this->end(), [targetId](const T& obj) { return obj.guid == targetId; });
        return *it;
    }

    typename T Find(const VkGuid& targetId) const
    {
        auto it = std::find_if(this->begin(), this->end(), [targetId](const T& obj) { return obj.guid == targetId; });
        return *it;
    }

    void PushBackack(const T& value) { stdVector.push_back(value); }
    void EmplaceBack(const T& value) { stdVector.emplace_back(value); }
    typename std::vector<T>::iterator begin() { return stdVector.begin(); }
    typename std::vector<T>::iterator end() { return stdVector.end(); }

    size_t size() const { return stdVector.size(); }
    T* Data() { return stdVector.data(); }
};

