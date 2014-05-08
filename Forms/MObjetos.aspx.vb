Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Forms_MObjetos
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean
    Dim dFechaResp As Date
    Dim permObjetos As Permisos
    Dim iID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permObjetos = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permObjetos.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

        If IsPostBack Then
            bPostBack = True
        Else
            Call LoadLists()
            Call UpdateDeposito()
        End If

        If Request.QueryString.Get("ID") IsNot Nothing Then
            If Not bPostBack Then
                Call LoadValues(Request.QueryString.Get("ID"))

                If Request.QueryString("Mode") IsNot Nothing Then
                    If UCase(Request.QueryString("Mode")) = "VIEW" Then
                        Call Functions.FormViewMode(Page)
                        Call FormUploadImage(False)
                        btnSave.Visible = False
                        btnDelete.Visible = False
                    End If
                Else
                    btnEdit.Visible = False
                    If Not permObjetos.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            If permObjetos.PermisoAgregar Then
                If txtID.Text = "" Then
                    txtID.Text = "Nuevo"
                    btnDelete.Visible = False
                End If
            Else
                Response.Redirect("/SinAcceso.aspx")
            End If
        End If

        If (permObjetos.PermisoAgregar = False And permObjetos.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
            Call FormUploadImage(False)
        End If

        'If Not permObjetos.PermisoAgregar Then btnSave.Visible = False
        If Not permObjetos.PermisoModificar Then btnEdit.Visible = False
        If Not permObjetos.PermisoEliminar Then btnDelete.Visible = False

        txtID.CssClass = "read_only"

    End Sub

    Private Sub FormUploadImage(State As Boolean)
        lblUploadFile.Visible = State
        fileUpload.Visible = State
        lblTituloImagen.Visible = State
        txtImageTitle.Visible = State
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM Objetos WHERE obj_id = " & ID & " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsObjetos As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsObjetos = cmd.ExecuteReader

        While dsObjetos.Read
            txtID.Text = dsObjetos("obj_id")
            txtNumInventario.Text = Trim(dsObjetos("obj_numero"))
            'If Not bPostBack Then cboDeposito.SelectedValue = Trim(dsObjetos("obj_dep_id"))

            cboDeposito.SelectedValue = dsObjetos("obj_dep_id")
            '' --------- ESTANTE
            Try
                cboEstante.SelectedValue = Trim(dsObjetos("obj_estante"))
                cboFila.SelectedValue = Trim(dsObjetos("obj_fila"))
                cboColumna.SelectedValue = Trim(dsObjetos("obj_columna"))
            Catch ex As ArgumentOutOfRangeException
                lblErrorMessages.Text = lblErrorMessages.Text & vbCrLf & "Depósito " & cboDeposito.Text & " ha cambiado. Debe revisar Estante (E.C.F)"
            End Try

            cboTipoBien.SelectedValue = dsObjetos("obj_tipo_bien_cod")
            cboDenominacion.SelectedValue = dsObjetos("obj_denominacion_cod")
            txtTitulo.Text = Trim(dsObjetos("obj_titulo"))
            txtAutor.Text = Trim(dsObjetos("obj_autor_creador"))
            txtAno.Text = Trim(dsObjetos("obj_anio"))
            cboMes.SelectedValue = dsObjetos("obj_mes")
            txtDia.Text = dsObjetos("obj_dia")
            cboDecada.SelectedValue = Trim(dsObjetos("obj_decada"))
            ' --------- MATERIAL TÉCNICA
            Dim dimCont As String() = Split(dsObjetos("obj_dimensiones"), "/")
            txtContAlto.Text = dimCont(0)
            txtContAncho.Text = dimCont(1)
            txtContProf.Text = dimCont(2)
            txtPeso.Text = Trim(dsObjetos("obj_peso"))
            cboConservacion.SelectedValue = dsObjetos("obj_estado_conservacion_cod")
            txtProcedencia.Text = Trim(dsObjetos("obj_procedencia"))
            txtFuncion.Text = Trim(dsObjetos("obj_funcion"))
            txtInscripciones.Text = Trim(dsObjetos("obj_inscripciones_marcas"))
            txtDescripcion.Text = Trim(dsObjetos("obj_descripcion_fisica"))
            txtObservaciones.Text = Trim(dsObjetos("obj_observaciones"))
            cboAdquisicion.SelectedValue = dsObjetos("obj_modo_adquisicion")
            txtPalabrasClaves.Text = Trim(dsObjetos("obj_palabra_clave"))
            txtRespNombre.Text = Trim(dsObjetos("obj_resp_nombre"))
            txtRespFecha.Text = Trim(dsObjetos("obj_resp_fecha"))
            txtDigitador.Text = Trim(dsObjetos("USERID_"))
            txtDigFecha.Text = Trim(dsObjetos("UPDATE_"))

            'Call LoadImages(txtID.Text, "OBJETOS")
        End While

        cn.Close()
    End Sub

    'Private Sub LoadImages(sID As String, Gallery As String)
    '    Dim sQuery As String = "SELECT img_id, img_cat_id, img_nombre, img_url " & _
    '                           "FROM galeria " & _
    '                           "WHERE img_galeria = '" & Gallery & "' " & _
    '                           "  AND img_cat_id = " & sID

    '    ' Conexión SQL Server
    '    Dim cn As SqlConnection = New SqlConnection(sCN)
    '    Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
    '    Dim dsImgObjetos As SqlDataReader

    '    ' Abrir conexión
    '    cn.Open()

    '    dsImgObjetos = cmd.ExecuteReader

    '    imgObjetos.Width = 0
    '    imgObjetos.Height = 0

    '    While dsImgObjetos.Read
    '        imgObjetos.ImageUrl = dsImgObjetos("img_url")
    '        imgObjetos.Width = 300
    '        imgObjetos.Height = 300
    '    End While

    '    cn.Close()
    'End Sub

    Private Sub LoadLists()
        'Dim sQueryDeposito As String = "SELECT dep_id, dep_name FROM deposito WHERE DELETE_ <> '*'"
        Dim sQueryCodigos As String = "SELECT cod_cod, cod_val FROM codigos WHERE cod_name = '%COD_NAME%' AND DELETE_ <> '*'"
        ' -------------------------
        ' Depóstio
        Dim dsDeposito As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DEPOSITO"))

        cboDeposito.DataSource = dsDeposito
        cboDeposito.DataValueField = "cod_cod"
        cboDeposito.DataTextField = "cod_val"
        cboDeposito.DataBind()
        dsDeposito.Dispose()

        ' -------------------------
        ' Tipo de Bien
        Dim dsTipoBien As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_DE_BIEN"))
        cboTipoBien.DataSource = dsTipoBien
        cboTipoBien.DataValueField = "cod_cod"
        cboTipoBien.DataTextField = "cod_val"
        cboTipoBien.DataBind()
        dsTipoBien.Dispose()

        ' -------------------------
        ' Denominación Objeto
        Dim dsDenomiacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DENOMINACION_OBJETO"))
        cboDenominacion.DataSource = dsDenomiacion
        cboDenominacion.DataValueField = "cod_cod"
        cboDenominacion.DataTextField = "cod_val"
        cboDenominacion.DataBind()
        dsDenomiacion.Dispose()

        ' -------------------------
        ' Décadas
        For i As Integer = ((CInt(Year(Date.Now) / 10) * 10) + 9) To 1709 Step -10
            cboDecada.Items.Add(New ListItem(CStr(i - 10) & "-" & CStr(i), CStr(i - 10) & "-" & CStr(i)))
        Next i

        ' -------------------------
        ' Estado Conservación
        Dim dsConservacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESTADO_CONSERVACION"))
        cboConservacion.DataSource = dsConservacion
        cboConservacion.DataValueField = "cod_cod"
        cboConservacion.DataTextField = "cod_val"
        cboConservacion.DataBind()
        dsConservacion.Dispose()

        ' -------------------------
        ' Modo de Adquisición
        Dim dsAdquisicion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_MODO_ADQUISICION"))
        cboAdquisicion.DataSource = dsAdquisicion
        cboAdquisicion.DataValueField = "cod_cod"
        cboAdquisicion.DataTextField = "cod_val"
        cboAdquisicion.DataBind()
        dsAdquisicion.Dispose()
    End Sub

    Private Sub UpdateDeposito()
        'Dim sQueryEstante As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos, deposito " & _
        '                              "WHERE cod_name = '_ESTANTE' " & _
        '                              "  AND dep_id = " & cboDeposito.SelectedValue & _
        '                              "  AND cod_cod BETWEEN dep_rack_ini AND dep_rack_fin " & _
        '                              "ORDER BY 1"
        'Dim sQueryFila As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos, deposito " & _
        '                           "WHERE cod_name = '_FILA' " & _
        '                           "  AND dep_id = " & cboDeposito.SelectedValue & _
        '                           "  AND cod_cod BETWEEN dep_row_ini AND dep_row_fin " & _
        '                           "ORDER BY 1"
        'Dim sQueryColumna As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos, deposito " & _
        '                              "WHERE cod_name = '_COLUMNA' " & _
        '                              "  AND dep_id = " & cboDeposito.SelectedValue & _
        '                              "  AND cod_cod BETWEEN dep_column_ini AND dep_column_fin " & _
        '                              "ORDER BY 1"

        Dim sQueryEstante As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos " & _
                                      "WHERE cod_name = '_ESTANTE' " & _
                                      "ORDER BY 1"
        Dim sQueryFila As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos " & _
                                   "WHERE cod_name = '_FILA' " & _
                                   "ORDER BY 1"
        Dim sQueryColumna As String = "SELECT RTRIM(cod_val) AS cod_val FROM codigos " & _
                                      "WHERE cod_name = '_COLUMNA' " & _
                                      "ORDER BY 1"

        Dim dsEFC As SqlDataSource

        ' Esante
        dsEFC = New SqlDataSource(sCN, sQueryEstante)
        cboEstante.DataSource = dsEFC
        cboEstante.DataValueField = "cod_val"
        cboEstante.DataTextField = "cod_val"
        cboEstante.DataBind()
        dsEFC.Dispose()

        ' Fila
        dsEFC = New SqlDataSource(sCN, sQueryFila)
        cboFila.DataSource = dsEFC
        cboFila.DataValueField = "cod_val"
        cboFila.DataTextField = "cod_val"
        cboFila.DataBind()
        dsEFC.Dispose()

        ' Columna
        dsEFC = New SqlDataSource(sCN, sQueryColumna)
        cboColumna.DataSource = dsEFC
        cboColumna.DataValueField = "cod_val"
        cboColumna.DataTextField = "cod_val"
        cboColumna.DataBind()
        dsEFC.Dispose()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles btnDelete.Click
        Dim sUpdateObjetos As String = "UPDATE Objetos " & _
               "SET DELETE_ = '*'" & _
               "   ,UPDATE_ = GETDATE()" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE obj_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateObjetos, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        Response.Redirect("VWObjetos.aspx")
    End Sub

    'Protected Sub cvalRespFecha_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvalRespFecha.ServerValidate
    '    If DateTime.TryParseExact(txtRespFecha.Text, "dd-MM-yyyy", Nothing, Globalization.DateTimeStyles.None, dFechaResp) Then
    '        args.IsValid = True
    '    Else
    '        args.IsValid = False
    '    End If
    'End Sub

    Protected Sub btnSave_Click(sender As Object, e As ImageClickEventArgs) Handles btnSave.Click
        DateTime.TryParseExact(txtRespFecha.Text, "dd-MM-yyyy", Nothing, Globalization.DateTimeStyles.None, dFechaResp)
        Call SaveObjetos()
        Response.Redirect("VWObjetos.aspx")
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MObjetos.aspx?ID=" & Request.QueryString("ID"))
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim sExt As String
        Dim sName As String
        Dim sNewName As String
        Dim sPath As String

        imgUpload.Width = 0
        imgUpload.Height = 0
        imgUpload.ImageUrl = ""

        If fileUpload.HasFile Then
            sName = fileUpload.FileName
            sExt = Path.GetExtension(sName)
            If Functions.ValExtension(sExt) Then
                If txtID.Text = "Nuevo" Then
                    Call SaveObjetos()
                End If

                sPath = "~/Gallery/Objetos/"
                sNewName = sPath & Guid.NewGuid().ToString() & "." & sExt

                fileUpload.SaveAs(MapPath(sNewName))
                imgUpload.Width = 300

                imgUpload.Height = 300
                imgUpload.ImageUrl = sNewName

                Functions.AddImageGallery("OBJETOS", CInt(txtID.Text), IIf(txtImageTitle.Text.Trim <> "", txtImageTitle.Text, sName), sNewName)

                lblErrUpload.Text = "Archivo cargado correctamente."
            Else
                lblErrUpload.Text = "El archivo no es de tipo imagen."
            End If
        Else
            lblErrUpload.Text = "Seleccione el archivo que desea subir."
        End If
    End Sub

    Private Sub SaveObjetos()
        Dim sInsertObjetos As String = "INSERT INTO Objetos " & _
                           "(obj_numero " & _
                           ",obj_dep_id " & _
                           ",obj_estante " & _
                           ",obj_columna " & _
                           ",obj_fila " & _
                           ",obj_tipo_bien_cod " & _
                           ",obj_denominacion_cod " & _
                           ",obj_titulo " & _
                           ",obj_autor_creador " & _
                           ",obj_anio " & _
                           ",obj_mes " & _
                           ",obj_dia " & _
                           ",obj_decada " & _
                           ",obj_dimensiones " & _
                           ",obj_peso " & _
                           ",obj_estado_conservacion_cod " & _
                           ",obj_procedencia " & _
                           ",obj_funcion " & _
                           ",obj_inscripciones_marcas " & _
                           ",obj_descripcion_fisica " & _
                           ",obj_observaciones " & _
                           ",obj_modo_adquisicion " & _
                           ",obj_resp_nombre " & _
                           ",obj_resp_fecha " & _
                           ",obj_palabra_clave " & _
                           ",DELETE_ " & _
                           ",INSERT_ " & _
                           ",UPDATE_ " & _
                           ",USERID_) " & _
                " VALUES( '" & Trim(txtNumInventario.Text) & "', " & _
                        "'" & cboDeposito.SelectedValue & "', " & _
                        "'" & cboEstante.SelectedValue & "', " & _
                        "'" & cboColumna.SelectedValue & "', " & _
                        "'" & cboFila.SelectedValue & "', " & _
                        "'" & cboTipoBien.SelectedValue & "', " & _
                        "'" & cboDenominacion.SelectedValue & "', " & _
                        "'" & Trim(txtTitulo.Text) & "', " & _
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(txtAno.Text) & "', " & _
                        "'" & Trim(cboMes.SelectedValue) & "', " & _
                        "'" & Trim(txtDia.Text) & "', " & _
                        "'" & Trim(cboDecada.SelectedValue) & "', " & _
                        "'" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "', " & _
                        "'" & Trim(txtPeso.Text) & "', " & _
                        "'" & Trim(cboConservacion.SelectedValue) & "', " & _
                        "'" & Trim(txtProcedencia.Text) & "', " & _
                        "'" & Trim(txtFuncion.Text) & "', " & _
                        "'" & Trim(txtInscripciones.Text) & "', " & _
                        "'" & Trim(txtDescripcion.Text) & "', " & _
                        "'" & Trim(txtObservaciones.Text) & "', " & _
                        "'" & Trim(cboAdquisicion.SelectedValue) & "', " & _
                        "'" & Trim(txtRespNombre.Text) & "', " & _
                        "'" & Format(dFechaResp, "yyyy-MM-dd") & "', " & _
                        "'" & Trim(txtPalabrasClaves.Text) & "', " & _
                        "' ', " & _
                        "GETDATE(), " & _
                        "GETDATE(), " & _
                        "'" & Trim(User.Identity.Name) & "'" & _
                        "); Select Scope_Identity()"

        Dim sUpdateObjetos As String = "UPDATE Objetos " & _
               "SET obj_numero = '" & Trim(txtNumInventario.Text) & "'" & _
               "   ,obj_dep_id = '" & Trim(cboDeposito.SelectedValue) & "'" & _
               "   ,obj_estante = '" & cboEstante.SelectedValue & "'" & _
               "   ,obj_columna = '" & cboColumna.SelectedValue & "'" & _
               "   ,obj_fila = '" & cboFila.SelectedValue & "'" & _
               "   ,obj_tipo_bien_cod = '" & cboTipoBien.SelectedValue & "'" & _
               "   ,obj_denominacion_cod = '" & cboDenominacion.SelectedValue & "'" & _
               "   ,obj_titulo = '" & Trim(txtTitulo.Text) & "'" & _
               "   ,obj_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,obj_anio = '" & Trim(txtAno.Text) & "'" & _
               "   ,obj_mes = '" & cboMes.SelectedValue & "'" & _
               "   ,obj_dia = '" & Trim(txtDia.Text) & "'" & _
               "   ,obj_decada = '" & cboDecada.SelectedValue & "'" & _
               "   ,obj_dimensiones = '" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "'" & _
               "   ,obj_peso = '" & Trim(txtPeso.Text) & "'" & _
               "   ,obj_estado_conservacion_cod = '" & cboConservacion.SelectedValue & "'" & _
               "   ,obj_procedencia = '" & Trim(txtProcedencia.Text) & "'" & _
               "   ,obj_funcion = '" & Trim(txtFuncion.Text) & "'" & _
               "   ,obj_inscripciones_marcas = '" & Trim(txtInscripciones.Text) & "'" & _
               "   ,obj_descripcion_fisica = '" & Trim(txtDescripcion.Text) & "'" & _
               "   ,obj_observaciones = '" & Trim(txtObservaciones.Text) & "'" & _
               "   ,obj_modo_adquisicion = '" & cboAdquisicion.SelectedValue & "'" & _
               "   ,obj_palabra_clave = '" & Trim(txtPalabrasClaves.Text) & "'" & _
               "   ,obj_resp_nombre = '" & Trim(txtRespNombre.Text) & "'" & _
               "   ,obj_resp_fecha = '" & Format(dFechaResp, "yyyy-MM-dd") & "'" & _
               "   ,UPDATE_ = GETDATE()" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE obj_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertObjetos, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            cmd = New SqlCommand(sUpdateObjetos, cn)
            cmd.ExecuteNonQuery()
        End If

        cn.Close()
    End Sub
End Class
