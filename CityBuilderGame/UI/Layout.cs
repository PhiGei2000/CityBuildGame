using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Mathematics;
using static CityBuilderGame.UI.StackOrientation;

namespace CityBuilderGame.UI
{
    public abstract class UiContainer : UIComponent
    {
        protected List<UIComponent> children = new List<UIComponent>();

        public IEnumerable<UIComponent> Children { get => children; }

        public UIComponent this[int i]
        {
            get => children[i];
        }

        public void AddChild(UIComponent component)
        {
            children.Add(component);
            component.Parent = this;
        }

        public void RemoveChild(UIComponent component)
        {
            children.Remove(component);
            component.Parent = null;
        }

        public override void Render(in Matrix4 projection, in Vector2 windowSize)
        {
            base.Render(in projection, in windowSize);

            UpdateChildPositions(in windowSize);

            foreach (var child in children)
            {
                child.Render(in projection, in windowSize);
            }

        }

        protected abstract void UpdateChildPositions(in Vector2 windowSize);
    }

    public enum StackOrientation
    {
        ROW, ROW_REVERSE, COLUMN, COLUMN_REVERSE
    }

    public class StackLayout : UiContainer
    {
        public StackOrientation StackOrientation { get; set; }

        public IConstraint Spacing { get; set; }

        public StackLayout(StackOrientation orientation = COLUMN)
        {
            StackOrientation = orientation;
        }

        internal override void UpdateSize(in RectangleF parentSize)
        {
            base.UpdateSize(parentSize);

            RectangleF innerSize = GetInnerRectangle();
            foreach (var child in children)
            {
                child.UpdateSize(in innerSize);
            }
        }

        protected override void UpdateChildPositions(in Vector2 windowSize)
        {
            // Vector2 size = GetSize();
            // Vector2 parentPosition = GetPosition();
            RectangleF area = GetInnerRectangle();

            PointF offset = new PointF(0, 0);
            if (StackOrientation == ROW_REVERSE)
            {
                offset.X += area.Width;
            }
            else if (StackOrientation == COLUMN_REVERSE)
            {
                offset.Y += area.Height;
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].X = new AbsoluteConstraint(offset.X);
                children[i].Y = new AbsoluteConstraint(offset.Y);

                SizeF childSize = children[i].GetRectangle().Size;
                switch (StackOrientation)
                {
                    case ROW:
                        offset.X += childSize.Width + Spacing.GetValue(area.Width);
                        break;
                    case ROW_REVERSE:
                        offset.X -= childSize.Width + Spacing.GetValue(area.Width);
                        break;
                    case COLUMN:
                        offset.Y += childSize.Height + Spacing.GetValue(area.Height);
                        break;
                    case COLUMN_REVERSE:
                        offset.Y -= childSize.Height + Spacing.GetValue(area.Height);
                        break;
                }
            }
        }
    }
}