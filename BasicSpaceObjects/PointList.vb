Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Geometry
Imports OpenTK
Imports OpenTK.Graphics.OpenGL



Public Class PointList
    Private twinkleLevelValue As Single
    Public Property TwinkleLevel() As Single
        Get
            Return twinkleLevelValue
        End Get
        Set(ByVal value As Single)
            twinkleLevelValue = value
        End Set
    End Property


    Private pointLoader As IDataLoader


    Public ListOfPoints As New LinkedList(Of Point)

    Public Sub New() 'DEFAULT CONSTRUCTOR sets twinkle level & instantiates 'real' file loader
        twinkleLevelValue = 0.2
        pointLoader = New DataLoader()
    End Sub

    Public Sub New(twinkleLevel As Integer, loader As IDataLoader)
        twinkleLevelValue = twinkleLevel
        pointLoader = loader
    End Sub


    'read in points from file
    'may throw exception, so wrap in try catch
    Public Sub ReadInPointFile(ByRef filename As String, Optional ByVal delimiter As String = " ", Optional ByVal xScaleFactor As Single = 1.0, Optional ByVal yScaleFactor As Single = 1.0, Optional ByVal zScaleFactor As Single = 1.0, Optional ByVal clearExistingStars As Boolean = True)

        Try
            If (clearExistingStars) Then
                ListOfPoints.Clear()
            End If

            ListOfPoints.Concat(pointLoader.ReadInPointFile(filename, delimiter, xScaleFactor, yScaleFactor, zScaleFactor))

        Catch ex As Exception
            Throw New Exception(ex.ToString())
        Finally
            'Try
            '        streamReader.Close()      'Xamarin Studio & especially MonoDevelop did not like this b/c there was a possibility it would 
            '        fileStream.Close()        'be trying to close objects that were never initialized
            'Catch
            'End Try
        End Try
    End Sub


    Function NumberOfPoints() As Integer
        Return ListOfPoints.Count()
    End Function

    Public Sub DeRes()
        ListOfPoints.Clear()
    End Sub

    Public Sub Graph() ' remember interface & remember to disable & reenable lighting & (?)depth testing like in ModelMaker
        'remember collidable interface & list of collision objects

    End Sub

End Class

