using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Simulator.Core
{
    public static class Helpers
    {
        private static float degreesToRadiansRatio;
        private static float radiansToDegreesRatio;
        private static Random rand;

        static Helpers() // this static constructor initializes these values just once long before the helper is called at run time
        {
            degreesToRadiansRatio = ((float) Math.PI) / 180;
            radiansToDegreesRatio = 180 / (float)Math.PI;
            rand = new Random(); //field initialization would also work
        }

        /// <summary>
        /// random single percision floating point number
        /// </summary>
        /// <param name="allowNegative"></param>
        /// <returns></returns>
        public static float RandomFloat(bool allowNegative = true)
        {
            if (allowNegative)
            {
                double d = rand.NextDouble();
                if (d > 0.5d)
                {
                    return (float)rand.NextDouble();
                }
                else
                {
                    return -(float)rand.NextDouble();
                }
            }
            else
            {
                return (float)rand.NextDouble();
            }
        }

        /// <summary>
        /// random 8 bit number
        /// </summary>
        /// <returns></returns>
        public static byte RandomByte() //this allows me to store an individual random byte to a variable so it can be compared later
        {
            byte[] byteHolder = new byte[1];
            rand.NextBytes(byteHolder);
            return byteHolder[0];
        }

        /// <summary>
        /// degrees to radians
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float DegreesToRadians(float d)
        {
            return degreesToRadiansRatio * d;
        }

        /// <summary>
        /// radians to degrees
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static float RadiansToDegrees(float r)
        {
            return radiansToDegreesRatio * r;
        }

        /// <summary>
        /// tests if 2 floats are the same within a tolerance
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool AreSame(float x, float y, float tolerance = 1e-5F)
        {
            if (float.IsNaN(x))
                return false;

            if (float.IsNaN(y))
                return false;

            if (Math.Abs(x - y) > 1e-5)
                return false;
            else
                return true;
        }






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
