Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Forms_MAudiovisual
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean
    Dim dFechaResp As Date
    Dim permAudiovisual As Permisos
    Dim iID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permAudiovisual = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permAudiovisual.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

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
                    If Not permAudiovisual.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            If permAudiovisual.PermisoAgregar Then
                If txtID.Text = "" Then
                    txtID.Text = "Nuevo"
                    btnDelete.Visible = False
                End If
            Else
                Response.Redirect("/SinAcceso.aspx")
            End If
        End If

        If (permAudiovisual.PermisoAgregar = False And permAudiovisual.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
            Call FormUploadImage(False)
        End If

        'If Not permAudiovisual.PermisoAgregar Then btnSave.Visible = False
        If Not permAudiovisual.PermisoModificar Then btnEdit.Visible = False
        If Not permAudiovisual.PermisoEliminar Then btnDelete.Visible = False

        txtID.CssClass = "read_only"

    End Sub

    Private Sub FormUploadImage(State As Boolean)
        lblUploadFile.Visible = State
        fileUpload.Visible = State
        lblTituloImagen.Visible = State
        txtImageTitle.Visible = State
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM audiovisual WHERE adv_id = " & ID & " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsAudiovisual As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsAudiovisual = cmd.ExecuteReader

        While dsAudiovisual.Read
            txtID.Text = dsAudiovisual("adv_id")
            txtNumInventario.Text = Trim(dsAudiovisual("adv_numero"))
            'If Not bPostBack Then cboDeposito.SelectedValue = Trim(dsAudiovisual("adv_dep_id"))

            cboDeposito.SelectedValue = dsAudiovisual("adv_dep_id")
            '' --------- ESTANTE
            Try
                cboEstante.SelectedValue = Trim(dsAudiovisual("adv_estante"))
                cboFila.SelectedValue = Trim(dsAudiovisual("adv_fila"))
                cboColumna.SelectedValue = Trim(dsAudiovisual("adv_columna"))
            Catch ex As ArgumentOutOfRangeException
                lblErrorMessages.Text = lblErrorMessages.Text & vbCrLf & "Depósito " & cboDeposito.Text & " ha cambiado. Debe revisar Estante (E.C.F)"
            End Try

            cboTipoBien.SelectedValue = dsAudiovisual("adv_tipo_bien_cod")
            cboDenominacion.SelectedValue = dsAudiovisual("adv_denominacion_cod")
            txtTitulo.Text = Trim(dsAudiovisual("adv_titulo"))
            txtContenido.Text = Trim(dsAudiovisual("adv_contenido"))
            txtAutor.Text = Trim(dsAudiovisual("adv_autor_creador"))
            txtAno.Text = Trim(dsAudiovisual("adv_anio"))
            cboMes.SelectedValue = dsAudiovisual("adv_mes")
            txtDia.Text = dsAudiovisual("adv_dia")
            cboDecada.SelectedValue = Trim(dsAudiovisual("adv_decada"))
            ' --------- MATERIAL TÉCNICA
            Dim dimCont As String() = Split(dsAudiovisual("adv_dimensiones_contenedor"), "/")
            txtContAlto.Text = dimCont(0)
            txtContAncho.Text = dimCont(1)
            txtContProf.Text = dimCont(2)
            Dim dimSop As String() = Split(dsAudiovisual("adv_dimensiones_soporte"), "/")
            txtSopAlto.Text = dimSop(0)
            txtSopAncho.Text = dimSop(1)
            txtSopProf.Text = dimSop(2)
            txtPeso.Text = Trim(dsAudiovisual("adv_peso"))
            txtDuracion.Text = Trim(dsAudiovisual("adv_tiempo_duracion"))
            cboConservacion.SelectedValue = dsAudiovisual("adv_estado_conservacion_cod")
            txtProcedencia.Text = Trim(dsAudiovisual("adv_procedencia"))
            txtFuncion.Text = Trim(dsAudiovisual("adv_funcion"))
            txtInscripciones.Text = Trim(dsAudiovisual("adv_inscripciones_marcas"))
            txtDescripcion.Text = Trim(dsAudiovisual("adv_descripcion_fisica"))
            txtObservaciones.Text = Trim(dsAudiovisual("adv_observaciones"))
            cboAdquisicion.SelectedValue = dsAudiovisual("adv_modo_adquisicion")
            txtPalabrasClaves.Text = Trim(dsAudiovisual("adv_palabra_clave"))
            txtRespNombre.Text = Trim(dsAudiovisual("adv_resp_nombre"))
            txtRespFecha.Text = Trim(dsAudiovisual("adv_resp_fecha"))
            txtDigitador.Text = Trim(dsAudiovisual("USERID_"))
            txtDigFecha.Text = Trim(dsAudiovisual("UPDATE_"))
        End While

        cn.Close()
    End Sub

    Private Sub LoadLists()
        'Dim sQueryDeposito As String = "SELECT dep_id, dep_name FROM deposito WHERE DELETE_ <> '*'"
        Dim sQueryCodigos As String = "SELECT cod_cod, cod_val FROM codigos WHERE cod_name = '%COD_NAME%' AND DELETE_ <> '*' ORDER BY cod_val"
        ' -------------------------
        ' Depóstio
        Dim dsDeposito As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DEPOSITO"))

        cboDeposito.DataSource = dsDeposito
        cboDeposito.DataValueField = "cod_cod"
        cboDeposito.DataTextField = "cod_val"
        cboDeposito.DataBind()
        cboDeposito.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboDeposito.SelectedValue = "-1"
        dsDeposito.Dispose()

        ' -------------------------
        ' Tipo de Bien
        Dim dsTipoBien As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_DE_BIEN"))
        cboTipoBien.DataSource = dsTipoBien
        cboTipoBien.DataValueField = "cod_cod"
        cboTipoBien.DataTextField = "cod_val"
        cboTipoBien.DataBind()
        cboTipoBien.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTipoBien.SelectedValue = "-1"
        dsTipoBien.Dispose()

        ' -------------------------
        ' Denominación Objeto
        Dim dsDenomiacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DENOMINA_OBJ_AUD"))
        cboDenominacion.DataSource = dsDenomiacion
        cboDenominacion.DataValueField = "cod_cod"
        cboDenominacion.DataTextField = "cod_val"
        cboDenominacion.DataBind()
        cboDenominacion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboDenominacion.SelectedValue = "-1"
        dsDenomiacion.Dispose()

        ' -------------------------
        ' Décadas
        'For i As Integer = ((CInt(Year(Date.Now) / 10) * 10) + 9) To 1709 Step -10
        '    cboDecada.Items.Add(New ListItem(CStr(i - 10) & "-" & CStr(i), CStr(i - 10) & "-" & CStr(i)))
        'Next i
        'cboDecada.Items.Add(New ListItem("-- ninguno --", "-1"))
        'cboDecada.SelectedValue = "-1"
        Dim dsDecada As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DECADA_AUD"))
        cboDecada.DataSource = dsDecada
        cboDecada.DataValueField = "cod_cod"
        cboDecada.DataTextField = "cod_val"
        cboDecada.DataBind()
        cboDecada.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboDecada.SelectedValue = "-1"
        dsDecada.Dispose()

        ' -------------------------
        ' Estado Conservación
        Dim dsConservacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESTADO_CONSERVACION"))
        cboConservacion.DataSource = dsConservacion
        cboConservacion.DataValueField = "cod_cod"
        cboConservacion.DataTextField = "cod_val"
        cboConservacion.DataBind()
        cboConservacion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboConservacion.SelectedValue = "-1"
        dsConservacion.Dispose()

        ' -------------------------
        ' Modo de Adquisición
        Dim dsAdquisicion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_MODO_ADQUISICION"))
        cboAdquisicion.DataSource = dsAdquisicion
        cboAdquisicion.DataValueField = "cod_cod"
        cboAdquisicion.DataTextField = "cod_val"
        cboAdquisicion.DataBind()
        cboAdquisicion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboAdquisicion.SelectedValue = "-1"
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
        cboEstante.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboEstante.SelectedValue = "-1"
        dsEFC.Dispose()

        ' Fila
        dsEFC = New SqlDataSource(sCN, sQueryFila)
        cboFila.DataSource = dsEFC
        cboFila.DataValueField = "cod_val"
        cboFila.DataTextField = "cod_val"
        cboFila.DataBind()
        cboFila.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboFila.SelectedValue = "-1"
        dsEFC.Dispose()

        ' Columna
        dsEFC = New SqlDataSource(sCN, sQueryColumna)
        cboColumna.DataSource = dsEFC
        cboColumna.DataValueField = "cod_val"
        cboColumna.DataTextField = "cod_val"
        cboColumna.DataBind()
        cboColumna.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboColumna.SelectedValue = "-1"
        dsEFC.Dispose()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As ImageClickEventArgs) Handles btnDelete.Click
        Dim sUpdateAudiovisual As String = "UPDATE AUDIOVISUAL " & _
               "SET DELETE_ = '*'" & _
               "   ,UPDATE_ = 'GETDATE()" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE adv_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateAudiovisual, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        Response.Redirect("VWAudiovisual.aspx")
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
        Dim sID As String = IIf(txtID.Text = "Nuevo", 0, txtID.Text)
        If Functions.IdentityExist(sID, "adv_id", txtNumInventario.Text, "adv_numero", "audiovisual") Then
            lblErrorMessages.Text = "El Número de Inventario existe. Favor ingrese un dato válido"
        Else
            Call SaveAudiovisual()
            Response.Redirect("VWAudiovisual.aspx")
        End If
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MAudiovisual.aspx?ID=" & Request.QueryString("ID"))
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
                    Call SaveAudiovisual()
                End If

                sPath = "~/Gallery/Audiovisual/"
                sNewName = sPath & Guid.NewGuid().ToString() & "." & sExt

                fileUpload.SaveAs(MapPath(sNewName))
                imgUpload.Width = 300

                imgUpload.Height = 300
                imgUpload.ImageUrl = sNewName

                Functions.AddImageGallery("AUDIOVISUAL", CInt(txtID.Text), IIf(txtImageTitle.Text.Trim <> "", txtImageTitle.Text, sName), sNewName)

                lblErrUpload.Text = "Archivo cargado correctamente."
            Else
                lblErrUpload.Text = "El archivo no es de tipo imagen."
            End If
        Else
            lblErrUpload.Text = "Seleccione el archivo que desea subir."
        End If
    End Sub

    Private Sub SaveAudiovisual()
        Dim sInsertAudiovisual As String = "INSERT INTO AUDIOVISUAL " & _
                           "(adv_numero " & _
                           ",adv_dep_id " & _
                           ",adv_estante " & _
                           ",adv_columna " & _
                           ",adv_fila " & _
                           ",adv_tipo_bien_cod " & _
                           ",adv_denominacion_cod " & _
                           ",adv_titulo " & _
                           ",adv_contenido " & _
                           ",adv_autor_creador " & _
                           ",adv_anio " & _
                           ",adv_mes " & _
                           ",adv_dia " & _
                           ",adv_decada " & _
                           ",adv_dimensiones_contenedor " & _
                           ",adv_dimensiones_soporte " & _
                           ",adv_peso " & _
                           ",adv_tiempo_duracion " & _
                           ",adv_estado_conservacion_cod " & _
                           ",adv_procedencia " & _
                           ",adv_funcion " & _
                           ",adv_inscripciones_marcas " & _
                           ",adv_descripcion_fisica " & _
                           ",adv_observaciones " & _
                           ",adv_modo_adquisicion " & _
                           ",adv_resp_nombre " & _
                           ",adv_resp_fecha " & _
                           ",adv_palabra_clave " & _
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
                        "'" & Trim(txtContenido.Text) & "', " & _
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(txtAno.Text) & "', " & _
                        "'" & Trim(cboMes.SelectedValue) & "', " & _
                        "'" & Trim(txtDia.Text) & "', " & _
                        "'" & Trim(cboDecada.SelectedValue) & "', " & _
                        "'" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "', " & _
                        "'" & Trim(txtSopAlto.Text) & "/" & Trim(txtSopAncho.Text) & "/" & Trim(txtSopProf.Text) & "', " & _
                        "'" & Trim(txtPeso.Text) & "', " & _
                        "'" & Trim(txtDuracion.Text) & "', " & _
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

        Dim sUpdateAudiovisual As String = "UPDATE AUDIOVISUAL " & _
               "SET adv_numero = '" & Trim(txtNumInventario.Text) & "'" & _
               "   ,adv_dep_id = '" & cboDeposito.SelectedValue & "'" & _
               "   ,adv_estante = '" & cboEstante.SelectedValue & "'" & _
               "   ,adv_columna = '" & cboColumna.SelectedValue & "'" & _
               "   ,adv_fila = '" & cboFila.SelectedValue & "'" & _
               "   ,adv_tipo_bien_cod = '" & cboTipoBien.SelectedValue & "'" & _
               "   ,adv_denominacion_cod = '" & cboDenominacion.SelectedValue & "'" & _
               "   ,adv_titulo = '" & Trim(txtTitulo.Text) & "'" & _
               "   ,adv_contenido = '" & Trim(txtContenido.Text) & "'" & _
               "   ,adv_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,adv_anio = '" & Trim(txtAno.Text) & "'" & _
               "   ,adv_mes = '" & cboMes.SelectedValue & "'" & _
               "   ,adv_dia = '" & Trim(txtDia.Text) & "'" & _
               "   ,adv_decada = '" & cboDecada.SelectedValue & "'" & _
               "   ,adv_dimensiones_contenedor = '" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "'" & _
               "   ,adv_dimensiones_soporte = '" & Trim(txtSopAlto.Text) & "/" & Trim(txtSopAncho.Text) & "/" & Trim(txtSopProf.Text) & "'" & _
               "   ,adv_peso = '" & Trim(txtPeso.Text) & "'" & _
               "   ,adv_tiempo_duracion = '" & Trim(txtDuracion.Text) & "'" & _
               "   ,adv_estado_conservacion_cod = '" & cboConservacion.SelectedValue & "'" & _
               "   ,adv_procedencia = '" & Trim(txtProcedencia.Text) & "'" & _
               "   ,adv_funcion = '" & Trim(txtFuncion.Text) & "'" & _
               "   ,adv_inscripciones_marcas = '" & Trim(txtInscripciones.Text) & "'" & _
               "   ,adv_descripcion_fisica = '" & Trim(txtDescripcion.Text) & "'" & _
               "   ,adv_observaciones = '" & Trim(txtObservaciones.Text) & "'" & _
               "   ,adv_modo_adquisicion = '" & cboAdquisicion.SelectedValue & "'" & _
               "   ,adv_palabra_clave = '" & Trim(txtPalabrasClaves.Text) & "'" & _
               "   ,adv_resp_nombre = '" & Trim(txtRespNombre.Text) & "'" & _
               "   ,adv_resp_fecha = '" & Format(dFechaResp, "yyyy-MM-dd") & "'" & _
               "   ,UPDATE_ = GETDATE()" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE adv_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertAudiovisual, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            cmd = New SqlCommand(sUpdateAudiovisual, cn)
            cmd.ExecuteNonQuery()
        End If

        cn.Close()
    End Sub
End Class
