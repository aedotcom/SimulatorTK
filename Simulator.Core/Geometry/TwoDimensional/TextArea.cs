using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Drawing;

using QuickFont;

namespace Simulator.Core.Geometry.TwoDimensional
{
    public class TextArea : ClickableArea, IDisposable
    {
        public Nullable<OpenTK.Graphics.Color4> OutlineColor { get; set; }
        public Nullable<OpenTK.Graphics.Color4> BackgroundColor { get; set; }
        public OpenTK.Graphics.Color4 TextColor { get; set; } //if there's no text, what's the point?
        public string Text { get; set; }
        private QFont currentFont;
        private int textSize;  //cannot be set outside the object to maintain consistency
        public int TextSize { get { return textSize; } }
        private string fontFilePath;  //can only be set when the QFont object is set up
        public string FontFilePath { get { return fontFilePath; } }

        public TextArea()
        {
            Text = ""; textSize = 14; X0 = 0; Y0 = 0; Width = 128; Height = 128;

            BackgroundColor = null;
            //BackgroundColor = new OpenTK.Graphics.Color4(0F, 0F, 1F, 1F)

            OutlineColor = null;
            //OutlineColor = new OpenTK.Graphics.Color4(0F, 1F, 0F, 1F)

            TextColor = new OpenTK.Graphics.Color4(1F, 0F, 0F, 1F);
            SetUpFont(textSize, "Fonts/woodenFont.qfont");
        }

        public TextArea(string text, int x0, int y0, int width, int height,
            Nullable<OpenTK.Graphics.Color4> backgroundColor,
            Nullable<OpenTK.Graphics.Color4> outlineColor,
            OpenTK.Graphics.Color4 textColor,
            int textsize, string fontfilepath = null)
        {
            Text = text; X0 = x0; Y0 = y0; Width = width; Height = height;
            BackgroundColor = backgroundColor; OutlineColor = outlineColor; TextColor = textColor;

            SetUpFont(textsize, fontfilepath);
        }


        public TextArea(TextArea T)
        {
            this.Copy(T);
        }

        public void Copy(TextArea T)
        {
            this.Text = T.Text;
            this.X0 = T.X0; this.Y0 = T.Y0;
            this.Height = T.Height; this.Width = T.Width;
            this.TextColor = T.TextColor;
            this.OutlineColor = T.OutlineColor;
            this.BackgroundColor = T.BackgroundColor;
            this.SetUpFont(T.TextSize, T.FontFilePath); //sets textSize & fontFilePath
        }


        public void SetUpFont(int textsize, string fontfilepath = null)
        {
            var builderConfig = new QFontBuilderConfiguration(false);
            builderConfig.ShadowConfig = null;
            builderConfig.TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit;


            if (fontfilepath.Contains(".qfont"))
            {
                currentFont = QFont.FromQFontFile(fontfilepath, textsize / 24f, new QFontLoaderConfiguration());
            }
            else
            {
                currentFont = new QFont(fontfilepath, textsize, builderConfig);
                fontFilePath = fontfilepath;
            }

            textSize = textsize;  //set the get only property for record keeping
        }


        public void Graph()
        {
            bool texture2DWasEnabled = GL.IsEnabled(EnableCap.Texture2D);
            bool depthTestWasEnabled = GL.IsEnabled(EnableCap.DepthTest);
            bool lightingWasEnabled = GL.IsEnabled(EnableCap.Lighting);

            //GL.Disable(EnableCap.Texture2D)
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);

            GL.PushMatrix();
            QFont.Begin();

            if (BackgroundColor != null)
            {
                GL.Begin(BeginMode.Quads);
                GL.Color4(BackgroundColor.Value);
                GL.Vertex2(X0, Y0);
                GL.Vertex2(X0 + Width, Y0);
                GL.Vertex2(X0 + Width, Y0 + Height);
                GL.Vertex2(X0, Y0 + Height);
                GL.End();
            }

            if (OutlineColor != null)
            {
                GL.Begin(BeginMode.LineLoop);
                GL.Color4(OutlineColor.Value);
                GL.Vertex2(X0, Y0);
                GL.Vertex2(X0 + Width, Y0);
                GL.Vertex2(X0 + Width, Y0 + Height);
                GL.Vertex2(X0, Y0 + Height);
                GL.End();
            }

            GL.Translate(X0, Y0, 0.0f);
            currentFont.Options.Colour = this.TextColor;
            currentFont.Print(Text, Width, QFontAlignment.Left);

            QFont.End();
            GL.PopMatrix();

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f); //make sure the color does not bleed over

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



        public System.Drawing.SizeF MeasureStringWithCurrentFont(string str)
        {
            return currentFont.Measure(str);  //size defpends on font
        }

        public System.Drawing.SizeF MeasureCurrentFontAndText()
        {
            return currentFont.Measure(Text);  //size defpends on font
        }

        public void Dispose()   //remember to do image dispose later, both C# copy & tell OpenGL to dispose - more important than font
        {
            try
            {
                currentFont.Dispose();
            }
            catch (Exception)
            {
                //nothing to do here for now
            }
        }
    }
}

