using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractsAPI.Models
{
    public class Contract
    {
        [Key]
        public int ID { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime ContractExpiryDate { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Service")]
        public int ServiceID { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }


    }
}
