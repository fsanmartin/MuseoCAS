Imports System.Data

Partial Class Forms_frmCodigos
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        e.Values("UPDATE_") = Date.Now
        e.Values("USERID_") = "fsanmartin"
    End Sub
End Class
