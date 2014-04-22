Imports System.Data.SqlClient

Partial Class Forms_MGrupos
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Cargar DropDownLists
        If IsPostBack Then
            bPostBack = True
        Else
            Call LoadLists()
        End If

        If Request.QueryString.Get("ID") IsNot Nothing Then
            If Not bPostBack Then Call LoadValues(Request.QueryString.Get("ID"))
        Else
            txtID.Text = "Nuevo"
            btnDelete.Visible = False
        End If

        If Request.QueryString("Mode") IsNot Nothing Then
            If UCase(Request.QueryString("Mode")) = "VIEW" Then
                Call FormViewMode(Page)
                btnSave.Visible = False
                btnDelete.Visible = False
            End If
        Else
            btnEdit.Visible = False
        End If
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM grupos WHERE grp_id = " & ID & " AND DELETE_ <> '*'"
        Dim sQueryAccesos As String = "SELECT * FROM grupos_funcionalidad WHERE grp_id = " & ID '& " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsGrupos As SqlDataReader
        Dim dsAccesos As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsGrupos = cmd.ExecuteReader

        While dsGrupos.Read
            txtID.Text = dsGrupos("grp_id")
            txtGrupo.Text = Trim(dsGrupos("grp_name"))
            txtDesc.Text = Trim(dsGrupos("grp_desc"))
        End While
        dsGrupos.Close()

        cmd = New SqlCommand(sQueryAccesos, cn)
        dsAccesos = cmd.ExecuteReader

        While dsAccesos.Read
            For Each itemView As ListItem In cblView.Items
                If dsAccesos("func_id") = itemView.Value And _
                   dsAccesos("perm_view") = "1" Then
                    itemView.Selected = True
                End If
            Next
            For Each itemView As ListItem In cblNew.Items
                If dsAccesos("func_id") = itemView.Value And _
                   dsAccesos("perm_new") = "1" Then
                    itemView.Selected = True
                End If
            Next
            For Each itemView As ListItem In cblEdit.Items
                If dsAccesos("func_id") = itemView.Value And _
                   dsAccesos("perm_mod") = "1" Then
                    itemView.Selected = True
                End If
            Next
            For Each itemView As ListItem In cblDelete.Items
                If dsAccesos("func_id") = itemView.Value And _
                   dsAccesos("perm_del") = "1" Then
                    itemView.Selected = True
                End If
            Next
        End While
        dsAccesos.Close()
        cmd.Dispose()

        cn.Close()
    End Sub

    Private Sub LoadLists()
        Dim sQueryFunc As String = "SELECT func_id, %VAL% FROM funcionalidad WHERE DELETE_ <> '*'"

        ' -------------------------
        ' CheckBoxList VIEW
        Dim dsView As New SqlDataSource(sCN, Replace(sQueryFunc, "%VAL%", "func_title"))

        cblView.DataSource = dsView
        cblView.DataValueField = "func_id"
        cblView.DataTextField = "func_title"
        cblView.DataBind()
        dsView.Dispose()

        ' -------------------------
        ' CheckBoxList NEW
        Dim dsNew As New SqlDataSource(sCN, Replace(sQueryFunc, "%VAL%", "'Agregar' AS func_title"))

        cblNew.DataSource = dsNew
        cblNew.DataValueField = "func_id"
        cblNew.DataTextField = "func_title"
        cblNew.DataBind()
        dsNew.Dispose()

        ' -------------------------
        ' CheckBoxList EDIT
        Dim dsEdit As New SqlDataSource(sCN, Replace(sQueryFunc, "%VAL%", "'Modificar' AS func_title"))

        cblEdit.DataSource = dsEdit
        cblEdit.DataValueField = "func_id"
        cblEdit.DataTextField = "func_title"
        cblEdit.DataBind()
        dsEdit.Dispose()

        ' -------------------------
        ' CheckBoxList DELETE
        Dim dsDelete As New SqlDataSource(sCN, Replace(sQueryFunc, "%VAL%", "'Eliminar' AS func_title"))

        cblDelete.DataSource = dsDelete
        cblDelete.DataValueField = "func_id"
        cblDelete.DataTextField = "func_title"
        cblDelete.DataBind()
        dsDelete.Dispose()

    End Sub

    Private Sub FormViewMode(ctrl As Control)
        For Each ctrl In ctrl.Controls
            If (ctrl.GetType() Is GetType(TextBox)) Then
                Dim txt As TextBox = CType(ctrl, TextBox)
                txt.ReadOnly = True
            ElseIf (ctrl.GetType() Is GetType(DropDownList)) Then
                Dim cbo As DropDownList = CType(ctrl, DropDownList)
                cbo.Enabled = False
            ElseIf (ctrl.GetType() Is GetType(CheckBoxList)) Then
                Dim cbo As CheckBoxList = CType(ctrl, CheckBoxList)
                cbo.Enabled = False
            End If
            If ctrl.HasControls Then
                Call FormViewMode(ctrl)
            End If
        Next
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As ImageClickEventArgs) Handles btnSave.Click
        Dim sInsertGrupo As String = "INSERT INTO grupos " & _
                           "(grp_name " & _
                           ",grp_desc " & _
                           ",DELETE_ " & _
                           ",INSERT_ " & _
                           ",UPDATE_ " & _
                           ",USERID_) " & _
                    " VALUES( '" & Trim(txtGrupo.Text) & "', " & _
                            "'" & Trim(txtDesc.Text) & "', " & _
                            "' ', " & _
                            "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "', " & _
                            "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "', " & _
                            "'" & Trim(User.Identity.Name) & "'" & _
                            ") "

        Dim sUpdateGrupo As String = "UPDATE grupos " & _
               "SET grp_name = '" & Trim(txtGrupo.Text) & "'" & _
               "   ,grp_desc = '" & Trim(txtDesc.Text) & "'" & _
               "   ,UPDATE_ = '" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE grp_id = " & txtID.Text

        Dim sDelete As String = "DELETE FROM grupos_funcionalidad WHERE grp_id = " & Trim(txtID.Text)

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" Then
            cmd = New SqlCommand(sInsertGrupo, cn)
            cmd.ExecuteNonQuery()
        Else
            cmd = New SqlCommand(sUpdateGrupo, cn)
            cmd.ExecuteNonQuery()
        End If

        ' Limpiar GRUPOS_FUNCIONALIDAD
        cmd = New SqlCommand(sDelete, cn)
        cmd.ExecuteNonQuery()

        ' Grabar los Accesos en GRUPOS_FUNCIONALIDAD
        Call CheckAccess(txtID.Text)

        cn.Close()

        Response.Redirect("VWGrupos.aspx")

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MGrupos.aspx?ID=" & Request.QueryString("ID"))
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles btnDelete.Click
        Dim sUpdateGrupo As String = "UPDATE grupos " & _
                       "SET DELETE_ = '*' " & _
                       "   ,UPDATE_ = '" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" & _
                       "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
                       "WHERE grp_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateGrupo, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        Response.Redirect("VWGrupos.aspx")
    End Sub

    Private Sub CheckAccess(GrupoID As String)
        Dim Celda As String() '= {"0", "0", "0", "0"}
        Dim sInsertGrupoFunc As String = _
                    "INSERT INTO grupos_funcionalidad " & _
                           "(grp_id " & _
                           ",func_id " & _
                           ",perm_view " & _
                           ",perm_new " & _
                           ",perm_mod " & _
                           ",perm_del) " & _
                    " VALUES( " & Trim(GrupoID) & ", "

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        For Each itemView As ListItem In cblView.Items
            Celda = {"0", "0", "0", "0"}

            If itemView.Selected Then
                Celda(0) = "1"
            End If

            For Each itemNew As ListItem In cblNew.Items
                If itemView.Value = itemNew.Value And itemNew.Selected Then
                    Celda(0) = "1"
                    Celda(1) = "1"
                End If
            Next
            For Each itemEdit As ListItem In cblEdit.Items
                If itemView.Value = itemEdit.Value And itemEdit.Selected Then
                    Celda(0) = "1"
                    Celda(2) = "1"
                End If
            Next
            For Each itemDel As ListItem In cblDelete.Items
                If itemView.Value = itemDel.Value And itemDel.Selected Then
                    Celda(0) = "1"
                    Celda(3) = "1"
                End If
            Next

            'Console.WriteLine(itemView.Text & " -> " & Celda(0) & ", " & Celda(1) & ", " & Celda(2) & ", " & Celda(3) & ", ")
            cmd = New SqlCommand(sInsertGrupoFunc & itemView.Value & ", " & Celda(0) & ", " & Celda(1) & ", " & Celda(2) & ", " & Celda(3) & ")", cn)
            cmd.ExecuteNonQuery()

        Next

        cn.Close()

        Response.Redirect("VWGrupos.aspx")

    End Sub
End Class
