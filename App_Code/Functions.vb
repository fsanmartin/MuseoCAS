Imports Microsoft.VisualBasic
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

    Public Shared Sub UpdateImageGallery(ByVal PhotoID As Integer, ByVal Name As String)
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sUpdateGallery As String = _
            "UPDATE galeria " & _
            " SET img_nombre = '" & Trim(Name) & "' " & _
            "WHERE img_id = " & PhotoID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateGallery, cn)
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
    End Sub

    Public Shared Sub DeleteImageGallery(ByVal PhotoID As Integer)
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sDeleteGallery As String = _
            "DELETE FROM galeria " & _
            "WHERE img_id = " & PhotoID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sDeleteGallery, cn)
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
    End Sub

    Public Shared Sub DeleteGallery(ByVal Gallery As String, ByVal CatID As Integer)
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sDeleteGallery As String = _
            "DELETE FROM galeria " & _
            "WHERE img_galeria = '" & Trim(Gallery) & "' " & _
            "  AND img_cat_id = " & CatID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sDeleteGallery, cn)
        cmd.ExecuteNonQuery()

        cmd.Dispose()
        cn.Close()
    End Sub

    Public Shared Sub DeleteReg(Tablename As String, FieldID As String, ID As Integer, Username As String)
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sDelete As String =
            " UPDATE " & Tablename & _
            " SET DELETE_ = '*'" & _
            "    ,UPDATE_ = GETDATE() " & _
            "    ,USERID_ = '" & Trim(Username) & "' " & _
            " WHERE " & FieldID & " = " & ID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sDelete, cn)
        cmd.ExecuteNonQuery()

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

        cn.Close()

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
            sOption(2) = Trim(System.Configuration.ConfigurationManager.AppSettings("site")) & Trim(dsMenu("func_url"))
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

        cn.Close()

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
            ElseIf (ctrl.GetType() Is GetType(CheckBoxList)) Then
                Dim cbo As CheckBoxList = CType(ctrl, CheckBoxList)
                cbo.Enabled = False
                cbo.CssClass = "read_only"
            ElseIf (ctrl.GetType() Is GetType(Label)) Then
                Dim lbl As Label = CType(ctrl, Label)
                If lbl.Text = "*" Then lbl.Visible = False
            ElseIf (ctrl.GetType() Is GetType(Button)) Then
                Dim btn As Button = CType(ctrl, Button)
                If btn.ID <> "btnQuit" Then
                    btn.Visible = False
                End If
            End If
            If ctrl.HasControls Then
                Call FormViewMode(ctrl)
            End If
        Next
    End Sub

    Public Shared Function LoadImages(sID As String, Gallery As String) As ArrayList
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim sImage(2) As String
        Dim arImagesOutput As New ArrayList
        Dim sQuery As String = "SELECT img_id, img_cat_id, img_nombre, img_url " & _
                               "FROM galeria " & _
                               "WHERE img_galeria = '" & Gallery & "' " & _
                               "  AND img_cat_id = " & sID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsImages As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsImages = cmd.ExecuteReader

        While dsImages.Read
            ReDim sImage(2)
            sImage(0) = dsImages("img_url")
            sImage(1) = dsImages("img_nombre")
            sImage(2) = dsImages("img_id")
            arImagesOutput.Add(sImage)
        End While

        cn.Close()

        Return arImagesOutput
    End Function

    Public Shared Function IdentityExist(sID As String, sIDFieldName As String, _
                                         sKey As String, sKeyFieldName As String, _
                                         sTable As String) As Boolean

        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim bReturn As Boolean
        Dim iExist As Short
        Dim sQuery As String = "SELECT COUNT(*) AS existe " & _
                               "FROM " & sTable & " " & _
                               "WHERE " & sKeyFieldName & " = '" & sKey & "' " & _
                               "  AND " & sIDFieldName & " <> " & Val(sID) & _
                               "  AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsExist As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsExist = cmd.ExecuteReader

        While dsExist.Read
            iExist = dsExist("existe")
        End While

        If iExist > 0 Then bReturn = True

        cn.Close()

        Return bReturn
    End Function

    Public Shared Sub SaveMaterial(MaterialName As String, CodName As String, CatID As Integer, cblMaterial As CheckBoxList)
        Dim sInsertMaterial As String = _
                    "INSERT INTO material " & _
                           "(mat_name " & _
                           ",mat_cat_id " & _
                           ",mat_cod_name " & _
                           ",mat_cod_cod) " & _
                    " VALUES( '" & Trim(MaterialName) & "', " & CatID & ", '" & Trim(CodName) & "', "

        ' Conexión SQL Server
        Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        For Each itemMat As ListItem In cblMaterial.Items
            If itemMat.Selected Then
                cmd = New SqlCommand(sInsertMaterial & "'" & itemMat.Value & "')", cn)
                cmd.ExecuteNonQuery()
            End If
        Next

        cn.Close()

    End Sub

    Public Shared Sub SaveImagesTitle(ByVal CtrlCont As Control)
        For Each ctrl In CtrlCont.Controls
            If (ctrl.GetType() Is GetType(TextBox)) Then
                Dim txt As TextBox = CType(ctrl, TextBox)
                If txt.ID.Contains("txtImageGallery") Then
                    Dim sName As String() = Split(txt.ID, "_")
                    Call Functions.UpdateImageGallery(sName(1), txt.Text)
                End If
            End If
            If ctrl.HasControls Then
                Call SaveImagesTitle(ctrl)
            End If
        Next
    End Sub
End Class
