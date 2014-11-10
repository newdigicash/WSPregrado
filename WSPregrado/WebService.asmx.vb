Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports Newtonsoft.Json

<System.Web.Services.WebService(Namespace:="http://ucuenca.edu.ec/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<System.Web.Script.Services.ScriptService()> _
<ToolboxItem(False)> _
Public Class WebService
    Inherits System.Web.Services.WebService
    'Obtiene información del plan de carrera de pregrado
    'author: @edwinsigcha
    'Web service (REST), operacion permitida GET
    'Parametro q es facultad + malla (Ejemplo: "arq01")
    'Parametro callback es nombre de la funcion generada por AJAX (Ejemplo: jq12345)
    'Retorna un objeto JSON con la info, o vacío si hay error
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=System.Web.Script.Services.ResponseFormat.Json, UseHttpGet:=True)> _
    Public Sub consultar(ByVal q As String, ByVal callback As String)
        Dim salida As String = ""
        Try
            Dim pc As New PlanCarrera() 'instacia para acceder al dataset
            If Me.getParametros(q, pc) Then 'asigna las ID para consultar al dataset
                salida = "[" + JsonConvert.SerializeObject(pc.getDatosGenerales()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getCompPlanes()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getOptativas()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getLibreEleccion()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getCreditosCiclo()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getCreditos()) + ","
                salida = salida + JsonConvert.SerializeObject(pc.getMalla()) + "]"
                salida = callback + "(" + salida + ")" 'compone el elemento
            End If
        Catch ex As Exception
            'salida = "error: no se puede consultar el plan de carrera" + ex.ToString
            salida = callback + "(" + JsonConvert.SerializeObject(New Object) + ")"
        Finally
            Context.Response.ContentType = "application/json; charset=utf-8" 'define cabecera
            Context.Response.Write(salida) 'muestra json
        End Try

    End Sub
    'Asigna los parametros para consultar al dataset
    'parametros (malla vigente,plan,carrera,denominacion, denominacion plural)
    Private Function getParametros(ByVal str As String, ByRef pc As PlanCarrera) As Boolean
        Select Case str
            Case "arq01" 'arquitectura
                pc.setParametros("4", "2", "1", "CICLO", "CICLOS")
                Return True
            Case "art01" 'musicales 
                pc.setParametros("1", "28", "3", "CICLO", "CICLOS")
                Return True
            Case "art02" 'diseño interiores
                pc.setParametros("1", "6", "171", "CICLO", "CICLOS")
                Return True
            Case "art03" 'visuales 
                pc.setParametros("1", "9", "2", "CICLO", "CICLOS")
                Return True
            Case "art04" 'danza teatro 
                pc.setParametros("1", "4", "173", "CICLO", "CICLOS")
                Return True
            Case "art05" 'diseño grafico
                pc.setParametros("1", "5", "171", "CICLO", "CICLOS")
                Return True
            Case "agr01" 'veterinaria 
                pc.setParametros("3", "1", "8", "CICLO", "CICLOS")
                Return True
            Case "agr02" 'agronomica 
                pc.setParametros("2", "2", "7", "CICLO", "CICLOS")
                Return True
            Case "hos01" 'admin turistica 
                pc.setParametros("1", "1", "175", "CICLO", "CICLOS")
                Return True
            Case "hos02" 'hoteleria 
                pc.setParametros("1", "1", "135", "CICLO", "CICLOS")
                Return True
            Case "hos03" 'gastronomia
                pc.setParametros("1", "2", "10", "CICLO", "CICLOS")
                Return True
            Case "hos04" 'turismo
                pc.setParametros("1", "1", "9", "CICLO", "CICLOS")
                Return True
            Case "eco01" 'sociologia
                pc.setParametros("1", "4", "55", "CICLO", "CICLOS")
                Return True
            Case "eco02" 'admin empresas
                pc.setParametros("1", "8", "43", "CICLO", "CICLOS")
                Return True
            Case "eco03" 'desarrollo emprendedores
                pc.setParametros("1", "1", "91", "CICLO", "CICLOS")
                Return True
            Case "eco04" 'ing empresas
                pc.setParametros("1", "2", "47", "CICLO", "CICLOS")
                Return True
            Case "eco05" 'marketing
                pc.setParametros("1", "4", "45", "CICLO", "CICLOS")
                Return True
            Case "eco06" 'economia
                pc.setParametros("1", "4", "42", "CICLO", "CICLOS")
                Return True
            Case "eco07" 'contabilidad
                pc.setParametros("1", "5", "44", "CICLO", "CICLOS")
                Return True
            Case "med01" 'estimulacion temprana
                pc.setParametros("4", "1", "15", "CICLO", "CICLOS")
                Return True
            Case "med02" 'nutricion
                pc.setParametros("4", "1", "14", "CICLO", "CICLOS")
                Return True
            Case "med03" 'medicina cirugia
                pc.setParametros("2", "1", "11", "CICLO", "CICLOS")
                Return True
            Case "med04" 'imagenologia
                pc.setParametros("3", "3", "17", "CICLO", "CICLOS")
                Return True
            Case "med05" 'fonoaudiologia
                pc.setParametros("3", "2", "13", "CICLO", "CICLOS")
                Return True
            Case "med06" 'lab clinico
                pc.setParametros("3", "2", "18", "CICLO", "CICLOS")
                Return True
            Case "med07" 'terapia fisica
                pc.setParametros("3", "3", "16", "CICLO", "CICLOS")
                Return True
            Case "med08" 'enfemeria
                pc.setParametros("3", "1", "12", "CICLO", "CICLOS")
                Return True
            Case "qui01" 'ing ambiental
                pc.setParametros("6", "1", "22", "CICLO", "CICLOS")
                Return True
            Case "qui02" 'bioquimica
                pc.setParametros("5", "3", "20", "CICLO", "CICLOS")
                Return True
            Case "qui03" 'ing industrial
                pc.setParametros("3", "2", "19", "CICLO", "CICLOS")
                Return True
            Case "qui04" 'ing quimica
                pc.setParametros("4", "2", "21", "CICLO", "CICLOS")
                Return True
            Case "fil01" 'matematicas fisica
                pc.setParametros("1", "6", "27", "CICLO", "CICLOS")
                Return True
            Case "fil02" 'geografia historia
                pc.setParametros("1", "6", "24", "CICLO", "CICLOS")
                Return True
            Case "fil03" 'edu. general basica
                pc.setParametros("1", "6", "32", "CICLO", "CICLOS")
                Return True
            Case "fil04" 'literatura inglesa
                pc.setParametros("1", "6", "26", "CICLO", "CICLOS")
                Return True
            Case "fil05" 'sociologia
                pc.setParametros("1", "6", "23", "CICLO", "CICLOS")
                Return True
            Case "fil06" 'leguajes audiovisuales
                pc.setParametros("1", "6", "25", "CICLO", "CICLOS")
                Return True
            Case "fil07" 'educacion fisica
                pc.setParametros("1", "6", "28", "CICLO", "CICLOS")
                Return True
            Case "fil08" 'edu. inicial
                pc.setParametros("1", "1", "174", "CICLO", "CICLOS")
                Return True
            Case "fil09" 'edu. general basica CTE
                pc.setParametros("1", "1", "176", "CICLO", "CICLOS")
                Return True
            Case "fil10" 'comunicacion social
                pc.setParametros("1", "7", "54", "CICLO", "CICLOS")
                Return True
            Case "fil11" 'periodismo
                pc.setParametros("1", "1", "51", "CICLO", "CICLOS")
                Return True
            Case "fil12" 'desarrollo cultural
                pc.setParametros("1", "3", "49", "CICLO", "CICLOS")
                Return True
            Case "fil13" 'relaciones publicas
                pc.setParametros("1", "1", "52", "CICLO", "CICLOS")
                Return True
            Case "fil14" 'cine audiovisuales
                pc.setParametros("1", "3", "126", "CICLO", "CICLOS")
                Return True
            Case "ing01" 'telecomunicaciones
                pc.setParametros("5", "1", "36", "CICLO", "CICLOS")
                Return True
            Case "ing02" 'civil
                pc.setParametros("7", "2", "34", "CICLO", "CICLOS")
                Return True
            Case "ing03" 'electrica
                pc.setParametros("5", "2", "35", "CICLO", "CICLOS")
                Return True
            Case "ing04" 'sistemas
                pc.setParametros("7", "1", "37", "CICLO", "CICLOS")
                Return True
            Case "jur01" 'orientacion familiar
                pc.setParametros("4", "2", "40", "CICLO", "CICLOS")
                Return True
            Case "jur02" 'trabajo social
                pc.setParametros("4", "2", "39", "CICLO", "CICLOS")
                Return True
            Case "jur03" 'derecho
                pc.setParametros("4", "2", "38", "CICLO", "CICLOS")
                Return True
            Case "odo01" 'odontologia
                pc.setParametros("2", "3", "41", "CICLO", "CICLOS")
                Return True
            Case "psi01" 'psicologia social
                pc.setParametros("1", "1", "76", "CICLO", "CICLOS")
                Return True
            Case "psi02" 'psic educativa
                pc.setParametros("1", "1", "77", "CICLO", "CICLOS")
                Return True
            Case "psi03" 'psic clinica
                pc.setParametros("1", "1", "75", "CICLO", "CICLOS")
                Return True
            Case "psi04" 'psic educacion temprana
                pc.setParametros("1", "1", "98", "CICLO", "CICLOS")
                Return True
            Case "psi05" ' psic ciencias de la educacion
                pc.setParametros("1", "1", "50", "CICLO", "CICLOS")
                Return True
            Case Else 'agrega directamente los parametros (malla;plan;carrera)
                Try
                    Dim p() As String = str.Split(";") 'separa los parametros
                    If p.Length > 3 Then 'agrega parametros
                        pc.setParametros(p(0), p(1), p(2), p(3), p(4))
                    Else 'debe haber al menos 3 parametros (ID)
                        pc.setParametros(p(0), p(1), p(2), "CICLO", "CICLOS")
                    End If
                    Return True
                Catch ex As Exception
                    Return False 'cualquier otra consulta
                End Try
        End Select
        Return False
    End Function


End Class

