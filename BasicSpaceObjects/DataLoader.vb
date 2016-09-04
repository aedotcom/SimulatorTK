Imports System.Collections.Generic
Imports System.IO
Imports Geometry
'Imports AdvancedSpaceObjects 'has to be done through intrface layer b/c VisualStudio cries over cyclical dependencies

Public Class DataLoader
    Implements IDataLoader


    'may throw exception, so wrap in try catch
    Public Function ReadInPointFile(ByRef filename As String, Optional ByVal delimiter As String = " ", Optional ByVal xScaleFactor As Single = 1.0, Optional ByVal yScaleFactor As Single = 1.0, Optional ByVal zScaleFactor As Single = 1.0) As LinkedList(Of Point) Implements IDataLoader.ReadInPointFile


        If (Not File.Exists(filename)) Then
            Throw New Exception("File could not be found to ReadInStars()")
        End If

        Dim ListOfPoints As New LinkedList(Of Point)

        Try
            Using fileStream As New FileStream(filename, FileMode.Open, FileAccess.Read)
                Using streamReader As New StreamReader(fileStream)

                    Dim currentLine As String
                    Do
                        currentLine = streamReader.ReadLine()

                        'Eat off any number of empty lines
                        While String.IsNullOrWhiteSpace(currentLine)
                            Try
                                currentLine = streamReader.ReadLine() 'try to get the next line
                            Catch
                                Return ListOfPoints 'if that throws an exception maybe we're done
                            End Try
                        End While

                        Dim tokens() As String 'the () means this is an array
                        tokens = currentLine.Split(New String() {delimiter}, StringSplitOptions.RemoveEmptyEntries) 'split on delimiter - and default delimiter is a space




                        'need to refactor to work w/ SeaOfSimulation
                        Dim currentPoint As New Point()
                        Select Case tokens.Length
                            Case 3
                                currentPoint.Location.SetXYZ(xScaleFactor * Single.Parse(tokens(0)),
                                                             yScaleFactor * Single.Parse(tokens(1)),
                                                             zScaleFactor * Single.Parse(tokens(2)))
                            Case 6
                                currentPoint.Location.SetXYZ(xScaleFactor * Single.Parse(tokens(0)),
                                                             yScaleFactor * Single.Parse(tokens(1)),
                                                             zScaleFactor * Single.Parse(tokens(2)))

                                currentPoint.SetColor(Byte.Parse(tokens(3)),
                                                      Byte.Parse(tokens(4)),
                                                      Byte.Parse(tokens(5)),
                                                      255)

                            Case Else
                                Throw New Exception("Cannot ReadInVertexFile() for StarField - wrong number of tokens on line")
                        End Select

                        ListOfPoints.AddLast(currentPoint)

                    Loop While currentLine <> String.Empty
                End Using
            End Using

            Return ListOfPoints

        Catch ex As Exception
            Throw New Exception(ex.ToString())
        Finally
            'Try
            '        streamReader.Close()      'Xamarin Studio & especially MonoDevelop did not like this b/c there was a possibility it would 
            '        fileStream.Close()        'be trying to close objects that were never initialized
            'Catch
            'End Try
        End Try
    End Function






End Class
