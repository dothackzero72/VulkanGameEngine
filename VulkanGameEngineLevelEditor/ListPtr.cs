using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{
    public sealed unsafe class ListPtr<T> : IDisposable where T : unmanaged
    {
   
        public T* _ptr { get; private set; }
        private List<T> _list;
        private uint count = 0;
        private uint objectAllocationCount = 1;
        private bool _disposed;

        public T* Ptr
        {
            get
            {
                return (T*)_ptr;
            }
        }

        public ListPtr(uint size)
        {
            if (size <= 0)  throw new ArgumentException("Size must be greater than 0.");
            
            count = size;
            objectAllocationCount *= 2;
            int elementSize = sizeof(T);
            int totalSize = elementSize * (int)objectAllocationCount;
            _ptr = (T*)Marshal.AllocHGlobal(totalSize);
            UpdateList();
        }

        public T this[int index]
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                if (index >= count) throw new Exception("Out of bounds");

                return _ptr[index];
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                if (index >= count) throw new Exception("Out of bounds");

                _ptr[index] = value;
                UpdateList();
            }
        }

        public void Add(T item)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
            
            count++;
            UpdateList(item);
        }

        private void UpdateList()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
            
            _list = new List<T>();
            for (int x = 0; x < count; x++)
            {
                _list.Add(_ptr[x]);
            }
        }

        private void UpdateList(T item)
        {
            if (count == 0) return;
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
            if (count >= objectAllocationCount)
            {
                objectAllocationCount = count * 2;
                int elementSize = sizeof(T);
                int totalSize = elementSize * (int)objectAllocationCount;
                var tempPtr = (T*)Marshal.AllocHGlobal(totalSize);

                byte* bytePtr = (byte*)_ptr;
                Buffer.MemoryCopy(tempPtr, bytePtr, totalSize, totalSize);

                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = tempPtr;
            }

            _list = new List<T>();
            for (int x = 0; x < count; x++)
            {
                _list.Add(_ptr[x]);
            }
        }

        public bool Remove(T item)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ListPtr<T>));
            }

            int index = _list.IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            if(_list.Count == 0)
            {
                return false;
            }
            else if (_list.Count <= 1)
            {
                _list.Remove(item);
                Dispose();
            }
            else
            {
                var moveElemenets = objectAllocationCount - index;
                var moveSize = sizeof(T) * moveElemenets;

                byte* basePtr = (byte*)_ptr + (index * sizeof(T));            
                byte* copyPtr = (byte*)_ptr + ((index + 1) * sizeof(T));
                Buffer.MemoryCopy(copyPtr, basePtr, moveSize, moveSize);

                byte* lastIndexPtr = (byte*)(_ptr + ((_list.Count) * sizeof(T)));
                ClearMemory(lastIndexPtr, sizeof(T));
                UpdateList();
            }

            return true;
        }

        unsafe void ClearMemory(byte* ptr, long size)
        {
            if (ptr == null ||
                size <= 0)
            {
                return;
            }

            byte* memoryPtr = ptr;
            for (long x = 0; x < size; x++)
            {
                *memoryPtr = 0;
                memoryPtr++;
            }
        }

        public void Dispose()
        {
            if ((IntPtr)_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = null;
            }
        }
    }
}
