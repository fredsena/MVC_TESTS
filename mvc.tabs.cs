using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public class CadastroFuncionarioDTO
    {
        public FuncionarioDTO funcionario { get; set; }
        public List<DependentesDTO> dependentes { get; set; }
        public List<DocumentosDTO> documentos { get; set; }


    }
}

//////

@model List<Mvc.Models.DependentesDTO>

Dependentes

<ul>
    @foreach (var x in Model)
    {

        <li>@x.DescDependente</li>

    }
</ul>

/////
@model List<Mvc.Models.DocumentosDTO>

Dependentes

<ul>
    @foreach (var x in Model)
    {

        <li>@x.DescDocDoc</li>
    }
</ul>


@model Mvc.Models.FuncionarioDTO

Funcionario

@Model.Nome


///////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Models
{
    public static class DataFactory
    {
        public static IEnumerable<CustomerModel> GetCustomers()
        {
            return new List<CustomerModel>()
            {
                new CustomerModel() { CustomerId = 1, FirstName = "Miguel", LastName = "Castro", Email = "miguelcastro67@gmail.com" },
                new CustomerModel() { CustomerId = 2, FirstName = "John", LastName = "Petersen", Email = "johnvpetersen@gmail.com" },
                new CustomerModel() { CustomerId = 3, FirstName = "Brian", LastName = "Noyes", Email = "briannoyes@gmail.com" },
                new CustomerModel() { CustomerId = 4, FirstName = "Andrew", LastName = "Brust", Email = "andrewbrust@gmail.com" },
                new CustomerModel() { CustomerId = 5, FirstName = "Rocky", LastName = "Lhotka", Email = "rockylhotka@gmail.com" }
            };
        }

        public static CadastroFuncionarioDTO GetFuncionario()
        {

            var func = new FuncionarioDTO() { FuncionarioId = 1, Nome = "Fred Sena" };

            var dep = new List<DependentesDTO>() { 
                        new DependentesDTO(){ DependenteId = 1 , DescDependente = "Filho1" , FuncionarioId = 1 }, 
                        new DependentesDTO(){ DependenteId = 2 , DescDependente = "Filho2" , FuncionarioId = 1 },
                        new DependentesDTO(){ DependenteId = 3 , DescDependente = "Esposa" , FuncionarioId = 1 },
                        new DependentesDTO(){ DependenteId = 4 , DescDependente = "Enteado" , FuncionarioId = 1 }
            };

            var doc = new List<DocumentosDTO>() 
            { 
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certidao Nascimento" , FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certidao Casamento" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Título de Eleitor" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Passaporte" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Certificação X" ,FuncionarioId = 1},
                        new DocumentosDTO() {DocumentoId = 1, DescDocDoc = "Diploma Curso Superior" ,FuncionarioId = 1}
            };

            CadastroFuncionarioDTO cad = new CadastroFuncionarioDTO
            {
                funcionario = func,
                dependentes = dep,
                documentos = doc
            };

            return cad;
        }
    }
}


///////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Models;

namespace Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var func = View(DataFactory.GetFuncionario());
            return func;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _FuncionarioTab()
        {
            var Funcionario = new FuncionarioDTO() { FuncionarioId = 1, Nome = "Fred Sena" };
            return PartialView(Funcionario);
        }

        public ActionResult _DocumentosTab(List<DocumentosDTO> Documento)
        {
            
            return PartialView(Documento);
        }

        public ActionResult _DependentesTab(List<DependentesDTO> Dependente)
        {
           
            return PartialView(Dependente);
        }
        

    }


}

/////////////////


@model Mvc.Models.CadastroFuncionarioDTO

@{
    ViewBag.Title = "Home Page";

    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
    ser.MaxJsonLength = int.MaxValue;
    string strCadastroFuncionario = ser.Serialize(Model);    
}


<!-- Tab Buttons -->
<ul id="tabstrip" class="nav nav-tabs" role="tablist">
    <li class="active"><a href="#_FuncionarioTab" role="tab" data-toggle="tab">Funcionario</a></li>
    <li><a href="#_DependentesTab" role="tab" data-toggle="tab">Dependentes</a></li>
    <li><a href="#_DocumentosTab" role="tab" data-toggle="tab">Documentos</a></li>
</ul>

<!-- Tab Content Containers -->
<div class="tab-content">
    <div class="tab-pane fade in active" id="_FuncionarioTab">@Html.Partial("_FuncionarioTab", @Model.funcionario)</div>
    <div class="tab-pane fade" id="_DependentesTab"></div>
    <div class="tab-pane fade" id="_DocumentosTab"></div>
</div>


@section customsJs {
    <script>
        var jsonCadastroFuncionario = @Html.Raw(strCadastroFuncionario);

    $(function () {
        $('#tabstrip a').click(function (e) {
            e.preventDefault()
            var tabID = $(this).attr("href").substr(1);
            $(".tab-pane").each(function () {
                console.log("clearing " + $(this).attr("id") + " tab");

                $(this).empty();
            });

            var data = { Documento: jsonCadastroFuncionario.documentos };

            $.ajax({
                url: '@Url.Action("_DocumentosTab","Home")',
                cache: false,
                data: data,
                type: "POST",
                dataType: "html",
                success: function (result) {
                    $("#" + tabID).html(result);
                }

            })
            $(this).tab('show')
        });
    });


    </script>
}

//////////





