using System.ComponentModel.DataAnnotations;

namespace ContractsAPI.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Contract> Contract { get; set; }
    }
}
