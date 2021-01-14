using System;
using OpenTK.Mathematics;

namespace CityBuildGame.ECS
{
    // TODO: Create Camera System
    public struct CameraComponent
    {
        public Vector3 front, right, up;
        public Vector3 position;

        public float yaw, pitch;

        public Matrix4 viewMatrix, projectionMatrix;

        public float fov;

        public (Matrix4 viewMatrix, Matrix4 projectionMatrix) Matrices => (viewMatrix, projectionMatrix);

        public bool vectorsOutdated;

        public CameraComponent(Vector3 position, float yaw, float pitch, float fov)
        {
            this.position = position;
            this.yaw = yaw;
            this.pitch = pitch;
            this.fov = fov;
            this.front = this.right = this.up = new Vector3();
            this.viewMatrix = this.projectionMatrix = new Matrix4();
            this.vectorsOutdated = true;
        }
    }
}