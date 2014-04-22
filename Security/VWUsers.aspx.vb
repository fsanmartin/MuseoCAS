
Partial Class Security_VWUsers
    Inherits System.Web.UI.Page
    Dim permUser As Permisos

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "btnEdit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = GridView1.DataKeys(index).Value.ToString

            Response.Redirect("ModifyUser.aspx?ID=" & regID)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permUser = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.ToString()))

        If Not permUser.PermisoVer Then
            Response.Redirect("/SinAcceso.aspx")
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim permUser As New Permisos(User.Identity.Name, "ModifyUser.aspx")

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Not permUser.PermisoModificar Then
                Dim btnEdit As ImageButton = CType(e.Row.FindControl("btnEdit"), ImageButton)
                btnEdit.Visible = False
            End If

            'If Not permUser.PermisoEliminar Then
            '    Dim btnDelete As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
            '    btnDelete.Visible = False
            'End If

        End If

    End Sub
End Class
