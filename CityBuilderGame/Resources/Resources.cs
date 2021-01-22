using CityBuilderGame.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using OpenTK.Graphics.OpenGL4;

namespace CityBuilderGame.Resources
{
    public interface IResource : IDisposable
    {
        ResourceTypes ResourceType { get; }
    }

    public interface IResource<T> : IResource where T : IResource
    {
    }

    public enum ResourceTypes
    {
        SHADER, TEXTURE, GEOMETRY, MATERIAL, FONT
    }

    public static class ResourceManager
    {
        private static Dictionary<string, IResource> resources = new Dictionary<string, IResource>();
        private static bool disposedValue = false;
        static ResourceManager()
        {
            LoadResources();
        }

        public static T GetResource<T>(string resourceID) where T : IResource
        {
            if (resources[resourceID] is T value)
            {
                return value;
            }

            throw new ArgumentOutOfRangeException(nameof(resourceID));
        }

        private static void LoadResources()
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

            // TODO: Move this to resource file
            // create renderQuad geometry
            float[] vertices = {
                0.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 0.0f,
                0.0f, 1.0f };

            uint[] indices = { 0, 1, 2, 0, 3, 1 };

            Geometry renderQuad = new Geometry(new VertexAttribute()
            {
                Location = 0,
                Size = 2,
                Type = VertexAttribPointerType.Float
            });
            renderQuad.BufferData(vertices, indices, 2 * sizeof(float));
            resources.Add("RENDER_QUAD_GEOMETRY", renderQuad);

            // create texturedRenderQuad geometry
            vertices = new float[] {
                0.0f, 0.0f, 0.0f, 0.0f,
                1.0f, 1.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 1.0f };

            Geometry texturedRenderQuad = new Geometry(
                new VertexAttribute()
                {
                    Location = 0,
                    Size = 2,
                    Type = VertexAttribPointerType.Float
                },
                new VertexAttribute()
                {
                    Location = 1,
                    Size = 2,
                    Type = VertexAttribPointerType.Float
                }
            );
            texturedRenderQuad.BufferData(vertices, indices, 4 * sizeof(float));
            resources.Add("TEXTURED_RENDER_QUAD_GEOMETRY", texturedRenderQuad);
        }

        private static void LoadResource(ResourceTypes type, XElement resourceElement)
        {
            string id = resourceElement.Attribute("id").Value;
            if (resourceElement.Attribute("filename") != null)
            {
                string filename = Path.Combine("Resources", resourceElement.Attribute("filename").Value);
                switch (type)
                {
                    case ResourceTypes.SHADER:
                        resources.Add(id, new Shader(filename));
                        break;
                    case ResourceTypes.GEOMETRY:
                        resources.Add(id, ObjLoader.LoadGeometry(filename));
                        break;
                    case ResourceTypes.TEXTURE:
                        resources.Add(id, new Texture(filename));
                        break;
                }
            }
        }

        // private static Geometry LoadGeometry(XElement resource)
        // {
        //     XNamespace geometryNs = "http://www.citybuildgame.de/resources/Geometry";
        //     // parse vertex attributes
        //     List<VertexAttribute> attributes = new List<VertexAttribute>();
        //     var vertexAttributesElement = resource.Element(geometryNs.GetName("VertexAttributes"));
        //     if (vertexAttributesElement != null)
        //     {
        //         attributes.AddRange(from attribute in vertexAttributesElement.Elements(geometryNs.GetName("VertexAttribute"))
        //                             select new VertexAttribute()
        //                             {
        //                                 Location = Convert.ToInt32(attribute.Attribute("location").Value),
        //                                 Size = Convert.ToInt32(attribute.Attribute("size").Value),
        //                                 Type = Enum.Parse<VertexAttribPointerType>(attribute.Attribute("type").Value)
        //                             });
        //     }


        // }

        public static void Free()
        {
            if (!disposedValue)
            {
                foreach (var (_, resource) in resources)
                {
                    resource.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}
