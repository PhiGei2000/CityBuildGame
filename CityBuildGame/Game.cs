using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

using CityBuildGame.Rendering;

namespace CityBuildGame
{
    public class Game : IDisposable
    {
        private GameWindow window;
        private List<Geometry> geometries;

        public Game(int width, int height, string title)
        {
            GameWindowSettings windowSettings = new GameWindowSettings()
            {
                RenderFrequency = 60,
                UpdateFrequency = 60
            };

            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height)
            };

            window = new GameWindow(windowSettings, nativeSettings);

            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.KeyUp += Window_KeyUp;
            window.Resize += Window_Resize;
        }

        private void Window_KeyUp(OpenTK.Windowing.Common.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape)
            {
                window.Close();
            }
        }

        private void Window_Resize(OpenTK.Windowing.Common.ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private void Window_RenderFrame(OpenTK.Windowing.Common.FrameEventArgs e)
        {
            GL.ClearColor(OpenTK.Mathematics.Color4.Black);

            foreach (var geometry in geometries)
            {
                geometry.Draw();
            }

            window.SwapBuffers();
        }

        private void Window_UpdateFrame(OpenTK.Windowing.Common.FrameEventArgs e)
        {

        }

        public void Dispose()
        {
            foreach (var geometry in geometries)
            {
                geometry.Dispose();
            }

            window.Dispose();
        }

        public void Run()
        {
            window.Run();
        }
    }
}