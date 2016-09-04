using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_Extensions
{
    public static class Helpers
    {
        /// <summary>
        /// System.Drawing.Color to unsigned integer 
        /// useful for VBO
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static uint SystemColorToRGBA(System.Drawing.Color color)
        {
            return (uint)color.A << 24 | (uint)color.B << 16 | (uint)color.G << 8 | (uint)color.R;
        }

        /// <summary>
        /// unsigned integer to System.Drawing.Color
        /// useful for VBO to System.Drawing.Color for File IO
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color UIntToSystemColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }



        /// <summary>
        /// make a System.Drawing.Bitmap object from tthe current OpenTK screen buffer
        /// </summary>
        /// <param name="clientRectangle"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GrabScreenBuffer(System.Drawing.Rectangle clientRectangle)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(clientRectangle.Width, clientRectangle.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(clientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, clientRectangle.Width, clientRectangle.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);

            return bmp;
        }
    }
}
