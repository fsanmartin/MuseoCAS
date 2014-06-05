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
            Call LoadImagesTable("OBJETOS", Request.QueryString.Get("ID"), panImagesTable)
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
        Dim sQueryMaterial As String = "SELECT * FROM material WHERE mat_name = 'OBJETOS' AND mat_cod_name = '_MATERIAL_OBJ' AND mat_cat_id = " & ID

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
            cboPeriodoEpoca.SelectedValue = dsObjetos("obj_periodo_epoca_cod")
            txtAutor.Text = Trim(dsObjetos("obj_autor_creador"))
            txtAno.Text = Trim(dsObjetos("obj_anio"))
            cboMes.SelectedValue = dsObjetos("obj_mes")
            txtDia.Text = dsObjetos("obj_dia")
            txtTecnica.Text = Trim(dsObjetos("obj_tecnica"))
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

        dsObjetos.Close()

        ' --------- MATERIAL
        cmd = New SqlCommand(sQueryMaterial, cn)
        Dim dsMaterial As SqlDataReader
        dsMaterial = cmd.ExecuteReader

        While dsMaterial.Read
            For Each itemMat As ListItem In cblMaterial.Items
                If dsMaterial("mat_cod_cod") = itemMat.Value Then
                    itemMat.Selected = True
                End If
            Next
        End While
        dsMaterial.Close()
        ' ---------

        cn.Close()

        Call Functions.ResizeTextbox(Page)
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
        ' Denominación Objeto
        Dim dsDenomiacion As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_DENOMINA_OBJ_OBJ"))
        cboDenominacion.DataSource = dsDenomiacion
        cboDenominacion.DataValueField = "cod_cod"
        cboDenominacion.DataTextField = "cod_val"
        cboDenominacion.DataBind()
        cboDenominacion.Items.Add(New ListItem("-- ninguno --", "-1"))
        cboDenominacion.SelectedValue = "-1"
        dsDenomiacion.Dispose()

        ' -------------------------
        ' Material CheckBoxList
        Dim dsMaterial As New SqlDataSource(sCN, Replace(sQueryCodigos, "%COD_NAME%", "_MATERIAL_OBJ"))

        cblMaterial.DataSource = dsMaterial
        cblMaterial.DataValueField = "cod_cod"
        cblMaterial.DataTextField = "cod_val"
        cblMaterial.DataBind()
        dsMaterial.Dispose()

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

        ' Eliminar imágenes de la galería
        Call Functions.DeleteGallery("OBJETOS", txtID.Text)

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

        Dim sID As String = IIf(txtID.Text = "Nuevo", 0, txtID.Text)

        If Functions.IdentityExist(sID, "obj_id", txtNumInventario.Text, "obj_numero", "objetos") Then
            lblErrorMessages.Text = "El Número de Inventario existe. Favor ingrese un dato válido"
        Else
            Call SaveObjetos()
            Response.Redirect("VWObjetos.aspx")
        End If
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
                txtImageTitle.Text = ""
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
                           ",obj_periodo_epoca_cod " & _
                           ",obj_autor_creador " & _
                           ",obj_anio " & _
                           ",obj_mes " & _
                           ",obj_dia " & _
                           ",obj_decada " & _
                           ",obj_tecnica " & _
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
                        "'" & cboPeriodoEpoca.SelectedValue & "', " & _
                        "'" & Trim(txtAutor.Text) & "', " & _
                        "'" & Trim(txtAno.Text) & "', " & _
                        "'" & Trim(cboMes.SelectedValue) & "', " & _
                        "'" & Trim(txtDia.Text) & "', " & _
                        "' ', " & _
                        "'" & Trim(txtTecnica.Text) & "', " & _
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
               "   ,obj_periodo_epoca_cod = '" & cboPeriodoEpoca.SelectedValue & "'" & _
               "   ,obj_autor_creador = '" & Trim(txtAutor.Text) & "'" & _
               "   ,obj_anio = '" & Trim(txtAno.Text) & "'" & _
               "   ,obj_mes = '" & cboMes.SelectedValue & "'" & _
               "   ,obj_dia = '" & Trim(txtDia.Text) & "'" & _
               "   ,obj_decada = ' '" & _
               "   ,obj_tecnica = '" & Trim(txtTecnica.Text) & "'" & _
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

        Dim sDeleteMaterial As String = "DELETE FROM material WHERE mat_name = 'OBJETOS' AND mat_cod_name = '_MATERIAL_OBJ' AND mat_cat_id = " & Trim(txtID.Text)

        ' Conexión SQL Server
        Dim cn As SqlConnection = New SqlConnection(sCN)
        Dim cmd As SqlCommand

        cn.Open()

        If txtID.Text = "Nuevo" And iID = 0 Then
            cmd = New SqlCommand(sInsertObjetos, cn)
            iID = cmd.ExecuteScalar()
            txtID.Text = iID
        Else
            ' Limpiar MATERIAL
            cmd = New SqlCommand(sDeleteMaterial, cn)
            cmd.ExecuteNonQuery()

            cmd = New SqlCommand(sUpdateObjetos, cn)
            cmd.ExecuteNonQuery()
        End If

        Call Functions.SaveMaterial("OBJETOS", "_MATERIAL_OBJ", txtID.Text, cblMaterial)
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
