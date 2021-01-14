using OpenTK.Mathematics;

namespace CityBuildGame.Resources
{
    public class Material : IResource<Material>
    {
        public ResourceTypes ResourceType => ResourceTypes.MATERIAL;

        public string Name;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float specularExponent;
        public float factor;
        public float opticalDensity;

        public void Dispose()
        {

        }

        public Material Get()
        {
            return this;
        }
    }
}