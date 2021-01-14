using System;
using OpenTK.Mathematics;

namespace CityBuildGame.ECS
{
    // TODO: Create Camera System
    public struct CameraComponent
    {
        private Vector3 front, right;
        private Vector3 position;

        private float yaw, pitch;

        private Matrix4 viewMatrix, projectionMatrix;

        private float fov;
        private float width, height;

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateViewMatrix();
            }
        }

        public Vector3 Front => front;

        public Vector3 Right => right;

        public float Width
        {
            set
            {
                width = value;
                UpdateProjectionMatrix();
            }
        }

        public float Height
        {
            set
            {
                height = value;
                UpdateProjectionMatrix();
            }
        }

        public float Yaw
        {
            get => yaw;
            set
            {
                yaw = 360 * (float)Math.Floor(value / 360);
                UpdateViewMatrix();
            }
        }

        public float Pitch
        {
            get => pitch;
            set
            {
                pitch = 360 * (float)Math.Floor(value / 360);
                UpdateViewMatrix();
            }
        }

        public (Matrix4 viewMatrix, Matrix4 projectionMatrix) Matrices => (viewMatrix, projectionMatrix);

        public CameraComponent(int width, int height, Vector3 position, float yaw, float pitch, float fov)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.yaw = yaw;
            this.pitch = pitch;
            this.fov = fov;
            this.front = this.right = new Vector3();
            this.viewMatrix = this.projectionMatrix = new Matrix4();

            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        private void UpdateViewMatrix()
        {
            front = Vector3.Normalize(new Vector3(
                (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw)),
                (float)Math.Sin(MathHelper.DegreesToRadians(pitch)),
                (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw))
            ));

#if DEBUG
            System.Diagnostics.Debug.WriteLine("front: " + front.ToString());
#endif

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));

            viewMatrix = Matrix4.LookAt(position, position + front, Vector3.UnitY);
        }

        private void UpdateProjectionMatrix()
        {
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), width / height, 0.1f, 100.0f);
        }
    }
}