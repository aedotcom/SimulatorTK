using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace Simulator.Core.Geometry
{
    public class Material : ICloneable, IDisposable, ITrinaryEquatable // IClonable could be dangerous b/c the texutreID could be disposed - unless yo set that to -1 in Dispose.........
    {
        private string origionalTextureFilePath;
        private int textureID;
        private bool containsTransparency;
        public float[] Ambient { get; set; }
        public float[] Diffuse { get; set; }
        public float[] Specular { get; set; }
        public float[] Emissive { get; set; }

        public Material()
        {
            origionalTextureFilePath = string.Empty;
            textureID = -1;
            SetDefaultMaterial();
        }


        public Material(float[] ambient, float[] diffuse, float[] specular, float[] emissive)
        {
            origionalTextureFilePath = string.Empty;
            textureID = -1;
            Set(ambient, diffuse, specular, emissive);
        }

        public Material(string filePath, float[] ambient, float[] diffuse, float[] specular, float[] emissive, System.Drawing.Color? transparentColor = null, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            Set(filePath, ambient, diffuse, specular, emissive, transparentColor, textureParameterName, textureMinFilter);
        }

        public void SetDefaultMaterial()
        {
            Dispose(); //dangerous if this object was copied
            origionalTextureFilePath = string.Empty;
            SetDefaultColors();
        }

        public void SetDefaultColors()
        {
            Ambient = new float[] { 0.7f, 0.7f, 0.7f, 1.0f };
            Diffuse = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            Specular = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            Emissive = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
        }

        public void SetWhite()
        {
            Ambient = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            Diffuse = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            Specular = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            Emissive = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
        }

        public void Set(float[] ambient, float[] diffuse, float[] specular, float[] emissive)
        {
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Emissive = emissive;
        }

        public void Set(string filePath, float[] ambient, float[] diffuse, float[] specular, float[] emissive, System.Drawing.Color? transparentColor = null, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            this.Ambient = ambient;
            this.Diffuse = diffuse;
            this.Specular = specular;
            this.Emissive = emissive;
            LoadTexture(filePath, transparentColor, textureParameterName, textureMinFilter); //sets origionalTextureFilePath
        }

        /// <summary>
        /// copy the material object
        /// dangerous - just copies the texture id for the unmanaged copy of the image in OpenGL
        /// </summary>
        /// <param name="sourceMaterial"></param>
        public void Copy(Material sourceMaterial)
        {
            this.origionalTextureFilePath = sourceMaterial.origionalTextureFilePath;
            this.textureID = sourceMaterial.textureID;
            this.Ambient = sourceMaterial.Ambient;
            this.Diffuse = sourceMaterial.Diffuse;
            this.Specular = sourceMaterial.Specular;
            this.Emissive = sourceMaterial.Emissive;
        }

        /// <summary>
        /// clones the material object
        /// dangerous - just copies the texture id for the unmanaged copy of the image in OpenGL
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Material clone = new Material();
            clone.origionalTextureFilePath = this.origionalTextureFilePath;
            clone.textureID = this.textureID;
            clone.Ambient = this.Ambient;
            clone.Diffuse = this.Diffuse;
            clone.Specular = this.Specular;
            clone.Emissive = this.Emissive;
            return clone; //auto down casts to object, must be case back later
        }


        public void LoadTexture(string filePath, System.Drawing.Color? transparentColor = null, TextureParameterName textureParameterName = TextureParameterName.TextureMinFilter, TextureMinFilter textureMinFilter = TextureMinFilter.Nearest)
        {
            //get rid of any existing texture
            Dispose(); //dangerous - the unmanaged resource named by textureID could be share if .Clone() or .Copy(Material sourceMaterial) was used

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(filePath);

            origionalTextureFilePath = filePath; //keep a record of where this image came from

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

        /// <summary>
        /// Apply this material to whatever is being drawn on the the screen
        /// </summary>
        public void Apply()
        {
            // Use GL.Material to set your object's material parameters.
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, Ambient);
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, Diffuse);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, Specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Emission, Emissive);

            if (textureID > 0)
            {
                //apply texture
                //--------------------------------------------------------------
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
            }
            else
            {
                GL.Disable(EnableCap.Texture2D);
            }
        }

        /// <summary>
        /// False if this object & the other object are 2 different types, or are two differen image, or one has animage texture & ther ohter has only reflective color perties
        /// Null if they have the same image (or both have no image) but different Ambient, Diffuse, Specular & Emissive array properties
        /// True if everything matches
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Nullable<bool> TrinaryEqual(object other)
        {
            if (other == null)
                return false; //dude, you're comparing w/ null, ?Que Passa?

            Material otherMaterial;
            if ((other is Material) == false)
            {
                //dude, these aren't event the same type of object!!!!!
                return false;
            }
            else
            {
                //OK, at least this & other are 2 Geometry.Material objects
                otherMaterial = other as Material;

                if ((this.textureID == otherMaterial.textureID) && (this.origionalTextureFilePath == otherMaterial.origionalTextureFilePath) && (this.containsTransparency == otherMaterial.containsTransparency))
                {
                    //They both have the same image
                    if (this.MaterialArrayPropertiesAreEqual(otherMaterial, 1e-5F))
                    {
                        //event the Ambient, Diffuse, Specular & Emissive array properties are the same
                        return true;
                    }
                    else
                    {
                        //null if they have the same image, but different Ambient, Diffuse, Specular & Emissive array properties, see method documentation above
                        return null;
                    }
                }
                else
                {
                    //it is not the same image (nor both no image), so false
                    return false;
                }
            }
        }


        /// <summary>
        /// Check if the Ambient, Diffuse, Specular & Emissive array properties are the same
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerence"></param>
        /// <returns></returns>
        public bool MaterialArrayPropertiesAreEqual(Material other, float tolerence = 1e-5F)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Math.Abs(this.Ambient[i] - other.Ambient[i]) > tolerence)
                    return false;

                if (Math.Abs(this.Diffuse[i] - other.Diffuse[i]) > tolerence)
                    return false;

                if (Math.Abs(this.Specular[i] - other.Specular[i]) > tolerence)
                    return false;

                if (Math.Abs(this.Emissive[i] - other.Emissive[i]) > tolerence)
                    return false;
            }
            //if you get here everything matched
            return true;
        }

        /// <summary>
        /// Tests if textureID > 0 to see if an image has loaded
        /// </summary>
        /// <returns></returns>
        public bool ContainsTexture()
        {
            return textureID > 0;
        }


        /// <summary>
        /// delete unmanaged copy of image in  OpenGL
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (textureID > 0)
                {
                    GL.DeleteTexture(textureID);
                }
            }
            catch (Exception)
            {
                //noting to do at this time
                //could be logged in future
            }
            finally
            {
                textureID = -1;
                origionalTextureFilePath = string.Empty;
            }
        }

    }



}
