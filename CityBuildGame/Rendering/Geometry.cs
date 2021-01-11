﻿using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace CityBuildGame.Rendering
{
    public class Geometry : IDisposable
    {
        private int vao;
        private int ebo;
        private int vbo;
        private int drawCount;

        public Geometry()
        {
            vbo = GL.GenBuffer();
            ebo = GL.GenBuffer();

            vao = GL.GenVertexArray();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }

        public void BufferData(Vertex[] vertices, uint[] indices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 8 * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            drawCount = indices.Length;
        }

        public void Draw()
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, drawCount);
        }

        public void Dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
        }
    }
}
