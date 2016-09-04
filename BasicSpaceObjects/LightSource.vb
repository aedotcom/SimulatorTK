Imports System
Imports System.Text
Imports System.Collections.Generic
Imports OpenTK
Imports OpenTK.Graphics.OpenGL


Public Class LightSource  'make interface so maybe someday this could be used w/ direct-x ? But that would not be cross platform.....



    Public Sub New()



        GL.Light(LightName.Light7, LightParameter.Position, New Single() {1.0F, 1.0F, -0.5F})
    End Sub

End Class
