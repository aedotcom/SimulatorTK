using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;
using QuickFont;
using System.Drawing.Imaging;


using Geometry;
using Geometry.Meshes;

using ButtonLibrary;
using log4net;

namespace FlightSimulator
{



    public class Program : GameWindow
    {
        const float rotation_speed = 180.0f;
        float angle;

        SingleMaterialVBO cube;

        TextArea textArea;
        ImageArea testImageArea;

        bool takeScreenShot = false;


        public Program() : base(800, 600)
        {
 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Version version = new Version(GL.GetString(StringName.Version).Substring(0, 3));
            Version target = new Version(1, 5);
            if (version < target)
            {
                throw new NotSupportedException(String.Format(
                    "OpenGL {0} is required (you only have {1}).", target, version));
            }


            this.VSync = VSyncMode.On;


            cube = new SingleMaterialVBO();

            cube.SetDemoCube("Colors.png");

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);



            testImageArea = new ImageArea("ship.png", Color.Magenta);
            testImageArea.X0 = 200;
            testImageArea.Y0 = 150;




         //   Color y = System.Drawing.Color.FromArgb(255, 0, 0);



            textArea = new TextArea();
            textArea.Text = "Hello World";
            textArea.TextColor = new OpenTK.Graphics.Color4(0, 255, 0, 255);
            textArea.Width = 96;


            GL.ClearColor(System.Drawing.Color.Black);


            GL.Enable(EnableCap.DepthTest);

            //GL.Enable(EnableCap.CullFace); //does this require points to be specified in a certain order?


            // Lighting //should be encapsulated somewhere
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 10.0f, 10.0f, 50f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

        }



        /// <summary>
        /// Unload resources at end of program
        /// </summary>
        /// <param name="e">not used</param>
        protected override void OnUnload(EventArgs e)  
        {
            base.OnUnload(e);
            textArea.Dispose();
            testImageArea.Dispose();

            cube.Dispose();
        }




        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            OpenTK.Input.KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[OpenTK.Input.Key.Escape])
                this.Exit();


            if (keyboard[OpenTK.Input.Key.S])
            {
                takeScreenShot = true;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //yes, this is necessary on every frame render
            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);  //cache perspective matrix!!! probably in CameraShip
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);

            Matrix4 lookat = Matrix4.LookAt(0, -5, -5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            angle += rotation_speed * (float)e.Time;
            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);



            cube.Graph();





            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, this.Height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();


            testImageArea.Graph();



            textArea.Graph();   //only works at end 



            GL.Flush();


            if (takeScreenShot)
            {
                System.Drawing.Bitmap bmp = Helpers.GrabScreenBuffer(this.ClientRectangle);
                bmp.Save("saveTest.png", System.Drawing.Imaging.ImageFormat.Png);
                bmp.Save("saveTest.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                takeScreenShot = false;
            }

            this.SwapBuffers();

        }




        protected static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Entry point of this example.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            log.Debug("Debug Test Message");
            log.Info("Info Test Message");
            log.Warn("Warning Test Message");
            log.Error("Error Test Message");
            log.Fatal("Fatal Test Message");


            using (Program example = new Program())
            {
                example.Run(5, 30);
            }
        }
    }
}
