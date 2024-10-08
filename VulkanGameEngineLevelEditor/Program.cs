using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDL2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using System.Threading;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using GlmSharp;
using Silk.NET.SDL;
using VulkanGameEngineLevelEditor.Vulkan;
using Window = Silk.NET.Windowing.Window;

namespace VulkanGameEngineLevelEditor
{
    public class Program
    {
        static IWindow window;
      //  static private Scene scene = new Scene();
        static private bool isInitialized = false;
      
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

      
        static void Main()
        {
            var opts = WindowOptions.DefaultVulkan;
            opts.Title = "Texture Demo";
            opts.Size = new Vector2D<int>(1280, 720);

            window = Silk.NET.Windowing.Window.Create(opts);
            window.Initialize();

            Scene scene = new Scene();
            scene.StartUp(window);
            while (true)
            {
                scene.DrawFrame();
            }
        }
    }

}