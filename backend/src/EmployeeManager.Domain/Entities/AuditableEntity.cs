namespace EmployeeManager.Domain.Entities
{
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Usuário que criou o registro
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Data/hora de criação
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Último usuário que alterou
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// Data da última alteração
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }
    }
}
