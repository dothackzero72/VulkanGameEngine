using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private T*[] _debugList; //Just here to make debugging easier.
        private uint _count = 0;
        private uint _size = 0;
        private uint _capacity = 1;
        private bool _disposed;
        private bool _ptrUpdatedExternally;

        public int Count => (int)_count;
        public uint UCount => _count;
        public T* Ptr
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                _ptrUpdatedExternally = true; 
                return _ptr;
            }
        }

        public ListPtr()
        {
            _ptr = null;
            _count = 0;
            _size = 0;
            _capacity = 1;
            _debugList = new T*[_capacity];
            _disposed = false;
            _ptrUpdatedExternally = false;
        }

        public ListPtr(uint size)
        {
            if (size <= 0)  throw new ArgumentException("Size must be greater than 0.");

            _count = size;
            _capacity = size;
            _size = (uint)sizeof(T) * _capacity;
            _debugList = new T*[_capacity];
            _ptr = (T*)Marshal.AllocHGlobal((int)_size);

            ClearMemory((byte*)_ptr, _size);
            UpdateList();
        }

        public ListPtr(T* ptr, uint size)
        {
            if (size <= 0) throw new ArgumentException("Size must be greater than 0.");

            _count = size;
            _capacity = size;
            _size = (uint)sizeof(T) * _capacity;
            _ptr = ptr;
            _debugList = new T*[_capacity];

            UpdateList();
        }

        public ListPtr(List<T> list)
        {
            if (list.Count <= 0) return;

            _count = (uint)list.Count;
            _capacity = (uint)list.Capacity;
            _size = (uint)sizeof(T) * _capacity;
            _debugList = new T*[_capacity];
            _ptr = (T*)Marshal.AllocHGlobal((int)_size);

            ClearMemory((byte*)_ptr, _size);
            for(int x = 0; x < _count; x++)
            {
                _ptr[x] = list[x];
            }

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
                int totalSize = sizeof(T) * (int)_capacity;
                T* newPtr = (T*)Marshal.AllocHGlobal(totalSize);

                Buffer.MemoryCopy(_ptr, newPtr, sizeof(T) * _count, sizeof(T) * _count);
                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = newPtr;

                T*[] newList = new T*[_capacity];
                Array.Copy(_debugList, newList, _count);
                _debugList = newList;

                UpdateList();
            }

            _ptr[_count] = item;
            _debugList[_count] = &_ptr[_count];
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
                        int moveSize = sizeof(T) * (int)(_count - (x - 1));
                        Buffer.MemoryCopy(_ptr + (x + 1), _ptr + x, moveSize, moveSize);
                    }
                    _count--;
                    ClearMemory((byte*)(_ptr + _count), sizeof(T));
                    UpdateList();
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            ClearMemory((byte*)_ptr, _size);
            UpdateList();
        }

        private void UpdateList()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            if (_debugList.Length < _count)
            {
                _debugList = new T*[_capacity];
            }

            for (uint x = 0; x < _count; x++)
            {
                _debugList[x] = &_ptr[x];
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                Console.WriteLine("ListPtr already disposed.");
                return;
            }

            if (_ptr != null)
            {
                Console.WriteLine($"Disposing ListPtr<{typeof(T).Name}> at {(IntPtr)_ptr:X}");
                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = null;
            }
            _disposed = true;

            if (disposing)
            {
                AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (!_disposed)
            {
                Dispose(true);
            }
        }
    }
}
