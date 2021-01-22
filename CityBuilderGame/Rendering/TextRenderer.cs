using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;

namespace CityBuilderGame.Rendering
{
    public class TextRenderer : IDisposable
    {
        private Bitmap bitmap;
        private Graphics gfx;
        private Rectangle dirty_region;
        private int texture;
        private bool disposedValue;

        public SizeF Size
        {
            get => bitmap.Size;
        }

        public TextRenderer(int width, int height)
        {
            bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bitmap);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
        }

        public SizeF GetTextSize(string text, Font font)
        {
            return gfx.MeasureString(text, font);
        }

        public void Resize(int width, int height)
        {
            bitmap.Dispose();
            gfx.Dispose();

            bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfx = Graphics.FromImage(bitmap);

            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            Clear();
        }

        public void Clear()
        {
            gfx.Clear(Color.Transparent);
            dirty_region = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        }


        public void DrawString(string text, Font font, Brush brush, PointF position)
        {
            gfx.DrawString(text, font, brush, position);

            SizeF size = gfx.MeasureString(text, font);
            dirty_region = Rectangle.Round(RectangleF.Union(dirty_region, new RectangleF(position, size)));
            dirty_region = Rectangle.Intersect(dirty_region, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }

        public void Upload()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);
            if (dirty_region != Rectangle.Empty)
            {
                BitmapData data = bitmap.LockBits(dirty_region, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexSubImage2D(TextureTarget.Texture2D, 0, dirty_region.X, dirty_region.Y, dirty_region.Width, dirty_region.Height, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);
                dirty_region = Rectangle.Empty;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    bitmap.Dispose();
                    gfx.Dispose();

                    GL.DeleteTexture(texture);
                }

                disposedValue = true;
            }
        }

        ~TextRenderer()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}