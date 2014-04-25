
Partial Class MasterPageAdmin
    Inherits System.Web.UI.MasterPage

    Protected Sub ThemeList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThemeList.SelectedIndexChanged
        Dim preferredTheme As HttpCookie = New HttpCookie("PreferredTheme")
        preferredTheme.Expires = DateTime.Now.AddMonths(3)
        preferredTheme.Value = ThemeList.SelectedValue
        Response.Cookies.Add(preferredTheme)
        Response.Redirect(Request.Url.ToString())
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not HttpContext.Current.User.Identity.IsAuthenticated Then
            Response.Redirect("/ErrorLogin.aspx")
        Else
            If Not Page.IsPostBack Then
                Dim selectedTheme As String = Page.Theme
                Dim preferredTheme As HttpCookie = Request.Cookies.Get("PreferredTheme")
                If preferredTheme IsNot Nothing Then
                    selectedTheme = preferredTheme.Value
                End If
                If Not String.IsNullOrEmpty(selectedTheme) Then
                    Dim item As ListItem = ThemeList.Items.FindByValue(selectedTheme)
                    If item IsNot Nothing Then
                        item.Selected = True
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        FormsAuthentication.SignOut()
        FormsAuthentication.RedirectToLoginPage()
    End Sub
End Class

