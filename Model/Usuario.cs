namespace Model
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Usuario")]
    public partial class Usuario
    {
        public int id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual Rol Rol { get; set; }
        public int Rol_id { get; set; }

        public ResponseModel Autenticarse()
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new TestContext())
                {
                    var usuario = ctx.Usuario.Where(x => x.Correo == this.Correo && x.Password == this.Password).SingleOrDefault();
                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.id.ToString());
                        rm.SetResponse(true);
                    }
                    else
                    {
                        rm.SetResponse(false, "Acceso denegado al sistema");
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return rm;
        }

        public Usuario Obtener(int id)
        {
            var usuario = new Usuario();

            try
            {
                using (var ctx = new TestContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    usuario = ctx.Usuario.Include("Rol")
                                         .Include("Rol.Permiso")
                                         .Where(x => x.id == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }
    }
}
