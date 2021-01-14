using Leopotam.Ecs;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace CityBuildGame.ECS
{
    class RenderSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorld world;
        private EcsFilter<RenderComponent, TransformationComponent> filter;
        private EcsFilter<CameraComponent> cameraFilter;
        private GameWindow gameWindow;

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
        }

        public void Destroy()
        {

        }

    }
}