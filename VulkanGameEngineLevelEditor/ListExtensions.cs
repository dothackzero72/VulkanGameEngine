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
        public static List<T> PtrToList<T>(this List<T> list, T* listPtr)
        {
            if (list == null)
            { 
               return null;
            }

            for (int x = 0; x < list.Count; x++)
            {
                list[x] = *(listPtr + x);
            }

            return list;
        }

        public static uint UCount<T>(this List<T> list)
        {
            return (uint)list.Count;
        }

        public static IntPtr ToPointer<T>(this List<T> list)
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf<T>() * list.Count());
        }
    }
}
