using System;
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
    public abstract class AbstractSingleMaterialMesh : AbstractMesh, IDisposable //IDisposable could be dangerous if the material is copied, and then 1 copy gets disposed & then ther other does not work b/c the other one
    {
        protected Material material;

        /// <summary>
        /// default constructor instantiates the default materail & calls base class AbsractMesh to set default Drawing Mode = Triangles
        /// </summary>
        public AbstractSingleMaterialMesh() : base()
        {
            material = new Material();
        }

        /// <summary>
        /// drawingMode constructor instantiates the default materail & passed the drawingMode parameter to the base class AbstractMesh constructor
        /// </summary>
        /// <param name="drawingMode"></param>
        public AbstractSingleMaterialMesh(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode) : base(drawingMode)
        {
            material = new Material();
        }

        /// <summary>
        /// Constructor to set primitive drawing mode & graphics card BufferUsage Hint
        /// </summary>
        /// <param name="drawingMode"></param>
        /// <param name="graphicsCardBufferUsageHint"></param>
        public AbstractSingleMaterialMesh(OpenTK.Graphics.OpenGL.PrimitiveType drawingMode, OpenTK.Graphics.OpenGL.BufferUsageHint graphicsCardBufferUsageHint)
            : base(drawingMode, graphicsCardBufferUsageHint)
        {
            
        }


        /// <summary>
        /// set default material
        /// </summary>
        public void SetDefaultMaterial()
        {
            material.SetDefaultMaterial();
        }

        /// <summary>
        /// set only the lighting properties of the texture
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="diffuse"></param>
        /// <param name="specular"></param>
        /// <param name="emissive"></param>
        public void SetMaterial(float[] ambient, float[] diffuse, float[] specular, float[] emissive)
        {
            material.Ambient = ambient;
            material.Diffuse = diffuse;
            material.Specular = specular;
            material.Emissive = emissive;
        }

        /// <summary>
        /// set material all properties, including texture
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ambient"></param>
        /// <param name="diffuse"></param>
        /// <param name="specular"></param>
        /// <param name="emissive"></param>
        /// <param name="transparentColor"></param>
        /// <param name="textureParameterName"></param>
        /// <param name="textureMinFilter"></param>
        public void SetMaterial(string filePath, float[] ambient, float[] diffuse, float[] specular, float[] emissive, System.Drawing.Color? transparentColor = null, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            material.Ambient = ambient;
            material.Diffuse = diffuse;
            material.Specular = specular;
            material.Emissive = emissive;
            // material.Dispose(); //dangerous - the unmanaged resource named by textureID could be share if .Clone() or .Copy(Material sourceMaterial) was used
            material.LoadTexture(filePath, transparentColor, textureParameterName, textureMinFilter);
        }

        /// <summary>
        /// load an image texture into the current material
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="transparentColor"></param>
        /// <param name="textureParameterName"></param>
        /// <param name="textureMinFilter"></param>
        public void LoadTexture(string filePath, System.Drawing.Color? transparentColor = null, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            material.LoadTexture(filePath, transparentColor, textureParameterName, textureMinFilter);
        }

        /// <summary>
        /// dispose the material that contains a reference id number to the copy of the image in OpenGL's unmanaged memory
        /// </summary>
        public void Dispose()
        {
            material.Dispose();
        }

    }
}
