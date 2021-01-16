using CityBuilderGame.Resources;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace CityBuilderGame.UI
{
    public class UIComponent
    {
        public UIComponent Parent { get; internal set; }

        public Color4 BackgroundColor { get; set; }

        public UiConstraint Position { get; set; }

        public UiConstraint Size { get; set; }

        public bool IsVisible { get; set; } = true;

        public virtual void Render()
        {
            if (!IsVisible) return;

            Vector2 position = GetPosition();
            Vector2 size = GetSize();

            Matrix3 transform = new Matrix3(
                size.X * 2, 0, position.X * 2 - 1.0f,
                0, -size.Y * 2, 1.0f - position.Y * 2,
                0, 0, 1
            );

            Geometry renderQuad = ResourceManager.GetResource<Geometry>("RENDER_QUAD_GEOMETRY").Get();
            Shader renderQuadShader = ResourceManager.GetResource<Shader>("RENDER_QUAD_SHADER").Get();

            renderQuadShader.Use();
            renderQuadShader.Upload("transform", transform, true);
            renderQuadShader.Upload("backgroundColor", (Vector4)BackgroundColor);

            renderQuad.Draw();
        }

        protected virtual Vector2 GetPosition()
        {
            Vector2 parentPosition = Parent?.GetPosition() ?? new Vector2(0, 0);
            Vector2 parentSize = Parent?.GetSize() ?? new Vector2(1, 1);
            return parentPosition + Position.GetValue(parentSize);
        }

        protected virtual Vector2 GetSize()
        {
            Vector2 parentSize = Parent?.GetSize() ?? new Vector2(1, 1);
            return Size.GetValue(parentSize);
        }
    }
}