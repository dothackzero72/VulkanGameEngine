using System;
using System.Collections;
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
        private T* _ptr;
        private T*[] _list;
        private uint _count = 0;
        private uint _capacity = 1;
        private bool _disposed;
        private bool _ptrUpdatedExternally;

        public T* Ptr
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                _ptrUpdatedExternally = true; 
                return _ptr;
            }
        }

        public T* ListObjs
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                _ptrUpdatedExternally = true;
                return _ptr;
            }
        }

        public ListPtr(uint size)
        {
            if (size <= 0)  throw new ArgumentException("Size must be greater than 0.");

            _count = size;
            _capacity = size;
            int elementSize = sizeof(T);
            int totalSize = elementSize * (int)_capacity;
            _ptr = (T*)Marshal.AllocHGlobal(totalSize);
            _list = new T*[_capacity];
            UpdateList();
        }

        ~ListPtr()
        {
            if (!_disposed)
            {
                Console.WriteLine($"Warning: ListPtr<{typeof(T).Name}> was not disposed properly.");
                Dispose();
            }
        }

        public T this[int index]
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                if (index >= _count) throw new IndexOutOfRangeException("Index out of bounds");
                if (_ptrUpdatedExternally) UpdateList();
                return _ptr[index];
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                if (index >= _count) throw new IndexOutOfRangeException("Index out of bounds");
                _ptr[index] = value;
                UpdateList();
            }
        }

        public void Add(T item)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            if (_count >= _capacity)
            {
                _capacity *= 2;
                int elementSize = sizeof(T);
                int totalSize = elementSize * (int)_capacity;
                T* newPtr = (T*)Marshal.AllocHGlobal(totalSize);

                Buffer.MemoryCopy(_ptr, newPtr, elementSize * _count, elementSize * _count);
                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = newPtr;

                T*[] newList = new T*[_capacity];
                Array.Copy(_list, newList, _count);
                _list = newList;

                UpdateList();
            }

            _ptr[_count] = item;
            _list[_count] = &_ptr[_count];
            _count++;
        }

        public bool Remove(T item)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            for (uint x = 0; x < _count; x++)
            {
                if (EqualityComparer<T>.Default.Equals(_ptr[x], item))
                {
                    if (x < _count - 1)
                    {
                        int moveSize = sizeof(T) * (int)(_count - x - 1);
                        Buffer.MemoryCopy(_ptr + x + 1, _ptr + x, moveSize, moveSize);
                    }
                    _count--;
                    ClearMemory((byte*)(_ptr + _count), sizeof(T));
                    UpdateList();
                    return true;
                }
            }
            return false;
        }

        private void UpdateList()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            if (_list.Length < _count)
            {
                _list = new T*[_capacity];
            }

            for (uint x = 0; x < _count; x++)
            {
                _list[x] = &_ptr[x];
            }
            _ptrUpdatedExternally = false;
        }

        private void ClearMemory(byte* ptr, long size)
        {
            if (ptr == null || size <= 0) return;
            for (int x = 0; x < size; x++)
            {
                ptr[x] = 0;
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
