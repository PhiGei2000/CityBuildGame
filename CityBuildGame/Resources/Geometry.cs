using System;
using System.Collections.Generic;
using System.Text;
using CityBuildGame.Rendering;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace CityBuildGame.Resources
{
    public class Geometry : IResource<Geometry>
    {
        private int vao;
        private int ebo;
        private int vbo;
        private int drawCount;
        private bool disposedValue;

        public ResourceTypes ResourceType => ResourceTypes.GEOMETRY;

        public Geometry()
        {
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            ebo = GL.GenBuffer();
            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }

        public void BufferData(Vertex[] vertices, uint[] indices)
        {
            GL.NamedBufferData(vbo, sizeof(float) * 8 * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.NamedBufferData(ebo, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);

            drawCount = indices.Length;
        }

        public void Draw()
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, drawCount, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(ebo);
                GL.DeleteVertexArray(vao);
                disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }

        public Geometry Get()
        {
            return this;
        }
    }
}
