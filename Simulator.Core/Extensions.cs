using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator.Core
{
    public static class Extensions
    {
        /// <summary>
        /// OpenTK color to System.Drawing.Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ToSystemColor(this OpenTK.Graphics.Color4 color)
        {
            return System.Drawing.Color.FromArgb(color.ToArgb());
        }

        /// <summary>
        ///  System.Drawing.Color color to OpenTK
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static OpenTK.Graphics.Color4 ToOpenTKColor4(this System.Drawing.Color color)
        {
            return new OpenTK.Graphics.Color4(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Left handed homogeneous(W=1) matrix * vector multiplication.
        /// Much more efficient than how it's done inside OpenTK
        /// </summary>
        /// <param name="M"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        public static OpenTK.Vector3 QuickTransform(this OpenTK.Matrix4 M, ref OpenTK.Vector3 V)
        {
            return new OpenTK.Vector3(
                M.Row0.X * V.X + M.Row0.Y * V.Y + M.Row0.Z * V.Z + M.Row0.W,
                M.Row1.X * V.X + M.Row1.Y * V.Y + M.Row1.Z * V.Z + M.Row1.W,
                M.Row2.X * V.X + M.Row2.Y * V.Y + M.Row2.Z * V.Z + M.Row2.W);
        }




    }
}
