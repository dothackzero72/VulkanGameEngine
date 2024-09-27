using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDL2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using System.Threading;

namespace VulkanGameEngineLevelEditor
{

    public class Program
    {
        static IntPtr window;
        static bool running;
        static Scene scene;
        static void Main(string[] args)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
            }

            // Create a new window given a title, size, and passes it a flag indicating it should be shown.
            window = SDL.SDL_CreateWindow(
                "SDL .NET 6 Tutorial",
                SDL.SDL_WINDOWPOS_UNDEFINED,
                SDL.SDL_WINDOWPOS_UNDEFINED,
                640,
            480,
                SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            
            VulkanRenderer.SetUpRenderer(window);

            scene = new Scene();
            scene.StartUp();

            while (window != IntPtr.Zero)
            {
                PollEvents();
                scene.Update(0);
                scene.Draw();
            }

            if (window == IntPtr.Zero)
            {
                Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
            }
        }

        static void PollEvents()
        {
            // Check to see if there are any events and continue to do so until the queue is empty.
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                }
            }
        }
    }


    //internal static class Program
    //{
    //    /// <summary>
    //    /// The main entry point for the application.
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);
    //        Form1 mainForm = new Form1();
    //        Application.Run(new Form1());
    //        mainForm.StartRenderer();
    //        mainForm.StopRenderer();

    //    }
    //}
}
