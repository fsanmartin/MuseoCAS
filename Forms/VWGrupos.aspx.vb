
Partial Class Forms_MGrupos
    Inherits System.Web.UI.Page

    Protected Sub grdGrupos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdGrupos.RowCommand
        If e.CommandName = "btnEdit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = grdGrupos.DataKeys(index).Value

            Response.Redirect("MGrupos.aspx?ID=" & regID)
        End If
    End Sub
End Class
