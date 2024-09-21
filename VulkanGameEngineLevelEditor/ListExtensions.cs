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
        public static IntPtr ToPointer<T>(this List<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            GCHandle handle = GCHandle.Alloc(list, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf<T>() * list.Count());
                return pointer;
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
