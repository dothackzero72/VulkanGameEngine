using Microsoft.VisualBasic.Devices;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe static class ListExtensions
    {
        public static readonly List<IntPtr> _allocatedPointers = new List<IntPtr>();
        public static readonly object _lock = new object();

        public static void TrackPtrs(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero && !_allocatedPointers.Contains(ptr))
            {
                _allocatedPointers.Add(ptr);
            }
        }

        public static T[] ToListArray<T>(this List<T> list)
        {
            if (list == null)
            {
                return null;
            }

            return list.ToArray();
        }

        public static T* ToListPtr<T>(this List<T> list) where T : unmanaged
        {
            if (list == null ||
                list.Count == 0)
            {
                return null;
            }

            int elementSize = sizeof(T); 
            int totalSize = elementSize * list.Count;
            IntPtr ptr = Marshal.AllocHGlobal(totalSize);

            try
            {
                T[] listArray = list.ToArray();
                byte* bytePtr = (byte*)ptr;

                for (int x = 0; x < listArray.Length; x++)
                {
                    fixed (T* elementPtr = &listArray[x])
                    {
                        System.Buffer.MemoryCopy(elementPtr, bytePtr + (x * elementSize), elementSize, elementSize);
                    }
                }

                TrackPtrs(ptr);
                return (T*)ptr;
            }
            catch (Exception ex)
            {
                Marshal.FreeHGlobal(ptr);
                throw;
            }
        }


        public static uint UCount<T>(this List<T> list)
        {
            return (uint)list.Count;
        }

        public static IntPtr ToPointer<T>(this List<T> list)
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf<T>() * list.Count());
        }

        public static void Dispose()
        {
            lock (_lock)
            {
                foreach (var ptr in _allocatedPointers)
                {
                    try
                    {
                        if (ptr != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(ptr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error freeing pointer {ptr:X}: {ex.Message}");
                    }
                }
                _allocatedPointers.Clear(); // Prevent double-free
            }
        }
    }
}
