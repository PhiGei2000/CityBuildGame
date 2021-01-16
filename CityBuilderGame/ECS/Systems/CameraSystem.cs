using System;
using Leopotam.Ecs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CityBuilderGame.ECS
{
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameWindow window;
        private EcsFilter<CameraComponent> filter;

        private Vector2 lastMousePos;

        public void Init()
        {
            window.Resize += (e) =>
            {
                ref CameraComponent camera = ref filter.Get1(0);
                UpdateProjectionMatrix(ref camera, e.Size);
            };
        }

        public void Run()
        {
            float dx = 0, dy = 0;
            int moveX = 0, moveZ = 0;
            var keyboardState = window.KeyboardState;
            if (keyboardState.IsAnyKeyDown)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    moveX++;
                }

                if (keyboardState.IsKeyDown(Keys.S))
                {
                    moveX--;
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    moveZ++;
                }

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    moveZ--;
                }

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    dy -= 10;
                }

                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    dy += 10;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    dx += 10;
                }

                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    dx -= 10;
                }
            }

            if (window.IsMouseButtonPressed(MouseButton.Left))
            {
                lastMousePos = window.MousePosition;
            }
            else if (window.IsMouseButtonDown(MouseButton.Left))
            {
                (window.MousePosition - lastMousePos).Deconstruct(out dx, out dy);
                lastMousePos = window.MousePosition;
            }

            ref CameraComponent camera = ref filter.Get1(0);
            if (moveX != 0 || moveZ != 0)
            {
                Vector3 frontNoY = Vector3.Normalize(new Vector3(camera.front.X, 0, camera.front.Z));
                camera.position += Vector3.Normalize(moveX * frontNoY + moveZ * camera.right) * 0.1f;
                camera.vectorsOutdated = true;
            }

            if (dx != 0 || dy != 0)
            {
                camera.pitch = Math.Clamp(camera.pitch - dy * 0.1f, -89, 0);


                camera.yaw += dx * 0.1f;
                while (camera.yaw >= 180)
                {
                    camera.yaw -= 360;
                }

                while (camera.yaw < 180)
                {
                    camera.yaw += 360;
                }
                camera.vectorsOutdated = true;
            }

            if (camera.vectorsOutdated)
            {
                UpdateCameraVectors(ref camera);
                UpdateViewMatrix(ref camera);
                camera.vectorsOutdated = false;
            }
        }

        private static void UpdateCameraVectors(ref CameraComponent camera)
        {
            camera.front = Vector3.Normalize(new Vector3(
                    (float)Math.Cos(MathHelper.DegreesToRadians(camera.pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(camera.yaw)),
                    (float)Math.Sin(MathHelper.DegreesToRadians(camera.pitch)),
                    (float)Math.Cos(MathHelper.DegreesToRadians(camera.pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(camera.yaw))
                ));

            camera.right = Vector3.Normalize(Vector3.Cross(camera.front, Vector3.UnitY));
            camera.up = Vector3.Normalize(Vector3.Cross(camera.right, camera.front));

#if DEBUG
            System.Diagnostics.Debug.WriteLine("camera front: " + camera.front.ToString());
#endif
        }

        private static void UpdateViewMatrix(ref CameraComponent camera)
        {
            camera.viewMatrix = Matrix4.LookAt(camera.position, camera.position + camera.front, Vector3.UnitY);
        }

        private static void UpdateProjectionMatrix(ref CameraComponent camera, Vector2i size)
        {
            camera.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.fov), (float)size.X / size.Y, 0.1f, 100.0f);
        }
    }
}