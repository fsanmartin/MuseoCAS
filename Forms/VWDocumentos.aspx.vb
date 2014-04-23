
Partial Class Forms_frmVWAudiovisual
    Inherits System.Web.UI.Page
    Dim permDocumentos As Permisos 'New Permisos("fsanmartin", "VWAudiovisual.aspx")

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        e.Values("DELETE_") = "*"
        e.Values("UPDATE_") = Date.Now
    End Sub

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "btnEdit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = GridView1.DataKeys(index).Value

            Response.Redirect("MDocumentos.aspx?ID=" & regID)
        ElseIf e.CommandName = "btnDelete" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = GridView1.DataKeys(index).Value

            Call Functions.DeleteReg("documentos", "doc_id", regID, User.Identity.Name)

            Response.Redirect("VWDocumentos.aspx")
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permDocumentos = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.ToString()))

        If Not permDocumentos.PermisoVer Then
            Response.Redirect("/SinAcceso.aspx")
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim permDocumentos As New Permisos(User.Identity.Name, "MDocumentos.aspx")

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnImage As ImageButton = CType(e.Row.FindControl("btnImage"), ImageButton)
            btnImage.Enabled = False

            If Not permDocumentos.PermisoModificar Then
                Dim btnEdit As ImageButton = CType(e.Row.FindControl("btnEdit"), ImageButton)
                btnEdit.Visible = False
            End If

            If Not permDocumentos.PermisoEliminar Then
                Dim btnDelete As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                btnDelete.Visible = False
            End If

            If Functions.Images("DOCUMENTO", GridView1.DataKeys(e.Row.RowIndex).Value) = 0 Then
                btnImage.Visible = False
            End If

        End If
    End Sub


End Class
