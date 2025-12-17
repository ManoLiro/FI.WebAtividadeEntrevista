$(document).ready(function () {
    $('#Beneficiarios').on("click", ModalBeneficiarios);
});

var beneficiarios = [];

if (typeof obj !== 'undefined' && obj && obj.Beneficiarios) {
    beneficiarios = JSON.parse(obj.Beneficiarios);
}

function ModalBeneficiarios() {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                                                                                                               ' +
        '        <div class="modal-dialog">                                                                                                                                                                 ' +
        '            <div class="modal-content">                                                                                                                                                            ' +
        '                <div class="modal-header">                                                                                                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>                                                                                         ' +
        '                    <h4 class="modal-title">Beneficiários</h4>                                                                                                                                     ' +
        '                </div>                                                                                                                                                                             ' +
        '                <div class="modal-body">                                                                                                                                                           ' +
        '                    <form id="formBeneficiarios" method="post">                                                                                                                                    ' +
        '                       <div class="row">                                                                                                                                                           ' +
        '                           <div class="col-md-4">                                                                                                                                                  ' +
        '                               <div class="form-group">                                                                                                                                            ' +
        '                                   <label for="CPFBeneficiario">CPF:</label>                                                                                                                       ' +
        '                                   <input required="required" type="text" class="form-control" id="CPFBeneficiario" name="CPFBeneficiario" placeholder="Ex.: 010.011.111-00" maxlength="14">       ' +
        '                               </div>                                                                                                                                                              ' +
        '                           </div>                                                                                                                                                                  ' +
        '                           <div class="col-md-6">                                                                                                                                                  ' +
        '                               <div class="form-group">                                                                                                                                            ' +
        '                                   <label for="NomeBeneficiario">Nome:</label>                                                                                                                     ' +
        '                                   <input required="required" type="text" class="form-control" id="NomeBeneficiario" name="NomeBeneficiario" placeholder="Ex.: Maria" maxlength="100">             ' +
        '                               </div>                                                                                                                                                              ' +
        '                           </div>                                                                                                                                                                  ' +
        '                           <div class="col-md-2">                                                                                                                                                  ' +
        '                               <div>                                                                                                                                                               ' +
        '                                   <button style="margin-top:25px" type="button" id="btnIncluirBeneficiario" class="btn btn-sm btn-success">Incluir</button>                                       ' +
        '                               </div>                                                                                                                                                              ' +
        '                           </div>                                                                                                                                                                  ' +
        '                       </div>                                                                                                                                                                      ' +
        '                    </form>                                                                                                                                                                        ' +
        '                    <div class="row" style="margin-top: 20px;">                                                                                                                                    ' +
        '                       <div class="col-md-12">                                                                                                                                                     ' +
        '                           <table class="table table-striped" id="tabelaBeneficiarios">                                                                                                            ' +
        '                               <thead>                                                                                                                                                             ' +
        '                                   <tr>                                                                                                                                                            ' +
        '                                       <th>CPF</th>                                                                                                                                                ' +
        '                                       <th>Nome</th>                                                                                                                                               ' +
        '                                       <th>Ações</th>                                                                                                                                              ' +
        '                                   </tr>                                                                                                                                                           ' +
        '                               </thead>                                                                                                                                                            ' +
        '                               <tbody id="listaBeneficiarios">                                                                                                                                     ' +
        '                               </tbody>                                                                                                                                                            ' +
        '                           </table>                                                                                                                                                                ' +
        '                       </div>                                                                                                                                                                      ' +
        '                    </div>                                                                                                                                                                         ' +
        '                </div>                                                                                                                                                                             ' +
        '                <div class="modal-footer">                                                                                                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>                                                                                             ' +
        '                </div>                                                                                                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                                                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                                                                                                            ';

    $('body').append(texto);
    var $modal = $('#' + random);

    $modal.find('#CPFBeneficiario').on('input', FormatarCPF);

    $('#CPFBeneficiario').on("change", function (e) {
        let cpf = e.currentTarget.value;
        if (!isCpfValid(cpf)) {
            return;
        }
    });

    $('#NomeBeneficiario').on("input", function (e) {
        this.value = this.value.replace(/[^a-zA-Záàâãéèêíìóòôõúùûç\s]/g, '');
    });

    $modal.find('#btnIncluirBeneficiario').on('click', function () {
        var cpf = $modal.find('#CPFBeneficiario').val();
        var nome = $modal.find('#NomeBeneficiario').val();

        if (cpf && nome) {
            if (cpf.length < 14) {
                $modal.find('#CPFBeneficiario').focus();
                $modal.find('#CPFBeneficiario').css("border-color", "red");
                toastr.error('Por favor, insira um CPF válido.', 'Atenção');
                return;
            }

            var cpfExists = beneficiarios.some(function (beneficiario) {
                return beneficiario.CPF === cpf;
            });

            if (cpfExists) {

                toastr.error('O CPF já está cadastrado na lista de beneficiários.', 'Atenção');
                return;
            }

            if (!isCpfValid(cpf)) {
                return;
            }

            beneficiarios.push({
                CPF: cpf,
                Nome: nome
            });

            AtualizarTabelaBeneficiarios($modal);

            $modal.find('#CPFBeneficiario').val('');
            $modal.find('#NomeBeneficiario').val('');
        }
        else {
            if (!cpf && nome) {
                $modal.find('#CPFBeneficiario').focus();
                $modal.find('#CPFBeneficiario').css("border-color", "red");
                $modal.find('#NomeBeneficiario').css("border-color", "green");
                toastr.error('Por favor, preencha o CPF do beneficiário.', 'Atenção');
            } else if (!nome && cpf) {
                $modal.find('#NomeBeneficiario').focus();
                $modal.find('#CPFBeneficiario').css("border-color", "green");
                $modal.find('#NomeBeneficiario').css("border-color", "red");
                toastr.error('Por favor, preencha o Nome do beneficiário.', 'Atenção');
            } else {
                $modal.find('#CPFBeneficiario').css("border-color", "red");
                $modal.find('#NomeBeneficiario').css("border-color", "red");
                toastr.error('Por favor, preencha o CPF e Nome do beneficiário.', 'Atenção');
            }
        }
    });

    AtualizarTabelaBeneficiarios($modal);

    $modal.on('hidden.bs.modal', function () {
        $(this).remove();
    });

    $modal.modal('show');
}

function AtualizarTabelaBeneficiarios($modal) {
    var $tbody = $modal.find('#listaBeneficiarios');
    $tbody.empty();

    beneficiarios.forEach(function (beneficiario, index) {
        var row =
            '<tr data-index="' + index + '">' +
            '<td class="cpf-cell">' + FormatarCPFString(beneficiario.CPF) + '</td>' +
            '<td class="nome-cell">' + beneficiario.Nome + '</td>' +
            '<td>' +
            '<button type="button" class="btn btn-sm btn-primary btn-alterar" data-index="' + index + '">Alterar</button> ' +
            '<button type="button" class="btn btn-sm btn-danger btn-excluir" data-index="' + index + '">Excluir</button>' +
            '</td>' +
            '</tr>';
        $tbody.append(row);
    });

    $tbody.find('.btn-excluir').on('click', function () {
        var index = $(this).data('index');
        beneficiarios.splice(index, 1);
        AtualizarTabelaBeneficiarios($modal);
    });

    $tbody.find('.btn-alterar').on('click', function () {
        var $btn = $(this);
        var index = $btn.data('index');
        var $row = $btn.closest('tr');
        var beneficiario = beneficiarios[index];

        // Transforma em modo edição
        $row.find('.cpf-cell').html(
            '<input type="text" class="form-control input-sm" id="edit-cpf-' + index + '" value="' + FormatarCPFString(beneficiario.CPF) + '" maxlength="14">'
        );
        $row.find('.nome-cell').html(
            '<input type="text" class="form-control input-sm" id="edit-nome-' + index + '" value="' + beneficiario.Nome + '" maxlength="100">'
        );

        // Aplica máscara de CPF no campo de edição
        $row.find('#edit-cpf-' + index).on('input', FormatarCPF);

        // Troca botões
        $btn.removeClass('btn-primary btn-alterar')
            .addClass('btn-success btn-salvar')
            .text('Salvar')
            .off('click')
            .on('click', function () {
                SalvarAlteracaoBeneficiario($modal, index, $row);
            });

        $btn.next('.btn-excluir')
            .removeClass('btn-danger btn-excluir')
            .addClass('btn-warning btn-cancelar')
            .text('Cancelar')
            .off('click')
            .on('click', function () {
                AtualizarTabelaBeneficiarios($modal);
            });
    });
}

function SalvarAlteracaoBeneficiario($modal, index, $row) {
    var novoCpf = $row.find('input[id^="edit-cpf"]').val();
    var novoNome = $row.find('input[id^="edit-nome"]').val();

    // Validações
    if (!novoCpf || !novoNome) {
        toastr.error('Digite o CPF e o Nome do beneficiário.', 'Atenção');
        return;
    }

    if (novoCpf.length != 14) {
        toastr.error('O CPF digitado é inválido.', 'Atenção');
        return;
    }

    if (!isCpfValid(novoCpf)) {
        return;
    }

    if (/[^a-zA-ZÀ-ÿ\s]/.test(novoNome)) {
        toastr.error('O nome do beneficiário deve conter somente letras.', 'Atenção');
        return;
    }

    //Aqui Verifico se CPF já existe (exceto o próprio)
    var cpfExiste = beneficiarios.some(function (beneficiario, idx) {
        return beneficiario.CPF === novoCpf && idx !== index;
    });

    if (cpfExiste) {
        toastr.error('O CPF já está cadastrado na lista de beneficiários.', 'Atenção');
        return;
    }

    beneficiarios[index].CPF = novoCpf;
    beneficiarios[index].Nome = novoNome;

    AtualizarTabelaBeneficiarios($modal);
}