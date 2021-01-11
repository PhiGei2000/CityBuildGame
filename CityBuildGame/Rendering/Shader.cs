using System;
using System.Collections.Generic;
using System.IO;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CityBuildGame.Rendering
{
    public class Shader : IDisposable
    {
        private Dictionary<string, int> uniformLocations;
        private int program;
        private bool disposedValue;

        public Shader(string filename)
        {
            string vertexShaderSource = File.ReadAllText($"{filename}.vert");
            string fragmentShaderSource = File.ReadAllText($"{filename}.frag");

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            CompileShader(vertexShader, vertexShaderSource);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            CompileShader(fragmentShader, fragmentShaderSource);

            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            GL.LinkProgram(program);

            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private void CompileShader(int shader, string shaderSource)
        {
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);
            if (infoLog != string.Empty)
            {
                Console.WriteLine(infoLog);
            }
        }

        public void Use()
        {
            GL.UseProgram(program);
        }

        public void Upload<T>(string location, T value)
        {
            int uniformLocation;
            if (uniformLocations.ContainsKey(location))
            {
                uniformLocation = uniformLocations[location];
            }
            else
            {
                uniformLocation = GL.GetAttribLocation(program, location);
                uniformLocations.Add(location, uniformLocation);
            }

            switch (value)
            {
                case float fl:
                    GL.Uniform1(uniformLocation, fl);
                    break;
                case Vector2 vec2:
                    GL.Uniform2(uniformLocation, vec2);
                    break;
                case Vector3 vec3:
                    GL.Uniform3(uniformLocation, vec3);
                    break;
                case Matrix3 mat3:
                    GL.UniformMatrix3(uniformLocation, true, ref mat3);
                    break;
                case Matrix4 mat4:
                    GL.UniformMatrix4(uniformLocation, true, ref mat4);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(program);

                disposedValue = true;
            }
        }

        ~Shader()
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