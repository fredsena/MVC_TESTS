
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Controllers
{
    public class UsuarioController : CustomController
    {
        public IUsuario _usuario { get; set; }
        public IUsuarioExternoHistorico _usuarioExterno { get; set; }
        public UsuarioController(IUsuario usuario, IUsuarioExternoHistorico usuarioExternoHistorico)
        {
            _usuario = usuario;
            _usuarioExterno = usuarioExternoHistorico;
        }

        
        public ActionResult Pesquisar()
        {
            int? idSistema = 0;
            int? idUorg = 0;
            var idTipoUsuario = "";
            var IdcBloqueado = "";

            ParamPesq pesq = BuscarPesquisa();
            if ((pesq != null) && (pesq.entity != null))
            {
                idSistema = ((Domain.ViewModels.Usuario)pesq.entity).IdeSistema;
                idUorg = ((Domain.ViewModels.Usuario)pesq.entity).IdeUnidadeOrganizacional;
                idTipoUsuario = ((Domain.ViewModels.Usuario)pesq.entity).IdcTipoUsuario;
                IdcBloqueado = ((Domain.ViewModels.Usuario)pesq.entity).IdcBloqueado;
                ViewBag.IdeInstituicao = ((Domain.ViewModels.Usuario)pesq.entity).IdeInstituicao;

            }

            ViewBag.IdcTipoUsuario = new SelectList(Entities.Utilidades.Utilidades.getTipoUsuario(), "Key", "Value", idTipoUsuario);
            ViewBag.Sistemas = new SelectList(_usuario.BuscarTodosSistemas().Select(x => new { IdeSistema = x.IdeSistema, NomSistema = x.SigSistema + " - " + x.NomSistema }), "IdeSistema", "NomSistema", idSistema);
            ViewBag.unidadeOrganizacional = new SelectList(_usuario.BuscarTodos(), "IdePessoaMD", "SigPessoa", idUorg);

            if (pesq != null)
            {
                return View((Domain.ViewModels.Usuario)pesq.entity);
            }
            return View();
        }

        
        
        public ActionResult Visualizar(int id)
        {
            var usuario = _usuario.GetById(id);
            return View(usuario);
        }

        [HttpPost]
        
        
        public JsonResult ExportarPesquisa(Usuario form)
        {
            var rel = _usuario.GetAll(form);
            List<string> listaCampos = new List<string>(
                new string[] { "DscLogin", "NomUsuario", "IdcTipoUsuario", "NomeInstituicao", "DtUltimoAcesso", "IdcBloqueado" }
            );

            foreach (var item in rel)
            {
                item.NomeInstituicao = item.IdeInstituicao == 1 ? item.NomeInstituicao + "/" + item.NomeUnidadeOrganizacional : item.NomeInstituicao;
                item.DtUltimoAcesso = item.DtUltimoAcesso;
                item.IdcBloqueado = item.IdcBloqueado == "S" ? "Bloqueado" : "Desbloqueado";
                item.IdcExcluido = item.IdcExcluido == "S" ? "Sim" : "Não";
                item.IdcTipoUsuario = Entities.Utilidades.Utilidades.getTipoUsuario().FirstOrDefault(t => t.Key == item.IdcTipoUsuario).Value;
            }

            TempData["Titulo"] = "Pesquisar Usuário";

            GerarDadosRelatorio<Usuario>(rel, listaCampos);

            return Json(new { ret = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        
        
        public JsonResult Pesquisar(DatatableParm parm, Domain.ViewModels.Usuario item)
        {
            if (item.IdeSistema != null || item.DscLogin != null || item.IdcTipoUsuario != null || item.IdeInstituicao.HasValue || item.NomeUnidadeOrganizacional != null || item.IdcBloqueado != null)
            {
                SalvarPesquisa(item, parm);
                int totalRecords = 0;
                var items = _usuario.GetAllByPage(item, ref totalRecords, parm.iDisplayStart, parm.iDisplayLength, parm.GetOrderByText(), parm.sSortDir_0);

                if (items.Count() == 0)
                {
                    SalvarMensagem(new Mensagem(Descricoes, Tipo.Alerta));
                    LimparPaginacao();
                }
                return Json(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = items.Count(),
                    iTotalDisplayRecords = totalRecords,
                    aaData = items.Select(x => new
                    {
                        IdeUsuario = x.IdeUsuario,
                        NomUsuario = x.NomUsuario,
                        DscLogin = x.DscLogin,
                        NomeInstituicao = x.IdeInstituicao == 1 ? x.NomeInstituicao + "/" + x.NomeUnidadeOrganizacional : x.NomeInstituicao,
                        IdeInstituicao = x.IdeInstituicao,
                        IdcTipoUsuario = x.IdcTipoUsuario,
                        IdeUnidadeOrganizacional = x.IdeUnidadeOrganizacional,
                        IdcAtivo = x.IdcAtivo,
                        IdcBloqueado = x.IdcBloqueado,
                        IdcExcluido = x.IdcExcluido,
                        DtUltimoAcesso = x.DtUltimoAcesso.HasValue ? x.DtUltimoAcesso.Value.ToString() : string.Empty,
                        DscTipoUsuario = Entities.Utilidades.Utilidades.getTipoUsuario().FirstOrDefault(t => t.Key == x.IdcTipoUsuario).Value
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SalvarMensagem(new Mensagem(Descricoes.018, Tipo.Alerta));
                return Json(new
                {
                    sEcho = parm.sEcho,
                    iTotalRecords = item,
                    iTotalDisplayRecords = 0,
                    aaData = item
                }, JsonRequestBehavior.AllowGet);
            }
        }

        
        public ActionResult Incluir()
        {
            ViewBag.Sistemas = new SelectList(_usuario.BuscarTodosSistemas().Select(x => new { IdeSistema = x.IdeSistema, NomSistema = x.SigSistema + " - " + x.NomSistema }), "IdeSistema", "NomSistema");
            return View();
        }

        [HttpPost]
        
        
        public ActionResult Incluir(Domain.ViewModels.Usuario model)
        {
            ViewBag.Sistemas = new SelectList(_usuario.BuscarTodosSistemas().Select(x => new { IdeSistema = x.IdeSistema, NomSistema = x.SigSistema + " - " + x.NomSistema }), "IdeSistema", "NomSistema");

            if (ModelState.IsValid)
            {
                var msg = ValidarSenhaForte(model.DscSenha);
                if (string.IsNullOrEmpty(msg))
                {
                    _usuario.SaveVerify(model);
                    model.IdeUsuario = _usuario.Save(model, true);
                    SalvarMensagem(new Mensagem(Descricoes, Tipo.Sucesso));
                    this.LimparPaginacao();
                    return RedirectToAction("Pesquisar");
                }
                else
                {
                    SalvarMensagem(new Mensagem(msg, Tipo.Alerta));
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        
        public ActionResult Alterar(int id)
        {
            ViewBag.Sistemas = new SelectList(_usuario.BuscarTodosSistemas(), "IdeSistema", "NomSistema");
            var usuario = _usuario.GetById(id);
            return View(usuario);
        }

        [HttpPost]
        
        
        public ActionResult Alterar(Domain.ViewModels.Usuario model)
        {
            model.perfils = new List<Perfil>();
            var msg = string.Empty;

            #region valida perfil
            if (model.IdcTipoUsuario != "SI" && model.IdcTipoUsuario != null)
            {
                for (int i = 0; i < Request.Form["idePerfil"].Split(',').Length; i++)
                {
                    var idePerfil = Request.Form["IdePerfil"].Split(',')[i];
                    var ideSistema = Request.Form["p_IdeSistema"].Split(',')[i];
                    var DatInicio = Request.Form["DatInicio"].Split(',')[i];
                    var DatFim = Request.Form["DatFim"].Split(',')[i];
                    var principal = Request.Form["principal" + ideSistema];


                    if (string.IsNullOrEmpty(DatInicio))
                    {
                        msg = (string.Concat(Descricoes, "Data Inicial"));
                        break;
                    }
                    if (!string.IsNullOrEmpty(DatFim))
                    {
                        if (DateTime.Parse(DatInicio) > DateTime.Parse(DatFim))
                        {
                            msg = (Descricoes);
                            break;
                        }
                    }
                    model.perfils.Add(new Perfil()
                    {
                        IdePerfil = int.Parse(idePerfil),
                        IdeSistema = int.Parse(ideSistema),
                        DatInicio = DateTime.Parse(DatInicio),
                        DatFim = !string.IsNullOrEmpty(DatFim) ? DateTime.Parse(DatFim) : (DateTime?)null,
                        IdcPrincipal = principal == idePerfil ? "S" : "N"
                    });


                }
                if (!string.IsNullOrEmpty(msg))
                {
                    SalvarMensagem(new Mensagem(msg, Tipo.Erro));
                    return RedirectToAction("Alterar", new { id = model.IdeUsuario });
                }
            }
            #endregion

            if (model.DscSenha != null || model.confirmSenha != null)
            {
                if (model.DscSenha != model.confirmSenha)
                {
                    SalvarMensagem(new Mensagem(Descricoes.001, Tipo.Alerta));
                    return View(model);
                }

                msg = ValidarSenhaForte(model.DscSenha);
                if (!string.IsNullOrEmpty(msg))
                {
                    SalvarMensagem(new Mensagem(msg, Tipo.Alerta));
                    return View(model);
                }
            }
            _usuario.Save(model, false);
            SalvarMensagem(new Mensagem(Descricoes, Tipo.Sucesso));
            return RedirectToAction("Pesquisar");
        }

        public ActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EsqueceuSenha(Domain.ViewModels.Usuario model)
        {
            var usuario = _usuario.GetUsuarioByEmail(model.dscEmail);
            var ip = ClientIP.GetClientIP(HttpContext.Request);
            if (usuario != null)
            {
                var token = _usuario.GerarTokenEmail(ip, usuario.DscLogin);
                var url = ConfigurationManager.AppSettings["UrlPadrao"].ToString();
                string linkAlterarSenha = url + "Usuario/RecuperarSenha?token=" + token.NumToken;

                _usuario.SendEmail();
                SalvarMensagem(new Mensagem(Descricoes.020, Tipo.Alerta));
            }
            else
                SalvarMensagem(new Mensagem(Descricoes.014, Tipo.Alerta));

            return View();
        }

        public ActionResult RecuperarSenha(string token)
        {
            var tokenSistema = (HttpContext.Application["tokenSistema"]);
            var usuarioToken = _usuario.GetUsuarioByToken(token, tokenSistema.NumToken);
            var expirar = _usuario.GerarTokenEmail("", usuarioToken.DscLogin);

            //Redireciona e desbloqueia o usuario caso o mesmo seja do tipo IA ou IC
            if (usuarioToken.IdcTipoUsuario == "IC" || usuarioToken.IdcTipoUsuario == "IA")
            {
                usuarioToken.IdcBloqueado = "N";
                _usuario.Save(usuarioToken, false);
                SalvarMensagem(new Mensagem(Descricoes, Tipo.Sucesso));
                return RedirectToAction("Account", "Login");
            }

            if (usuarioToken != null)
            {
                _usuario.ExpirarToken(expirar);
                return View(usuarioToken);
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(Usuario model)
        {
            ModelState.Remove("IdeSistema");

            if (ModelState.IsValid)
            {
                model.IdcBloqueado = "N";
                Entities.HisSenhaUsuarioExterno usuarioExterno = new Entities.HisSenhaUsuarioExterno();
                usuarioExterno.IdeUsuario = model.IdeUsuario;

                string getMensagemValidacao = ValidarSenhaForte(model.DscSenha);
                string validarUsuarioExterno = EhUsuarioExterno(model.IdeUsuario) ? string.Empty : Descricoes.002;
                if (string.IsNullOrEmpty(getMensagemValidacao) && string.IsNullOrEmpty(validarUsuarioExterno))
                {
                    usuarioExterno.DscSenha = Crypto.getMD5Hash(model.DscSenha);
                    if (_usuarioExterno.Save(usuarioExterno))
                    {
                        _usuario.Save(model, false);

                        SalvarMensagem(new Mensagem(Descricoes.021, Tipo.Sucesso));
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        SalvarMensagem(new Mensagem(Descricoes.023, Tipo.Alerta));
                        return View();
                    }
                }
                else
                {
                    SalvarMensagem(new Mensagem(!string.IsNullOrEmpty(getMensagemValidacao) ? getMensagemValidacao : validarUsuarioExterno, Tipo.Erro));
                    return View();
                }
            }

            return View(model);

        }
        private bool EhUsuarioExterno(int user)
        {
            if (user != 0)
                if (_usuario.GetById(user).IdcTipoUsuario == "EX")
                    return true;
                else
                    return false;
            return false;
        }
        /// <summary>
        /// Função que verifica se a string informada “Tes123@#$” will be accepted.
        /// UMA LETRA MINUSCULA
        /// UMA LETRA MAIUSCULA
        /// UM NUMERO
        /// UM ESPECIAL
        /// NO MINIMO 8 CARACTERES
        /// </summary>
        /// <param name=”password”></param>
        /// <returns></returns>
        public static string ValidarSenhaForte(string password)
        {
            int tamanhoMinimo = 8;
            int tamanhoMinusculo = 1;
            int tamanhoMaiusculo = 1;
            int tamanhoNumeros = 1;
            int tamanhoCaracteresEspeciais = 1;

            // Definição de letras minusculas
            Regex regTamanhoMinusculo = new Regex("[a-z]");

            // Definição de letras minusculas
            Regex regTamanhoMaiusculo = new Regex("[A-Z]");

            // Definição de letras minusculas
            Regex regTamanhoNumeros = new Regex("[0-9]");

            // Definição de letras minusculas
            Regex regCaracteresEspeciais = new Regex("[^a-zA-Z0-9]");

            // Verificando tamanho minimo
            if (password.Length < tamanhoMinimo)
                return Descricoes.003;

            // Verificando caracteres minusculos
            if (regTamanhoMinusculo.Matches(password).Count < tamanhoMinusculo)
                return Descricoes.004;

            // Verificando caracteres maiusculos
            if (regTamanhoMaiusculo.Matches(password).Count < tamanhoMaiusculo)
                return Descricoes.005;

            // Verificando numeros
            if (regTamanhoNumeros.Matches(password).Count < tamanhoNumeros)
                return Descricoes.006;

            // Verificando os diferentes
            if (regCaracteresEspeciais.Matches(password).Count < tamanhoCaracteresEspeciais)
                return Descricoes.007;

            return string.Empty;
        }

        
        public ActionResult Delete(int id)
        {
            return View();
        }

        
        
        public JsonResult Excluir(Usuario model, int id)
        {
            var p = _usuario.GetById(id);
            model.UpdateValues(p);
            _usuario.Delete(id);
            LimparPaginacao();
            SalvarMensagem(new Mensagem(Descricoes, Tipo.Sucesso));
            return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificarExclusao()
        {
            var msg = new Mensagem(Descricoes, Tipo.Confirmacao);
            return Json(new { Descricao = msg.Descricao, Tipo = msg.Tipo.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}
