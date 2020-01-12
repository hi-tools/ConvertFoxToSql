Option Strict Off
Option Explicit On
Module mdlConnect

    Structure Rev_Num
        Dim start As Integer
        Dim end_Renamed As Integer
        Dim strNum As String
        Dim Type As t
    End Structure
    Public Enum t
        Farsi
        latin
        Number
    End Enum
    Dim map(255) As Short
    Public Sub setTemp()
        For i = 0 To 255
            map(i) = i
        Next
        map(128) = 48 : map(129) = 49 : map(130) = 50 : map(131) = 51
        map(132) = 52 : map(133) = 53 : map(134) = 54 : map(135) = 55
        map(136) = 56 : map(137) = 57
        map(40) = 41 : map(41) = 40
        map(138) = 161 : map(139) = 45
        map(140) = 191 : map(141) = 194 : map(142) = 198 : map(143) = 193
        map(144) = 199 : map(145) = 199 : map(146) = 200 : map(147) = 200
        map(148) = 129 : map(149) = 129 : map(150) = 202 : map(151) = 202
        map(152) = 203 : map(153) = 203 : map(154) = 204 : map(155) = 204
        map(156) = 141 : map(157) = 141 : map(158) = 205 : map(159) = 205
        map(160) = 206 : map(161) = 206 : map(162) = 207 : map(163) = 208
        map(164) = 209 : map(165) = 210 : map(166) = 142 : map(167) = 211
        map(168) = 211 : map(169) = 212 : map(170) = 212 : map(171) = 213
        map(172) = 213 : map(173) = 214 : map(174) = 214 : map(175) = 216
        map(224) = 217 : map(225) = 218 : map(226) = 218 : map(227) = 218
        map(228) = 218 : map(229) = 219 : map(230) = 219 : map(231) = 219
        map(232) = 219 : map(233) = 221 : map(234) = 221 : map(235) = 222
        map(236) = 222 : map(237) = 223 : map(238) = 223 : map(239) = 144
        map(240) = 144 : map(241) = 225 : map(243) = 225 : map(244) = 227
        map(245) = 227 : map(246) = 228 : map(247) = 228 : map(248) = 230
        map(249) = 229 : map(250) = 229 : map(251) = 229 : map(252) = 237
        map(253) = 237 : map(254) = 237
    End Sub
    Public Function ConvD2W(ByRef MyString As String) As String
        Dim i As Object
        Dim p, t As String
        Dim x As Short
        Dim ci As Short

        Dim temp As String
        Dim myChar As Char

        temp = ""
        For i = Len(MyString) To 1 Step -1
            'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            myChar = Chr(map(Asc(Mid(MyString, i, 1))))
            ci = Asc(Mid(MyString, i, 1))
            If (ci >= 146 And ci <= 160 And ci Mod 2 = 0) Or (ci >= 167 And ci <= 173 And ci Mod 2 = 1) Or (ci >= 233 And ci <= 241 And ci Mod 2 = 1) Or ci = 226 Or ci = 229 Or ci = 230 Or ci = 244 Or ci = 225 Or ci = 226 Or ci = 246 Or ci = 249 Or ci = 252 Or ci = 253 Then
                temp = temp & myChar & " "
            ElseIf ci = 242 Then
                temp = temp & "áÇ"
            Else
                temp = temp & myChar
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object i. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'

        Next
        t = "" : p = "" : x = 1
        While x <= Len(temp)
            If (Asc(Mid(temp, x, 1)) >= 48 And Asc(Mid(temp, x, 1)) <= 57) Or Asc(Mid(temp, x, 1)) = 47 Then
                p = Mid(temp, x, 1) & p
            Else
                t = t & p & Mid(temp, x, 1)
                p = ""
            End If
            x = x + 1
        End While
        temp = RevNumInstr(temp)
        ConvD2W = temp
    End Function
    Public Function IsLetter(ByVal ch As Char)
        If AscW(ch) >= AscW("A") AndAlso AscW(ch) <= AscW("z") Then
            Return True
        End If
        

        Return False
    End Function
    Public Function IsNumer(ByVal ch As Char)
        If ch = "\" Or ch = "/" Or ch = "." Then
            Return True
        End If
        If Char.IsDigit(ch) Then
            Return True
        End If

        Return False
    End Function
    Public Function FindNumber(ByVal MyString As String, ByVal s As Integer) As Integer
        Dim sTemp As String = ""
        Dim [end] As Integer = s
        For i = s To Len(MyString)
            sTemp = Mid(MyString, i, 1)
            If IsNumer(sTemp) Then
                [end] += 1
            Else
                If [end] < Len(MyString) Then
                    If Mid(MyString, [end], 1) = "-" Then
                        [end] -= 1
                    End If
                End If

                'If (sTemp = "-" Or sTemp = "x") AndAlso (i + 1 < Len(MyString) And IsNumer(Mid(MyString, i + 1, 1))) Then
                '[end] += 1
                'Else
                Exit For
                'End If
            End If
        Next
        Return [end]
    End Function
    Public Function FindLetter(ByVal MyString As String, ByVal s As Integer) As Integer
        Dim sTemp As String = ""
        Dim [end] As Integer = s
        For i = s To Len(MyString)
            sTemp = Mid(MyString, i, 1)
            If IsLetter(sTemp) Then
                [end] += 1
            Else
                If [end] < Len(MyString) Then
                    If Mid(MyString, [end], 1) = "-" Then
                        [end] -= 1
                    End If
                End If
                Exit For
            End If

        Next
        Return [end]
    End Function
    Public Function FindOtherLetter(ByVal MyString As String, ByVal s As Integer) As Integer
        Dim sTemp As String = ""
        Dim [end] As Integer = s
        For i = s To Len(MyString)
            sTemp = Mid(MyString, i, 1)
            If Not IsLetter(sTemp) AndAlso Not IsNumer(sTemp) AndAlso Not (sTemp = "-" Or sTemp = "¡") Then
                [end] += 1
            Else
                If [end] < Len(MyString) Then
                    If Mid(MyString, [end], 1) = "-" Then
                        [end] -= 1
                    End If
                End If
                Exit For
            End If

        Next
        Return [end]
    End Function
    Public Function RevNumInstr(ByRef MyString As String) As String
        Dim sTemp As String = 0

        MyString = MyString.Replace("-", "$")
        Dim TRNum() As Rev_Num = Nothing
        Dim l As Long = 1
        Dim k As Long = 0
        Dim x As Integer
        While l < Len(MyString)
            sTemp = Mid(MyString, l, 1)
            If sTemp = "-" Or sTemp = "¡" Then
                ReDim Preserve TRNum(k)
                TRNum(k).strNum = "-"
                TRNum(k).Type = t.latin
                l += 2
                k += 1
            End If
            If IsLetter(sTemp) Then
                ReDim Preserve TRNum(k)
                x = FindLetter(MyString, l)
                If l - 1 = 0 Then
                    TRNum(k).strNum = Mid(MyString, l, x - l)
                Else
                    TRNum(k).strNum = Mid(MyString, l - 1, x - l + 1)
                End If

                TRNum(k).Type = t.latin
                l = x
                k += 1
            ElseIf IsNumer(sTemp) Then
                ReDim Preserve TRNum(k)
                x = FindNumber(MyString, l)
                If l - 1 = 0 Then
                    TRNum(k).strNum = Mid(MyString, l, x - l)
                Else
                    TRNum(k).strNum = Mid(MyString, l - 1, x - l + 1)
                End If
                TRNum(k).Type = t.Number
                l = x

                k += 1
            ElseIf Not IsNumer(sTemp) AndAlso Not IsLetter(sTemp) Then
                ReDim Preserve TRNum(k)
                x = FindOtherLetter(MyString, l)
                If l - 1 = 0 Then
                    TRNum(k).strNum = Mid(MyString, l, x - l)
                Else
                    TRNum(k).strNum = Mid(MyString, l - 1, x - l + 1)
                End If
                TRNum(k).Type = t.Farsi
                l = x
                k += 1
            End If
            l += 1
        End While


        'sTemp = Mid(MyString, i, 1)
        Dim s As String = ""
        For ind As Integer = 0 To k - 1

            If TRNum(ind).Type = t.Number Then
                If ind + 1 < k - 1 Then
                    If TRNum(ind + 1).Type = t.latin Then
                        Dim stmp As String = TRNum(ind + 1).strNum
                        TRNum(ind + 1).strNum = TRNum(ind).strNum
                        TRNum(ind).strNum = stmp
                    End If
                End If

            End If
        Next
        For ind As Integer = 0 To k - 1
                If TRNum(ind).Type = t.latin Or TRNum(ind).Type = t.Number Then
                    s &= StrReverse(TRNum(ind).strNum)
                Else
                    s &= TRNum(ind).strNum
                End If

        Next



        Return s.Replace("$", "-")

    End Function
    Public Function Convert(ByVal dt As DataTable) As DataTable
        setTemp()
        For Each dr As DataRow In dt.Rows
            dr.BeginEdit()
            For Each col As DataColumn In dr.Table.Columns
                'If col.ColumnName = "descript" Then


                '    If col.ColumnName = "l_descript" Then
                '        dr(col) = dr(col)
                '    Else
                Try
                    dr(col) = ConvD2W((dr(col)))
                Catch ex As Exception

                End Try

                '    End If
                'End If
                

            Next
        Next
        Return dt
    End Function
End Module