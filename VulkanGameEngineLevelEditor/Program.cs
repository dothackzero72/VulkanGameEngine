using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDL2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using System.Threading;
using Silk.NET.GLFW;
namespace VulkanGameEngineLevelEditor
{
    public class Program
    {
        //        static Scene scene;
        //        static unsafe void Main(string[] args)
        //        {
        //            // Initialize GLFW
        //            var glfw = Glfw.GetApi();
        //            if (!glfw.Init())
        //            {
        //                System.Console.WriteLine("Failed to initialize GLFW");
        //                return;
        //            }

        //            // Create a window
        //            var window = glfw.CreateWindow(800, 600, "Hello GLFW with Silk.NET", null, null);
        //            if (window == null)
        //            {
        //                System.Console.WriteLine("Failed to create GLFW window");
        //                glfw.Terminate();
        //                return;
        //            }

        //            glfw.MakeContextCurrent(window);
        //            glfw.ShowWindow(window);

        //            // Setup OpenGL
        //            //gl = GL.GetApi(glfw.GetProcAddress);
        //            //
        //            // Main loop


        //            VulkanRenderer.SetUpRenderer(window);
        //            scene = new Scene();
        //            scene.StartUp();
        //            while (!glfw.WindowShouldClose(window))
        //            {
        //                // gl.Clear((uint)ClearBufferMask.ColorBufferBit);
        //                glfw.SwapBuffers(window);
        //                glfw.PollEvents();
        //                scene.Update(0);
        //                scene.Draw();
        //            }

        //            // Cleanup
        //            glfw.Terminate();
        //        }
        //    }
        //}
        //public class Program
        //{
        //    static public IntPtr window;
        //    static bool running;
        //    static Scene scene;
        //    static void Main(string[] args)
        //    {
        //        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        //        {
        //            Console.WriteLine($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
        //        }

        //        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        //        window = SDL.SDL_CreateWindow(
        //            "SDL .NET 6 Tutorial",
        //            SDL.SDL_WINDOWPOS_UNDEFINED,
        //            SDL.SDL_WINDOWPOS_UNDEFINED,
        //            640,
        //        480,
        //            SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);


        //        VulkanRenderer.SetUpRenderer(window);

        //        scene = new Scene();
        //        scene.StartUp();

        //        while (window != IntPtr.Zero)
        //        {
        //            PollEvents();
        //            scene.Update(0);
        //            scene.Draw();
        //        }

        //        if (window == IntPtr.Zero)
        //        {
        //            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        //        }
        //    }

        //    static void PollEvents()
        //    {
        //        // Check to see if there are any events and continue to do so until the queue is empty.
        //        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
        //        {
        //            switch (e.type)
        //            {
        //                case SDL.SDL_EventType.SDL_QUIT:
        //                    running = false;
        //                    break;
        //            }
        //        }
        //    }
        //}



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 mainForm = new Form1();
            Application.Run(new Form1());
            mainForm.StartRenderer();
            mainForm.StopRenderer();

        }

    }

}