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

            cboDeposito.SelectedValue = Trim(dsFotografias("fot_dep_id"))
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
            txtAutor.Text = Trim(dsFotografias("fot_autor_creador"))
            cboAnio.SelectedValue = dsFotografias("fot_anio")
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

            Call LoadImages(txtID.Text, "FOTOGRAFIA")
        End While

        cn.Close()
    End Sub

    Private Sub LoadImages(sID As String, Gallery As String)
        Dim sQuery As String = "SELECT img_id, img_cat_id, img_nombre, img_url " & _
                               "FROM galeria " & _
                               "WHERE img_galeria = '" & Gallery & "' " & _
                               "  AND img_cat_id = " & sID

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsImgFotografias As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsImgFotografias = cmd.ExecuteReader

        imgFotografias.Width = 0
        imgFotografias.Height = 0

        While dsImgFotografias.Read
            imgFotografias.ImageUrl = dsImgFotografias("img_url")
            imgFotografias.Width = 300
            imgFotografias.Height = 300
        End While

        cn.Close()
    End Sub

    Private Sub LoadLists()
        Dim sQueryDeposito As String = "SELECT dep_id, dep_name FROM deposito WHERE DELETE_ <> '*'"
        Dim sQueryCodigos As String = "SELECT cod_cod, cod_val FROM codigos WHERE cod_name = '%COD_NAME%' AND DELETE_ <> '*'"
        ' -------------------------
        ' Depóstio
        Dim dsDeposito As New SqlDataSource(sCN, sQueryDeposito)

        cboDeposito.DataSource = dsDeposito
        cboDeposito.DataValueField = "dep_id"
        cboDeposito.DataTextField = "dep_name"
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
        ' Año
        For i As Integer = Year(Date.Now) To 1709 Step -1
            cboAnio.Items.Add(New ListItem(i, i))
        Next i

        ' -------------------------
        ' Especialidad
        Dim dsEspecialidad As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESPECIALIDAD"))
        cboEspecialidad.DataSource = dsEspecialidad
        cboEspecialidad.DataValueField = "cod_cod"
        cboEspecialidad.DataTextField = "cod_val"
        cboEspecialidad.DataBind()
        dsEspecialidad.Dispose()

        ' -------------------------
        ' Estructura
        Dim dsEstructura As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_ESTRUCTURA_FORMAL"))
        cboEstructura.DataSource = dsEstructura
        cboEstructura.DataValueField = "cod_cod"
        cboEstructura.DataTextField = "cod_val"
        cboEstructura.DataBind()
        dsEstructura.Dispose()

        ' -------------------------
        ' Tipo de Material
        Dim dsTipoMaterial As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_MATERIAL"))
        cboTipoMaterial.DataSource = dsTipoMaterial
        cboTipoMaterial.DataValueField = "cod_cod"
        cboTipoMaterial.DataTextField = "cod_val"
        cboTipoMaterial.DataBind()
        dsTipoMaterial.Dispose()

        ' -------------------------
        ' Tono
        Dim dsTono As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TONO"))
        cboTono.DataSource = dsTono
        cboTono.DataValueField = "cod_cod"
        cboTono.DataTextField = "cod_val"
        cboTono.DataBind()
        dsTono.Dispose()

        ' -------------------------
        ' Soporte
        Dim dsSoporte As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_SOPORTE_IMAGEN"))
        cboSoporteImagen.DataSource = dsSoporte
        cboSoporteImagen.DataValueField = "cod_cod"
        cboSoporteImagen.DataTextField = "cod_val"
        cboSoporteImagen.DataBind()
        dsSoporte.Dispose()

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
        Call SaveFotografias()
        Response.Redirect("VWFotografias.aspx")
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
                           ",fot_autor_creador " & _
                           ",fot_anio " & _
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
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(cboAnio.SelectedValue) & "', " & _
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
               "   ,fot_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,fot_anio = '" & Trim(cboAnio.SelectedValue) & "'" & _
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

        cn.Close()
    End Sub
End Class
