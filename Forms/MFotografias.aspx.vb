Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Forms_MFotografias
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean
    Dim dFechaResp As Date
    Dim permFotografias As Permisos
    Dim iID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permFotografias = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permFotografias.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

        If IsPostBack Then
            bPostBack = True
        Else
            Call LoadLists()
            Call UpdateDeposito()
        End If

        If Request.QueryString.Get("ID") IsNot Nothing Then
            Call LoadImagesTable("FOTOGRAFIA", Request.QueryString.Get("ID"), panImagesTable)
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
                    If Not permFotografias.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            If permFotografias.PermisoAgregar Then
                If txtID.Text = "" Then
                    txtID.Text = "Nuevo"
                    btnDelete.Visible = False
                End If
            Else
                Response.Redirect("/SinAcceso.aspx")
            End If
        End If

        If (permFotografias.PermisoAgregar = False And permFotografias.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
            Call FormUploadImage(False)
        End If

        'If Not permFotografias.PermisoAgregar Then btnSave.Visible = False
        If Not permFotografias.PermisoModificar Then btnEdit.Visible = False
        If Not permFotografias.PermisoEliminar Then btnDelete.Visible = False

        txtID.CssClass = "read_only"

    End Sub

    Private Sub FormUploadImage(State As Boolean)
        lblUploadFile.Visible = State
        fileUpload.Visible = State
        lblTituloImagen.Visible = State
        txtImageTitle.Visible = State
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM fotografia WHERE fot_id = " & ID & " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsFotografias As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsFotografias = cmd.ExecuteReader

        While dsFotografias.Read
            txtID.Text = dsFotografias("fot_id")
            txtNumInventario.Text = Trim(dsFotografias("fot_numero"))
            'If Not bPostBack Then cboDeposito.SelectedValue = Trim(dsFotografias("fot_dep_id"))

            cboDeposito.SelectedValue = dsFotografias("fot_dep_id")
            '' --------- ESTANTE
            Try
                cboEstante.SelectedValue = Trim(dsFotografias("fot_estante"))
                cboFila.SelectedValue = Trim(dsFotografias("fot_fila"))
                cboColumna.SelectedValue = Trim(dsFotografias("fot_columna"))
            Catch ex As ArgumentOutOfRangeException
                lblErrorMessages.Text = lblErrorMessages.Text & vbCrLf & "Depósito " & cboDeposito.Text & " ha cambiado. Debe revisar Estante (E.C.F)"
            End Try

            cboTipoBien.SelectedValue = dsFotografias("fot_tipo_bien_cod")
            txtTitulo.Text = Trim(dsFotografias("fot_titulo"))
            cboPeriodoEpoca.SelectedValue = dsFotografias("fot_periodo_epoca_cod")
            txtAutor.Text = Trim(dsFotografias("fot_autor_creador"))
            txtAno.Text = Trim(dsFotografias("fot_anio"))
            cboMes.SelectedValue = dsFotografias("fot_mes")
            txtDia.Text = Trim(dsFotografias("fot_dia"))
            txtLugar.Text = Trim(dsFotografias("fot_lugar"))
            txtFondo.Text = Trim(dsFotografias("fot_fondo"))
            txtFechaEntrada.Text = Trim(dsFotografias("fot_fecha_entrada"))
            cboAdquisicion.SelectedValue = dsFotografias("fot_modo_adquisicion")
            txtProcedencia.Text = Trim(dsFotografias("fot_procedencia"))
            cboEspecialidad.SelectedValue = dsFotografias("fot_especialidad_cod")
            cboEstructura.SelectedValue = dsFotografias("fot_estructura_formal_cod")
            txtDescripcion.Text = Trim(dsFotografias("fot_descripcion"))
            cboTipoMaterial.SelectedValue = dsFotografias("fot_tipo_material_cod")
            cboTono.SelectedValue = dsFotografias("fot_tono_cod")
            cboSoporteImagen.SelectedValue = dsFotografias("fot_soporte_cod")
            Dim dimSop As String() = Split(dsFotografias("fot_dimensiones_soporte"), "/")
            txtSopAlto.Text = dimSop(0)
            txtSopAncho.Text = dimSop(1)
            txtSopProf.Text = dimSop(2)
            Dim dimCont As String() = Split(dsFotografias("fot_dimensiones_imagen"), "/")
            txtContAlto.Text = dimCont(0)
            txtContAncho.Text = dimCont(1)
            txtContProf.Text = dimCont(2)
            txtInscripciones.Text = Trim(dsFotografias("fot_inscripciones_marcas"))
            cboConservacion.SelectedValue = dsFotografias("fot_estado_conservacion_cod")
            txtObservaciones.Text = Trim(dsFotografias("fot_observaciones"))
            txtPalabrasClaves.Text = Trim(dsFotografias("fot_palabra_clave"))
            txtRespNombre.Text = Trim(dsFotografias("fot_resp_nombre"))
            txtRespFecha.Text = Trim(dsFotografias("fot_resp_fecha"))
            txtDigitador.Text = Trim(dsFotografias("USERID_"))
            txtDigFecha.Text = Trim(dsFotografias("UPDATE_"))

        End While

        cn.Close()

        Call Functions.ResizeTextbox(Page)
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
        ' Periodo Epoca
        Dim dsPeriodo As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_PERIODO_EPOCA"))
        cboPeriodoEpoca.DataSource = dsPeriodo
        cboPeriodoEpoca.DataValueField = "cod_cod"
        cboPeriodoEpoca.DataTextField = "cod_val"
        cboPeriodoEpoca.DataBind()
        cboPeriodoEpoca.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboPeriodoEpoca.SelectedValue = "-1"
        dsPeriodo.Dispose()

        ' -------------------------
        ' Año
        ''For i As Integer = Year(Date.Now) To 1709 Step -1
        ''    cboAnio.Items.Add(New ListItem(i, i))
        ''Next i
        ''cboAnio.Items.Add(New ListItem("-- ninguno --", "-1"))
        ''cboAnio.SelectedValue = "-1"

        ' -------------------------
        ' Especialidad
        Dim dsEspecialidad As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESPECIALIDAD"))
        cboEspecialidad.DataSource = dsEspecialidad
        cboEspecialidad.DataValueField = "cod_cod"
        cboEspecialidad.DataTextField = "cod_val"
        cboEspecialidad.DataBind()
        cboEspecialidad.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboEspecialidad.SelectedValue = "-1"
        dsEspecialidad.Dispose()

        ' -------------------------
        ' Estructura
        Dim dsEstructura As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESTRUCTURA_FORMAL"))
        cboEstructura.DataSource = dsEstructura
        cboEstructura.DataValueField = "cod_cod"
        cboEstructura.DataTextField = "cod_val"
        cboEstructura.DataBind()
        cboEstructura.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboEstructura.SelectedValue = "-1"
        dsEstructura.Dispose()

        ' -------------------------
        ' Tipo de Material
        Dim dsTipoMaterial As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_MATERIAL"))
        cboTipoMaterial.DataSource = dsTipoMaterial
        cboTipoMaterial.DataValueField = "cod_cod"
        cboTipoMaterial.DataTextField = "cod_val"
        cboTipoMaterial.DataBind()
        cboTipoMaterial.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTipoMaterial.SelectedValue = "-1"
        dsTipoMaterial.Dispose()

        ' -------------------------
        ' Tono
        Dim dsTono As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TONO"))
        cboTono.DataSource = dsTono
        cboTono.DataValueField = "cod_cod"
        cboTono.DataTextField = "cod_val"
        cboTono.DataBind()
        cboTono.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTono.SelectedValue = "-1"
        dsTono.Dispose()

        ' -------------------------
        ' Soporte
        Dim dsSoporte As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_SOPORTE_IMAGEN"))
        cboSoporteImagen.DataSource = dsSoporte
        cboSoporteImagen.DataValueField = "cod_cod"
        cboSoporteImagen.DataTextField = "cod_val"
        cboSoporteImagen.DataBind()
        cboSoporteImagen.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboSoporteImagen.SelectedValue = "-1"
        dsSoporte.Dispose()

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
        Dim sUpdateFotografias As String = "UPDATE fotografia " & _
               "SET DELETE_ = '*'" & _
               "   ,UPDATE_ = GETDATE()" & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE fot_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateFotografias, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        ' Eliminar imágenes de la galería
        Call Functions.DeleteGallery("FOTOGRAFIA", txtID.Text)

        Response.Redirect("VWFotografias.aspx")
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

        If Functions.IdentityExist(sID, "fot_id", txtNumInventario.Text, "fot_numero", "fotografia") Then
            lblErrorMessages.Text = "El Número de Inventario existe. Favor ingrese un dato válido"
        Else
            Call SaveFotografias()
            Response.Redirect("VWFotografias.aspx")
        End If
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MFotografias.aspx?ID=" & Request.QueryString("ID"))
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
                    Call SaveFotografias()
                End If

                sPath = "~/Gallery/Fotografias/"
                sNewName = sPath & Guid.NewGuid().ToString() & "." & sExt

                fileUpload.SaveAs(MapPath(sNewName))
                imgUpload.Width = 300

                imgUpload.Height = 300
                imgUpload.ImageUrl = sNewName

                Functions.AddImageGallery("FOTOGRAFIA", CInt(txtID.Text), IIf(txtImageTitle.Text.Trim <> "", txtImageTitle.Text, sName), sNewName)

                lblErrUpload.Text = "Archivo cargado correctamente."
                txtImageTitle.Text = ""
            Else
                lblErrUpload.Text = "El archivo no es de tipo imagen."
            End If
        Else
            lblErrUpload.Text = "Seleccione el archivo que desea subir."
        End If
    End Sub

    Private Sub SaveFotografias()
        Dim sInsertFotografias As String = "INSERT INTO fotografia " & _
                           "(fot_numero " & _
                           ",fot_dep_id " & _
                           ",fot_estante " & _
                           ",fot_columna " & _
                           ",fot_fila " & _
                           ",fot_tipo_bien_cod " & _
                           ",fot_titulo " & _
                           ",fot_periodo_epoca_cod " & _
                           ",fot_autor_creador " & _
                           ",fot_anio " & _
                           ",fot_mes " & _
                           ",fot_dia " & _
                           ",fot_lugar " & _
                           ",fot_fondo " & _
                           ",fot_fecha_entrada " & _
                           ",fot_modo_adquisicion " & _
                           ",fot_procedencia " & _
                           ",fot_estructura_formal_cod " & _
                           ",fot_especialidad_cod " & _
                           ",fot_descripcion " & _
                           ",fot_tipo_material_cod " & _
                           ",fot_tono_cod " & _
                           ",fot_soporte_cod " & _
                           ",fot_dimensiones_soporte " & _
                           ",fot_dimensiones_imagen " & _
                           ",fot_inscripciones_marcas " & _
                           ",fot_estado_conservacion_cod " & _
                           ",fot_observaciones " & _
                           ",fot_resp_nombre " & _
                           ",fot_resp_fecha " & _
                           ",fot_palabra_clave " & _
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
                        "'" & Trim(txtTitulo.Text) & "', " & _
                        "'" & cboPeriodoEpoca.SelectedValue & "', " & _
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(txtAno.Text) & "', " & _
                        "'" & Trim(cboMes.SelectedValue) & "', " & _
                        "'" & Trim(txtDia.Text) & "', " & _
                        "'" & Trim(txtLugar.Text) & "', " & _
                        "'" & Trim(txtFondo.Text) & "', " & _
                        "'" & Trim(txtFechaEntrada.Text) & "', " & _
                        "'" & Trim(cboAdquisicion.SelectedValue) & "', " & _
                        "'" & Trim(txtProcedencia.Text) & "', " & _
                        "'" & Trim(cboEspecialidad.SelectedValue) & "', " & _
                        "'" & Trim(cboEstructura.SelectedValue) & "', " & _
                        "'" & Trim(txtDescripcion.Text) & "', " & _
                        "'" & Trim(cboTipoMaterial.SelectedValue) & "', " & _
                        "'" & Trim(cboTono.SelectedValue) & "', " & _
                        "'" & Trim(cboSoporteImagen.SelectedValue) & "', " & _
                        "'" & Trim(txtSopAlto.Text) & "/" & Trim(txtSopAncho.Text) & "/" & Trim(txtSopProf.Text) & "', " & _
                        "'" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "', " & _
                        "'" & Trim(txtInscripciones.Text) & "', " & _
                        "'" & Trim(cboConservacion.SelectedValue) & "', " & _
                        "'" & Trim(txtObservaciones.Text) & "', " & _
                        "'" & Trim(txtRespNombre.Text) & "', " & _
                        "'" & Format(dFechaResp, "yyyy-MM-dd") & "', " & _
                        "'" & Trim(txtPalabrasClaves.Text) & "', " & _
                        "' ', " & _
                        "GETDATE(), " & _
                        "GETDATE(), " & _
                        "'" & Trim(User.Identity.Name) & "'" & _
                        "); Select Scope_Identity()"

        Dim sUpdateFotografias As String = "UPDATE fotografia " & _
               "SET fot_numero = '" & Trim(txtNumInventario.Text) & "'" & _
               "   ,fot_dep_id = '" & cboDeposito.SelectedValue & "'" & _
               "   ,fot_estante = '" & cboEstante.SelectedValue & "'" & _
               "   ,fot_columna = '" & cboColumna.SelectedValue & "'" & _
               "   ,fot_fila = '" & cboFila.SelectedValue & "'" & _
               "   ,fot_tipo_bien_cod = '" & cboTipoBien.SelectedValue & "'" & _
               "   ,fot_titulo = '" & Trim(txtTitulo.Text) & "'" & _
               "   ,fot_periodo_epoca_cod = '" & cboPeriodoEpoca.SelectedValue & "'" & _
               "   ,fot_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,fot_anio = '" & Trim(txtAno.Text) & "'" & _
               "   ,fot_mes = '" & cboMes.SelectedValue & "'" & _
               "   ,fot_dia = '" & Trim(txtDia.Text) & "'" & _
               "   ,fot_lugar = '" & Trim(txtLugar.Text) & "'" & _
               "   ,fot_fondo = '" & Trim(txtFondo.Text) & "'" & _
               "   ,fot_fecha_entrada = '" & Trim(txtFechaEntrada.Text) & "'" & _
               "   ,fot_modo_adquisicion = '" & cboAdquisicion.SelectedValue & "'" & _
               "   ,fot_procedencia = '" & Trim(txtProcedencia.Text) & "'" & _
               "   ,fot_especialidad_cod = '" & Trim(cboEspecialidad.SelectedValue) & "'" & _
               "   ,fot_estructura_formal_cod = '" & Trim(cboEstructura.SelectedValue) & "'" & _
               "   ,fot_descripcion = '" & Trim(txtDescripcion.Text) & "'" & _
               "   ,fot_tipo_material_cod = '" & Trim(cboTipoMaterial.SelectedValue) & "'" & _
               "   ,fot_tono_cod = '" & Trim(cboTono.SelectedValue) & "'" & _
               "   ,fot_soporte_cod = '" & Trim(cboSoporteImagen.SelectedValue) & "'" & _
               "   ,fot_dimensiones_soporte = '" & Trim(txtSopAlto.Text) & "/" & Trim(txtSopAncho.Text) & "/" & Trim(txtSopProf.Text) & "'" & _
               "   ,fot_dimensiones_imagen = '" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "'" & _
               "   ,fot_inscripciones_marcas = '" & Trim(txtInscripciones.Text) & "'" & _
               "   ,fot_estado_conservacion_cod = '" & cboConservacion.SelectedValue & "'" & _
               "   ,fot_observaciones = '" & Trim(txtObservaciones.Text) & "'" & _
               "   ,fot_palabra_clave = '" & Trim(txtPalabrasClaves.Text) & "'" & _
               "   ,fot_resp_nombre = '" & Trim(txtRespNombre.Text) & "'" & _
               "   ,fot_resp_fecha = '" & Format(dFechaResp, "yyyy-MM-dd") & "'" & _
               "   ,UPDATE_ = GETDATE() " & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE fot_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertFotografias, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            cmd = New SqlCommand(sUpdateFotografias, cn)
            cmd.ExecuteNonQuery()
        End If

        Call Functions.SaveImagesTitle(panImagesTable)

        cn.Close()
    End Sub

    Private Sub LoadImagesTable(ByVal Gallery As String, ByVal ID As String, ByVal PanelDynamics As Panel)
        ' Variables Galería
        Dim arImages As New ArrayList
        Dim iPhoto As Integer = 1
        Dim i As Integer
        Dim iAlto As Integer
        Dim iAncho As Integer
        Dim iFactorAlto As Integer = 120
        Dim sImg As String()
        Dim imgImage As System.Drawing.Image
        Dim sPath As String

        ' Manejo de Tabla de Imágenes
        Dim table As New Table
        Dim tr As TableRow
        Dim td As TableCell
        Dim img As HtmlImage

        ' Manejo de tabla de controles de cada imagen
        Dim tableINT As Table
        Dim trINT As TableRow
        Dim tdINT As TableCell
        Dim txt As TextBox
        Dim btn As Button
        Dim lt As Literal

        table.CssClass = "auto-style4"

        If ID <> "Nuevo" Then
            arImages = Functions.LoadImages(Trim(ID), Trim(Gallery))
            If arImages.Count > 0 Then
                For i = 0 To arImages.Count - 1
                    sImg = CType(arImages.Item(i), String())
                    sPath = System.Configuration.ConfigurationManager.AppSettings("site") & Replace(sImg(0), "~", "")
                    imgImage = System.Drawing.Image.FromFile(Server.MapPath(sPath))

                    If imgImage.Width > imgImage.Height Then
                        iAncho = CType(imgImage.Width / imgImage.Height * iFactorAlto, Integer)
                        iAlto = iFactorAlto
                    Else
                        iAlto = CType(imgImage.Height / imgImage.Width * iFactorAlto, Integer)
                        iAncho = iFactorAlto
                    End If

                    If iPhoto = 1 Then
                        ' Nueva Fila
                        tr = New TableRow
                    End If

                    ' Nueva celda
                    td = New TableCell
                    td.CssClass = "auto-style4"

                    ' Control de Imagen
                    img = New HtmlImage
                    img.Src = sPath
                    img.Alt = sImg(0)
                    img.Width = iAncho
                    img.Height = iAlto

                    ' Controles internos de cada imagen
                    tableINT = New Table
                    trINT = New TableRow
                    tdINT = New TableCell
                    txt = New TextBox
                    btn = New Button
                    AddHandler btn.Click, AddressOf btnDynamics_Click

                    tableINT.CssClass = "auto-style4"
                    tdINT.CssClass = "auto-style4"

                    ' Título de la Imagen
                    txt.ID = "txtImageGallery_" & sImg(2)
                    txt.MaxLength = 200
                    txt.Width = iFactorAlto
                    txt.Text = sImg(1)
                    txt.CssClass = "textbox"

                    ' Botón para eliminar la imagen
                    btn.ID = "btnDeleteImage_" & sImg(2)
                    btn.Text = "Eliminar"
                    btn.OnClientClick = "return confirm('¿Esta seguro de eliminar esta imagen?');"

                    ' Agregar Controles
                    tdINT.Controls.Add(txt)
                    lt = New Literal
                    lt.Text = "<br />"
                    tdINT.Controls.Add(lt)
                    tdINT.Controls.Add(btn)

                    trINT.Controls.Add(tdINT)
                    tableINT.Controls.Add(trINT)

                    ' Insertar imagen con vínculo
                    lt = New Literal
                    lt.Text = "<a target=""_blank"" href=""" & sPath & """>"
                    td.Controls.Add(lt)
                    td.Controls.Add(img)
                    lt = New Literal
                    lt.Text = "</a>"
                    td.Controls.Add(lt)

                    td.Controls.Add(tableINT)
                    tr.Controls.Add(td)
                    table.Controls.Add(tr)

                    If iPhoto = 3 Then iPhoto = 0
                    iPhoto = iPhoto + 1
                Next i
            End If
        End If

        PanelDynamics.Controls.Add(table)

    End Sub

    Protected Sub btnDynamics_Click(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim sNameControl As String() = Split(btn.ID, "_")
        Dim txt As TextBox = CType(panImagesTable.FindControl("txtImageGallery_" & sNameControl(1)), TextBox)

        ' Eliminar Imagen seleccionada
        Call Functions.DeleteImageGallery(sNameControl(1))
        btn.Text = "Imagen eliminada"
        btn.Enabled = False
        btn.CssClass = "button_disabled"
        txt.Enabled = False
        txt.CssClass = "read_only"
    End Sub

End Class
