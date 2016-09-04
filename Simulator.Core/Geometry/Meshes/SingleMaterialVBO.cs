﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing.Imaging;

namespace Simulator.Core.Geometry.Meshes
{
    /// <summary>
    /// VertexBufferObject with only a single material
    /// </summary>
    public class SingleMaterialVBO : AbstractSingleMaterialMesh, IGraphable
    {
        private int VertexBufferID, IndexBufferID, NumberOfElements;
        private TextureVBODataPoint[] VertexBufferArray;
        private ushort[] IndexArray;

        /// <summary>
        /// default constructor = instantiate default material object & DrawingMode = Triangles
        /// </summary>
        public SingleMaterialVBO() : base() 
        {

        }

        /// <summary>
        /// Constructor to set primitive drawing mode 
        /// </summary>
        /// <param name="drawingMode"></param>
        public SingleMaterialVBO(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode) : base(drawingMode)
        {

        }

        /// <summary>
        /// Constructor to set primitive drawing mode & graphics card BufferUsage Hint
        /// </summary>
        /// <param name="drawingMode"></param>
        /// <param name="graphicsCardBufferUsageHint"></param>
        public SingleMaterialVBO(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode, OpenTK.Graphics.OpenGL.BufferUsageHint graphicsCardBufferUsageHint)
            : base(drawingMode, graphicsCardBufferUsageHint)
        {
            
        }

        public void SetDemoCube(string textureFile = null)
        {
            VertexBufferArray = new TextureVBODataPoint[]
            {
                    new TextureVBODataPoint(-1.0f, -1.0f,  1.0f, -1.0f, -1.0f,  1.0f, 0f, 1f),
                    new TextureVBODataPoint( 1.0f, -1.0f,  1.0f,  1.0f, -1.0f,  1.0f, 1f, 1f),
                    new TextureVBODataPoint( 1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f, 1f, 0f),
                    new TextureVBODataPoint(-1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f, 0f, 0f),
                    new TextureVBODataPoint(-1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, 0f, 1f),
                    new TextureVBODataPoint( 1.0f, -1.0f, -1.0f,  1.0f, -1.0f, -1.0f, 1f, 1f), 
                    new TextureVBODataPoint( 1.0f,  1.0f, -1.0f,  1.0f,  1.0f, -1.0f, 1f, 0f),
                    new TextureVBODataPoint(-1.0f,  1.0f, -1.0f, -1.0f,  1.0f, -1.0f, 0f, 0f) 
            };

            IndexArray = new ushort[]
            {
                0, 1, 2, 
                2, 3, 0, // front face triangles

                3, 2, 6, 
                6, 7, 3, // top face triangles

                7, 6, 5, 
                5, 4, 7, // back face triangles

                4, 0, 3, 
                3, 7, 4, // left face triangles

                0, 1, 5, 
                5, 4, 0, // bottom face triangles

                1, 5, 6, 
                6, 2, 1  // right face triangles
            };

            GenerateVertexBuffer();
            material.SetWhite();
            if (!string.IsNullOrWhiteSpace(textureFile))
            {
                material.LoadTexture(textureFile);
            }
        }

        /// <summary>
        /// To create a VBO:
        /// 1) Generate the buffer handles for the vertex and element buffers.
        /// 2) Bind the vertex buffer handle and upload your vertex data. Check that the buffer was uploaded correctly.
        /// 3) Bind the element buffer handle and upload your element data. Check that the buffer was uploaded correctly.
        /// </summary>
        protected void GenerateVertexBuffer()
        {
            int size;
            GL.GenBuffers(1, out this.VertexBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexBufferArray.Length * BlittableValueType.StrideOf(VertexBufferArray)), VertexBufferArray, GraphicsCardBufferUsageHint);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (VertexBufferArray.Length * BlittableValueType.StrideOf(VertexBufferArray) != size)
            { 
                throw new Exception("Vertex data not sent to graphics card correctly"); 
            }
            // Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.GenBuffers(1, out this.IndexBufferID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.IndexBufferID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(IndexArray.Length * sizeof(ushort)), IndexArray, GraphicsCardBufferUsageHint);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (IndexArray.Length * sizeof(ushort) != size)
            { 
                throw new Exception("Element data not sent to graphics card correctly"); 
            }
            // Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            this.NumberOfElements = IndexArray.Length;
        }

        /// <summary>
        /// To draw a VBO:
        /// 1) Ensure that the VertexArray client state is enabled.
        /// 2) Bind the vertex and element buffer handles.
        /// 3) Set up the data pointers (vertex, normal, color) according to your vertex format.
        /// 4) Call DrawIndexArray. (Note: the last parameter is an offset into the element buffer
        //    and will usually be IntPtr.Zero).
        /// </summary>
        public override void Graph()
        {
            //GL.EnableClientState(ArrayCap.ColorArray) //no colors, just lighting materials and/or textures
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.IndexBufferID);

            material.Apply();

            GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(VertexBufferArray), new IntPtr(0));
            GL.NormalPointer(NormalPointerType.Float, BlittableValueType.StrideOf(VertexBufferArray), new IntPtr(Vector3.SizeInBytes));

            GL.TexCoordPointer(2, TexCoordPointerType.Float, BlittableValueType.StrideOf(VertexBufferArray), new IntPtr(2 * Vector3.SizeInBytes));

            
            GL.DrawElements(DrawingMode, this.NumberOfElements, DrawElementsType.UnsignedShort, IntPtr.Zero);
             
            /*
            GL.DrawArrays(DrawingMode, 0, 3); //for drawing w/out element index array
             */
        }




    }
}