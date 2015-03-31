using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Exemplo.Model;

namespace Exemplo.Controllers
{

    public class AlunoController : Controller
    {
        public ActionResult Aluno()
        {
            ViewBag.Message = "Aluno";

            return View();
        }

        [HttpPost]
        public void gravarAluno(string objeto)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Aluno aluno = serializer.Deserialize<Aluno>(HttpUtility.UrlDecode(objeto));
            aluno.Gravar();
        }

        [HttpPost]
        public void excluirAluno(int cod_aluno)
        {
            Aluno aluno = new Aluno();
            aluno.cod_aluno = cod_aluno;
            aluno.Excluir();

        }

        [HttpPost]
        public ActionResult consultarAluno(string objeto, string view)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Aluno aluno = serializer.Deserialize<Aluno>(HttpUtility.UrlDecode(objeto));
            return View(view, aluno.listar());
        }

        [HttpPost]
        public ActionResult visualizarAluno(int codigo)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Aluno aluno = new Aluno();
            List<Aluno> ListAluno = new List<Aluno>();
            aluno.cod_aluno = codigo;
            ListAluno = aluno.listar();
            ModelState.Clear();
            return PartialView("AlunoForm", ListAluno[0]);
        }

        [HttpPost]
        public void alterarAluno(string objeto)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Aluno aluno = serializer.Deserialize<Aluno>(HttpUtility.UrlDecode(objeto));
            aluno.Alterar();
        }
    }
}
