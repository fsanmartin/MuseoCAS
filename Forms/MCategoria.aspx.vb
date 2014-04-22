
Partial Class Forms_frmMNTCodigos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString.Get("ID") IsNot Nothing Then
            DetailsView1.DefaultMode = DetailsViewMode.Edit
        End If
    End Sub

    Protected Sub DetailsView1_ItemInserting(sender As Object, e As DetailsViewInsertEventArgs) Handles DetailsView1.ItemInserting
        e.Values("DELETE_") = " "
        e.Values("INSERT_") = Date.Now
        e.Values("UPDATE_") = Date.Now
        e.Values("USERID_") = Trim(User.Identity.Name)
    End Sub

    Protected Sub DetailsView1_ItemUpdating(sender As Object, e As DetailsViewUpdateEventArgs) Handles DetailsView1.ItemUpdating
        e.NewValues("INSERT_") = e.OldValues("INSERT_")
        e.NewValues("DELETE_") = " "
        e.NewValues("UPDATE_") = Date.Now
        e.NewValues("USERID_") = Trim(User.Identity.Name)
    End Sub

    Protected Sub DetailsView1_ItemDeleting(sender As Object, e As DetailsViewDeleteEventArgs) Handles DetailsView1.ItemDeleting
        e.Values("DELETE_") = "*"
        'e.Values("UPDATE_") = Date.Now
        'e.Values("USERID_") = "fsanmartin"
    End Sub

    Private Sub EndEditing()
        Response.Redirect("frmVWCategoria.aspx")
    End Sub

    Protected Sub DetailsView1_ItemInserted(sender As Object, e As DetailsViewInsertedEventArgs) Handles DetailsView1.ItemInserted
        If e.Exception Is Nothing Then
            Call EndEditing()
        End If
    End Sub

    Protected Sub DetailsView1_ItemUpdated(sender As Object, e As DetailsViewUpdatedEventArgs) Handles DetailsView1.ItemUpdated
        If e.Exception Is Nothing Then
            Call EndEditing()
        End If
    End Sub

    Protected Sub DetailsView1_ItemDeleted(sender As Object, e As DetailsViewDeletedEventArgs) Handles DetailsView1.ItemDeleted
        If e.Exception Is Nothing Then
            Call EndEditing()
        End If
    End Sub
End Class
