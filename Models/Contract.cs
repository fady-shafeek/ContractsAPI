using System.ComponentModel.DataAnnotations;

namespace ContractsAPI.Models
{
    public class Contract
    {
        [Key]
        public int ID { get; set; }
        public DateOnly ContractDate { get; set; }
        public DateOnly ContractExpiryDate { get; set; }
        //remove??
        public int ClientID { get; set; }
        public int ServiceID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }


    }
}
