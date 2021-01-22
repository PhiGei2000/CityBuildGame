using System;
using System.Drawing;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace CityBuilderGame.UI
{
    public class Button : Label
    {
        public event EventHandler OnClick;

        public Button(string text) : base(text)
        {

        }

        internal override void HandleOnMouseDown(MouseButtonEventArgs e)
        {
            if (mouseOver)
            {
                OnClick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}