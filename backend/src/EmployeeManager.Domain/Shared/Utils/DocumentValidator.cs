using System.Text.RegularExpressions;

namespace EmployeeManager.Domain.Shared.Utils
{
    public static class DocumentValidator
    {
        public static bool IsCpf(string cpf)
        {
            // Verifica se o CPF é nulo/vazio
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Cálculo dos dígitos verificadores
            int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multipliers1[i];

            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            string digit = remainder.ToString();
            tempCpf += digit;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multipliers2[i];

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            digit += remainder.ToString();

            // Verifica se os dígitos calculados conferem
            return cpf.EndsWith(digit);
        }
    }
}
