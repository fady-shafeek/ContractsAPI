using System.ComponentModel.DataAnnotations;

namespace ContractsAPI.Models
{
    public class Service
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; }

    }
}
