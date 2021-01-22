using System;
using OpenTK.Mathematics;

namespace CityBuilderGame.UI
{

    public enum Alignment
    {
        CENTER, START, END
    }

    public interface IConstraint
    {
        float GetValue(float value);
    }

    public class AbsoluteConstraint : IConstraint
    {
        public float Value { get; set; }

        public AbsoluteConstraint(float value)
        {
            Value = value;
        }

        public float GetValue(float value)
        {
            return Value;
        }

        public static implicit operator AbsoluteConstraint(float value)
        {
            return new AbsoluteConstraint(value);
        }
    }

    public class RelativeConstraint : IConstraint
    {
        public float Factor { get; set; }

        public RelativeConstraint(float factor)
        {
            Factor = factor;
        }

        public float GetValue(float value)
        {
            return value * Factor;
        }
    }

    public class BoxConstraint
    {
        public IConstraint Top { get; set; }

        public IConstraint Right { get; set; }

        public IConstraint Bottom { get; set; }

        public IConstraint Left { get; set; }

        public BoxConstraint(IConstraint length)
        {
            Top = Right = Bottom = Left = length;
        }

        public BoxConstraint(IConstraint top, IConstraint right, IConstraint bottom, IConstraint left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}