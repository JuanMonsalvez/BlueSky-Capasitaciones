using bluesky.App_Code;                 // AdminPage
using bluesky.Models;
using bluesky.Services.Security;        // PasswordHasher
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bluesky.Admin
{
    public partial class AdminUsuarios : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRolesNuevo();
                BindGrid();
            }
        }

        private void CargarRolesNuevo()
        {
            using (var db = new ApplicationDbContext())
            {
                ddlRolNuevo.DataSource = db.Roles.OrderBy(r => r.Nombre).ToList();
                ddlRolNuevo.DataTextField = "Nombre";
                ddlRolNuevo.DataValueField = "Id";
                ddlRolNuevo.DataBind();

                // Si no existe "Usuario", lo manejamos en code-behind al crear.
            }
        }

        private void BindGrid()
        {
            using (var db = new ApplicationDbContext())
            {
                var data = db.Usuarios
                    .Select(u => new
                    {
                        u.Id,
                        u.NombreCompleto,
                        u.Correo,
                        u.Activo,
                        u.FechaRegistro,
                        RolId = u.RolId
                    })
                    .OrderBy(u => u.NombreCompleto)
                    .ToList();

                gvUsuarios.DataSource = data;
                gvUsuarios.DataBind();
            }
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            // Llenar dropdown de roles y marcar el actual
            var ddl = (DropDownList)e.Row.FindControl("ddlRol");
            if (ddl == null) return;

            int rolIdActual = 0;
            int.TryParse(DataBinder.Eval(e.Row.DataItem, "RolId")?.ToString(), out rolIdActual);

            using (var db = new ApplicationDbContext())
            {
                ddl.DataSource = db.Roles.OrderBy(r => r.Nombre).ToList();
                ddl.DataTextField = "Nombre";
                ddl.DataValueField = "Id";
                ddl.DataBind();
            }

            if (rolIdActual > 0 && ddl.Items.FindByValue(rolIdActual.ToString()) != null)
                ddl.SelectedValue = rolIdActual.ToString();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Guardar":
                    GuardarUsuarioFila(id, (GridViewRow)((Control)e.CommandSource).NamingContainer);
                    break;
                case "ResetPass":
                    ResetearPassword(id);
                    break;
                case "Eliminar":
                    EliminarUsuario(id);
                    break;
            }
        }

        private void GuardarUsuarioFila(int id, GridViewRow row)
        {
            var ddlRol = (DropDownList)row.FindControl("ddlRol");
            var chkActivo = (CheckBox)row.FindControl("chkActivo");

            using (var db = new ApplicationDbContext())
            {
                var user = db.Usuarios.Find(id);
                if (user == null) { ShowMsg("Usuario no encontrado", true); return; }

                int rolId;
                if (!int.TryParse(ddlRol.SelectedValue, out rolId))
                {
                    // Si algo falla, intenta asignar rol "Usuario"
                    var rolUsuario = db.Roles.FirstOrDefault(r => r.Nombre == "Usuario");
                    if (rolUsuario != null) rolId = rolUsuario.Id;
                }

                user.RolId = rolId > 0 ? (int?)rolId : null;
                user.Activo = chkActivo.Checked;
                db.SaveChanges();
            }

            ShowMsg("Cambios guardados.");
            BindGrid();
        }

        private void ResetearPassword(int id)
        {
            var temporal = GenerarPasswordTemporal(); // cumple la política
            using (var db = new ApplicationDbContext())
            {
                var user = db.Usuarios.Find(id);
                if (user == null) { ShowMsg("Usuario no encontrado", true); return; }

                user.PasswordHash = PasswordHasher.HashPassword(temporal);
                db.SaveChanges();
            }

            ShowMsg($"Contraseña reseteada: <strong>{temporal}</strong> (cámbiala al iniciar sesión).");
        }

        private void EliminarUsuario(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Usuarios.Find(id);
                if (user == null) { ShowMsg("Usuario no encontrado", true); return; }

                // opción segura: desactivar en vez de borrar real
                db.Usuarios.Remove(user);
                db.SaveChanges();
            }

            ShowMsg("Usuario eliminado.");
            BindGrid();
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            // Validación de servidor para la contraseña fuerte
            var pass = txtPassword.Text ?? "";
            if (!EsPasswordFuerte(pass))
            {
                ShowMsg("La contraseña debe tener 8+ con mayúscula, minúscula y número.", true);
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                if (db.Usuarios.Any(u => u.Correo == txtCorreo.Text.Trim()))
                {
                    ShowMsg("El correo ya existe.", true);
                    return;
                }

                int rolIdNuevo;
                Rol rolSeleccionado = null;

                // Intentar con el rol del combo; si no, usar "Usuario"
                if (int.TryParse(ddlRolNuevo.SelectedValue, out rolIdNuevo))
                    rolSeleccionado = db.Roles.FirstOrDefault(r => r.Id == rolIdNuevo);

                if (rolSeleccionado == null)
                    rolSeleccionado = db.Roles.FirstOrDefault(r => r.Nombre == "Usuario");

                var usuario = new Models.Usuario
                {
                    NombreCompleto = txtNombre.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    PasswordHash = PasswordHasher.HashPassword(pass),
                    RolId = rolSeleccionado?.Id,
                    Activo = true,
                    FechaRegistro = DateTime.UtcNow
                };

                db.Usuarios.Add(usuario);
                db.SaveChanges();
            }

            // limpiar form y recargar
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtPassword.Text = "";
            ShowMsg("Usuario creado correctamente.");
            BindGrid();
        }

        // --- utilidades ---

        private void ShowMsg(string html, bool error = false)
        {
            lblMsg.CssClass = error ? "alert alert-danger" : "alert alert-success";
            lblMsg.Text = html;
        }

        private static bool EsPasswordFuerte(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            // al menos 8, una minúscula, una mayúscula y un número
            System.Text.RegularExpressions.Regex rx =
                new System.Text.RegularExpressions.Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$");
            return rx.IsMatch(s);
        }

        private static string GenerarPasswordTemporal()
        {
            // 10 chars: incluye mayúscula, minúscula y número
            const string lower = "abcdefghjkmnpqrstuvwxyz";
            const string upper = "ABCDEFGHJKMNPQRSTUVWXYZ";
            const string nums = "23456789";
            var rnd = new Random();

            // Reemplazo de función local "pick" por método privado estático
            string basePwd = PickChars(upper, 2, rnd) + PickChars(lower, 6, rnd) + PickChars(nums, 2, rnd);
            return basePwd;
        }

        // Método auxiliar para reemplazar la función local
        private static string PickChars(string src, int n, Random rnd)
        {
            var ch = new char[n];
            for (int i = 0; i < n; i++) ch[i] = src[rnd.Next(src.Length)];
            return new string(ch);
        }
    }
}
