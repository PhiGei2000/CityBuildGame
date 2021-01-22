using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using Leopotam.Ecs;

using ResourceManager = CityBuilderGame.Resources.ResourceManager;
using CityBuilderGame.ECS;
using CityBuilderGame.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common;
using System.Resources;
using CityBuilderGame.Resources;
using CityBuilderGame.UI;

namespace CityBuilderGame
{
    public class Game : IDisposable
    {
        private GameWindow window;
        private EcsWorld world;
        private EcsSystems systems;

        public MainMenu MainMenu;

        public Game(int width, int height, string title)
        {
            GameWindowSettings windowSettings = new GameWindowSettings()
            {
                RenderFrequency = 200
            };

            NativeWindowSettings nativeSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(width, height),
                Title = title
            };

            window = new GameWindow(windowSettings, nativeSettings);
            Init();

            window.RenderFrame += Window_RenderFrame;
            window.KeyDown += Window_KeyUp;
            window.Resize += Window_Resize;
        }

        private void Window_KeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                window.Close();
            }
            else if (e.Key == Keys.F)
            {
                if (window.IsFullscreen)
                {
                    window.WindowBorder = WindowBorder.Resizable;
                    window.WindowState = WindowState.Normal;
                    window.Size = new Vector2i(800, 600);
                }
                else
                {
                    window.WindowBorder = WindowBorder.Hidden;
                    window.WindowState = WindowState.Fullscreen;
                }
            }
        }

        private void Window_Resize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private void Window_RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            systems.Run();

            Vector2 size = window.Size;
            MainMenu.Draw(in size);

            window.SwapBuffers();
        }

        public void Dispose()
        {
            ResourceManager.Free();

            window.Dispose();
        }

        public void Init()
        {
            GL.ClearColor(Color4.LightSkyBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            world = new EcsWorld();
            systems = new EcsSystems(world)
                .Add(new RenderSystem())
                .Add(new CameraSystem())
                .Inject(window)
                .Inject(this);

            systems.Init();

            EcsEntity playerEntity = world.NewEntity();
            playerEntity.Replace(new CameraComponent(new Vector3(0, 5, 5), -90.0f, -45.0f, 60));

            EcsEntity groundEntity = world.NewEntity();
            groundEntity.Replace(new TransformationComponent()
            {
                Position = new Vector3(0, 0, 0),
                Rotation = new Quaternion(0, 0, 0),
                Scale = new Vector3(1, 1, 1)
            });

            groundEntity.Replace(new RenderComponent()
            {
                geometry = ResourceManager.GetResource<Geometry>("GROUND_GEOMETRY"),
                shader = ResourceManager.GetResource<Shader>("GROUND_SHADER"),
                diffuse = ResourceManager.GetResource<Texture>("GROUND_TEXTURE")
            });

            MainMenu = new MainMenu();
            window.Resize += (e) => MainMenu.Resize(e.Width, e.Height);
        }

        public void Run()
        {
            window.Run();
        }
    }
}