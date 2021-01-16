using Leopotam.Ecs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

using OpenTK.Graphics.OpenGL4;

namespace CityBuilderGame.ECS
{
    class RenderSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsFilter<RenderComponent, TransformationComponent> filter;
        private EcsFilter<CameraComponent> cameraFilter;
        private Game game;

        public void Init()
        {

        }

        public void Run()
        {
            ref CameraComponent camera = ref cameraFilter.Get1(0);
            (Matrix4 viewMatrix, Matrix4 projectionMatrix) = camera.Matrices;

            foreach (var entity in filter)
            {
                ref RenderComponent renderComponent = ref filter.Get1(entity);
                ref TransformationComponent transformationComponent = ref filter.Get2(entity);

                renderComponent.shader.Use();
                renderComponent.shader.Upload("model", transformationComponent.Transformation);
                renderComponent.shader.Upload("view", viewMatrix);
                renderComponent.shader.Upload("projection", projectionMatrix);

                renderComponent.geometry.Draw();
            }

            GL.Enable(EnableCap.Blend);
            game.MainMenu.Render();
            GL.Disable(EnableCap.Blend);
        }

        public void Destroy()
        {

        }

    }
}