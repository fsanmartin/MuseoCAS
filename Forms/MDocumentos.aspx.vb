Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class Forms_MAudiovisual
    Inherits System.Web.UI.Page

    Dim sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString
    Dim bPostBack As Boolean
    Dim dFechaResp As Date
    Dim permDocumentos As Permisos
    Dim iID As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        permDocumentos = New Permisos(User.Identity.Name, System.IO.Path.GetFileName(Request.Url.LocalPath))

        If Not permDocumentos.PermisoVer Then Response.Redirect("/SinAcceso.aspx")

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
                    If Not permDocumentos.PermisoModificar Then
                        Response.Redirect("/SinAcceso.aspx")
                    End If
                End If
            End If
        Else
            If permDocumentos.PermisoAgregar Then
                If txtID.Text = "" Then
                    txtID.Text = "Nuevo"
                    btnDelete.Visible = False
                End If
            Else
                Response.Redirect("/SinAcceso.aspx")
            End If
        End If

        If (permDocumentos.PermisoAgregar = False And permDocumentos.PermisoModificar = False) Then
            Call Functions.FormViewMode(Page)
        End If

        'If Not permAudiovisual.PermisoAgregar Then btnSave.Visible = False
        If Not permDocumentos.PermisoModificar Then btnEdit.Visible = False
        If Not permDocumentos.PermisoEliminar Then btnDelete.Visible = False

        txtID.CssClass = "read_only"

    End Sub

    Private Sub FormUploadImage(State As Boolean)
        lblUploadFile.Visible = State
        fileUpload.Visible = State
        lblTituloImagen.Visible = State
        txtImageTitle.Visible = State
    End Sub

    Private Sub LoadValues(ByVal ID As String)
        Dim sQuery As String = "SELECT * FROM documentos WHERE doc_id = " & ID & " AND DELETE_ <> '*'"

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
        Dim dsDocumentos As SqlDataReader

        ' Abrir conexión
        cn.Open()

        dsDocumentos = cmd.ExecuteReader

        While dsDocumentos.Read
            txtID.Text = dsDocumentos("doc_id")
            txtNumInventario.Text = Trim(dsDocumentos("doc_numero"))
            'If Not bPostBack Then cboDeposito.SelectedValue = Trim(dsDocumentos("doc_dep_id"))

            cboDeposito.SelectedValue = dsDocumentos("doc_dep_id")
            '' --------- ESTANTE
            Try
                cboEstante.SelectedValue = Trim(dsDocumentos("doc_estante"))
                cboFila.SelectedValue = Trim(dsDocumentos("doc_fila"))
                cboColumna.SelectedValue = Trim(dsDocumentos("doc_columna"))
            Catch ex As ArgumentOutOfRangeException
                lblErrorMessages.Text = lblErrorMessages.Text & vbCrLf & "Depósito " & cboDeposito.Text & " ha cambiado. Debe revisar Estante (E.C.F)"
            End Try

            cboTipoBien.SelectedValue = dsDocumentos("doc_tipo_bien_cod")
            cboDenominacion.SelectedValue = dsDocumentos("doc_denominacion_cod")
            txtTitulo.Text = Trim(dsDocumentos("doc_titulo"))
            cboMateria.SelectedValue = dsDocumentos("doc_materia_cod")
            cboTipoContenido.SelectedValue = dsDocumentos("doc_tipo_cont_cod")
            txtColeccion.Text = Trim(dsDocumentos("doc_coleccion"))
            cboIdiomas.SelectedValue = dsDocumentos("doc_idiomas_cod")
            txtAutor.Text = Trim(dsDocumentos("doc_autor_creador"))
            txtEditorial.Text = Trim(dsDocumentos("doc_editorial"))
            cboAnio.SelectedValue = dsDocumentos("doc_anio")
            cboPais.SelectedValue = dsDocumentos("doc_pais_cod")
            cboTipoEncuadernacion.SelectedValue = dsDocumentos("doc_tipo_encuadernacion_cod")
            cboTipoImpresion.SelectedValue = dsDocumentos("doc_tipo_impresion_cod")
            ' --------- MATERIAL TÉCNICA
            Dim dimCont As String() = Split(dsDocumentos("doc_dimensiones"), "/")
            txtContAlto.Text = dimCont(0)
            txtContAncho.Text = dimCont(1)
            txtContProf.Text = dimCont(2)
            txtPaginas.Text = Trim(dsDocumentos("doc_nro_paginas"))
            txtPeso.Text = Trim(dsDocumentos("doc_peso"))
            cboConservacion.SelectedValue = dsDocumentos("doc_estado_conservacion_cod")
            txtProcedencia.Text = Trim(dsDocumentos("doc_procedencia"))
            txtFuncion.Text = Trim(dsDocumentos("doc_funcion"))
            txtInscripciones.Text = Trim(dsDocumentos("doc_inscripciones_marcas"))
            txtDescripcion.Text = Trim(dsDocumentos("doc_descripcion_fisica"))
            txtObservaciones.Text = Trim(dsDocumentos("doc_observaciones"))
            cboAdquisicion.SelectedValue = dsDocumentos("doc_modo_adquisicion")
            txtPalabrasClaves.Text = Trim(dsDocumentos("doc_palabra_clave"))
            txtRespNombre.Text = Trim(dsDocumentos("doc_resp_nombre"))
            txtRespFecha.Text = Trim(dsDocumentos("doc_resp_fecha"))
            txtDigitador.Text = Trim(dsDocumentos("USERID_"))
            txtDigFecha.Text = Trim(dsDocumentos("UPDATE_"))
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
        Dim dsDenomiacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DENOMINA_OBJ_DOC"))
        cboDenominacion.DataSource = dsDenomiacion
        cboDenominacion.DataValueField = "cod_cod"
        cboDenominacion.DataTextField = "cod_val"
        cboDenominacion.DataBind()
        cboDenominacion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboDenominacion.SelectedValue = "-1"
        dsDenomiacion.Dispose()

        ' -------------------------
        ' Materia
        Dim dsMateria As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_MATERIA"))
        cboMateria.DataSource = dsMateria
        cboMateria.DataValueField = "cod_cod"
        cboMateria.DataTextField = "cod_val"
        cboMateria.DataBind()
        cboMateria.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboMateria.SelectedValue = "-1"
        dsMateria.Dispose()

        ' -------------------------
        ' Tipo de Contenido
        Dim dsContenido As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_CONTENIDO"))
        cboTipoContenido.DataSource = dsContenido
        cboTipoContenido.DataValueField = "cod_cod"
        cboTipoContenido.DataTextField = "cod_val"
        cboTipoContenido.DataBind()
        cboTipoContenido.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTipoContenido.SelectedValue = "-1"
        dsContenido.Dispose()

        ' -------------------------
        ' Idiomas
        Dim dsIdiomas As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_IDIOMAS"))
        cboIdiomas.DataSource = dsIdiomas
        cboIdiomas.DataValueField = "cod_cod"
        cboIdiomas.DataTextField = "cod_val"
        cboIdiomas.DataBind()
        cboIdiomas.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboIdiomas.SelectedValue = "-1"
        dsIdiomas.Dispose()

        ' -------------------------
        ' Año
        For i As Integer = Year(Date.Now) To 1709 Step -1
            cboAnio.Items.Add(New ListItem(i, i))
        Next i
        cboAnio.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboAnio.SelectedValue = "-1"

        ' -------------------------
        ' Paises
        Dim dsPaises As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_PAISES"))
        cboPais.DataSource = dsPaises
        cboPais.DataValueField = "cod_cod"
        cboPais.DataTextField = "cod_val"
        cboPais.DataBind()
        cboPais.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboPais.SelectedValue = "-1"
        dsPaises.Dispose()

        ' -------------------------
        ' Tipo de Encuadernación
        Dim dsEncuadernacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_ENCUADERNACION"))
        cboTipoEncuadernacion.DataSource = dsEncuadernacion
        cboTipoEncuadernacion.DataValueField = "cod_cod"
        cboTipoEncuadernacion.DataTextField = "cod_val"
        cboTipoEncuadernacion.DataBind()
        cboTipoEncuadernacion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTipoEncuadernacion.SelectedValue = "-1"
        dsEncuadernacion.Dispose()

        ' -------------------------
        ' Tipo de Impresión
        Dim dsImpresion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_TIPO_IMPRESION"))
        cboTipoImpresion.DataSource = dsImpresion
        cboTipoImpresion.DataValueField = "cod_cod"
        cboTipoImpresion.DataTextField = "cod_val"
        cboTipoImpresion.DataBind()
        cboTipoImpresion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboTipoImpresion.SelectedValue = "-1"
        dsImpresion.Dispose()

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
        Dim sUpdateDocumentos As String = "UPDATE DOCUMENTOS " & _
               "SET DELETE_ = '*'" & _
               "   ,UPDATE_ = GETDATE() " & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE doc_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        cmd = New SqlCommand(sUpdateDocumentos, cn)
        cmd.ExecuteNonQuery()

        cn.Close()

        Response.Redirect("VWDocumentos.aspx")
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

        If Functions.IdentityExist(sID, "doc_id", txtNumInventario.Text, "doc_numero", "documentos") Then
            lblErrorMessages.Text = "El Número de Inventario existe. Favor ingrese un dato válido"
        Else
            Call SaveDocumentos()
            Response.Redirect("VWDocumentos.aspx")
        End If
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As ImageClickEventArgs) Handles btnEdit.Click
        Response.Redirect("MDocumentos.aspx?ID=" & Request.QueryString("ID"))
    End Sub

    Private Sub SaveDocumentos()
        Dim sInsertDocumentos As String = "INSERT INTO documentos " & _
                           "(doc_numero " & _
                           ",doc_dep_id " & _
                           ",doc_estante " & _
                           ",doc_columna " & _
                           ",doc_fila " & _
                           ",doc_tipo_bien_cod " & _
                           ",doc_denominacion_cod " & _
                           ",doc_titulo " & _
                           ",doc_materia_cod " & _
                           ",doc_tipo_cont_cod " & _
                           ",doc_coleccion " & _
                           ",doc_idiomas_cod " & _
                           ",doc_autor_creador " & _
                           ",doc_editorial " & _
                           ",doc_anio " & _
                           ",doc_pais_cod " & _
                           ",doc_material_id " & _
                           ",doc_tipo_encuadernacion_cod " & _
                           ",doc_tipo_impresion_cod " & _
                           ",doc_dimensiones " & _
                           ",doc_nro_paginas " & _
                           ",doc_peso " & _
                           ",doc_estado_conservacion_cod " & _
                           ",doc_procedencia " & _
                           ",doc_funcion " & _
                           ",doc_inscripciones_marcas " & _
                           ",doc_descripcion_fisica " & _
                           ",doc_observaciones " & _
                           ",doc_modo_adquisicion " & _
                           ",doc_resp_nombre " & _
                           ",doc_resp_fecha " & _
                           ",doc_palabra_clave " & _
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
                        "'" & cboMateria.SelectedValue & "', " & _
                        "'" & cboTipoContenido.SelectedValue & "', " & _
                        "'" & Trim(txtColeccion.Text) & "', " & _
                        "'" & cboIdiomas.SelectedValue & "', " & _
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(txtEditorial.Text) & "', " & _
                        "'" & cboAnio.SelectedValue & "', " & _
                        "'" & cboPais.SelectedValue & "', " & _
                        "'', " & _
                        "'" & cboTipoEncuadernacion.SelectedValue & "', " & _
                        "'" & cboTipoImpresion.SelectedValue & "', " & _
                        "'" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "', " & _
                        "'" & Trim(txtPaginas.Text) & "', " & _
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

        Dim sUpdateDocumentos As String = "UPDATE documentos " & _
               "SET doc_numero = '" & Trim(txtNumInventario.Text) & "'" & _
               "   ,doc_dep_id = '" & cboDeposito.SelectedValue & "'" & _
               "   ,doc_estante = '" & cboEstante.SelectedValue & "'" & _
               "   ,doc_columna = '" & cboColumna.SelectedValue & "'" & _
               "   ,doc_fila = '" & cboFila.SelectedValue & "'" & _
               "   ,doc_tipo_bien_cod = '" & cboTipoBien.SelectedValue & "'" & _
               "   ,doc_denominacion_cod = '" & cboDenominacion.SelectedValue & "'" & _
               "   ,doc_titulo = '" & Trim(txtTitulo.Text) & "'" & _
               "   ,doc_materia_cod = '" & cboMateria.SelectedValue & "'" & _
               "   ,doc_tipo_cont_cod = '" & cboTipoContenido.SelectedValue & "'" & _
               "   ,doc_coleccion = '" & Trim(txtColeccion.Text) & "'" & _
               "   ,doc_idiomas_cod = '" & cboIdiomas.SelectedValue & "'" & _
               "   ,doc_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,doc_editorial = '" & Trim(txtEditorial.Text) & "'" & _
               "   ,doc_anio = '" & cboAnio.SelectedValue & "'" & _
               "   ,doc_pais_cod = '" & cboPais.SelectedValue & "'" & _
               "   ,doc_material_id = ''" & _
               "   ,doc_tipo_encuadernacion_cod = '" & cboTipoEncuadernacion.SelectedValue & "'" & _
               "   ,doc_tipo_impresion_cod = '" & cboTipoImpresion.SelectedValue & "'" & _
               "   ,doc_dimensiones = '" & Trim(txtContAlto.Text) & "/" & Trim(txtContAncho.Text) & "/" & Trim(txtContProf.Text) & "'" & _
               "   ,doc_nro_paginas = '" & Trim(txtPaginas.Text) & "'" & _
               "   ,doc_peso = '" & Trim(txtPeso.Text) & "'" & _
               "   ,doc_estado_conservacion_cod = '" & cboConservacion.SelectedValue & "'" & _
               "   ,doc_procedencia = '" & Trim(txtProcedencia.Text) & "'" & _
               "   ,doc_funcion = '" & Trim(txtFuncion.Text) & "'" & _
               "   ,doc_inscripciones_marcas = '" & Trim(txtInscripciones.Text) & "'" & _
               "   ,doc_descripcion_fisica = '" & Trim(txtDescripcion.Text) & "'" & _
               "   ,doc_observaciones = '" & Trim(txtObservaciones.Text) & "'" & _
               "   ,doc_modo_adquisicion = '" & cboAdquisicion.SelectedValue & "'" & _
               "   ,doc_palabra_clave = '" & Trim(txtPalabrasClaves.Text) & "'" & _
               "   ,doc_resp_nombre = '" & Trim(txtRespNombre.Text) & "'" & _
               "   ,doc_resp_fecha = '" & Format(dFechaResp, "yyyy-MM-dd") & "'" & _
               "   ,UPDATE_ = GETDATE() " & _
               "   ,USERID_ = '" & Trim(User.Identity.Name) & "' " & _
             "WHERE doc_id = " & txtID.Text

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertDocumentos, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            cmd = New SqlCommand(sUpdateDocumentos, cn)
            cmd.ExecuteNonQuery()
        End If

        cn.Close()
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
                    Call SaveDocumentos()
                End If

                sPath = "~/Gallery/Documentos/"
                sNewName = sPath & Guid.NewGuid().ToString() & "." & sExt

                fileUpload.SaveAs(MapPath(sNewName))
                imgUpload.Width = 300

                imgUpload.Height = 300
                imgUpload.ImageUrl = sNewName

                Functions.AddImageGallery("DOCUMENTO", CInt(txtID.Text), IIf(txtImageTitle.Text.Trim <> "", txtImageTitle.Text, sName), sNewName)

                lblErrUpload.Text = "Archivo cargado correctamente."
            Else
                lblErrUpload.Text = "El archivo no es de tipo imagen."
            End If
        Else
            lblErrUpload.Text = "Seleccione el archivo que desea subir."
        End If
    End Sub
End Class
