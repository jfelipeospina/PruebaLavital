using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.IO;
using MiPrimerProyectoMVC.Tags;
using Helper;
using Model.Commons;

namespace MiPrimerProyectoMVC.Controllers
{
    [AutenticadoAttribute]
    public class HomeController : Controller
    {
        private Alumno alumno = new Alumno();
        private AlumnoCurso alumno_curso = new AlumnoCurso();
        private Curso curso = new Curso();
        private Adjunto adjunto = new Adjunto();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarAlumnos(AnexGRID grid) 
        {
            return Json(alumno.Listar(grid));
        }

        // home/ver/1
        [PermisoAttribute(Permiso = RolesPermisos.Alumno_Puede_Visualizar_Un_Alumno)]
        public ActionResult Ver(int id)
        {
            return View(alumno.Obtener(id));
        }

        // home/Cursos/?Alumno_id=1
        public PartialViewResult Cursos(int Alumno_id) 
        {
            // Listamos los cursos de un alumno
            ViewBag.CursosElegidos = alumno_curso.Listar(Alumno_id);

            // Listamos todos los cursos DISPONIBLES
            ViewBag.Cursos = curso.Todos(Alumno_id);

            // Modelo
            alumno_curso.Alumno_id = Alumno_id;

            return PartialView(alumno_curso);
        }

        // home/Adjuntos/?Alumno_id=1
        public PartialViewResult Adjuntos(int Alumno_id)
        {
            ViewBag.Adjuntos = adjunto.Listar(Alumno_id);
            return PartialView();
        }

        public JsonResult GuardarCurso(AlumnoCurso model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = model.Guardar();

                if (rm.response)
                {
                    rm.function = "CargarCursos()";
                }
            }

            return Json(rm);
        }

        public JsonResult GuardarAdjunto(Adjunto model, HttpPostedFileBase Archivo)
        {
            var rm = new ResponseModel();

            if(Archivo != null)
            {
                // Nombre del archivo, es decir, lo renombramos para que no se repita nunca
                string archivo = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Archivo.FileName);

                // La ruta donde lo vamos guardar
                Archivo.SaveAs(Server.MapPath("~/uploads/" + archivo));

                // Establecemos en nuestro modelo el nombre del archivo
                model.Archivo = archivo;

                rm = model.Guardar();

                if (rm.response)
                {
                    rm.function = "CargarAdjuntos()";
                }
            }

            rm.SetResponse(false, "Debe adjuntar una archivo");

            return Json(rm);
        }

        public ActionResult Crud(int id = 0)
        {
            if (id == 0)
            {
                if (!FrontUser.TienePermiso(RolesPermisos.Alumno_Puede_Crear_Nuevo_Registro))
                {
                    return Redirect("~/home");
                }
            }
            return View(
                id == 0 ? new Alumno()
                        : alumno.Obtener(id)
            );
        }

        public JsonResult Guardar(Alumno model)
        {
            var rm = new ResponseModel();

            if (ModelState.IsValid)
            {
                rm = model.Guardar();

                if (rm.response) 
                {
                    rm.href = Url.Content("~/home");
                }
            }

            return Json(rm);
        }

        public ActionResult Eliminar(int id)
        {
            alumno.id = id;
            alumno.Eliminar();
            return Redirect("~/home");
        }

        public ActionResult Salir()
        {
            SessionHelper.DestroyUserSession();
            return Redirect("~/");
        }
    }
}