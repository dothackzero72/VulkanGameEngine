using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class MemoryPool<T> where T : class, new()
    {
        private const uint FailedToFind = uint.MaxValue;
        public const byte MemoryBlockUsed = 1;
        public const byte FreeMemoryBlock = 0;

        private byte* MemoryBlock;
        private byte* MemoryBlockInUse;
        public uint ObjectCount;
        private int ObjectSize;

        public MemoryPool()
        {

        }

        public void CreateMemoryPool(uint objectCount)
        {
            this.ObjectCount = objectCount;
            ObjectSize = sizeof(T);
            MemoryBlock = (byte*)Marshal.AllocHGlobal((int)(ObjectCount * ObjectSize));
            MemoryBlockInUse = (byte*)Marshal.AllocHGlobal((int)(ObjectCount));

            for (uint x = 0; x < objectCount; x++)
            {
                MemoryBlockInUse[x] = FreeMemoryBlock;
            }
        }

        public T AllocateMemoryLocation()
        {
            uint memoryIndex = FindNextFreeMemoryBlockIndex();
            if (memoryIndex == FailedToFind)
            {
                throw new InvalidOperationException("Memory pool is full.");
            }

            T newObject = new T();
            T* objectPtr = (T*)(MemoryBlock + memoryIndex * ObjectSize);
            *objectPtr = newObject;

            MemoryBlockInUse[memoryIndex] = MemoryBlockUsed;
            return newObject;
        }

        public unsafe List<T> ViewMemoryPool()
        {
            List<T> memoryList = new List<T>((int)ObjectCount);
            for (uint x = 0; x < ObjectCount; x++)
            {
                T* objectPtr = (T*)(MemoryBlock + (x * ObjectSize));
                if ((byte*)objectPtr >= (byte*)MemoryBlock &&
                    (byte*)objectPtr < (byte*)MemoryBlock + (ObjectCount * ObjectSize) &&
                    MemoryBlockInUse[x] == MemoryBlockUsed)
                {
                    memoryList.Add(*objectPtr);
                }
                else
                {
                    memoryList.Add(default);
                }
            }

            return memoryList;
        }

        public List<byte> ViewMemoryBlockUsage()
        {
            List<byte> usage = new List<byte>((int)ObjectCount);
            for (uint i = 0; i < ObjectCount; i++)
            {
                usage.Add(MemoryBlockInUse[i]);
            }
            return usage;
        }

        private uint FindNextFreeMemoryBlockIndex()
        {
            for (uint i = 0; i < ObjectCount; i++)
            {
                if (MemoryBlockInUse[i] == FreeMemoryBlock)
                {
                    return i;
                }
            }
            return FailedToFind;
        }

        public void Destroy()
        {
            for (uint x = 0; x < ObjectCount; x++)
            {
                if (MemoryBlockInUse[x] == MemoryBlockUsed)
                {
                    T* objectPtr = (T*)(MemoryBlock + x * ObjectSize);
                    *objectPtr = default;
                    MemoryBlockInUse[x] = FreeMemoryBlock;
                }
            }

            if (MemoryBlock != null)
            {
                Marshal.FreeHGlobal((IntPtr)MemoryBlock);
                MemoryBlock = null;
            }

            if (MemoryBlockInUse != null)
            {
                Marshal.FreeHGlobal((IntPtr)MemoryBlockInUse);
                MemoryBlockInUse = null;
            }
        }
    }
}