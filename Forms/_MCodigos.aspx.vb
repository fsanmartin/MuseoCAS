
Partial Class Forms_MCodigos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString.Get("ID") IsNot Nothing Then
            DetailsView1.DefaultMode = DetailsViewMode.Edit
        End If
    End Sub

    Private Sub EndEditing()
        Response.Redirect("VWCodigos.aspx")
    End Sub

    Protected Sub DetailsView1_ItemInserting(sender As Object, e As DetailsViewInsertEventArgs) Handles DetailsView1.ItemInserting
        e.Values("cod_cod") = Functions.NextVal(Trim(e.Values("cod_name")), 5)
        e.Values("DELETE_") = " "
        e.Values("INSERT_") = Date.Now
        e.Values("UPDATE_") = Date.Now
        e.Values("USERID_") = Trim(User.Identity.Name)
    End Sub

    Protected Sub DetailsView1_ItemUpdating(sender As Object, e As DetailsViewUpdateEventArgs) Handles DetailsView1.ItemUpdating
        e.NewValues("cod_cod") = e.OldValues("cod_cod")
        e.NewValues("DELETE_") = " "
        e.NewValues("UPDATE_") = Date.Now
        e.NewValues("USERID_") = Trim(User.Identity.Name)
    End Sub

    Protected Sub DetailsView1_ItemUpdated(sender As Object, e As DetailsViewUpdatedEventArgs) Handles DetailsView1.ItemUpdated
        If e.Exception Is Nothing Then
            Call EndEditing()
        End If
    End Sub

    Protected Sub DetailsView1_ItemInserted(sender As Object, e As DetailsViewInsertedEventArgs) Handles DetailsView1.ItemInserted
        If e.Exception Is Nothing Then
            Call EndEditing()
        End If
    End Sub

    Private Sub LoadNextVal()
        Dim txtCodigo As TextBox = CType(DetailsView1.FindControl("Textbox2"), TextBox)
        Dim cboTabla As DropDownList = CType(DetailsView1.FindControl("DropDownList1"), DropDownList)
        If cboTabla.Text <> "" Then txtCodigo.Text = Functions.NextVal(cboTabla.Text, 5)
    End Sub

    Protected Sub TextBox2_Load1(sender As Object, e As EventArgs)
        Dim txtCodigo As TextBox = CType(DetailsView1.FindControl("Textbox2"), TextBox)
        txtCodigo.Enabled = False
    End Sub

    Protected Sub DetailsView1_PageIndexChanging(sender As Object, e As DetailsViewPageEventArgs) Handles DetailsView1.PageIndexChanging

    End Sub
End Class
