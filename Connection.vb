Public Class Connection
    Private _Connection As OleDb.OleDbConnection
    Private _DataReader As OleDb.OleDbDataReader
    Private _Command As OleDb.OleDbCommand
    Private _ConnectionString As String
    Public Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property
    Public Function ExecuteToDataTable(ByVal CommandText As String) As System.Data.DataTable
        Dim dt As New DataTable
        ExecuteCommandText(CommandText)
        If _DataReader IsNot Nothing Then dt.Load(_DataReader)
        Return dt
    End Function
    Public Function ExecuteToDataRow(ByVal CommandText As String) As System.Data.DataRow
        Dim dt As New DataTable
        ExecuteCommandText(CommandText)
        dt.Load(_DataReader)
        Return dt.Rows(0)
    End Function
    Public Sub ExecuteNonQuery(ByVal CommandText As String)
        Dim dt As New DataTable
        If _Connection Is Nothing Then
            _Connection = New OleDb.OleDbConnection(_ConnectionString)
            If _Connection.State = ConnectionState.Closed Then
                _Connection.Open()
            End If
        End If
        _Command = New OleDb.OleDbCommand(CommandText, _Connection)
        _Command.ExecuteNonQuery()
    End Sub
    Private Sub ExecuteCommandText(ByVal CommandText As String)
        If CommandText = "" Then Return
        _Connection = New OleDb.OleDbConnection(_ConnectionString)
        If _Connection.State = ConnectionState.Closed Then
            _Connection.Open()
        End If
        _Command = New OleDb.OleDbCommand(CommandText, _Connection)
        Try
            _DataReader = _Command.ExecuteReader
        Catch ex As Exception

        End Try
    End Sub
    Public Sub SetForceConnectuion(ByVal Connection As IDbConnection)
        _Connection = Connection
    End Sub

End Class
