using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityBuildGame.Rendering {
    public struct Vertex {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 textureCoord;

        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoord) {
            this.position = position;
            this.normal = normal;
            this.textureCoord = textureCoord;
        }
    }
}
