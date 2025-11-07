# BlueSky Financial – Portal de Capacitación (Frontend WebForms)

Portal corporativo de capacitación con generación de evaluaciones asistida por IA y flujos para usuarios y administradores.  
Esta versión contiene **todo el frontend funcional** (ASP.NET Web Forms, MasterPages, rutas, estilos y navegación) listo para ejecutar en Visual Studio.

---

## ✨ Características

- **ASP.NET Web Forms (.NET Framework 4.7.2)** con MasterPages (`Site.Master`, `Auth.Master`)
- **Módulos:** Público, Autenticación, Usuario (cursos/evaluaciones), Admin (dashboard/gestión), AI (generar evaluación, chatbot)
- **Navegación funcional** entre todas las pantallas (sin backend aún)
- **Bootstrap local** + Font Awesome + CSS del proyecto
- **Web.config** limpio y estable para DEV (sin credenciales)

---

## 🧱 Estructura de carpetas

/MasterPages
Site.Master (+ .cs, .designer.cs)
Auth.Master (+ .cs, .designer.cs)

/Public
Default.aspx (+ .cs, .designer.cs)

/Auth
IniciarSesion.aspx (+ .cs, .designer.cs)
CrearSesion.aspx (+ .cs, .designer.cs)
OlvidasteContrasena.aspx (+ .cs, .designer.cs)

/Usuario
Cursos.aspx (+ .cs, .designer.cs)
CursoDetalle.aspx (+ .cs, .designer.cs)
SeleccionarEvaluacion.aspx (+ .cs, .designer.cs)
Evaluacion.aspx (+ .cs, .designer.cs)
ResultadoEvaluacion.aspx (+ .cs, .designer.cs)
MiProgreso.aspx (+ .cs, .designer.cs)

/Admin
AdminDashboard.aspx (+ .cs, .designer.cs)
AdminCursos.aspx (+ .cs, .designer.cs)
AdminUsuarios.aspx (+ .cs, .designer.cs)
AdminReportes.aspx (+ .cs, .designer.cs)

/AI
GenerarEvaluacion.aspx (+ .cs, .designer.cs)
ChatBot.aspx (+ .cs, .designer.cs)

/Content/css
bootstrap.min.css, font-awesome.min.css, app.css, site.css, ...
/Scripts
bootstrap.bundle.min.js

Global.asax (+ .cs)
Web.config, Web.Debug.config, Web.Release.config
