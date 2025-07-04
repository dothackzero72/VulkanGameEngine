#pragma once
#include <vector>
#include <algorithm>
#include <stdexcept>
#include "VkGuid.h"

template<typename T>
struct Vector2Traits 
{
    static const VkGuid& GetId(const T& obj) { return obj.id; }
    static const VkGuid& GetGuid(const T& obj) { return obj.guid; }
    static const size_t& GetVectorMapKey(const T& obj) { return obj.vectorMapKey; }
};

template<typename T>
class Vector2
{
private:
    std::vector<T> stdVector;
    std::unordered_map<int, size_t> idToIndex;

public:

    void PushBack(const T& value) 
    { 
        size_t index = stdVector.size();
        stdVector.push_back(value); 
        idToIndex[Vector2Traits<T>::GetVectorMapKey(value)] = index;
    }

    void EmplaceBack(const T& value) 
    {
        size_t index = stdVector.size();
        stdVector.emplace_back(value);
        idToIndex[Vector2Traits<T>::GetVectorMapKey(value)] = index;
    }

    T& operator[](size_t index)
    {
        if (index >= size())
        {
            throw std::out_of_range("Index out of range");
        }
        return stdVector[index];
    }

    const T& operator[](size_t index) const
    {
        if (index >= size())
        {
            throw std::out_of_range("Index out of range");
        }
        return stdVector[index];
    }

    T& Find(int targetId)
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) {  Vector2Traits<T>::GetId(obj) == targetId; });
        if (it == end())
        {
            throw std::out_of_range("Element not found with given id");
        }
        return *it;
    }

    const T& Find(int targetId) const
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) { return  Vector2Traits<T>::GetId(obj) == targetId; });
        if (it == end())
        {
            throw std::out_of_range("Element not found with given id");
        }
        return *it;
    }

    T& Find(const VkGuid& targetId)
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) { return Vector2Traits<T>::GetGuid(obj) == targetId; });
        if (it == end())
        {
            throw std::out_of_range("Element not found with given guid");
        }
        return *it;
    }

    const T& Find(const VkGuid& targetId) const
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) { return Vector2Traits<T>::GetGuid(obj) == targetId; });
        if (it == end())
        {
            throw std::out_of_range("Element not found with given guid");
        }
        return *it;
    }

    bool Exists(int targetId) const
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) { return Vector2Traits<T>::GetId(obj) == targetId; });
        return it != end();
    }

    bool Exists(const VkGuid& targetId) const
    {
        auto it = std::find_if(begin(), end(), [targetId](const T& obj) { return  Vector2Traits<T>::GetGuid(obj) == targetId; });
        return it != end();
    }

    typename std::vector<T>::iterator begin() { return stdVector.begin(); }
    typename std::vector<T>::iterator end() { return stdVector.end(); }
    typename std::vector<T>::const_iterator begin() const { return stdVector.begin(); }
    typename std::vector<T>::const_iterator end() const { return stdVector.end(); }

    size_t size() const { return stdVector.size(); }
    T* Data() { return stdVector.data(); }
    bool Any() const { return !stdVector.empty(); }
};