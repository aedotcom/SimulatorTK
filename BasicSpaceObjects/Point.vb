Imports Geometry

Public Class Point
    Public IsSelected As Boolean = False
    'Public Vertex As Integer = -1  'change over to vertex buffer
    Public Location As Vector3H1
    Public Color(4) As Byte
    Public Sub SetColor(array As Byte())
        Color(0) = array(0)
        Color(1) = array(1)
        Color(2) = array(2)
        Color(4) = array(4)
    End Sub

    Public Sub SetColor(r As Byte, g As Byte, b As Byte, a As Byte)
        Color(0) = r
        Color(1) = g
        Color(2) = b
        Color(4) = a
    End Sub
End Class
