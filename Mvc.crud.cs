    public static class Util
    {
        public static Dictionary<string, string> SimNao()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("S", "Sim");
            items.Add("N", "Não");
            return items;
        }
        public static Dictionary<string, string> TipoFuncionario()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("IC", "Interno Colaborador");
            items.Add("IE", "Interno Empresa");
            items.Add("EX", "Externo");
            items.Add("SI", "Sistema");
            return items;
        }
        public static Dictionary<string, string> TipoAdmissao()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add("MD", "Módulo");
            items.Add("AG", "Agrupador Funcional");
            items.Add("ET", "Elemento de tela");
            items.Add("AC", "Ações");
            items.Add("RG", "Regra");
            items.Add("SE", "Serviço");
            //listaFuncionalidade.Add("AD", "Auditado");
            return items;
        }
    }

        public ActionResult Create()
        {
            AdmissaoModel admissao = new AdmissaoModel() {AdmisaoAtivo = "S" };

            ViewBag.TipoAdmissaoId = new SelectList(SRH.Mvc.Util.Util.TipoAdmissao(), "key", "Value");

            return View(admissao);
        }
		
		    public class Mensagem
    {
        public string Text { get; set; }
        public TipoMensagem TipoMensagem { get; set; }

    }
	
	    public enum TipoMensagem
    {
        Success = 1,
        Warning = 2,
        Error = 3,
        Info = 4,
    }
	
	@model SRH.Mvc.Models.AdmissaoModel
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
    //ViewBag.Title = "Incluir Funcionalidade";
    //ViewBag.Current = " Cadastro / " + Html.ActionLink("Funcionalidade", "Pesquisar") + " / Incluir";

    SRH.Mvc.Core.Mensagem mensagem = (SRH.Mvc.Core.Mensagem)Session["Mensagem"];
    Session["Mensagem"] = null;
    
}

@using (Html.BeginForm("Create", "Customer", FormMethod.Post, new { @class = "form", @role = "form" }))
{
    
    <section class="panel panel-default">
        <article class="panel-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        @Html.LabelFor(x => x.DescricaoAdmissao, new { @class = "control-label" })
                        @Html.TextBoxFor(model => model.DescricaoAdmissao, new { @class = "form-control", @maxlength = "100" })
                        @Html.ValidationMessageFor(model => model.DescricaoAdmissao)
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        @Html.LabelFor(x => x.TipoAdmissaoId, new { @class = "control-label required" })
                        @Html.DropDownListFor(model => model.TipoAdmissaoId, null, "Selecione", new { @class = "form-control", onchange = "validarDropDown(this)" })
                        @Html.ValidationMessageFor(x => x.TipoAdmissaoId)
                    </div>
                </div>
            </div>
            <div class="line line-dashed b-b line-lg pull-in"></div>
            <div class="row">
                <div class="col-md-3">
                    @Html.LabelFor(x => x.AdmisaoAtivo, new { @class = "control-label required" })
                    <div class="col-sm-12">
                        <div class="radio i-checks radio-inline">
                            <label>@Html.RadioButtonFor(x => x.AdmisaoAtivo, "S", new { @checked = true })<i></i> Sim</label>
                        </div>
                        <div class="radio i-checks radio-inline">
                            <label>@Html.RadioButtonFor(x => x.AdmisaoAtivo, "N")<i></i> Não</label>
                        </div>
                    </div>
                </div>                
            </div>

            <div id="divAdmissaoManter" class="hide">
                <br />
                <div class="row">
                    <div class="col-md-12">
                        <div class="line line-dashed b-b line-lg pull-in"></div>
                        <div class="form-group">
                            <label class="control-label">Criar as Admissões do Manter</label>
                            <br />
                            <div class="col-md-3">
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="Incluir">
                                    <i></i><span style="font-weight:normal">Incluir</span>
                                </label>
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="Alterar">
                                    <i></i><span style="font-weight:normal">Alterar</span>
                                </label>
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="Excluir">
                                    <i></i><span style="font-weight:normal">Excluir</span>
                                </label>
                            </div>
                            <div class="col-md-3">
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="Visualizar">
                                    <i></i><span style="font-weight:normal">Visualizar</span>
                                </label>
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="Pesquisar">
                                    <i></i><span style="font-weight:normal">Pesquisar</span>
                                </label>
                                <label class="checkbox m-n i-checks">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <input type="checkbox" id="funcionalidadeCrud" name="funcionalidadeCrud" value="ExportarPesquisa">
                                    <i></i><span style="font-weight:normal">ExportarPesquisa</span>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="hide" id="divBtnSelecionarIcone">
                <div class="form-group">
                    <div class="col-md-6">
                        <div class="line line-dashed b-b line-lg pull-in"></div>
                        <div class="form-group">
                            <table>
                                <tr>
                                    <td>
                                        <label class="control-label">&nbsp;</label>
                                        <br />
                                        <a href="#imagemModal" data-loading="false" class="btn btn-success" data-toggle="modal" data-dismiss="modal" data-backdrop="static"><span class="fa fa-share"></span> Selecionar Ícone</a>
                                    </td>
                                    <td>
                                        <label class="control-label">&nbsp;</label>
                                        <br />
                                        <div style="margin-left:25px; margin-right:25px" data-toggle="context">
                                            <span class="m-b-xs block">
                                                <i id="imgFuncionalidade" class="@Model.DescricaoAdmissao"></i>
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    @Html.Partial("~/Views/Customer/FormIcon.cshtml")
                </div>
            </div>

            <br />
        </article>

        <footer class="panel-footer">
            <button type="submit" class="btn btn-primary"><i class="fa fa-save"></i> Salvar</button>
            <a href="@Url.Action("Pesquisar")" class="btn btn-default"><i class="fa fa-close"></i> Cancelar</a>
        </footer>
    </section>
}

@section customsJs {
    <script type="text/javascript">

        //$("#IdeSistema").change(function (e) {
        //    var ideSistema = $('#IdeSistema :selected').val();
        //    $.post('/Admissao/BuscarAdmissaoBase', { ideSistema: ideSistema }, function (data) {
        //        var html = '<option value="">Não se Aplica</option>';
        //        $.each(data, function (index, value) {
        //            html = html + '<option value="' + value.IdeFuncionalidade + '">' + value.NomFuncionalidade + '</option>';
        //        });
        //        $("#IdeFuncionalidadePrincipal").html(html);
        //    });
        //});

        function validarDropDown(element) {
            var tipo = $('#TipoAdmissaoId').val();

            $("#divAdmissaoManter").addClass("hide");
            if (tipo == "AG") {
                $("#divAdmissaoManter").removeClass("hide");
            }

            if (tipo == "MD") {
                $("#divBtnSelecionarIcone").removeClass("hide");
            } else {
                $("#divBtnSelecionarIcone").addClass("hide");
                $("#imgFuncionalidade").removeClass();
                $("#DscCaminhoImg").val("");
            }
        }

        // Transfere a imagem selecionada no formulário Modal para a página solicitante (Incluir ou Alterar)
        $("#btnSelecionarIcone").click(function () {
            var img = $("#imgSelecionada").val();
            $("#imgFuncionalidade").removeAttr("class");
            $("#imgFuncionalidade").addClass(img);
            $("#DscCaminhoImg").val(img);
        });

        // Seleciona a imagem no formulário Modal
        $("a.padder-v").click(function () {
            var img = $(this).children().children().attr("class");
            $(".padder-v").removeClass("active");
            $("#imgSelecionada").val(img);
            $(this).addClass("active");
        })

    </script>
}


@model IEnumerable<Domain.Entities.Controle.Coordenacao>

@{
    ViewBag.Title = "Coordenações";
    Layout = "~/Views/Shared/_Layout.cshtml";

    .Infra.Core.Message.Message mensagem = (.Infra.Core.Message.Message)Session["Mensagem"];
    Session["Mensagem"] = null;

    System.Web.Script.Serialization.JavaScriptSerializer ser = new System.Web.Script.Serialization.JavaScriptSerializer();
    ser.MaxJsonLength = int.MaxValue;
    string strCoordenacoes = ser.Serialize(Model);            
}

@{
    if (mensagem != null)
    {
        string alertMsg = "alert-" + mensagem.TypeMessage.ToString().ToLower();

        <div class="panel-body">
            <div class="alert @alertMsg">
                <button type="button" class="close" data-dismiss="alert">×</button>
                @mensagem.Text
            </div>
        </div>
    }
}

<section class="panel panel-primary" ng-controller="CoordenacaoController">
    <header class="panel-heading">
        <h4 class="panel-title">Coordenação</h4>
    </header>
    <div class="panel-body">
        <div class="panel panel-default m-n">
            <div class="panel-heading"><h5 class="panel-title">Nova coordenação</h5></div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="arredondado" class="col-sm-2 control-label">Nome da coordenação</label>
                        <div class="col-sm-6 col-xs-12">
                            <input ng-change="IsExisteEsteNomeCadastrado()" ng-model="Coordenacao.NomCoordenacao" type="text" name="arredondado" id="arredondado" class="form-control rounded" placeholder="">
                        </div>

                        <div class="col-sm-4 col-xs-12">                            
                            <a ng-if="Coordenacao.Id > 0" ng-click="CancelarEdicao()" href="#" class="btn btn-s-md btn-default"><i class="fa fa-long-arrow-left"></i> Cancelar</a>                            
                            <a ng-disabled="IsBloquearEdicao" ng-if="Coordenacao.Id > 0" ng-click="SalvarCoordenacao(false)" href='#' class="btn btn-s-md btn-primary"><i class="fa fa-save"></i> Salvar</a>
                            <a ng-disabled="IsBloquearEdicao" ng-if="Coordenacao.Id == 0" ng-click="SalvarCoordenacao(true)" href='#' class="btn btn-s-md btn-success "><i class="fa fa-plus"></i> Adicionar</a>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <br />

        <div class="panel panel-default m-n">
            <div class="panel-heading"><h5 class="panel-title">Coordenações cadastradas</h5></div>
            <div class="table-responsive">
               
                <div ng-if="Coordenacoes.length == 0" class="panel-body">
                    <div class="alert alert-info">
                        @Core.Mensagem.Texto.MSG017
                    </div>
                </div>
                   
                <table ng-if="Coordenacoes.length > 0" class="table b-t b-light">
                    <thead>
                        <tr>
                            <th class="sort" data-sort="municipio" style="width:50%"><label>Nome da Coordenação</label> </th>
                            <th class="sort th-140 text-center" data-sort="categoria" style="width:15%"><label>Assuntos</label> </th>
                            <th class="sort th-140 text-center" data-sort="data" style="width:15%"><label>Colaboradores</label></th>
                            <th class="sort th-50 text-center" data-sort="estado" style="width:20%"><label>Ações</label></th>
                        </tr>
                    </thead>
                    <tbody class="list">
                        <tr ng-repeat="coordenacao in Coordenacoes" ng-class="{'well': coordenacao.IdcAtivo == 'N'}">
                            <td>{{coordenacao.NomCoordenacao}}</td>
                            <td class="text-center"><i class="i i-file2"></i> ({{coordenacao.AssuntosVinculados.length}})</td>
                            <td class="text-center"><i class="i i-users3"></i> ({{coordenacao.Colaboradores.length}})</td>
                            <td class="text-center">
                                <a ng-disabled="coordenacao.IdcAtivo == 'N'" ng-click="SelecionarCoordenacao(coordenacao)" href="#" class="btn btn-info" title="Editar coordenação"><i class="fa fa-pencil"></i></a>
                                <a ng-disabled="coordenacao.IdcAtivo == 'N'" href="@Url.Action("Assuntos", "Coordenacao")/{{coordenacao.Id}}" class="btn btn-warning" title="Vincular assuntos"><i class="i i-file2"></i></a>
                                <a ng-disabled="coordenacao.IdcAtivo == 'N'" href="@Url.Action("Colaboradores", "Coordenacao")/{{coordenacao.Id}}" class="btn btn-primary" title="Vincular colaboradores"><i class="i i-users3"></i></a>
                                <a ng-click="Desativar(coordenacao.Id)" ng-if="coordenacao.IdcAtivo == 'S'" href="#"class="btn btn-danger" title="Desativar coordenação"><i class="i i-folder-open"></i></a>
                                <a ng-click="Reativar(coordenacao.Id)" ng-if="coordenacao.IdcAtivo == 'N'" href="#" class="btn btn-default" title="Reativar coordenação"><i class="i i-undo"></i></a>
                            </td>
                        </tr>
                    </tbody>
                </table>
                    
                
            </div>
        </div>

    </div>
</section>

@section scripts
{
    <script type="text/javascript">

    appAngular.controller('CoordenacaoController', ['$scope', '$http', function ($scope, $http) {
        $scope.Coordenacoes = @Html.Raw(strCoordenacoes);
        $scope.Coordenacao = { Id : 0 };
        $scope.IsBloquearEdicao = true;
        $scope.SelecionarCoordenacao = function (coordenacao) {
            $scope.Coordenacao = {
                Id: coordenacao.Id,
                NomCoordenacao:coordenacao.NomCoordenacao
            };
        };

        $scope.CancelarEdicao = function () {
            $scope.Coordenacao = { Id : 0 };
        };

        $scope.IsExisteEsteNomeCadastrado = function()
        {
            if($scope.Coordenacoes.Any(function(item){ return item.NomCoordenacao == $scope.Coordenacao.NomCoordenacao;}))
            {
                $scope.IsBloquearEdicao = true;
            }else{
                $scope.IsBloquearEdicao = false;
            }
        }

        $scope.Desativar = function(id)
        {
            bootbox.dialog(
            {
                message: "@Core.Mensagem.Texto.MSG011",
                title: "Confirmação",
                buttons: {
                    Cancelar: {
                        label: "Cancelar",
                        className: "btn-default",
                        callback: function() {

                        }
                    },
                    Confirmar: {
                        label: "Sim",
                        className: "btn-primary",
                        callback: function() {
                            $http.post('@Url.Action("Desativar", "Coordenacao")', {id:id})
                             .success(function (data, status, headers, config) {
                                 console.log(data);
                                 if(data.mensagem.TypeMessage == '@Convert.ToInt32(.Infra.Core.Message.TypeMessage.Success)')
                                 {
                                     var coordenacaoDaLista = $scope.Coordenacoes.Where(function(item){ return item.Id == id;}).First();
                                     coordenacaoDaLista.IdcAtivo = "N";
                                     toastr.success(data.mensagem.Text, "Sucesso");
                                 }else{
                                     toastr.error(data.mensagem.Text, "Erro");
                                 }
                             })
                             .error(function (data, status, headers, config) {
                                 toastr.error(data.mensagem.Text, "Erro");
                             });
                        }
                    }
                }
            });

        }

        $scope.Reativar = function(id)
        {
            bootbox.dialog(
            {
                message: "@Core.Mensagem.Texto.MSG029",
                title: "Confirmação",
                buttons: {
                    Cancelar: {
                        label: "Cancelar",
                        className: "btn-default",
                        callback: function() {

                        }
                    },
                    Confirmar: {
                        label: "Sim",
                        className: "btn-primary",
                        callback: function() {
                            $http.post('@Url.Action("Reativar", "Coordenacao")', {id : id})
                             .success(function (data, status, headers, config) {
                                 console.log(data);
                                 if(data.mensagem.TypeMessage == '@Convert.ToInt32(.Infra.Core.Message.TypeMessage.Success)')
                                 {
                                     var coordenacaoDaLista = $scope.Coordenacoes.Where(function(item){ return item.Id == id;}).First();
                                     coordenacaoDaLista.IdcAtivo = "S";
                                     toastr.success(data.mensagem.Text, "Sucesso");
                                 }else{
                                     toastr.error(data.mensagem.Text, "Erro");
                                 }
                             })
                             .error(function (data, status, headers, config) {
                                 toastr.error(data.mensagem.Text, "Erro");
                             });
                        }
                    }
                }
            });
        }

        $scope.SalvarCoordenacao = function (isNovaCoordenacao) {
            $http.post('@Url.Action("Save", "Coordenacao")', $scope.Coordenacao)
                     .success(function (data, status, headers, config) {                         
                         if(data.mensagem.TypeMessage == '@Convert.ToInt32(.Infra.Core.Message.TypeMessage.Success)')
                         {                             
                             console.log(isNovaCoordenacao);                             
                             if(!isNovaCoordenacao)
                             {
                                 $scope.Coordenacoes.First(function(item){ return item.Id == $scope.Coordenacao.Id;}).NomCoordenacao = $scope.Coordenacao.NomCoordenacao;
                             }

                             if(isNovaCoordenacao)
                             {
                                 $scope.Coordenacoes.push(data.Coordenacao);
                             }
                             
                             $scope.Coordenacoes = $scope.Coordenacoes.OrderBy(function(item){ return item.NomCoordenacao;});

                             $scope.Coordenacao = {Id:0};
                             toastr.success(data.mensagem.Text, "Sucesso");
                         }else{
                             toastr.error(data.mensagem.Text, "Erro");
                         }
                     })
                     .error(function (data, status, headers, config) {
                         toastr.error(data.mensagem.Text, "Erro");
                     });
        };

    }]);
</script>
}

using DataTransfer.Entities;
using Domain.Entities.Controle;
using Domain.Interfaces.Controle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using .Infra.Core.Extension;
using Core.Exception;
using System.Reflection;
using .Infra.Core.Message;

namespace Presentation.Mvc.Controllers
{
    public class CoordenacaoController : BaseController
    {        
        public ActionResult Index()
        {
            try
            {
                List<Coordenacao> coordenacoes = (from coordenacao in AppBase.Uow.CoordenacaoRepository.GetAllCoordenacoes().ToList()
                                                  select new Coordenacao
                                                  {
                                                      Id = coordenacao.Id,
                                                      IdcAtivo = coordenacao.IdcAtivo,
                                                      NomCoordenacao = coordenacao.NomCoordenacao,
                                                      AssuntosVinculados = (from assunto in coordenacao.AssuntosVinculados.Where(a => a.IdcAtivo == "S").ToList()
                                                                            select new VinculoCoordenacaoAssunto
                                                                            {
                                                                                Id = assunto.Id,
                                                                                Assunto = new Assunto { Id = assunto.IdeAssunto, NomAssunto = assunto.Assunto.NomAssunto }
                                                                            }).ToList(),
                                                      Colaboradores = (from colaborador in coordenacao.Colaboradores.Where(c => c.IdcAtivo == "S").ToList()
                                                                       select new Colaborador
                                                                       {
                                                                           Id = colaborador.Id,
                                                                           NomColaborador = colaborador.NomColaborador
                                                                       }).ToList()
                                                  }).OrderBy(c=>c.NomCoordenacao).ToList();

                return View(coordenacoes);
            }
            catch (Exception exc)
            {                
                throw exc;
            }            
        }      

        public JsonResult Save(Coordenacao coordenacao)
        {
            try
            {
                bool isNovaCoordenacao = coordenacao.Id == 0;
                
                AppBase.CoordenacaoAP.SalvarCoordenacaoLog(coordenacao);

                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = Core.Mensagem.Texto.MSG001,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Success
                };
                if(isNovaCoordenacao)
                {
                    coordenacao.AssuntosVinculados = new List<VinculoCoordenacaoAssunto>();
                    coordenacao.Colaboradores = new List<Colaborador>();
                }
                return Json(new { mensagem = mensagem, Coordenacao = coordenacao }, JsonRequestBehavior.AllowGet);
            }
            catch (RNG027CoordenacaoExistenteException exc)
            {
                exc.RunBusinessRule(() => { });
                
                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = exc.MessageException.Message,
                    TypeMessage = exc.MessageException.TypeMessage
                };

                return Json(new { mensagem = mensagem, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                string msgErro = exc.InnerException != null ? exc.Message : exc.Message + " - " + exc.InnerException.Message;
                
                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = msgErro,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Error
                };

                return Json(new { mensagem = mensagem, }, JsonRequestBehavior.AllowGet);
            }
        }        

        public void VincularAssuntos(int ideCoordenacao, List<int> idsAssutosParaVincular, List<int> idsAssutosParaDesvincular)
        {
            try
            {
                AppBase.CoordenacaoAP.ConfigurarVinculosDeAssuntosLog(ideCoordenacao, idsAssutosParaVincular, idsAssutosParaDesvincular);

                Session["Mensagem"] = new .Infra.Core.Message.Message
                {
                    Text = Core.Mensagem.Texto.MSG010,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Success
                };
            }
            catch (Exception exc)
            {
                string msgErro = exc.InnerException != null ? exc.Message : exc.Message + " - " + exc.InnerException.Message;

                Session["Mensagem"] = new .Infra.Core.Message.Message
                {
                    Text = msgErro,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Error
                };
            }
        }

        public JsonResult Desativar(int id)
        {
            try
            {
                AppBase.CoordenacaoAP.DesativarCoordenacaoEDesafazerVinculosLog(id);
                
                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = Core.Mensagem.Texto.MSG027,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Success
                };

                return Json(new { mensagem = mensagem }, JsonRequestBehavior.AllowGet);                
            }
            catch (Exception exc)
            {                
                string msgErro = exc.InnerException != null ? exc.Message : exc.Message + " - " + exc.InnerException.Message;

                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = msgErro,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Error
                };

                return Json(new { mensagem = mensagem, }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Reativar(int id)
        {            
            try
            {
                AppBase.CoordenacaoAP.ReativarCoordenacaoLog(id);                

                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = Core.Mensagem.Texto.MSG030,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Success
                };

                return Json(new { mensagem = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                string msgErro = exc.InnerException != null ? exc.Message : exc.Message + " - " + exc.InnerException.Message;

                Message mensagem = new .Infra.Core.Message.Message
                {
                    Text = msgErro,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Error
                };

                return Json(new { mensagem = mensagem, }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Assuntos(int id)
        {
            try
            {
                List<Assunto> assuntos = AppBase.Uow.AssuntoRepository.GetAssuntosAtivos().ToList();
                Coordenacao coordenacaoDB = AppBase.Uow.CoordenacaoRepository.GetCoordenacao(id);
                Coordenacao coordenacao = new Coordenacao();
                coordenacaoDB.CopiarPropriedades<Coordenacao>(ref coordenacao);
                List<VinculoCoordenacaoAssunto> assuntosVinculados = coordenacao.AssuntosVinculados.ToList();
                List<VinculoCoordenacaoAssuntoTO> vinculosDeAssuntos = (from assunto in assuntos
                                                                        select new VinculoCoordenacaoAssuntoTO
                                                                        {
                                                                            IdeAssunto = assunto.Id,
                                                                            NomeAssunto = assunto.NomAssunto,
                                                                            IdeCoordenacao =
                                                                                assunto.CoordenacoesVinculadas.Any(c => c.IdcAtivo == "S")
                                                                                ? assunto.CoordenacoesVinculadas.FirstOrDefault(c => c.IdcAtivo == "S").IdeCoordenacao
                                                                                : 0,
                                                                            NomeCoordenacao =
                                                                                assunto.CoordenacoesVinculadas.Any(c => c.IdcAtivo == "S")
                                                                                ? assunto.CoordenacoesVinculadas.FirstOrDefault(c => c.IdcAtivo == "S").Coordenacao.NomCoordenacao
                                                                                : "---",
                                                                        }).ToList();
                coordenacao.AssuntosVinculados = null;
                coordenacao.Colaboradores = null;

                string jsonCoordenacao = coordenacao.ToJSON();
                ViewData["Coordenacao"] = jsonCoordenacao;
                return View(vinculosDeAssuntos);
                //return View(new List<VinculoCoordenacaoAssuntoTO>());
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public ActionResult Colaboradores(int id)
        {
            try
            {
                Coordenacao coordenacao = new Coordenacao();
                Coordenacao coordenacaoDB = AppBase.Uow.CoordenacaoRepository.GetCoordenacao(id);
                coordenacaoDB.CopiarPropriedades<Coordenacao>(ref coordenacao);

                List<Colaborador> colaboradoresAtivos = (from colaborador in AppBase.Uow.ColaboradorRepository.GetColaboradoresAtivosDaSFF().ToList()
                                                         select new Colaborador
                                                         {
                                                             Id = colaborador.Id,
                                                             NomColaborador = colaborador.NomColaborador,
                                                             Coordenacao = colaborador.Coordenacao != null 
                                                                          ? new Coordenacao { NomCoordenacao = colaborador.Coordenacao.NomCoordenacao }
                                                                          : new Coordenacao { NomCoordenacao = "---" },
                                                             IdeCoordenacao = colaborador.IdeCoordenacao
                                                         }).ToList();

                coordenacao.AssuntosVinculados = null;
                coordenacao.Colaboradores = null;

                ViewData["Coordenacao"] = coordenacao;

                return View(colaboradoresAtivos);
            }
            catch (Exception exc)
            {                
                throw exc;
            }
        }

        public void VincularColaboradores(int ideCoordenacao, List<int> idsColaboradoresParaVincular, List<int> idsColaboradoresParaDesvincular)
        {
            try
            {
                AppBase.CoordenacaoAP.ConfigurarVinculosDeColaboradoresLog(ideCoordenacao, idsColaboradoresParaVincular, idsColaboradoresParaDesvincular);

                Session["Mensagem"] = new .Infra.Core.Message.Message
                {
                    Text = Core.Mensagem.Texto.MSG001,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Success
                };

            }
            catch (Exception exc)
            {
                string msgErro = exc.InnerException != null ? exc.Message : exc.Message + " - " + exc.InnerException.Message;

                Session["Mensagem"] = new .Infra.Core.Message.Message
                {
                    Text = msgErro,
                    TypeMessage = .Infra.Core.Message.TypeMessage.Error
                };
            }
        }
    }
}
