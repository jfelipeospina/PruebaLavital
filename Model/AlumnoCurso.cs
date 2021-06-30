namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("AlumnoCurso")]
    public partial class AlumnoCurso
    {
        public int id { get; set; }

        public int Alumno_id { get; set; }

        public int Curso_id { get; set; }

        [Required(ErrorMessage="Debe ingresar una nota para el alumno.")]
        [Range(0, 20, ErrorMessage="Debe ingresar una nota entre 0 a 20.")]
        public decimal Nota { get; set; }

        public virtual Alumno Alumno { get; set; }

        public virtual Curso Curso { get; set; }

        public ResponseModel Guardar() 
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new TestContext())
                {
                    ctx.Entry(this).State = EntityState.Added;

                    rm.SetResponse(true);
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }

            return rm;
        }

        public List<AlumnoCurso> Listar(int Alumno_id)
        {
            var alumnocursos = new List<AlumnoCurso>();

            try
            {
                using (var ctx = new TestContext())
                {
                    alumnocursos = ctx.AlumnoCurso.Include("Curso")
                                                  .Where(x => x.Alumno_id == Alumno_id)
                                                  .ToList();
                }
            }
            catch (Exception E)
            {

                throw;
            }

            return alumnocursos;
        }
    }
}
