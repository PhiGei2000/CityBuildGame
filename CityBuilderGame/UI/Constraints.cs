using System;
using OpenTK.Mathematics;

namespace CityBuilderGame.UI
{
    public interface IConstraint
    {
        float GetValue(float parentValue);
    }

    public struct AbsoluteConstraint : IConstraint
    {
        private float value;

        public float Value { set => this.value = value; }

        public AbsoluteConstraint(float value)
        {
            this.value = value;
        }

        public float GetValue(float parentValue)
        {
            return value;
        }
    }

    public struct RelativeConstraint : IConstraint
    {
        private float factor;

        public float RelativeSize { set => factor = value; }

        public RelativeConstraint(float relativeSize)
        {
            factor = relativeSize;
        }

        public float GetValue(float parentValue)
        {
            return parentValue * factor;
        }
    }

    public struct UiConstraint
    {
        public IConstraint X { get; set; }

        public IConstraint Y { get; set; }

        public Vector2 GetValue(Vector2 parentValue)
        {
            return new Vector2(
                X.GetValue(parentValue.X),
                Y.GetValue(parentValue.Y)
            );
        }
    }
}