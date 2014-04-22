Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class Permisos
    Private _Username As String
    Private _Funcionalidad As String
    Private _PermisoVer As Boolean = False
    Private _PermisoAgregar As Boolean = False
    Private _PermisoModificar As Boolean = False
    Private _PermisoEliminar As Boolean = False

    Private sCN As String = ConfigurationManager.ConnectionStrings("ColegioCN").ConnectionString

    Public Property Username() As String
        Get
            Return _Username
        End Get
        Set(ByVal value As String)
            _Username = value
        End Set
    End Property

    Public Property Funcionalidad() As String
        Get
            Return _Funcionalidad
        End Get
        Set(ByVal value As String)
            _Funcionalidad = value
        End Set
    End Property

    Public ReadOnly Property PermisoVer() As Boolean
        Get
            Return _PermisoVer
        End Get
    End Property

    Public ReadOnly Property PermisoAgregar() As Boolean
        Get
            Return _PermisoAgregar
        End Get
    End Property

    Public ReadOnly Property PermisoModificar() As Boolean
        Get
            Return _PermisoModificar
        End Get
    End Property

    Public ReadOnly Property PermisoEliminar() As Boolean
        Get
            Return _PermisoEliminar
        End Get
    End Property

    Sub New(sUsername As String, sFuncionalidad As String)
        If Trim(sUsername) <> "" Then
            ' Conexión SQL Server
            Dim cn As SqlConnection = New SqlConnection(sCN)
            Dim sQuery As String = "SELECT p.grp_id, f.func_program, p.perm_view, p.perm_new, p.perm_mod, p.perm_del " & _
                                   "FROM FUNCIONALIDAD f INNER JOIN GRUPOS_FUNCIONALIDAD p ON f.func_id = p.func_id " & _
                                   "                     INNER JOIN USUARIOS_GRUPOS g ON g.grp_id = p.grp_id " & _
                                   "WHERE g.UserName = '" & sUsername & "' " & _
                                   "  AND f.func_program = '" & Trim(sFuncionalidad) & "'"
            Dim cmd As SqlCommand = New SqlCommand(sQuery, cn)
            Dim dsPermisos As SqlDataReader

            _Username = sUsername
            _Funcionalidad = sFuncionalidad

            ' Abrir conexión
            cn.Open()

            dsPermisos = cmd.ExecuteReader

            While dsPermisos.Read
                If dsPermisos("perm_view") = "1" Then _PermisoVer = True
                If dsPermisos("perm_new") = "1" Then _PermisoAgregar = True
                If dsPermisos("perm_mod") = "1" Then _PermisoModificar = True
                If dsPermisos("perm_del") = "1" Then _PermisoEliminar = True
            End While

            dsPermisos.Close()
            cn.Close()
        End If
    End Sub

End Class
