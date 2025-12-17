function isCpfValid(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');

    if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) {
        toastr.error('Por favor, insira um CPF válido.', 'Atenção');
        return false
    };

    let soma = 0, resto;

    //Primeiro Digito
    for (let i = 1; i <= 9; i++)
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.substring(9, 10))) {
        toastr.error('Por favor, insira um CPF válido.', 'Atenção');
        return false
    };

    //Segundo Digito
    soma = 0;
    for (let i = 1; i <= 10; i++)
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.substring(10, 11))) {
        toastr.error('Por favor, insira um CPF válido.', 'Atenção');
        return false
    };

    return true;
}

const FormatarCPF = (e) => {

    const input = e.currentTarget;

    let valor = input.value;
    valor = valor.replace(/\D/g, "");

    if (valor.length > 11) {
        valor = valor.slice(0, 11);
    }

    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
    valor = valor.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    input.value = valor;
}

function FormatarCPFString(cpfBeneficiario) {
    cpfBeneficiario = cpfBeneficiario.replace(/\D/g, "");

    if (cpfBeneficiario.length > 11) {
        cpfBeneficiario = cpfBeneficiario.slice(0, 11);
    }

    cpfBeneficiario = cpfBeneficiario.replace(/(\d{3})(\d)/, "$1.$2");
    cpfBeneficiario = cpfBeneficiario.replace(/(\d{3})(\d)/, "$1.$2");
    cpfBeneficiario = cpfBeneficiario.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    return cpfBeneficiario;
}

async function BuscarCEP(cep) {
    const url = `https://viacep.com.br/ws/${cep}/json/`;

    try {
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(`Erro de rede: ${response.status}`);
        }

        const dados = await response.json();

        $('#Estado').val(dados.uf);
        $('#Cidade').val(dados.localidade);
        $('#Logradouro').val(dados.logradouro);

        toastr.success('Dados do CEP importados com sucesso!', 'Tudo Certo');

    } catch (error) {
        toastr.error('Ocorreu um erro ao buscar os dados do CEP: ' + error, 'Atenção');
    }
}

$(document).ready(function () {
    $('#CPF').on("input", FormatarCPF);
    $('#CPF').on("change", function (e) {
        let cpf = e.currentTarget.value;
        if (!isCpfValid(cpf)) {
            return;
        }
    });

    $('.only-letters').on("input", function (e) {
        this.value = this.value.replace(/[^a-zA-Záàâãéèêíìóòôõúùûç\s]/g, '');
    });

    $('#CEP').on("change", function (e) {
        const cep = this.value.replace(/\D/g, '');
        if (cep.length === 8) {
            BuscarCEP(cep);
        }
    });
    $('#CEP').on("input", function (e) {
        if (!this.value) return ""
        this.value = this.value.replace(/\D/g, '')
        this.value = this.value.replace(/(\d{5})(\d)/, '$1-$2')
    });

    $('#Telefone').on("input", function (e) {
        if (!this.value) return ""
        this.value = this.value.replace(/\D/g, '')
        this.value = this.value.replace(/(\d{2})(\d)/, '($1) $2')
        this.value = this.value.replace(/(\d{5})(\d)/, '$1-$2')
        if (this.value.length > 15) {
            this.value = this.value.slice(0, 15);
        }
    });
});