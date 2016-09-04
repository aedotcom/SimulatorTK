using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Simulator.Core.Geometry;

namespace Simulator.Core.Geometry.Meshes
{
    public abstract class AbstractMesh : IGraphable
    {
        protected OpenTK.Vector3 center;

        public OpenTK.Graphics.OpenGL.PrimitiveType DrawingMode { get; set; }

        public OpenTK.Graphics.OpenGL.BufferUsageHint GraphicsCardBufferUsageHint { get; set; }

        /// <summary>
        /// default constructor in base abstract class sets Triangles as the default drawing mode
        /// </summary>
        public AbstractMesh()
        {
            DrawingMode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
            GraphicsCardBufferUsageHint = BufferUsageHint.StaticDraw;
        }

        /// <summary>
        /// Constructor to set primitive drawing mode & graphics card BufferUsage Hint
        /// </summary>
        /// <param name="drawingMode"></param>
        /// <param name="graphicsCardBufferUsageHint"></param>
        public AbstractMesh(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode, OpenTK.Graphics.OpenGL.BufferUsageHint graphicsCardBufferUsageHint = BufferUsageHint.StaticDraw)
        {
            DrawingMode = drawingMode;
            GraphicsCardBufferUsageHint = graphicsCardBufferUsageHint;
        }

        /// <summary>
        /// override this in sub classes
        /// </summary>
        public virtual void Graph()
        {

        }
    }
}
