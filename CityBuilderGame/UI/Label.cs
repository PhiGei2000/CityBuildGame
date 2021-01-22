using System.Drawing;
using CityBuilderGame.Rendering;
using CityBuilderGame.Resources;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace CityBuilderGame.UI
{
    public class Label : UIComponent
    {
        private TextRenderer renderer;

        public Font Font { get; set; } = new Font("Arial", 12);

        public Alignment TextHorizontalAlignment { get; set; } = Alignment.CENTER;

        public Alignment TextVerticalAlignment { get; set; } = Alignment.CENTER;

        private string text;

        public Label(string text)
        {
            renderer = new TextRenderer(1, 1);
            this.text = text;
        }

        public override void Render(in Matrix4 projection, in Vector2 windowSize)
        {
            base.Render(projection, windowSize);

            RectangleF innerArea = GetInnerRectangle();
            if (innerArea.Size != renderer.Size)
            {
                PointF textPos = GetTextPosition();

                renderer.Resize((int)innerArea.Width, (int)innerArea.Height);
                renderer.DrawString(text, Font, Brushes.Black, textPos);
            }

            Matrix3 transform = new Matrix3(
                innerArea.Width, 0, innerArea.X - windowSize.X * 0.5f,
                0, -innerArea.Height, windowSize.Y * 0.5f - innerArea.Y,
                0, 0, 1
            );

            Shader texturedShader = ResourceManager.GetResource<Shader>("TEXTURED_QUAD_SHADER");
            Geometry renderQuad = ResourceManager.GetResource<Geometry>("TEXTURED_RENDER_QUAD_GEOMETRY").Get();

            GL.ActiveTexture(TextureUnit.Texture0);
            renderer.Upload();

            texturedShader.Use();
            texturedShader.Upload("quadTexture", 0);
            texturedShader.Upload("projection", projection, true);
            texturedShader.Upload("transform", transform, true);

            renderQuad.Draw();

        }

        private PointF GetTextPosition()
        {
            SizeF textSize = renderer.GetTextSize(text, Font);
            RectangleF textArea = GetInnerRectangle();
            PointF textPosition = new PointF();

            switch (TextHorizontalAlignment)
            {
                case Alignment.CENTER:
                    textPosition.X += (textArea.Width - textSize.Width) / 2.0f;
                    break;
                case Alignment.END:
                    textPosition.X += textArea.Width - textSize.Width;
                    break;
            }

            switch (TextVerticalAlignment)
            {
                case Alignment.CENTER:
                    textPosition.Y += (textArea.Height - textSize.Height) / 2.0f;
                    break;
                case Alignment.END:
                    textPosition.Y += textArea.Height - textSize.Height;
                    break;
            }

            return textPosition;
        }
    }
}