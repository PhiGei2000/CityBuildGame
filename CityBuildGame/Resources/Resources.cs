using CityBuildGame.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CityBuildGame.Resources
{
    public interface IResource : IDisposable
    {
        ResourceTypes ResourceType { get; }
    }

    public interface IResource<T> : IResource where T : IResource
    {
        T Get();
    }

    public enum ResourceTypes
    {
        SHADER, TEXTURE, GEOMETRY, MATERIAL
    }

    public class ResourceManager : IDisposable
    {
        public enum ResourceIDs
        {
            GROUND_SHADER,
            GROUND_TEXTURE,
            GROUND_GEOMETRY
        }

        private Dictionary<ResourceIDs, IResource> resources = new Dictionary<ResourceIDs, IResource>();
        private bool disposedValue;
        public ResourceManager()
        {
            LoadResources();
        }

        public IResource<T> GetResource<T>(ResourceIDs resourceID) where T : IResource
        {
            if (resources[resourceID] is IResource<T> value)
            {
                return value;
            }

            throw new ArgumentOutOfRangeException(nameof(resourceID));
        }

        private void LoadResources()
        {
            XNamespace xmlns = "http://www.citybuildgame.de/resources";
            XName resourceName = xmlns.GetName("Resource");
            XDocument doc = XDocument.Load("./Resources/Resources.xml");

            foreach (var resourceType in doc.Root.Elements())
            {
                string elementName = resourceType.Name.LocalName;
                ResourceTypes? type = elementName switch
                {
                    "Shaders" => ResourceTypes.SHADER,
                    "Textures" => ResourceTypes.TEXTURE,
                    "Geometries" => ResourceTypes.GEOMETRY,
                    "Materials" => ResourceTypes.MATERIAL,
                    _ => null
                };

                if (type == null)
                {
                    continue;
                }

                foreach (var resource in resourceType.Elements(resourceName))
                {
                    LoadResource(type.Value, resource);
                }
            }

        }

        private void LoadResource(ResourceTypes type, XElement resourceElement)
        {
            ResourceIDs id = Enum.Parse<ResourceIDs>(resourceElement.Attribute("id").Value);
            string filename = Path.Combine("Resources", resourceElement.Attribute("filename").Value);
            switch (type)
            {
                case ResourceTypes.SHADER:
                    resources.Add(id, new Shader(filename));
                    break;
                case ResourceTypes.GEOMETRY:
                    resources.Add(id, ObjLoader.LoadGeometry(filename));
                    break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                foreach (var (_, resource) in resources)
                {
                    resource.Dispose();
                }

                disposedValue = true;
            }
        }

        ~ResourceManager()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
