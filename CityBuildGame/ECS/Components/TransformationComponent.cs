using OpenTK.Mathematics;

namespace CityBuildGame.ECS
{
    public struct TransformationComponent
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        private Matrix4 transform;

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateTransform();
            }
        }

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                UpdateTransform();
            }
        }

        public Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                UpdateTransform();
            }
        }

        public Matrix4 Transformation => transform;

        private void UpdateTransform()
        {
            transform = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(position);
        }
    }
}