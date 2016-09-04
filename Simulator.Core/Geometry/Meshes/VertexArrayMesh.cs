using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Simulator.Core.Geometry.Meshes
{

    // http://www.opentk.com/doc/graphics/extensions  //check if extensions are supported
    
    public class VertexArrayMesh : AbstractSingleMaterialMesh, IGraphable
    {
        public Vector3[] Vertices { get; set; }
        private int VertexBufferID;

        public Vector3[] Normals { get; set; }
        private int NormalBufferID;

        public Vector2[] Texcoords { get; set; }
        private int TexCoordBufferID;

        public ushort[] Indices { get; set; }
        private int NumberOfElements;
        private int IndexBufferID;

        public uint[] Colors { get; set; }
        private int ColorBufferID;


        /// <summary>
        /// default constructor = instantiate default material object & DrawingMode = Triangles
        /// </summary>
        public VertexArrayMesh() : base()
        {

        }

        /// <summary>
        /// default constructor = instantiate default material object & DrawingMode = Triangles
        /// </summary>
        public VertexArrayMesh(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode) : base(drawingMode)
        {

        }

        /// <summary>
        /// demo based on https://github.com/Airstriker/VBO-OpenTK-Example
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="vertexColor1"></param>
        /// <param name="vertexColor2"></param>
        /// <param name="vertexColor3"></param>
        /// <param name="vertexColor4"></param>
        /// <param name="vertexColor5"></param>
        /// <param name="vertexColor6"></param>
        /// <param name="vertexColor7"></param>
        /// <param name="vertexColor8"></param>
        public void SetDemoCube(string imageFile, System.Drawing.Color vertexColor1, System.Drawing.Color vertexColor2, System.Drawing.Color vertexColor3, System.Drawing.Color vertexColor4, System.Drawing.Color vertexColor5, System.Drawing.Color vertexColor6, System.Drawing.Color vertexColor7, System.Drawing.Color vertexColor8)
        {
            this.Vertices = new Vector3[]
            {
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f, -1.0f, -1.0f), 
                new Vector3( 1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f)
            };

            this.Normals = new Vector3[]
            {
                new Vector3(-1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f, -1.0f,  1.0f),
                new Vector3( 1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f,  1.0f,  1.0f),
                new Vector3(-1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f, -1.0f, -1.0f),
                new Vector3( 1.0f,  1.0f, -1.0f),
                new Vector3(-1.0f,  1.0f, -1.0f),
            };

            this.Colors = new uint[]
            {
                Helpers.SystemColorToRGBA(vertexColor1), //put .ColorToRgba32 in Extensions not Utilities - extended (child) vectors & mat + extension methods
                Helpers.SystemColorToRGBA(vertexColor2),
                Helpers.SystemColorToRGBA(vertexColor3),
                Helpers.SystemColorToRGBA(vertexColor4),
                Helpers.SystemColorToRGBA(vertexColor5),
                Helpers.SystemColorToRGBA(vertexColor6),
                Helpers.SystemColorToRGBA(vertexColor7),
                Helpers.SystemColorToRGBA(vertexColor8),
            };

            this.Texcoords = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),

                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };

            this.Indices = new ushort[]
            {
                // front face
                0, 1, 2, 
                2, 3, 0,
                // top face
                3, 2, 6, 
                6, 7, 3,
                // back face
                7, 6, 5, 
                5, 4, 7,
                // left face
                4, 0, 3, 
                3, 7, 4,
                // bottom face
                0, 1, 5, 
                5, 4, 0,
                // right face
                1, 5, 6, 
                6, 2, 1
            };

            material.SetDefaultMaterial();
            material.LoadTexture(imageFile);
            GenerateVertexBuffers();
        }



        protected void GenerateVertexBuffers()
        {
            if (this.Vertices == null) return;
            //if ((this.materialPatches == null) || (this.materialPatches.Count == 0)) return;

            int bufferSize;

            // Color Array Buffer
            if (this.Colors != null)
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out this.ColorBufferID);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.ColorBufferID);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.Colors.Length * sizeof(uint)), this.Colors, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (this.Colors.Length * sizeof(uint) != bufferSize)
                    throw new Exception("Vertex array not uploaded correctly");

                //// Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // Normal Array Buffer
            if (this.Normals != null)
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out this.NormalBufferID);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.NormalBufferID);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.Normals.Length * Vector3.SizeInBytes), this.Normals, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (this.Normals.Length * Vector3.SizeInBytes != bufferSize)
                    throw new Exception("Normal array not uploaded correctly");

                //// Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // TexCoord Array Buffer
            if (this.Texcoords != null)
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out this.TexCoordBufferID);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.TexCoordBufferID);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.Texcoords.Length * 8), this.Texcoords, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (this.Texcoords.Length * 8 != bufferSize)
                    throw new Exception("TexCoord array not uploaded correctly");

                //// Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            };


            // Vertex Array Buffer
            {
                // Generate Array Buffer Id
                GL.GenBuffers(1, out this.VertexBufferID);

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferID);

                // Send data to buffer
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.Vertices.Length * Vector3.SizeInBytes), this.Vertices, BufferUsageHint.StaticDraw);

                // Validate that the buffer is the correct size
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
                if (this.Vertices.Length * Vector3.SizeInBytes != bufferSize)
                    throw new Exception("Vertex array not uploaded correctly");

                //// Clear the buffer Binding
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

            // Element Array Buffer
            // Generate Array Buffer Id
            GL.GenBuffers(1, out this.IndexBufferID);

            // Bind current context to Array Buffer ID
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.IndexBufferID);

            // Send data to buffer
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.Indices.Length * sizeof(ushort)), this.Indices, BufferUsageHint.StaticDraw);

            // Validate that the buffer is the correct size
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);

            if (this.Indices.Length * sizeof(ushort) != bufferSize)
                throw new Exception("Element array not uploaded correctly");

            //// Clear the buffer Binding
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            // Store the number of elements for the DrawElements call
            NumberOfElements = Indices.Length;


        }



        public void Graph(/*CameraShip eyePosition*/)
        {

            // Push current Array Buffer state so we can restore it later
            GL.PushClientAttrib(ClientAttribMask.ClientVertexArrayBit);

            if (this.VertexBufferID == 0) return;
            //if ((this.materialPatches == null) || (this.materialPatches.Count == 0)) return;


            material.Apply();

            if (GL.IsEnabled(EnableCap.Lighting))
            {
                // Normal Array Buffer
                if (this.NormalBufferID != 0)
                {
                    // Bind to the Array Buffer ID
                    GL.BindBuffer(BufferTarget.ArrayBuffer, this.NormalBufferID);

                    // Set the Pointer to the current bound array describing how the data ia stored
                    GL.NormalPointer(NormalPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

                    // Enable the client state so it will use this array buffer pointer
                    GL.EnableClientState(EnableCap.NormalArray);
                }
            }
            else
            {
                // Color Array Buffer (Colors not used when lighting is enabled)
                if (this.ColorBufferID != 0)
                {
                    // Bind to the Array Buffer ID
                    GL.BindBuffer(BufferTarget.ArrayBuffer, this.ColorBufferID);

                    // Set the Pointer to the current bound array describing how the data ia stored
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, sizeof(int), IntPtr.Zero);

                    // Enable the client state so it will use this array buffer pointer
                    GL.EnableClientState(EnableCap.ColorArray);
                }
            }

            // Texture Array Buffer
            if (GL.IsEnabled(EnableCap.Texture2D))
            {
                if (this.TexCoordBufferID != 0)
                {
                    // Bind to the Array Buffer ID
                    GL.BindBuffer(BufferTarget.ArrayBuffer, this.TexCoordBufferID);

                    // Set the Pointer to the current bound array describing how the data ia stored
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 8, IntPtr.Zero);

                    // Enable the client state so it will use this array buffer pointer
                    GL.EnableClientState(EnableCap.TextureCoordArray);
                }
            }


            // Vertex Array Buffer
            {
                // Bind to the Array Buffer ID
                GL.BindBuffer(BufferTarget.ArrayBuffer, this.VertexBufferID);

                // Set the Pointer to the current bound array describing how the data ia stored
                GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, IntPtr.Zero);

                // Enable the client state so it will use this array buffer pointer
                GL.EnableClientState(EnableCap.VertexArray);
            }

            // Element Array Buffer
            {
                // Bind to the Array Buffer ID
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.IndexBufferID);

                // Draw the elements in the element array buffer
                // Draws up items in the Color, Vertex, TexCoordinate, and Normal Buffers using indices in the ElementArrayBuffer
                GL.DrawElements(DrawingMode, this.NumberOfElements, DrawElementsType.UnsignedShort, IntPtr.Zero);

                //http://www.opentk.com/doc/chapter/2/opengl/geometry/attributes  ????
            }

            // Restore the state
            GL.PopClientAttrib();
        }


    }
}
