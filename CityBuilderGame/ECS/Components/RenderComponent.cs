using CityBuilderGame.Resources;

namespace CityBuilderGame.ECS
{
    struct RenderComponent
    {
        public Geometry geometry;
        public Shader shader;
        public Texture diffuse;
    }
}