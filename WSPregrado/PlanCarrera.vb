'Clase para obtener el plan de carrera y la malla (pregrado)
'author: @edwinsigcha
Class PlanCarrera
    Private dMallaId As Decimal
    Private dPlacarId As Decimal
    Private dCarreraId As Decimal
    Private sDenominacionNivel As String
    Private sDenominacionNivelPlural As String
    'Private clsGenerales As RN_GestionAcademica.clsGenerales
    Private dsRepPlanes As New AD_GestionAcademica.dsReportesPlanes
    'Constructor inicializa los adaptadores
    Public Sub New()
        'Me.clsGenerales = New RN_GestionAcademica.clsGenerales '
    End Sub
    'Asigna los parametros para la consulta al dataset
    Public Sub setParametros(ByVal idmalla As String, ByVal idplancar As String, ByVal idcarrera As String, ByVal denom As String, ByVal plural As String)
        Me.dMallaId = CDec(idmalla)
        Me.dPlacarId = CDec(idplancar)
        Me.dCarreraId = CDec(idcarrera)
        Me.sDenominacionNivel = denom
        Me.sDenominacionNivelPlural = plural
    End Sub
    'Obtiene los datos generales del plan de carrera
    Public Function getDatosGenerales() As Object
        'Dim dsRepPlanes As New AD_GestionAcademica.dsReportesPlanes
        Dim dsDatosGenerales As New Data.DataSet
        Dim dtCmpAuto As New Data.DataTable
        Dim dtModalidades As New Data.DataTable
        'consultar datos generales
        dsDatosGenerales.Tables.Clear()
        If Me.dMallaId = 0 Then
            dtCmpAuto = dsRepPlanes.RecuperarDatosGeneralesCarrera(Me.dPlacarId, Me.dCarreraId)
        Else
            Dim adpPlanesCrr As New AD_GestionAcademica.dsPlanesCarreraTableAdapters.dtDatosGeneralesCarreraTableAdapter
            dtCmpAuto = adpPlanesCrr.RecuperarDatosGnlCrrXMllyPln(Me.dPlacarId, Me.dCarreraId, Me.dMallaId)
        End If
        'consultar la modalidad de estudio
        dtModalidades = dsRepPlanes.RecuperarModalidadesXplanCarrera(Me.dPlacarId, Me.dCarreraId)
        dsDatosGenerales.Tables.Add(dtCmpAuto)
        dsDatosGenerales.Tables.Add(dtModalidades)
        Return dsDatosGenerales
    End Function
    'Obtiene los componentes de una carrera (fundamentos, perfil, objetivos, etc)
    Public Function getCompPlanes() As Object
        Dim dsPlanCarrera As New AD_GestionAcademica.dsPlanesCarreraTableAdapters.SGA_COMPONENTES__PLANESTableAdapter
        Dim dtComponentes As New Data.DataTable 'guarda los componentes
        dtComponentes = dsPlanCarrera.RecuperarComponentesXplanCarrera(Me.dPlacarId, Me.dCarreraId)
        Return dtComponentes
    End Function
    'Obtiene las asignaturas optativas por ciclos
    Public Function getOptativas() As Object
        'Leer variable desde el Web.config
        Dim dEjeforId As Decimal = CInt(System.Configuration.ConfigurationManager.AppSettings("IdOptativas"))
        Dim dtCmpAuto As New Data.DataTable 'tabla de optativas
        dtCmpAuto = dsRepPlanes.AsignaturasOptativasXplanCarrera(Me.dMallaId, Me.dPlacarId, Me.dCarreraId, dEjeforId)
        Return dtCmpAuto
    End Function
    'Obtiene las asignaturas de libre opcion
    Public Function getLibreEleccion() As Object
        'Leer variable desde el Web.config
        Dim dEjeforId As Decimal = CInt(System.Configuration.ConfigurationManager.AppSettings("IdLibreEleccion"))
        Dim dtCmpAuto As New Data.DataTable 'tabla de materias
        dtCmpAuto = dsRepPlanes.AsignaturasLibreEleccionXplanCarrera(Me.dMallaId, Me.dPlacarId, Me.dCarreraId, dEjeforId)
        Return dtCmpAuto
    End Function
    'Obtiene la distribucion de creditos por ciclo
    Public Function getCreditosCiclo() As Object
        Dim dtCmpAuto As New Data.DataTable 'tabla de creditos por semestre
        dtCmpAuto = dsRepPlanes.RecuperarCreditosXsemestre(Me.dMallaId, Me.dPlacarId, Me.dCarreraId)
        Return dtCmpAuto
    End Function
    'Obtiene los creditos totales
    Public Function getCreditos() As Object
        Dim dtCmpAuto As New Data.DataTable 'tabla de creditos
        Dim dtMalla As New Data.DataTable
        Dim taPlanesCarrera As New AD_GestionAcademica.dsPlanesCarreraTableAdapters.SGA_PLANES_CARRERASTableAdapter
        Dim dtPlanesCarrera As AD_GestionAcademica.dsPlanesCarrera.SGA_PLANES_CARRERASDataTable
        Dim dHorasCredito As Integer
        Dim dHorasServicio As Integer 'trabajo academico 60h
        Dim dCreditosPasantia As Integer 'horas pasantias
        Dim dCreditosGraduacion As Integer 'creditos por graduacion
        Dim dMalla As String 'nombre de la malla
        Dim dPlan As String 'nombre del plan de carrrera
        Dim dtAnexos As New Data.DataTable
        Dim dsSalida As New Data.DataSet
        'consulta los credito y horas del plan de carrera
        dtPlanesCarrera = taPlanesCarrera.RecuperarDatosPorClavePrincipal(Me.dPlacarId, Me.dCarreraId)
        dtCmpAuto = dsRepPlanes.RecuperarDistribucionCreditos(Me.dMallaId, Me.dPlacarId, Me.dCarreraId)
        dtMalla = dsRepPlanes.RecuperarMallaXid(Me.dMallaId, Me.dPlacarId, Me.dCarreraId)
        dHorasCredito = dsRepPlanes.RecuperarHorasCredito
        dCreditosPasantia = CInt(dtMalla.Rows(0).Item("HORAS_VINCULACION"))
        dHorasServicio = CInt(dtMalla.Rows(0).Item("HORAS_SERVICIO"))
        dCreditosGraduacion = CInt(dtMalla.Rows(0).Item("CREDITOS_GRADUACION"))
        dMalla = dtMalla.Rows(0).Item("NOMBRE")
        dPlan = dtPlanesCarrera.Rows(0).Item("NOMBRE")
        'compone la cabecera de totales
        dtAnexos.Columns.Add("HorasCredito", GetType(Integer))
        dtAnexos.Columns.Add("HorasServicio", GetType(Integer))
        dtAnexos.Columns.Add("CreditosPasantia", GetType(Integer))
        dtAnexos.Columns.Add("CreditosGraduacion", GetType(Integer))
        dtAnexos.Columns.Add("Malla", GetType(String))
        dtAnexos.Columns.Add("Plan", GetType(String))
        'agrega variables a la tabla
        dtAnexos.Rows.Add(dHorasCredito, dHorasServicio, dCreditosPasantia, dCreditosGraduacion, dMalla, dPlan)
        'guarda las tablas de resultados
        dsSalida.Tables.Add(dtCmpAuto)
        dsSalida.Tables.Add(dtAnexos)
        Return dsSalida
    End Function
    'Obtiene la matriz curricular
    Public Function getMalla() As Object
        Try
            Dim taAsigMallas As New AD_GestionAcademica.dsMallasTableAdapters.TAASIGNATURASMALLASTableAdapter
            Dim dtAsignaturasMallas As AD_GestionAcademica.dsMallas.TAASIGNATURASMALLASDataTable 'consulta todas
            'sdfsdf
            dtAsignaturasMallas = taAsigMallas.RecuperarDatos(Me.dMallaId, Me.dPlacarId, Me.dCarreraId) 'las materias
            Dim dtPlanesCarrera As AD_GestionAcademica.dsPlanesCarrera.SGA_PLANES_CARRERASDataTable
            Dim taPlanesCarrera As New AD_GestionAcademica.dsPlanesCarreraTableAdapters.SGA_PLANES_CARRERASTableAdapter
            dtPlanesCarrera = taPlanesCarrera.RecuperarDatosPorClavePrincipal(Me.dPlacarId, Me.dCarreraId)
            Dim iDuracion As Int32 = CType(dtPlanesCarrera.Rows(0)("DURACION"), Integer) 'cuantos ciclos tiene
            Dim i As Integer
            'Dim iCreditosTotal As Integer = 0 '
            Dim dtMatriz As New Data.DataTable 'tabla de materias por ciclo
            Dim dsSalida As New Data.DataSet 'tablas de todas las materias
            'arma la malla con materias por ciclo
            For i = 1 To iDuracion 'recorre todos los ciclos
                dtMatriz = New Data.DataTable 'cabecera de la tabla de materias
                dtMatriz.Columns.Add("Ciclo", GetType(String))
                dtMatriz.Columns.Add("Codigo", GetType(String))
                dtMatriz.Columns.Add("Nombre", GetType(String))
                dtMatriz.Columns.Add("Creditos", GetType(Integer))
                dtMatriz.Columns.Add("Color", GetType(String))
                dtMatriz.Columns.Add("Total", GetType(Integer))
                Dim iCreditosCiclo As Integer = 0
                For Each drAsignatura As DataRow In dtAsignaturasMallas.Rows 'materias de un ciclo
                    If i = drAsignatura("NIVEL_NUMERO") Then
                        iCreditosCiclo = iCreditosCiclo + CType(drAsignatura("TOTAL_CREDITOS"), Integer) 'suma los creditos
                        Dim adpEjesformacion As New AD_GestionAcademica.dsMallasTableAdapters.SGA_EJES_FORMACIONTableAdapter
                        Dim dtEjesFormacion As New AD_GestionAcademica.dsMallas.SGA_EJES_FORMACIONDataTable 'buscar el eje de formacion
                        dtEjesFormacion = adpEjesformacion.RecuperarDatosPorId(CType(drAsignatura("EJEFOR_ID"), Decimal)) 'ejem: BAS, PRO
                        Dim sColor As String = dtEjesFormacion.Rows(0).Item("COLOR").ToString() 'obtiene el color asociado al eje de formacion
                        dtMatriz.Rows.Add(i.ToString(), drAsignatura("ASIGNATURA_ID"), drAsignatura("NOMBRE"), CType(drAsignatura("TOTAL_CREDITOS"), Integer), sColor, iCreditosCiclo)
                    End If
                Next
                dsSalida.Tables.Add(dtMatriz) 'conserva la tabla de materias por ciclo
                'iCreditosTotal = iCreditosTotal + iCreditosCiclo
            Next
            Return dsSalida
        Catch ex As Exception
            Return New Data.DataSet 'retorna un dataset vacio en caso de error
        End Try
    End Function
End Class