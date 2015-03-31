$(document).ready(function () {
    //Funcionalidade carregadas no momento de abertura da pagina
    inciarPagina();
    //Limpa os valores do formulário e aplica os valores padrão
    LimparFormulario();
});

function inciarPagina() {
    //Limpa as Validações do Formulário
    //LimparValidacoes();
    //Botão que grava dados do formulário
    $("#btnGravar").click(function () {
        GravarAluno();
    });
    //Botão que realia consulta dos dados 
    $("#btnConsultar").click(function () {
        //LimparValidacoes();
        ConsultarAluno();
    });

    //Botão que grava dados do formulário
    $("#btnLimpar").click(function () {
        LimparFormulario();
    });

    //Botão que grava dados do formulário (Modal)
    $("#btnAlterar").click(function () {
        AlterarAluno();
    });

    //Aplica as mascaras
    RegrasFormulario();

}

function ExcluirAluno(cod_aluno)
{
    var left = (screen.width / 2) - (450 / 2);
    var top = (screen.height / 2) - (450 / 2);

    alertify.confirm("Tem certeza que deseja excluir esse aluno?", function (resultado) {
        if (resultado) {
            //ajax call for delete       
            $.ajax({
                url: '/Aluno/excluirAluno?cod_aluno=' + cod_aluno,
                type: 'post',
                contentType: 'application/json;',
                //data: data.dados,
                async: false,
                success: function (data, status) {
                    if (status == "success") {
                        LimparFormulario();
                        alertify.success("Registro Excluído.");
                    }
                },
                error: function (data) {
                    alertify.error("Atenção! Ocorreu um erro nessa funcionalidade.");
                }
            });

        } else {
            alertify.error("You've clicked cancel");
        }
    }).moveTo(left, top).set('labels', { ok: 'Sim', cancel: 'Não' }).set('defaultFocus', 'cancel');;
}

function ConsultarAluno()
{
    var aluno = JSON.stringify(GetObjetoAluno(0));
    var url = "/Aluno/consultarAluno?objeto=" + aluno + "&view=AlunoGrid";

    $.ajax({
        url: url,
        type: 'POST',
        complete: function (data, status)
        {
            $("#divConsulta").html(data.responseText);
            $("#divConsulta").show();

        }
        
    });
}

function GravarAluno() {
    
    var aluno = GetObjetoAluno(0);
    
    if (ValidarDadosAluno(aluno))
    {
        var url = "/Aluno/gravarAluno?objeto=" + JSON.stringify(aluno);
        $.ajax({
            url: url,
            type: 'POST',
            success: function (data, status) {
                if (status == "success") {
                    alert('Operação realizada com sucesso.');
                    LimparFormulario();
                }
                else {
                    alert('Atenção! Ocorreu um erro nessa funcionalidade.');
                }
            }
        });
    }
}

function VisualizarAluno(cod_aluno) {
    var url = "/Aluno/visualizarAluno?codigo=" + cod_aluno;
    $.ajax({
        url: url,
        type: 'POST',
        success: function (data, status) {
            if (status == "success") {
                $("#divForm").html(data.responseText);
                inciarPagina();

            }
            else {
                alert('Atenção! Ocorreu um erro nessa funcionalidade.');
            }
        }
    });
}

function AlterarAluno() {
    var aluno = GetObjetoAluno($("#cod_aluno").val());
    if (ValidarDadosAluno(aluno)) {
        var url = "/Aluno/alterarAluno?objeto=" + JSON.stringify(aluno);

        sog.carregando = true;
        sog.tipo = 'post';
        sog.complete(
                url,
                function (data, status) {
                    var left = (screen.width / 2) - (450 / 2);
                    var top = (screen.height / 2) - (450 / 2);
                    if (status == "success") {
                        alertify.alert('Operação realizada com sucesso.').moveTo(left, top).set('onok', function () { alertify.success('Sucesso.'); });
                    }
                    else {
                        alertify.alert('Atenção! Ocorreu um erro nessa funcionalidade.').moveTo(left, top).set('onok', function () { alertify.error('Erro.'); });
                    }
                }
            );
    }
}

function ValidarDadosAluno(objeto) {
    var validado = true;
    
    //Validação de Campos Obrigatorios
    if (objeto.nom_aluno == '' || objeto.nom_aluno == undefined)
    {
        $("#nom_aluno").validationEngine('showPrompt', "Campo obrigatório.", 'red', 'topRight', true);
        validado = false;
    }
    if (objeto.num_cpf == '' || objeto.num_cpf == undefined)
    {
        $("#num_cpf").validationEngine('showPrompt', "Campo obrigatório.", 'red', 'topRight', true);
        validado = false;
    }
    if (!validado) {
        //ScrollToInputId();
    }
    return validado;

}

function GetObjetoAluno(id) {
    var aluno = new Object();

    aluno.cod_aluno = id;
    aluno.nom_aluno = $("#nom_aluno").val();
    aluno.num_cpf = $("#num_cpf").val().replace(/\./g, '').replace(/\-/g, '');
    if ($("#uf_cod_uf").val() != "")
    {
        aluno.uf = new Object();
        aluno.uf.cod_uf = $("#uf_cod_uf").val()
    }

    return aluno;
}

function RegrasFormulario() {
    //Mascaras de Campos
    /**************************************************************************************************************/
    //$("#num_cpf").mask("999.999.999-99");
  
    //Permite só letras
    /**************************************************************************************************************/
    //$("#nom_aluno").keyup(HabilitarApenasLetras);
    /**************************************************************************************************************/
}

function LimparFormulario()
{
    $("#btnGravar").show();
    $("#btnConsultar").show();
    $("#btnLimpar").show();
    $("#btnAlterar").hide();

    $("input[type=text]").val('');
    $("select").val('');

    $("input[type=text]").each(function () {
        $(this).validationEngine('hide');
    });
    $("select").each(function () {
        $(this).validationEngine('hide');
    })
    $("input[type=text]").focus(function () {
        $(this).validationEngine('hide');
    });
    $("select").focus(function () {
        $(this).validationEngine('hide');
    })

}
