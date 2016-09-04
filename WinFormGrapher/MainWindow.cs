using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Diagnostics;

namespace WinFormGrapher
{
    public partial class MainWindow : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        ushort framesPerSecond;
        long milisecondsPerFrame;
        bool frameOverDue;

        //separate update stopwatch?  restarting every frame would mess that up...

        static float angle = 0.0f;


        public MainWindow()
        {
            InitializeComponent();
            framesPerSecond = 30;
            milisecondsPerFrame = 1000 / (long)framesPerSecond;
            frameOverDue = true; //haven't drawn any frames yet
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl1.KeyDown += new KeyEventHandler(glControl_KeyDown);
            glControl1.KeyUp += new KeyEventHandler(glControl_KeyUp);
            glControl1.Resize += new EventHandler(glControl_Resize);
            glControl1.Paint += new PaintEventHandler(glControl_Paint);

            Text =
                GL.GetString(OpenTK.Graphics.OpenGL.StringName.Vendor) + " " +
                GL.GetString(OpenTK.Graphics.OpenGL.StringName.Renderer) + " " +
                GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version);

            GL.ClearColor(Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);

            Application.Idle += Application_Idle;

            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(glControl1, EventArgs.Empty);


            //start stopwatch for first time
            stopwatch.Start();

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;


            glControl1.Dispose();
            base.OnClosing(e);
			
			//safety check or cause more problems?
			Application.Exit();
        }


        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                Render();
            }
        }


        #region event handlers

        void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                GrabScreenshot().Save("screenshot.png");
            }
        }

        void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        void glControl_Resize(object sender, EventArgs e)
        {
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.Viewport(0, 0, c.ClientSize.Width, c.ClientSize.Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }



        void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        #endregion


        private void Render()
        {
            //if it's not time for another render & we're not running slow (frame over due) then don't do anything
            if ((stopwatch.ElapsedMilliseconds < milisecondsPerFrame) && (!frameOverDue))
            {
                return;
            }

            //restart the stop watch so we can measure how long it takes to draw this frame
            stopwatch.Restart();

            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
            angle += 7;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawCube();

            glControl1.SwapBuffers();

            if (stopwatch.ElapsedMilliseconds >= milisecondsPerFrame)
            {
                //we're running slow
                frameOverDue = true;
            }
            else
            {
                frameOverDue = false;
                stopwatch.Restart();
            }
        }



        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Silver);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Honeydew);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Moccasin);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }


        Bitmap GrabScreenshot()
        {
            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
            bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte,
                data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }


        //[STAThread]
        //public static void Main()
        //{
        //    using (MainWindow example = new MainWindow())
        //    {
        //        //// Get the title and category  of this example using reflection.
        //        //ExampleAttribute info = ((ExampleAttribute)example.GetType().GetCustomAttributes(false)[0]);
        //        //example.Text = String.Format("OpenTK | {0} {1}: {2}", info.Category, info.Difficulty, info.Title);
        //        example.ShowDialog();
        //    }
        //}
    }
}
