namespace ContractsAPI.Models.NewFolder
{
    public class CustomerContractDTO
    {
        public string CustomerName { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime ContractExpiryDate { get; set; }
        public string ServiceType { get; set; }

    }
}
