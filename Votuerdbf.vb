Public Class Votuerdbf
  Private Connect As Odbc.OdbcConnection
  Private Command As Odbc.OdbcCommand
  Private mPath As String

  Public Sub New(ByVal Path As String)
    Connect = New Odbc.OdbcConnection("Driver={Microsoft FoxPro VFP Driver (*.dbf)};UID=;PWD=;SourceDB=" & Path & ";SourceType=DBF;Exclusive=No;BackgroundFetch=Yes;Collate=Machine;Null=Yes;Deleted=Yes;")
    mPath = Path
  End Sub

  Public Sub Open()
    If Connect.State = ConnectionState.Closed Then
      If Connect.ConnectionString = "" Then
        Connect.ConnectionString = "Driver={Microsoft FoxPro VFP Driver (*.dbf)};UID=;PWD=;SourceDB=" & mPath & ";SourceType=DBF;Exclusive=No;BackgroundFetch=Yes;Collate=Machine;Null=Yes;Deleted=Yes;"
      End If
      Connect.Open()
    End If
  End Sub

  Public Function [Select](ByVal FileName As String, ByVal Columns As String, ByVal Condition As String) As DataTable
    If Connect.State = ConnectionState.Closed Then Connect.Open()
    Command = New Odbc.OdbcCommand("SELECT " & Columns & " FROM " & FileName & IIf(Condition.Length = 0, "", " WHERE " & Condition), Connect)
    Dim da As Odbc.OdbcDataAdapter = New Odbc.OdbcDataAdapter(Command)
    Dim dt As New DataTable
    da.Fill(dt)
    Return dt
  End Function
End Class
