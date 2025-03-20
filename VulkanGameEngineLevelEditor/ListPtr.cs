using Silk.NET.Vulkan;
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
    public unsafe class ListPtr<T> : IEnumerable<T>, IDisposable
    {
        private T* _ptr;
        private T*[] _debugList; //Just here to make debugging easier.
        private uint _count = 0;
        private uint _capacity = 0;
        private bool _disposed;
        private bool _ptrUpdatedExternally;
        private List<GCHandle> _handles;
        private List<T> _refList;
        private readonly bool _isValueType;

        public int Count => (int)_count;
        public uint UCount => _count;
        public T* Ptr
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                return _ptr;
            }
        }

        public ListPtr()
        {
            _isValueType = typeof(T).IsValueType;
            _ptr = null;
            _count = 0;
            _capacity = 0;
            _debugList = new T*[_capacity];
            _disposed = false;
            _handles = _isValueType ? null : new List<GCHandle>();
            _refList = _isValueType ? null : new List<T>();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public ListPtr(T obj)
        {
            _isValueType = typeof(T).IsValueType;
            _ptr = null;
            _count = 0;
            _capacity = 0;
            _debugList = new T*[_capacity];
            _disposed = false;
            _handles = _isValueType ? null : new List<GCHandle>();
            _refList = _isValueType ? null : new List<T>();

            UpdateList();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public ListPtr(uint size)
        {
            if (size <= 0) throw new ArgumentException("Size must be greater than 0.");
            _isValueType = typeof(T).IsValueType;
            _count = size;
            _capacity = size;
            _debugList = new T*[_capacity];
            _handles = _isValueType ? null : new List<GCHandle>((int)size);
            _refList = _isValueType ? null : new List<T>();

            int elementSize = _isValueType ? sizeof(T) : sizeof(T*);
            _ptr = (T*)Marshal.AllocHGlobal(elementSize * (int)_capacity);

            ClearMemory((byte*)_ptr, elementSize * (int)_capacity);

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public ListPtr(T* ptr, uint size)
        {
            if (size <= 0) throw new ArgumentException("Size must be greater than 0.");

            _isValueType = typeof(T).IsValueType;
            _count = size;
            _capacity = size;
            _debugList = new T*[_capacity];
            _handles = _isValueType ? null : new List<GCHandle>((int)size);
            _refList = _isValueType ? null : new List<T>();
            _ptr = ptr;

            UpdateList();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public ListPtr(List<T> list)
        {
            if (list.Count <= 0) return;

            _count = (uint)list.Count;
            _capacity = (uint)list.Capacity;
            _debugList = new T*[_capacity];
            _ptr = (T*)Marshal.AllocHGlobal(sizeof(T) * (int)_capacity);
            _handles = new List<GCHandle>(sizeof(T));
            _refList = _isValueType ? null : new List<T>();

            ClearMemory((byte*)_ptr, sizeof(T) * (int)_capacity);
            for (int x = 0; x < _count; x++)
            {
                _ptr[x] = list[x];
            }

            UpdateList();

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        public ListPtr(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            
            if (collection.Count() <= 0)
            {
                _isValueType = typeof(T).IsValueType;
                _ptr = null;
                _count = 0;
                _capacity = 0;
                _debugList = new T*[_capacity];
                _disposed = false;
                _handles = _isValueType ? null : new List<GCHandle>();
                _refList = _isValueType ? null : new List<T>();

                AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
                return;
            }

            _isValueType = typeof(T).IsValueType;
            _count = (uint)collection.Count();
            _capacity = (uint)collection.Count();
            _debugList = new T*[_capacity];
            _handles = _isValueType ? null : new List<GCHandle>((int)_count);
            _refList = _isValueType ? null : new List<T>();

            int elementSize = _isValueType ? sizeof(T) : sizeof(void*);
            _ptr = (T*)Marshal.AllocHGlobal(elementSize * (int)_capacity);

            int x = 0;
            foreach (var item in collection)
            {
                if (_isValueType)
                {
                    ((T*)_ptr)[x] = item;
                    _debugList[x] = (T*)_ptr + x;
                }
                else
                {
                    GCHandle handle = GCHandle.Alloc(item, GCHandleType.Normal);
                    _handles.Add(handle);
                    _refList.Add(item);
                }
                x++;
            }
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
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
                return _isValueType ? ((T*)_ptr)[index] : *((T**)_ptr)[index];
            }
            set
            {
                if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
                if (index >= _count) throw new IndexOutOfRangeException("Index out of bounds");
                if (_isValueType)
                {
                    ((T*)_ptr)[index] = value;
                    _debugList[index] = (T*)_ptr + index;
                }
                else
                {
                    GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
                    if (index < _handles.Count)
                    {
                        _handles[index].Free();
                        _handles[index] = handle;
                        _refList[index] = value;
                    }
                    else
                    {
                        _handles.Add(handle);
                        _refList.Add(value);
                    }
                    ((T**)_ptr)[index] = (T*)handle.AddrOfPinnedObject();
                    _debugList[index] = ((T**)_ptr)[index];
                }
            }
        }

        public void Add(T item)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
            if (_count >= _capacity)
            {
                _capacity = _capacity == 0 ? 1 : _capacity * 2;
                int elementSize = _isValueType ? sizeof(T) : sizeof(void*);
                T* newPtr = (T*)Marshal.AllocHGlobal(elementSize * (int)_capacity);

                if (_ptr != null)
                {
                    System.Buffer.MemoryCopy(_ptr, newPtr, elementSize * _count, elementSize * _count);
                    Marshal.FreeHGlobal((IntPtr)_ptr);
                }

                _ptr = newPtr;
                _debugList = new T*[_capacity];
                Array.Copy(_debugList, _debugList, _count);
            }

            if (_isValueType)
            {
                ((T*)_ptr)[_count] = item;
                _debugList[_count] = (T*)_ptr + _count;
            }
            else
            {
                GCHandle handle = GCHandle.Alloc(item, GCHandleType.Normal);
                _handles.Add(handle);
                _refList.Add(item);
            }
            UpdateList();
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
                        System.Buffer.MemoryCopy(_ptr + (x + 1), _ptr + x, moveSize, moveSize);
                    }
                    _count--;
                    ClearMemory((byte*)(_ptr + _count), sizeof(T));
                    UpdateList();
                    return true;
                }
            }
            return false;
        }

        public bool Remove(uint index)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));
            if (index >= _count) throw new IndexOutOfRangeException("Index out of bounds");

            if (index < _count - 1)
            {
                int moveSize = sizeof(T) * (int)(_count - (index - 1));
                System.Buffer.MemoryCopy(_ptr + (index + 1), _ptr + index, moveSize, moveSize);
            }
            _count--;
            ClearMemory((byte*)(_ptr + _count), sizeof(T));
            UpdateList();
            return true;
        }

        public void Clear()
        {
            ClearMemory((byte*)_ptr, sizeof(T) * (int)_capacity);
            _count = 0;
            _capacity = 0;
            _debugList = new T*[0];
            UpdateList();
        }

        private void UpdateList()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            if (_debugList.Length < _count)
            {
                _debugList = new T*[_capacity];
            }

            if (_isValueType)
            {
                for (uint x = 0; x < _count; x++)
                {
                    _debugList[x] = &_ptr[x];
                }
            }
            else
            {
                for (int x = 0; x < _count; x++)
                {
                    var a = (T)_handles[x].Target;
                    _debugList[x] = &a;
                }
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
            if (_disposed) return;
            if (_isValueType && _ptr != null)
            {
                Marshal.FreeHGlobal((IntPtr)_ptr);
                _ptr = null;
            }
            if (!_isValueType && _handles != null)
            {
                foreach (var handle in _handles) if (handle.IsAllocated) handle.Free();
                _handles = null;
                _refList = null;
            }
            _disposed = true;
            if (disposing) AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            if (!_disposed)
            {
                Dispose(true);
            }
        }

        public static bool operator ==(ListPtr<T> l1, ListPtr<T> l2)
        {
            if (ReferenceEquals(l1, l2)) return true;
            if (l1 is null || l2 is null) return false;
            return l1.Ptr == l2.Ptr;
        }

        public static bool operator !=(ListPtr<T> l1, ListPtr<T> l2)
        {
            if (ReferenceEquals(l1, l2)) return true;
            if (l1 is null || l2 is null) return false;
            return l1.Ptr != l2.Ptr;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ListPtr<T>));

            List<T> list = new List<T>((int)_count);
            for (uint x = 0; x < _count; x++)
            {
                list.Add(_ptr[x]);
            }

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
