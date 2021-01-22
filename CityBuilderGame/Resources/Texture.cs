using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL4;

namespace CityBuilderGame.Resources
{
    public class Texture : IResource<Texture>
    {
        uint texture;
        TextureUnit activeUnit;
        bool active = false;

        public ResourceTypes ResourceType => ResourceTypes.TEXTURE;

        public Texture(string filename)
        {
            GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap tex = new Bitmap(filename);
            BitmapData data = tex.LockBits(new Rectangle(0, 0, tex.Width, tex.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            tex.UnlockBits(data);
        }

        public void Dispose()
        {
            GL.DeleteTexture(texture);
            System.GC.SuppressFinalize(this);
        }
    }
}