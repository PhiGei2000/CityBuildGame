using System.Drawing;
using CityBuilderGame.Resources;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace CityBuilderGame.UI
{
    public class UIComponent
    {
        internal RectangleF parentSize;

        public UIComponent Parent { get; internal set; }

        public Color4 BackgroundColor { get; set; }

        public IConstraint X { get; set; } = new AbsoluteConstraint(0);

        public IConstraint Y { get; set; } = new AbsoluteConstraint(0);

        public IConstraint Width { get; set; }

        public IConstraint Height { get; set; }

        public BoxConstraint Margin { get; set; } = new BoxConstraint(new AbsoluteConstraint(0));

        public BoxConstraint Padding { get; set; } = new BoxConstraint(new AbsoluteConstraint(0));

        public BoxConstraint BorderSize { get; set; } = new BoxConstraint(new AbsoluteConstraint(0));

        public Color4 BorderColor { get; set; } = Color4.Black;

        public bool IsVisible { get; set; } = true;

        public virtual void Render(in Matrix4 projection, in Vector2 windowSize)
        {
            if (!IsVisible) return;

            RectangleF area = GetRectangle();

            Vector4 borderSize = new Vector4(
                BorderSize.Top.GetValue(area.Height),
                BorderSize.Right.GetValue(area.Width),
                BorderSize.Bottom.GetValue(area.Height),
                BorderSize.Left.GetValue(area.Width)
            );


            Matrix3 transform = new Matrix3(
                area.Width, 0, area.X - windowSize.X * 0.5f,
                0, -area.Height, windowSize.Y * 0.5f - area.Y,
                0, 0, 1
            );

            Geometry renderQuad = ResourceManager.GetResource<Geometry>("RENDER_QUAD_GEOMETRY").Get();
            Shader renderQuadShader = ResourceManager.GetResource<Shader>("RENDER_QUAD_SHADER").Get();

            renderQuadShader.Use();
            renderQuadShader.Upload("transform", transform, true);
            renderQuadShader.Upload("projection", projection, true);
            renderQuadShader.Upload("backgroundColor", (Vector4)BackgroundColor);
            renderQuadShader.Upload("borderColor", (Vector4)BorderColor);
            renderQuadShader.Upload("borderSize", borderSize);

            renderQuad.Draw();
        }

        internal virtual void UpdateSize(in RectangleF parentSize)
        {
            this.parentSize = parentSize;
        }

        public virtual RectangleF GetRectangle()
        {
            RectangleF area = new RectangleF(
                parentSize.X + X.GetValue(parentSize.Width),
                parentSize.Y + Y.GetValue(parentSize.Height),
                Width.GetValue(parentSize.Width),
                Height.GetValue(parentSize.Height)
            );

            float marginTop = Margin.Top.GetValue(parentSize.Size.Height);
            float marginRight = Margin.Right.GetValue(parentSize.Size.Width);
            float marginBottom = Margin.Bottom.GetValue(parentSize.Size.Height);
            float marginLeft = Margin.Left.GetValue(parentSize.Size.Width);

            area.Offset(marginLeft, marginTop);
            area.Height -= marginTop + marginBottom;
            area.Width -= marginLeft + marginRight;

            return area;
        }

        public virtual RectangleF GetInnerRectangle()
        {
            RectangleF outerArea = GetRectangle();

            float paddingTop = Padding.Top.GetValue(outerArea.Size.Height);
            float paddingRight = Padding.Right.GetValue(outerArea.Size.Width);
            float paddingBottom = Padding.Bottom.GetValue(outerArea.Size.Height);
            float paddingLeft = Padding.Left.GetValue(outerArea.Size.Width);

            outerArea.Offset(paddingLeft, paddingTop);
            outerArea.Height -= paddingTop + paddingBottom;
            outerArea.Width -= paddingLeft + paddingRight;

            return outerArea;
        }
    }
}