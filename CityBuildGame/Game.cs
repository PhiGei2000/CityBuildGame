using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using Leopotam.Ecs;

using CityBuildGame.Resources;
using CityBuildGame.ECS;
using CityBuildGame.Rendering;
using OpenTK.Mathematics;

namespace CityBuildGame
{
    public class Game : IDisposable
    {
        private ResourceManager resources;
        private GameWindow window;
        private EcsWorld world;
        private EcsSystems systems;

        public Game(int width, int height, string title)
        {
            GameWindowSettings windowSettings = new GameWindowSettings()
            {
                RenderFrequency = 200
            };

            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(width, height),
                Title = title
            };

            window = new GameWindow(windowSettings, nativeSettings);
            resources = new ResourceManager();
            Init();

            window.RenderFrame += Window_RenderFrame;
            window.KeyDown += Window_KeyUp;
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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(OpenTK.Mathematics.Color4.LightSkyBlue);

            systems.Run();

            window.SwapBuffers();
        }

        public void Dispose()
        {
            resources.Dispose();

            window.Dispose();
        }

        public void Init()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            world = new EcsWorld();
            systems = new EcsSystems(world)
                .Add(new RenderSystem())
                .Inject(window);

            systems.Init();

            EcsEntity playerEntity = world.NewEntity();
            playerEntity.Replace(new CameraComponent(window.Size.X, window.Size.Y, new Vector3(0, 5, 5), -90.0f, -45.0f, 60));

            EcsEntity groundEntity = world.NewEntity();
            groundEntity.Replace(new TransformationComponent()
            {
                Position = new Vector3(0, 0, 0),
                Rotation = new Quaternion(0, 0, 0),
                Scale = new Vector3(1, 1, 1)
            });

            groundEntity.Replace(new RenderComponent()
            {
                geometry = resources.GetResource<Geometry>(ResourceManager.ResourceIDs.GROUND_GEOMETRY).Get(),
                shader = resources.GetResource<Shader>(ResourceManager.ResourceIDs.GROUND_SHADER).Get()
            });
        }

        public void Run()
        {
            window.Run();
        }
    }
}