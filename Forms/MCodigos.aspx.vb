Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Forms_MCodigos
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean
    Dim dFechaResp As Date
    Dim permCodigos As Permisos
    Dim iID As Integer
    Dim sReturn As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permCodigos = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permCodigos.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

        If IsPostBack Then
            bPostBack = True
        Else
            Call LoadLists()
        End If

        If Request.QueryString.Get("ID") IsNot Nothing Then
            If Not bPostBack Then
                Call LoadValues(Request.QueryString.Get("ID"))

                cboTabla.CssClass = "read_only"
                cboTabla.Enabled = False

                If Request.QueryString("Mode") IsNot Nothing Then
                    If UCase(Request.QueryString("Mode")) = "VIEW" Then
                        Call Functions.FormViewMode(Page)
                        btnSave.Visible = False
                        btnDelete.Visible = False
                    End If
                Else
                    btnEdit.Visible = False
                    If Not permCodigos.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            If Request.QueryString("List") IsNot Nothing Then
                sReturn = Request.QueryString("List")
                cboTabla.SelectedValue = sReturn
            End If
            If permCodigos.PermisoAgregar Then
                If txtID.Text = "" Then
                    txtID.Text = "Nuevo"
                    btnDelete.Visible = False
                    cboTabla.CssClass = "textboxs"
                    cboTabla.Enabled = True
                End If
            Else
                Response.Redirect("/SinAcceso.aspx")
            End If
        End If

        If (permCodigos.PermisoAgregar = False And permCodigos.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
        End If

        If Not permCodigos.PermisoModificar Then btnEdit.Visible = False
        If Not permCodigos.PermisoEliminar Then btnDelete.Visible = False

        txtID.CssClass = "read_only"
        txtDigitador.CssClass = "read_only"
        txtDigFecha.CssClass = "read_only"
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM codigos WHERE cod_id = " & ID & " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsCodigos As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsCodigos = cmd.ExecuteReader

        While dsCodigos.Read
            txtID.Text = dsCodigos("cod_id")
            cboTabla.SelectedValue = dsCodigos("cod_cod")
            txtValor.Text = Trim(dsCodigos("cod_val"))
            txtDigitador.Text = Trim(dsCodigos("USERID_"))
            txtDigFecha.Text = Trim(dsCodigos("UPDATE_"))
        End While

        cn.Close()
    End Sub

    Private Sub LoadLists()
        Dim sQueryCodigos As String = "SELECT rtrim(cod_cod) as cod_cod, rtrim(cod_val) as cod_val FROM codigos WHERE cod_name = '%COD_NAME%' AND DELETE_ <> '*'"
        ' -------------------------
        ' Maestro de Tablas de Códigos
        Dim dsTabla As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "__TABLES"))
        cboTabla.DataSource = dsTabla
        cboTabla.DataValueField = "cod_cod"
        cboTabla.DataTextField = "cod_val"
        cboTabla.DataBind()
        dsTabla.Dispose()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles btnDelete.Click
        Dim sUpdateCodigos As String = "UPDATE codigos " & _
               "SET DELETE_ = '*'" & _
               "   ,UPDATE_ = GETDATE() " & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE cod_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateCodigos, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        Response.Redirect("VWCodigos.aspx?List=" & cboTabla.SelectedValue)
    End Sub

    'Protected Sub cvalRespFecha_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvalRespFecha.ServerValidate
    '    If DateTime.TryParseExact(txtRespFecha.Text, "dd-MM-yyyy", Nothing, Globalization.DateTimeStyles.None, dFechaResp) Then
    '        args.IsValid = True
    '    Else
    '        args.IsValid = False
    '    End If
    'End Sub

    Protected Sub btnSave_Click(sender As Object, e As ImageClickEventArgs) Handles btnSave.Click
        Call SaveCodigos()
        Response.Redirect("VWCodigos.aspx?List=" & cboTabla.SelectedValue)
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MCodigos.aspx?ID=" & Request.QueryString("ID"))
    End Sub

    Private Sub SaveCodigos()
        Dim sInsertCodigos As String = "INSERT INTO codigos " & _
                           "(cod_name " & _
                           ",cod_cod " & _
                           ",cod_val " & _
                           ",DELETE_ " & _
                           ",INSERT_ " & _
                           ",UPDATE_ " & _
                           ",USERID_) " & _
                " VALUES( '" & Trim(cboTabla.SelectedValue) & "', " & _
                        "'" & Functions.NextVal(cboTabla.Text, 5) & "', " & _
                        "'" & Trim(txtValor.Text) & "', " & _
                        "' ', " & _
                        "GETDATE(), " & _
                        "GETDATE(), " & _
                        "'" & Trim(User.Identity.Name) & "'" & _
                        "); Select Scope_Identity()"

        Dim sUpdateCodigos As String = "UPDATE codigos " & _
               "SET cod_val = '" & Trim(txtValor.Text) & "'" & _
               "   ,UPDATE_ = GETDATE() " & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
               "WHERE cod_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertCodigos, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            cmd = New SqlCommand(sUpdateCodigos, cn)
            cmd.ExecuteNonQuery()
        End If

        cn.Close()
    End Sub

    Protected Sub btnVolver_Click(sender As Object, e As ImageClickEventArgs) Handles btnVolver.Click
        Response.Redirect("VWCodigos.aspx?List=" & cboTabla.SelectedValue)
    End Sub
End Class
