using System;
using System.Linq;

namespace FI.AtividadeEntrevista.Helper
{
    public class StringFormatter
    {
        public static string FormatarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                throw new ArgumentException("CPF inválido. Deve conter exatamente 11 dígitos numéricos.");
            }

            return string.Format(@"{0:000\.000\.000\-00}", Convert.ToInt64(cpf));
        }

        public static string RemoverFormatacaoCPF(string cpfFormatado)
        {
            if (string.IsNullOrEmpty(cpfFormatado))
            {
                throw new ArgumentException("CPF formatado não pode ser nulo ou vazio.");
            }

            return new string(cpfFormatado.Where(char.IsDigit).ToArray());
        }
    }
}
