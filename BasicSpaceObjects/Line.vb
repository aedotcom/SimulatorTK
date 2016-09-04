Public Class Line
    Public IsSelected As Boolean = False

    Public TextureCoordinate1 As Integer = -1
    Public Vertex1 As Integer = -1
    Public Color1(4) As Byte
    Public Sub SetColor1(array As Byte())
        Color1(0) = array(0)
        Color1(1) = array(1)
        Color1(2) = array(2)
        Color1(4) = array(4)
    End Sub
    Public Sub SetColor1(r As Byte, g As Byte, b As Byte, a As Byte)
        Color1(0) = r
        Color1(1) = g
        Color1(2) = b
        Color1(4) = a
    End Sub

    Public TextureCoordinate2 As Integer = -1
    Public Vertex2 As Integer = -1 'This could be refactored into a VertexInformation class which would let you type LineObject.Vertex1.Index & LineObject.Vertex1.Color
    Public Color2(4) As Byte       'however, (A) that would mean slower performance due to jumping across 2 references instead of 1
    Public Sub SetColor2(array As Byte()) 'and (B) you have to handle a Line different than a Triangle or Point anyhow.
        Color2(0) = array(0) 'To achieve full abstraction, everything have to be a generic Polygon
        Color2(1) = array(1) 'This is the tension - conceptual abstraction vs processor speed.
        Color2(2) = array(2)
        Color2(4) = array(4)
    End Sub
    Public Sub SetColor2(r As Byte, g As Byte, b As Byte, a As Byte)
        Color2(0) = r
        Color2(1) = g
        Color2(2) = b
        Color2(4) = a
    End Sub
End Class
