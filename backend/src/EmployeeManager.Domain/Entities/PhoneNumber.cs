namespace EmployeeManager.Domain.Entities
{
    public class PhoneNumber
    {
        public PhoneNumber() { }
        public PhoneNumber(string number, bool isActive)
        {
            // Adicionar validação do número se necessário
            if (string.IsNullOrEmpty(number))
                throw new ArgumentException("Número não pode ser vazio.");

            Number = number;
            IsActive = isActive;
        }

        public int Id { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; } = true;
    }
}