using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using CityBuildGame.Rendering;
using OpenTK.Mathematics;

namespace CityBuildGame.Resources
{
    struct Model
    {
        public Geometry geometry;
        public Material material;
    }

    public abstract class ObjLoader
    {
        // private Vector3 ReadColor(string[] colorString)
        // {
        //     float r = Convert.ToSingle(colorString[1]);
        //     float g = Convert.ToSingle(colorString[2]);
        //     float b = Convert.ToSingle(colorString[3]);

        //     return new Vector3(r, g, b);
        // }

        // private static Material LoadMaterial(string materialFile)
        // {
        //     Material mtl = new Material();
        //     using StreamReader reader = new StreamReader(materialFile);

        //     string line;
        //     while (!reader.EndOfStream)
        //     {
        //         line = reader.ReadLine();
        //         string[] lineParts = line.Split(' ');

        //         if (lineParts[0] == "newmtl")
        //         {
        //             mtl.Name = lineParts[1];
        //         }
        //         // ambient reflectifity
        //         else if (lineParts[0] == "Ka")
        //         {
        //             mtl.ambient = ReadColor(lineParts[1..3]);
        //         }
        //         // diffuse reflectifity
        //         else if (lineParts[0] == "Kd")
        //         {
        //             mtl.diffuse = ReadColor(lineParts[1..3]);
        //         }
        //         // specular reflectifity
        //         else if (lineParts[0] == "Ks")
        //         {
        //             mtl.diffuse = ReadColor(lineParts[1..3]);
        //         }
        //         // specular exponent
        //         else if (lineParts[0] == "Ns")
        //         {
        //             mtl.specularExponent = Convert.ToSingle(lineParts[1]);
        //         }
        //         // optical density
        //         else if (lineParts[0] == "Ni")
        //         {
        //             mtl.opticalDensity = Convert.ToSingle(lineParts[1]);
        //         }
        //         else if (lineParts[0] == "d")
        //         {

        //         }
        //     }

        //     return mtl;
        // }
        private static Vector2 ReadVector2(string[] lineParts)
        {
            float x = Convert.ToSingle(lineParts[0], CultureInfo.InvariantCulture);
            float y = Convert.ToSingle(lineParts[1], CultureInfo.InvariantCulture);
            return new Vector2(x, y);
        }

        private static Vector3 ReadVector3(string[] lineParts)
        {
            float x = Convert.ToSingle(lineParts[0], CultureInfo.InvariantCulture);
            float y = Convert.ToSingle(lineParts[1], CultureInfo.InvariantCulture);
            float z = Convert.ToSingle(lineParts[2], CultureInfo.InvariantCulture);
            return new Vector3(x, y, z);
        }

        public static Geometry LoadGeometry(string filename)
        {
            using StreamReader reader = new StreamReader(filename);
            List<Vector3> positions = new List<Vector3>();
            List<Vector2> texCoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<(int pos, int tex, int normal)> verticesData = new List<(int pos, int tex, int normal)>();
            List<uint> indices = new List<uint>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split('\x20');

                switch (parts[0])
                {
                    case "v":
                        positions.Add(ReadVector3(parts[1..4]));
                        break;
                    case "vt":
                        texCoords.Add(ReadVector2(parts[1..3]));
                        break;
                    case "vn":
                        normals.Add(ReadVector3(parts[1..4]));
                        break;
                    case "f":
                        for (int i = 1; i <= 3; i++)
                        {
                            string[] dataParts = parts[i].Split('/');
                            var data = (
                                Convert.ToInt32(dataParts[0]),
                                Convert.ToInt32(dataParts[1]),
                                Convert.ToInt32(dataParts[2])
                            );

                            int index = verticesData.IndexOf(data);

                            if (index == -1)
                            {
                                verticesData.Add(data);
                                index = verticesData.Count - 1;
                            }
                            indices.Add((uint)index);
                        }
                        break;
                }
            }
            Vertex[] vertices = verticesData.Select((data) =>
            {
                return new Vertex(positions[data.pos - 1], normals[data.normal - 1], texCoords[data.tex - 1]);
            }).ToArray();

            Geometry geometry = new Geometry();
            geometry.BufferData(vertices, indices.ToArray());

            return geometry;
        }
    }
}