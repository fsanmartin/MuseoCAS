Imports System.Data.SqlClient

Partial Class Security_AddUser
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim permUser As Permisos

    Protected Sub CreateUserWizard1_CreatedUser(sender As Object, e As EventArgs) Handles CreateUserWizard1.CreatedUser
        Dim cboGrupo As DropDownList = CType(CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("cboGrupo"), DropDownList)

        Dim sInsertGrupo As String = _
            "INSERT INTO usuarios_grupos VALUES ( " & _
               cboGrupo.SelectedValue & ", " & _
               "'" & CreateUserWizard1.UserName & "' )"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sInsertGrupo, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permUser = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.ToString()))

        If Not permUser.PermisoAgregar Then
            Response.Redirect("/SinAcceso.aspx")
        End If
    End Sub
End Class
