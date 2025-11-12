using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using bluesky.App_Code;    // AdminPage
using bluesky.Models;      // ApplicationDbContext y modelos
using OfficeOpenXml;       // EPPlus 4.5.3.3
using OfficeOpenXml.Style;

namespace bluesky.Admin
{
    public partial class AdminReportes : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            DateTime? desde = TryParseDate(txtDesde.Text);
            DateTime? hasta = TryParseDate(txtHasta.Text);
            if (hasta.HasValue) hasta = hasta.Value.Date.AddDays(1).AddTicks(-1);

            var tipo = ddlReporte.SelectedValue;

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    DataTable table;

                    switch (tipo)
                    {
                        case "Usuarios":
                            table = BuildUsuariosTable(db, desde, hasta);
                            break;
                        case "Cursos":
                            table = BuildCursosTable(db, desde, hasta);
                            break;
                        case "Inscripciones":
                            table = BuildInscripcionesTable(db, desde, hasta);
                            break;
                        case "Evaluaciones":
                            table = BuildEvaluacionesTable(db, desde, hasta);
                            break;
                        case "Intentos":
                            table = BuildIntentosTable(db, desde, hasta);
                            break;
                        default:
                            lblMsg.Text = "Tipo de reporte no reconocido.";
                            return;
                    }

                    if (table.Rows.Count == 0)
                    {
                        lblMsg.Text = "No hay datos para exportar con los filtros aplicados.";
                        return;
                    }

                    var fileName = $"Reporte_{tipo}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                    ExportDataTableToXlsx(table, fileName, tipo);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al generar el reporte: " + HttpUtility.HtmlEncode(ex.Message);
            }
        }

        private static DateTime? TryParseDate(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            DateTime dt;
            if (DateTime.TryParse(s, out dt)) return dt.Date;
            return null;
        }

        // ===== Tablas =====

        private DataTable BuildUsuariosTable(ApplicationDbContext db, DateTime? desde, DateTime? hasta)
        {
            var q = db.Usuarios.AsQueryable();
            if (desde.HasValue) q = q.Where(u => u.FechaRegistro >= desde.Value);
            if (hasta.HasValue) q = q.Where(u => u.FechaRegistro <= hasta.Value);

            var data = q
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    u.Id,
                    u.NombreCompleto,
                    u.Correo,
                    Rol = u.Rol != null ? u.Rol.Nombre : "",
                    u.Activo,
                    u.FechaRegistro,
                    u.FechaNacimiento
                }).ToList();

            var dt = new DataTable("Usuarios");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("NombreCompleto", typeof(string));
            dt.Columns.Add("Correo", typeof(string));
            dt.Columns.Add("Rol", typeof(string));
            dt.Columns.Add("Activo", typeof(bool));
            dt.Columns.Add("FechaRegistro", typeof(DateTime));
            dt.Columns.Add("FechaNacimiento", typeof(DateTime));

            foreach (var r in data)
                dt.Rows.Add(r.Id, r.NombreCompleto, r.Correo, r.Rol, r.Activo, r.FechaRegistro, (object)r.FechaNacimiento ?? DBNull.Value);

            return dt;
        }

        private DataTable BuildCursosTable(ApplicationDbContext db, DateTime? desde, DateTime? hasta)
        {
            var q = db.Cursos.AsQueryable();
            if (desde.HasValue) q = q.Where(c => c.FechaCreacion >= desde.Value);
            if (hasta.HasValue) q = q.Where(c => c.FechaCreacion <= hasta.Value);

            var data = q
                .OrderBy(c => c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.Descripcion,
                    Area = c.Area != null ? c.Area.Nombre : "",
                    Instructor = c.Instructor != null ? c.Instructor.NombreCompleto : "",
                    c.DuracionHoras,
                    Nivel = c.Nivel.ToString(),
                    c.Activo,
                    c.FechaCreacion,
                    c.FechaActualizacion
                }).ToList();

            var dt = new DataTable("Cursos");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Instructor", typeof(string));
            dt.Columns.Add("DuracionHoras", typeof(int));
            dt.Columns.Add("Nivel", typeof(string));
            dt.Columns.Add("Activo", typeof(bool));
            dt.Columns.Add("FechaCreacion", typeof(DateTime));
            dt.Columns.Add("FechaActualizacion", typeof(DateTime));

            foreach (var r in data)
                dt.Rows.Add(r.Id, r.Titulo, r.Descripcion, r.Area, r.Instructor, r.DuracionHoras, r.Nivel, r.Activo, r.FechaCreacion, (object)r.FechaActualizacion ?? DBNull.Value);

            return dt;
        }

        private DataTable BuildInscripcionesTable(ApplicationDbContext db, DateTime? desde, DateTime? hasta)
        {
            var q = db.UsuarioCursos.AsQueryable();
            if (desde.HasValue) q = q.Where(x => x.FechaInscripcion >= desde.Value);
            if (hasta.HasValue) q = q.Where(x => x.FechaInscripcion <= hasta.Value);

            var data = q
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    Usuario = x.Usuario != null ? x.Usuario.NombreCompleto : "",
                    Correo = x.Usuario != null ? x.Usuario.Correo : "",
                    Curso = x.Curso != null ? x.Curso.Titulo : "",
                    x.FechaInscripcion,
                    x.Progreso,
                    x.Completado,
                    x.FechaComplecion
                }).ToList();

            var dt = new DataTable("Inscripciones");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Correo", typeof(string));
            dt.Columns.Add("Curso", typeof(string));
            dt.Columns.Add("FechaInscripcion", typeof(DateTime));
            dt.Columns.Add("Progreso", typeof(decimal));
            dt.Columns.Add("Completado", typeof(bool));
            dt.Columns.Add("FechaComplecion", typeof(DateTime));

            foreach (var r in data)
                dt.Rows.Add(r.Id, r.Usuario, r.Correo, r.Curso, r.FechaInscripcion, r.Progreso, r.Completado, (object)r.FechaComplecion ?? DBNull.Value);

            return dt;
        }

        private DataTable BuildEvaluacionesTable(ApplicationDbContext db, DateTime? desde, DateTime? hasta)
        {
            var q = db.Evaluaciones.AsQueryable();
            if (desde.HasValue) q = q.Where(e => e.FechaCreacion >= desde.Value);
            if (hasta.HasValue) q = q.Where(e => e.FechaCreacion <= hasta.Value);

            var data = q
                .OrderBy(e => e.Id)
                .Select(e => new
                {
                    e.Id,
                    Curso = e.Curso != null ? e.Curso.Titulo : "",
                    e.Titulo,
                    e.Tipo,
                    e.NumeroPreguntas,
                    e.TiempoMinutos,
                    e.PuntajeAprobacion,
                    e.Version,
                    e.Activa,
                    e.FechaCreacion
                }).ToList();

            var dt = new DataTable("Evaluaciones");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Curso", typeof(string));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Tipo", typeof(int));
            dt.Columns.Add("NumeroPreguntas", typeof(int));
            dt.Columns.Add("TiempoMinutos", typeof(int));
            dt.Columns.Add("PuntajeAprobacion", typeof(decimal));
            dt.Columns.Add("Version", typeof(int));
            dt.Columns.Add("Activa", typeof(bool));
            dt.Columns.Add("FechaCreacion", typeof(DateTime));

            foreach (var r in data)
                dt.Rows.Add(r.Id, r.Curso, r.Titulo, r.Tipo, r.NumeroPreguntas, r.TiempoMinutos, r.PuntajeAprobacion, r.Version, r.Activa, r.FechaCreacion);

            return dt;
        }

        private DataTable BuildIntentosTable(ApplicationDbContext db, DateTime? desde, DateTime? hasta)
        {
            var q = db.IntentosEvaluacion.AsQueryable();
            if (desde.HasValue) q = q.Where(i => i.FechaInicio >= desde.Value);
            if (hasta.HasValue) q = q.Where(i => i.FechaInicio <= hasta.Value);

            var data = q
                .OrderBy(i => i.Id)
                .Select(i => new
                {
                    i.Id,
                    Usuario = i.Usuario != null ? i.Usuario.NombreCompleto : "",
                    Correo = i.Usuario != null ? i.Usuario.Correo : "",
                    Evaluacion = i.Evaluacion != null ? i.Evaluacion.Titulo : "",
                    Curso = (i.Evaluacion != null && i.Evaluacion.Curso != null) ? i.Evaluacion.Curso.Titulo : "",
                    i.FechaInicio,
                    i.FechaFin,
                    i.PuntajeObtenido,
                    i.Aprobado
                }).ToList();

            var dt = new DataTable("IntentosEvaluacion");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Correo", typeof(string));
            dt.Columns.Add("Curso", typeof(string));
            dt.Columns.Add("Evaluacion", typeof(string));
            dt.Columns.Add("FechaInicio", typeof(DateTime));
            dt.Columns.Add("FechaFin", typeof(DateTime));
            dt.Columns.Add("PuntajeObtenido", typeof(decimal));
            dt.Columns.Add("Aprobado", typeof(bool));

            foreach (var r in data)
                dt.Rows.Add(r.Id, r.Usuario, r.Correo, r.Curso, r.Evaluacion, r.FechaInicio,
                            (object)r.FechaFin ?? DBNull.Value, r.PuntajeObtenido, r.Aprobado);

            return dt;
        }

        // ===== Exportar a Excel (EPPlus) =====

        private void ExportDataTableToXlsx(DataTable table, string fileName, string sheetName)
        {
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add(SafeSheetName(sheetName));

                // Encabezados
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    ws.Cells[1, c + 1].Value = table.Columns[c].ColumnName;
                    ws.Cells[1, c + 1].Style.Font.Bold = true;
                }

                // Datos
                for (int r = 0; r < table.Rows.Count; r++)
                    for (int c = 0; c < table.Columns.Count; c++)
                        ws.Cells[r + 2, c + 1].Value = table.Rows[r][c] is DBNull ? null : table.Rows[r][c];

                ws.Cells.AutoFitColumns();
                ws.View.FreezePanes(2, 1);
                ws.Cells[1, 1, 1, table.Columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[1, 1, 1, table.Columns.Count].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(240, 240, 240));

                var bytes = pck.GetAsByteArray();

                var resp = HttpContext.Current.Response;
                resp.Clear();
                resp.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                resp.AddHeader("content-disposition", $"attachment;  filename={fileName}");
                resp.BinaryWrite(bytes);
                resp.End();
            }
        }

        private static string SafeSheetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Datos";
            var invalid = new[] { '\\', '/', '*', '[', ']', ':', '?' };
            foreach (var ch in invalid) name = name.Replace(ch.ToString(), "");
            return name.Length > 28 ? name.Substring(0, 28) : name;
        }
    }
}
