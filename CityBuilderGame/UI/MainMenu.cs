using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace CityBuilderGame.UI
{
    public class MainMenu
    {
        private Matrix4 projection;

        private StackLayout mainLayout;

        public bool IsVisible
        {
            get => mainLayout.IsVisible;
            set => mainLayout.IsVisible = value;
        }

        public MainMenu()
        {
            mainLayout = new StackLayout
            {
                X = new AbsoluteConstraint(25),
                Y = new RelativeConstraint(0.4f),
                Height = new AbsoluteConstraint(150),
                Width = new AbsoluteConstraint(50),
                BackgroundColor = Color4.Transparent,
                Spacing = new AbsoluteConstraint(10)
            };

            mainLayout.AddChild(new Label("New Game")
            {
                Height = new AbsoluteConstraint(50),
                Width = new AbsoluteConstraint(100),
                BackgroundColor = new Color4(0.1f, 0.1f, 0.7f, 0.2f)
            });
        }

        public void Draw(in Vector2 windowSize)
        {
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);

            mainLayout.Render(in projection, in windowSize);

            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Resize(int width, int height)
        {
            projection = Matrix4.CreateOrthographic(width, height, 0, 1);

            RectangleF size = new RectangleF(0, 0, width, height);
            mainLayout.UpdateSize(in size);
        }
    }
}