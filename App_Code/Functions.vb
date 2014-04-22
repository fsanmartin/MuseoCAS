﻿Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class Functions
    Public Shared Sub AddImageGallery(ByVal Gallery As String, ByVal CatID As Integer, ByVal Title As String, ByVal Filename As String)
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sInsertGallery As String = _
            "INSERT INTO galeria " & _
                   "(img_galeria " & _
                   ",img_cat_id " & _
                   ",img_nombre " & _
                   ",img_url) " & _
            " VALUES( '" & Trim(Gallery) & "', " & _
                    "" & CatID & ", " & _
                    "'" & Trim(Title) & "', " & _
                    "'" & Trim(Filename) & "' ) "

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sInsertGallery, cn)
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
    End Sub

    Public Shared Function Images(ByVal Gallery As String, ByVal CatID As Integer) As Integer
        Dim iReturn As Integer = 0
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sQuery As String = _
                "SELECT COUNT(*) AS images " & _
                "FROM galeria " & _
                "WHERE img_galeria = '" & Trim(Gallery) & "'" & _
                "  AND img_cat_id = " & CatID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsImages As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsImages = cmd.ExecuteReader

        While dsImages.Read
            iReturn = dsImages("images")
        End While

        Return iReturn
    End Function

    Public Shared Function ReturnMenu(ByVal Username As String) As ArrayList
        Dim arMenu As New ArrayList
        Dim sOption(3) As String

        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sQuery As String = _
                "SELECT c.cat_title, m.men_title, f.func_url, f.func_program " & _
                "FROM FUNCIONALIDAD f INNER JOIN GRUPOS_FUNCIONALIDAD p ON f.func_id = p.func_id " & _
                "                     INNER JOIN USUARIOS_GRUPOS g ON g.grp_id = p.grp_id " & _
                "	 				  INNER JOIN MENU m ON m.men_func_id = f.func_id " & _
                "					  INNER JOIN CATEGORIA c ON c.cat_id = m.men_cat_id " & _
                "WHERE g.UserName = '" & Trim(Username) & "'" & _
                "  AND p.perm_view = 1 " & _
                "ORDER BY c.cat_title, m.men_title"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsMenu As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsMenu = cmd.ExecuteReader

        While dsMenu.Read
            ReDim sOption(3)
            sOption(0) = Trim(dsMenu("cat_title"))
            sOption(1) = Trim(dsMenu("men_title"))
            sOption(2) = Trim(dsMenu("func_url"))
            sOption(3) = Trim(dsMenu("func_program"))
            arMenu.Add(sOption)
        End While

        dsMenu.Close()
        cmd.Dispose()
        cn.Close()

        Return arMenu
    End Function

    Public Shared Function ValExtension(ByVal sExtension As String) As Boolean
        Select Case sExtension
            Case ".jpg", ".jpeg", ".png", ".gif", ".bmp"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Shared Function NextVal(Code As String, Length As String) As String
        Dim sNextVal As String = Code

        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sQuery As String = _
                "SELECT COUNT(*) + 1 AS conteo " & _
                "FROM codigos  " & _
                "WHERE cod_name = '" & Trim(Code) & "'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsNext As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsNext = cmd.ExecuteReader

        While dsNext.Read
            sNextVal = dsNext("conteo")
            sNextVal = sNextVal.PadLeft(Length, "0")
        End While

        Return sNextVal
    End Function

    Public Shared Sub FormViewMode(ctrl As Control)
        For Each ctrl In ctrl.Controls
            If (ctrl.GetType() Is GetType(TextBox)) Then
                Dim txt As TextBox = CType(ctrl, TextBox)
                txt.CssClass = "read_only"
                txt.ReadOnly = True
            ElseIf (ctrl.GetType() Is GetType(DropDownList)) Then
                Dim cbo As DropDownList = CType(ctrl, DropDownList)
                If cbo.ID <> "ThemeList" Then
                    cbo.Enabled = False
                    cbo.CssClass = "read_only"
                End If
            ElseIf (ctrl.GetType() Is GetType(Label)) Then
                Dim lbl As Label = CType(ctrl, Label)
                If lbl.Text = "*" Then lbl.Visible = False
            End If
            If ctrl.HasControls Then
                Call FormViewMode(ctrl)
            End If
        Next
    End Sub

End Class