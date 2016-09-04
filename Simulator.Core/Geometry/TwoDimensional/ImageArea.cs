using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using QuickFont;

namespace Simulator.Core.Geometry.TwoDimensional
{
    public class ImageArea : ClickableArea, IDisposable
    {
        private int textureID;
        private bool containsTransparency;

        public ImageArea(string filePath, System.Drawing.Color? transparentColor = null, int x0 = 0, int y0 = 0, int width = 128, int height = 128, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            X0 = x0;
            Y0 = y0;
            this.Width = width;
            this.Height = height;
            System.Drawing.Bitmap image = new Bitmap(filePath);

            ///use XML to specify image files & if they should be transparent :)
            if (transparentColor != null)
            {
                image.MakeTransparent((System.Drawing.Color)transparentColor);
                containsTransparency = true;
            }

            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            image.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, textureParameterName, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, textureParameterName, (int)textureMinFilter);
        }

        public void Graph()
        {
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f); //disable any previous colors

            bool texture2DWasEnabled = GL.IsEnabled(EnableCap.Texture2D);
            bool depthTestWasEnabled = GL.IsEnabled(EnableCap.DepthTest);
            bool lightingWasEnabled = GL.IsEnabled(EnableCap.Lighting);

            GL.Disable(EnableCap.DepthTest); //painter's algorithm
            GL.Disable(EnableCap.Lighting);


            /******************************************
             * More Efficient if this is done once in the calling
             * function instead of for each image area
             * + it's hard to know the width & height of the 
             * entire window for the GL.Ortho(....) command
             * - so those might have to be passsed in as parameters...
             ***********************************************
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, windowWidth, windowHeight, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            *********************************************/

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, this.textureID);
            if (containsTransparency)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }

            GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0); GL.Vertex2(X0, Y0);
                GL.TexCoord2(1, 0); GL.Vertex2(X0 + Width, Y0);
                GL.TexCoord2(1, 1); GL.Vertex2(X0 + Width, Y0 + Height);
                GL.TexCoord2(0, 1); GL.Vertex2(X0, Y0 + Height);
            GL.End();

            //restore the origional settings
            if (texture2DWasEnabled)
            { GL.Enable(EnableCap.Texture2D); }
            else
            { GL.Disable(EnableCap.Texture2D); }

            if (depthTestWasEnabled)
            { GL.Enable(EnableCap.DepthTest); }
            else
            { GL.Disable(EnableCap.DepthTest); }

            if (lightingWasEnabled)
            { GL.Enable(EnableCap.Lighting); }
            else
            { GL.Disable(EnableCap.Lighting); }
        }

        public void Dispose()
        {
            try
            {
                GL.DeleteTexture(textureID);
                textureID = -1;
            }
            catch (Exception)
            {
                //noting to do
            }
        }


    }
}
