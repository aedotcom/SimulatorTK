using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Simulator.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace NUnitTestsNameSpace
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void OpenTKtoSystemColor()
        {
            OpenTK.Graphics.Color4 openTKColor = new OpenTK.Graphics.Color4(255, 45, 77, 96);
            System.Drawing.Color systemColor = openTKColor.ToSystemColor();
            Assert.AreEqual(openTKColor.R, systemColor.R / (float)255);
            Assert.AreEqual(openTKColor.G, systemColor.G / (float)255);
            Assert.AreEqual(openTKColor.B, systemColor.B / (float)255);
            Assert.AreEqual(openTKColor.A, systemColor.A / (float)255);
        }

        [Test]
        public void SystemToOpenTKColor()
        {
            System.Drawing.Color systemColor = System.Drawing.Color.FromArgb(255, 55, 77, 90);
            OpenTK.Graphics.Color4 openTKColor = systemColor.ToOpenTKColor4();
            Assert.AreEqual(openTKColor.R, systemColor.R / (float)255);
            Assert.AreEqual(openTKColor.G, systemColor.G / (float)255);
            Assert.AreEqual(openTKColor.B, systemColor.B / (float)255);
            Assert.AreEqual(openTKColor.A, systemColor.A / (float)255);
        }


        [Test]
        public void QuickTransform() 
        {
            OpenTK.Matrix4 A = new OpenTK.Matrix4();
            A[0, 0] = 16; A[0, 1] = 15; A[0, 2] = 14; A[0, 3] = 13;
            A[1, 0] = 12; A[1, 1] = 11; A[1, 2] = 10; A[1, 3] = 9;
            A[2, 0] = 8;  A[2, 1] = 7;  A[2, 2] = 6;  A[2, 3] = 5;

            OpenTK.Vector3 x = new OpenTK.Vector3(1, 5, 9);
            OpenTK.Vector3 b = A.QuickTransform(ref x);
            OpenTK.Vector3 expected = new OpenTK.Vector3(230, 166, 102);
            Assert.IsTrue(expected == b);
        }


        [Test]
        public void Demo()
        {
            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                };
 
                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };
 
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                };
 
                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
 
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
 
                    GL.Begin(PrimitiveType.Triangles);
 
                    GL.Color3(Color.Red);
                    GL.Vertex2(-1.0f, 1.0f);
                    GL.Color3(Color.SpringGreen);
                    GL.Vertex2(0.0f, -1.0f);
                    GL.Color3(Color.Blue);
                    GL.Vertex2(1.0f, 1.0f);
 
                    GL.End();
 
                    game.SwapBuffers();
                };
 
                // Run the game at 60 updates per second
                game.Run(60.0);
            }

        }

    }
}
