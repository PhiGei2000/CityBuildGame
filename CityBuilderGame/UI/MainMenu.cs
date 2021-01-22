using System;
using System.Drawing;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

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

        public event EventHandler OnButtonClick;

        public MainMenu(GameWindow window)
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
            window.MouseMove += mainLayout.HandleOnMouseMove;
            window.MouseDown += mainLayout.HandleOnMouseDown;

            EventHandler eventHandlerMouseEnter = (sender, e) =>
            {
                (sender as UIComponent).BackgroundColor = new Color4(0.0f, 0.0f, 0.7f, 0.5f);
            };

            EventHandler eventHandlerMouseLeave = (sender, e) =>
            {
                (sender as UIComponent).BackgroundColor = new Color4(0.1f, 0.1f, 0.7f, 0.2f);
            };

            EventHandler eventHandlerButtonClick = (sender, e) =>
            {
                OnButtonClick?.Invoke(sender, e);
            };

            Button CreateButton(string title)
            {
                Button button = new Button(title)
                {
                    Height = new AbsoluteConstraint(30),
                    Width = new AbsoluteConstraint(100),
                    BackgroundColor = new Color4(0.1f, 0.1f, 0.7f, 0.2f)
                };
                button.OnMouseEnter += eventHandlerMouseEnter;
                button.OnMouseLeave += eventHandlerMouseLeave;
                button.OnClick += eventHandlerButtonClick;

                return button;
            }

            mainLayout.AddChild(CreateButton("New Game"));
            mainLayout.AddChild(CreateButton("Load Game"));
            mainLayout.AddChild(CreateButton("Settings"));
            mainLayout.AddChild(CreateButton("Quit"));
        }

        public void Draw(in Vector2 windowSize)
        {
            if (IsVisible)
            {
                GL.Enable(EnableCap.Blend);
                GL.Disable(EnableCap.DepthTest);

                mainLayout.Render(in projection, in windowSize);

                GL.Disable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
            }
        }

        public void Resize(int width, int height)
        {
            projection = Matrix4.CreateOrthographic(width, height, 0, 1);

            RectangleF size = new RectangleF(0, 0, width, height);
            mainLayout.UpdateSize(in size);
        }
    }
}