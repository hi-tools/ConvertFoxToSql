Public Class MainForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dlg As New OpenFileDialog
        dlg.DefaultExt = "*.dbf"
        dlg.Filter = "Foxpro(*.dbf)|*.dbf"
        btnConnect.Enabled = False
        If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Text = dlg.FileName
            Dim cls As New Votuerdbf(TextBox1.Text)
            Dim f As New IO.FileInfo(TextBox1.Text)
            If f.Exists Then
                Dim dt As DataTable = cls.Select(TextBox1.Text.Trim.Replace(".dbf", ""), "*", "")
                DataGridView1.DataSource = Convert(dt)
                'DataGridView1.DataSource = dt
                DataGridView1.Grid.RetrieveStructure()
                TextBox2.Text = "xxx_" & f.Name.ToLower().Trim.Replace(".dbf", "")
                btnConnect.Enabled = True
            End If
           
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        If txtServerName.Text = "" Then MessageBox.Show("کاربر گرامی لطفآ آدرس سرور را وارد نمائید", "توجـــــــه") : Return
        If txtDataBaseName.Text = "" Then MessageBox.Show("کاربر گرامی لطفآ نام پایگاه داده را وارد نمائید", "توجـــــــه") : Return
        If txtLogin.Text = "" Then MessageBox.Show("کاربر گرامی لطفآ نام کاربری را وارد نمائید", "توجـــــــه") : Return
        If txtPass.Text = "" Then MessageBox.Show("کاربر گرامی لطفآ رمز عبور را وارد نمائید", "توجـــــــه") : Return
        Dim ConnectionString As String = "Provider=SQLOLEDB;Data Source=" & """" & txtServerName.Text & """" & ";User ID=" & """" & txtLogin.Text & """" & ";Password=" & txtPass.Text & ";Initial Catalog=" & txtDataBaseName.Text
        Dim cls As New Connection
        cls.ConnectionString = ConnectionString
        Dim dt As DataTable = DataGridView1.DataSource
        Dim str As String = ""
        Dim str1 As String = ""
        str = " BEGIN TRY  DROP TABLE " & TextBox2.Text.Trim & " END TRY  BEGIN CATCH END CATCH " & vbCrLf

        str &= "CREATE TABLE " & TextBox2.Text.Trim & "("
        For Each col As DataColumn In dt.Columns
            str1 &= IIf(str1 = "", "", ",") & col.ColumnName & " VARCHAR(max)"
        Next
        str &= str1 & ")" & vbCrLf
        For Each dr As DataRow In dt.Rows
            Dim strInsert As String = ""
            Dim strValue As String = ""
            For Each col As DataColumn In dt.Columns
                strInsert &= IIf(strInsert = "", "", ",") & col.ColumnName
                If IsDBNull(dr(col)) Then
                    strValue &= IIf(strValue = "", "", ",") & "'NULL'"
                Else
                    strValue &= IIf(strValue = "", "", ",") & "'" & CStr(dr(col)).Replace("'", "''").Trim & "'"
                End If
            Next
            str &= IIf(str = "", "", vbCrLf) & "INSERT INTO " & TextBox2.Text.Trim & "(" & strInsert & ") VALUES(" & strValue & ")"
        Next
        cls.ExecuteNonQuery(str)
        'btnConnect.Enabled = False
        MsgBox("انتقال به پایان رسید")
    End Sub
End Class
