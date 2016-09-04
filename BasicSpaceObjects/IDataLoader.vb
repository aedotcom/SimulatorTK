Public Interface IDataLoader

    Function ReadInPointFile(ByRef filename As String, Optional ByVal delimiter As String = " ", Optional ByVal xScaleFactor As Single = 1.0, Optional ByVal yScaleFactor As Single = 1.0, Optional ByVal zScaleFactor As Single = 1.0) As LinkedList(Of Point)


End Interface
