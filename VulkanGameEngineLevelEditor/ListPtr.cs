using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{
    public sealed unsafe class ListPtr<T> : IDisposable where T : unmanaged
    {
        private readonly List<T> _list;
        private IntPtr _ptr;
        private bool _disposed;

        public T* ptr => (T*)_ptr;
        public List<T> List => _list;
        public uint Count => (uint)_list.UCount();

        public ListPtr()
        {
            _list = new List<T>();
            UpdatePointer();
        }

        public ListPtr(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("List cannot be null or empty.");
            }

            _list = list;
            UpdatePointer();
        }

        public ListPtr(uint size)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than 0.");
            }

            _list = new List<T>((int)size);
            for (int x = 0; x < size; x++)
            {
                _list.Add(default(T));
            }
            UpdatePointer();
        }

        ~ListPtr()
        {
            Dispose(false);
        }

        public T* ToBasePtr()
        {
            return ptr;
        }

        public T this[int index]
        {
            get => _list[index];
            set
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(ListPtr<T>));
                }

                _list[index] = value;
                UpdatePointer();
            }
        }

        public void Add(T item)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ListPtr<T>));
            }
            _list.Add(item);
            UpdatePointer();
        }
        public bool Remove(T item)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ListPtr<T>));
            }

            bool removed = _list.Remove(item);
            if (removed)
            {
                if (_list.Count == 0)
                {
                    Dispose(true);
                }
                else
                {
                    UpdatePointer(); 
                }
            }
            return removed;
        }

        private void UpdatePointer()
        {
            if (_ptr != IntPtr.Zero)
            {
                lock (ListExtensions._lock)
                {
                    ListExtensions._allocatedPointers.Remove(_ptr);
                    Marshal.FreeHGlobal(_ptr);
                }
            }

            if (_list.Count == 0)
            {
                _ptr = IntPtr.Zero;
                return;
            }

            int elementSize = sizeof(T);
            int totalSize = elementSize * _list.Count;
            _ptr = Marshal.AllocHGlobal(totalSize);

            T[] listArray = _list.ToArray();
            byte* bytePtr = (byte*)_ptr;

            for (int x = 0; x < listArray.Length; x++)
            {
                fixed (T* elementPtr = &listArray[x])
                {
                    Buffer.MemoryCopy(elementPtr, bytePtr + (x * elementSize), elementSize, elementSize);
                }
            }

            lock (ListExtensions._lock)
            {
                ListExtensions.TrackPtrs(_ptr);
            }
        }

        public void Clear()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ListPtr<T>));
            }

            _list.Clear();
            Dispose(true);
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
                return;
            }

            if (_ptr != IntPtr.Zero)
            {
                lock (ListExtensions._lock)
                {
                    ListExtensions._allocatedPointers.Remove(_ptr); 
                    Marshal.FreeHGlobal(_ptr);
                    _ptr = IntPtr.Zero;
                }
            }

            _disposed = true;
        }
    }
}
