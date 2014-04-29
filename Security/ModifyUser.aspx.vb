Imports System.Data.SqlClient

Partial Class Security_ModifyUser
    Inherits System.Web.UI.Page
    Dim permGrupo As Permisos
    Dim bPostBack As Boolean
    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permGrupo = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permGrupo.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

        If IsPostBack Then
            bPostBack = True
        Else
            Call LoadList()
        End If

        If Request.QueryString.Get("ID") IsNot Nothing Then
            If Not bPostBack Then
                Call LoadValues(Request.QueryString.Get("ID"))

                If Request.QueryString("Mode") IsNot Nothing Then
                    If UCase(Request.QueryString("Mode")) = "VIEW" Then
                        Call Functions.FormViewMode(Page)
                        Call PasswordState(False)
                        btnSave.Visible = False
                        btnDelete.Visible = False
                    End If
                Else
                    btnEdit.Visible = False
                    If Not permGrupo.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            Response.Redirect("VWUsers.aspx")
        End If

        If (permGrupo.PermisoAgregar = False And permGrupo.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
            Call PasswordState(False)
        End If

        'If Not permAudiovisual.PermisoAgregar Then btnSave.Visible = False
        If Not permGrupo.PermisoModificar Then btnEdit.Visible = False
        If Not permGrupo.PermisoEliminar Then btnDelete.Visible = False

        UserName.CssClass = "read_only"

    End Sub

    Private Sub LoadList()
        Dim sQuery As String = "SELECT grp_id, grp_name FROM grupos "

        ' -------------------------
        ' CheckBoxList VIEW
        Dim dsGrupo As New SqlDataSource(sCN, sQuery)

        cblGrupos.DataSource = dsGrupo
        cblGrupos.DataValueField = "grp_id"
        cblGrupos.DataTextField = "grp_name"
        cblGrupos.DataBind()
        dsGrupo.Dispose()
    End Sub

    Private Sub PasswordState(State As Boolean)
        PasswordLabel.Visible = State
        Password.Visible = State
        'PasswordRequired.Enabled = State
        ConfirmPasswordLabel.Visible = State
        ConfirmPassword.Visible = State
        'ConfirmPasswordRequired.Enabled = State
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim userKey As New Guid(ID)
        Dim User As MembershipUser = Membership.GetUser(userKey)
        Dim sQuery As String = "SELECT g.grp_id AS grp_id, g.grp_name AS grp_name FROM GRUPOS g, USUARIOS_GRUPOS u WHERE g.grp_id = u.grp_id AND u.UserName = '" & User.UserName & "'"

        UserName.Text = User.UserName
        Email.Text = User.Email

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsGrupoUsuario As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsGrupoUsuario = cmd.ExecuteReader

        While dsGrupoUsuario.Read
            For Each itemView As ListItem In cblGrupos.Items
                If dsGrupoUsuario("grp_id") = itemView.Value Then
                    itemView.Selected = True
                End If
            Next
        End While

        cn.Close()
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("ModifyUser.aspx?ID=" & Request.QueryString("ID"))
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles btnDelete.Click
        Dim User As MembershipUser = Membership.GetUser(UserName.Text)

        User.IsApproved = False
        Membership.UpdateUser(User)

        Response.Redirect("VWUsers.aspx")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As ImageClickEventArgs) Handles btnSave.Click
        Dim User As MembershipUser = Membership.GetUser(UserName.Text)

        User.ChangePassword(User.ResetPassword(), Password.Text)
        User.Email = Email.Text

        User.IsApproved = True

        Dim sDeleteGrupos As String = "DELETE FROM USUARIOS_GRUPOS WHERE username = '" & User.UserName & "'"
        Dim sInsertGrupo As String = "INSERT INTO usuarios_grupos (grp_id, username) VALUES (%GRUPO%, '" & User.UserName & "')"
        'Dim sUpdateGrupo As String = _
        '    "UPDATE usuarios_grupos " & _
        '    "SET grp_id = " & cboGrupo.SelectedValue & _
        '    "WHERE username = '" & User.UserName & "'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sDeleteGrupos, cn)
        cmd.ExecuteNonQuery()

        For Each itemGrupo As ListItem In cblGrupos.Items
            If itemGrupo.Selected Then
                cmd = New SqlCommand(Replace(sInsertGrupo, "%GRUPO%", itemGrupo.Value), cn)
                cmd.ExecuteNonQuery()
            End If
        Next

        cn.Close()

        Membership.UpdateUser(User)

        Response.Redirect("VWUsers.aspx")
    End Sub
End Class
