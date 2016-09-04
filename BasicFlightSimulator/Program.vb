Option Strict On
Imports System
Imports System.Drawing
Imports Microsoft.VisualBasic
Imports OpenTK
Imports OpenTK.Graphics
Imports OpenTK.Graphics.OpenGL
Imports OpenTK.Input
Imports QuickFont
Imports Simulator.Core.Geometry.TwoDimensional
Imports Simulator.Core
Imports Simulator.Core.Geometry.Meshes

''' <summary>
''' Mono will only support a Visual Basic Console Application & Visual Studio insists a 
''' Console Application must have a main method.  Also Mono puts the Main method in a regular class, 
''' not a Module like Visual Studio. That's why this file has to be like it is.
''' Also, Mono does not interpret the default namespace of Visual Basic correctly, so a VB project 
''' can consume C# but C# cannot consume VB in MonoDevelop.
''' </summary>
''' <remarks></remarks>
Public Class Program
    Inherits GameWindow

#Region "private variables"
    Private angle As Single
    Private cube As VBO
    Private textArea As TextArea
    Private imageArea As ImageArea
    Private takeScreenShot As Boolean
    'Private Shared ReadOnly log As ILog = LogManager.GetLogger("VB Game Window Logger")
#End Region

    ''' <summary>
    ''' initialize new Program object with GameWindow size
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New(800, 600) 'size should be loaded from XML
    End Sub

    ''' <summary>
    ''' static main method provide starting place for program execution
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Main()
        System.Console.WriteLine("starting...")

        ''log = LogManager.GetLogger("VB Game Window Logger") 'caused errors
        'log4net.Config.XmlConfigurator.Configure()
        'log.Debug("Debug Test Message")
        'log.Info("Info Test Message")
        'log.Warn("Warning Test Message")
        'log.Error("Error Test Message")
        'log.Fatal("Fatal Test Message")

        Using basicSimulator As New Program
            basicSimulator.Run(4, 30)
        End Using
    End Sub

    ''' <summary>
    ''' setup variables in OnLoad event
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Me.VSync = VSyncMode.On

        Dim version As System.Version
        version = New Version(GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version).Substring(0, 3))
        Console.WriteLine("OpenGL version = " + version.ToString())

        takeScreenShot = False

        cube = New VBO()
        cube.SetDemoCube("Colors.png")


        GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest)
        imageArea = New ImageArea("ship.png", Color.Magenta)
        imageArea.X0 = 200
        imageArea.Y0 = 150

        textArea = New TextArea()
        textArea.Text = "Hello World"
        textArea.TextColor = New OpenTK.Graphics.Color4(0, 255, 0, 255)
        textArea.Width = 96

        GL.ClearColor(System.Drawing.Color.Black)
        GL.Enable(EnableCap.DepthTest)

        'GL.Enable(EnableCap.CullFace);  'does this require points to be specified in a certain order?

        ' // Lighting //should be encapsulated somewhere
        GL.Light(LightName.Light0, LightParameter.Position, New Single() {10.0F, 10.0F, 50.0F})
        GL.Light(LightName.Light0, LightParameter.Ambient, New Single() {0.3F, 0.3F, 0.3F, 1.0F})
        GL.Light(LightName.Light0, LightParameter.Diffuse, New Single() {1.0F, 1.0F, 1.0F, 1.0F})
        GL.Light(LightName.Light0, LightParameter.Specular, New Single() {1.0F, 1.0F, 1.0F, 1.0F})
        GL.Light(LightName.Light0, LightParameter.SpotExponent, New Single() {1.0F, 1.0F, 1.0F, 1.0F})
        GL.LightModel(LightModelParameter.LightModelAmbient, New Single() {0.2F, 0.2F, 0.2F, 1.0F})
        GL.LightModel(LightModelParameter.LightModelTwoSide, 1)
        GL.LightModel(LightModelParameter.LightModelLocalViewer, 1)
        GL.Enable(EnableCap.Lighting)
        GL.Enable(EnableCap.Light0)

        ' GL.Disable(EnableCap.Lighting)

    End Sub

    ''' <summary>
    ''' Dispose of unmanaged resources in OnUnLoad event
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnUnLoad(e As EventArgs)
        MyBase.OnUnload(e)
        textArea.Dispose()
        imageArea.Dispose()
        cube.Dispose()
    End Sub

    ''' <summary>
    ''' handle window resize event
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        GL.Viewport(0, 0, Me.Width, Me.Height)

        'Setup perspective view - this should be done in CameraShip
        Dim aspectRatio As Single
        Dim perspective As OpenTK.Matrix4
        Dim zNear As Single = 1  'zFar & zNear should be public properties of CamaraShip
        Dim zFar As Single = 1000
        Try 'if to check Height == zero would be faster
            aspectRatio = Me.Width / CType(Me.Height, Single)
        Catch ex As Exception
            aspectRatio = 1
        Finally
            perspective = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4, aspectRatio, zNear, zFar)
            GL.MatrixMode(MatrixMode.Projection)
            GL.LoadMatrix(perspective) 'ref perspective in C#
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnUpdateFrame(e As FrameEventArgs)
        MyBase.OnUpdateFrame(e)

        'THIS WOULD PROBABLY WORK IN A C# CLASS LIKE CAMERA SHIP, BUT CAMERA SHIP DOES NOT REALLY INVOLVE THE KEYBOARD...
        'Dim keyboard As OpenTK.Input.KeyboardState
        'keyboard = OpenTK.Input.Keyboard.GetState()
        'If (keyboard[OpenTK.Input.Key.Escape]) Then
        '    Me.Exit()
        'End If

        'THIS HAD BETTER WORK INSIDE CAMARA SHIP!!!!!!!!!!!!
        'http://www.opentk.com/doc/input/joystick
        'http://www.opentk.com/book/export/html/1197
        'OpenTK.Input.Joystick.GetState(0)





        If Me.Keyboard(Key.Escape) Then
            Me.Exit()
        End If

        If Me.Keyboard(Key.S) Then
            takeScreenShot = True
        End If
    End Sub



    ''' <summary>
    ''' Render the screen
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnRenderFrame(e As FrameEventArgs)
        MyBase.OnRenderFrame(e)
        GL.Clear(ClearBufferMask.ColorBufferBit Or ClearBufferMask.DepthBufferBit)

        ''''''''''''''''''''''''''''''''''''''''''''''''''
        ' 3D graphics
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        'perspective + lookAt necesary in each renderframe :(
        'Setup perspective view - this should be done in CameraShip
        Dim aspectRatio As Single
        Dim perspective As OpenTK.Matrix4
        Dim zNear As Single = 1  'zFar & zNear should be public properties of CamaraShip
        Dim zFar As Single = 1000
        Try 'if to check Height == zero would be faster
            aspectRatio = Me.Width / CType(Me.Height, Single)
        Catch ex As Exception
            aspectRatio = 1
        Finally
            perspective = Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.PiOver4, aspectRatio, zNear, zFar)
            GL.MatrixMode(MatrixMode.Projection)
            GL.LoadMatrix(perspective)  'ref perspective in C#
        End Try

        Dim lookat As OpenTK.Matrix4 = Matrix4.LookAt(0, -5, -5, 0, 0, 0, 0, 1, 0)
        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadMatrix(lookat) 'ref lookat in C#

        angle += 180 * CType(e.Time, Single)
        GL.Rotate(angle, 0.0F, 1.0F, 0.0F)

        cube.Graph()

        ''''''''''''''''''''''''''''''''''''''''''''''''''
        ' 2D graphics
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        GL.MatrixMode(MatrixMode.Projection)
        GL.LoadIdentity()
        GL.Ortho(0, Me.Width, Me.Height, 0, -1, 1)
        GL.MatrixMode(MatrixMode.Modelview)
        GL.LoadIdentity()

        imageArea.Graph()
        textArea.Graph()   'only works at end ?

        GL.Flush() 'make sure all OpenGL rendering is done

        If (takeScreenShot) Then
            Dim bmp As System.Drawing.Bitmap
            bmp = Helpers.GrabScreenBuffer(Me.ClientRectangle)
            bmp.Save("saveTest.png", System.Drawing.Imaging.ImageFormat.Png)
            bmp.Save("saveTest.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
            takeScreenShot = False
        End If

        Me.SwapBuffers()
    End Sub


End Class

