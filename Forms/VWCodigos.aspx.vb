
Partial Class Forms_VWCodigos
    Inherits System.Web.UI.Page
    Dim permCodigos As Permisos
    Dim sList As String

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        Dim permCodigos As New Permisos(User.Identity.Name, "MCodigos.aspx")

        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not permCodigos.PermisoModificar Then
                Dim btnEdit As ImageButton = CType(e.Row.FindControl("btnEdit"), ImageButton)
                btnEdit.Visible = False
            End If

            If Not permCodigos.PermisoEliminar Then
                Dim btnDelete As ImageButton = CType(e.Row.FindControl("btnDelete"), ImageButton)
                btnDelete.Visible = False
            End If
        End If
    End Sub

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "btnEdit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = GridView1.DataKeys(index).Value

            Response.Redirect("MCodigos.aspx?ID=" & regID)
        ElseIf e.CommandName = "btnDelete" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim regID As String = GridView1.DataKeys(index).Value

            Call Functions.DeleteReg("codigos", "cod_id", regID, User.Identity.Name)

            Response.Redirect("VWCodigos.aspx")
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permCodigos = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath()))

        If Not permCodigos.PermisoVer Then
            Response.Redirect("/SinAcceso.aspx")
        End If

        If Request.QueryString("List") IsNot Nothing Then
            sList = Request.QueryString("List")
        End If

    End Sub

    Protected Sub btnNew_Click(sender As Object, e As ImageClickEventArgs) Handles btnNew.Click
        Response.Redirect("MCodigos.aspx?List=" & cboCodigo.SelectedValue)
    End Sub

    Protected Sub cboCodigo_DataBound(sender As Object, e As EventArgs) Handles cboCodigo.DataBound
        If sList IsNot Nothing Then cboCodigo.SelectedValue = sList.PadRight(20, " ")
    End Sub
End Class
