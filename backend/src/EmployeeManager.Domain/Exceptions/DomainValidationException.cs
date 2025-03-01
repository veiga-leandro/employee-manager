namespace EmployeeManager.Domain.Exceptions
{
    public class DomainValidationException(string message) : Exception(message)
    {
        public static void When(bool condition, string message)
        {
            if (condition)
                throw new DomainValidationException(message);
        }
    }
}
