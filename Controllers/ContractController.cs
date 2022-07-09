using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContractsAPI.Models;
using ContractsAPI.Models.NewFolder;
using System.Collections;


namespace ContractsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly ContractContext _context;

        public ContractController(ContractContext context)
        {
            _context = context;
        }





        //Get customers from database with server-side pagination
        [HttpGet("/Contracts")]
        public async Task<ActionResult<IEnumerable<CustomerContractDTO>>> GetContract(int pagesize = 20, int pagenumber = 1)
        {

            List<CustomerContractDTO> customerContracts = new List<CustomerContractDTO>();

            List<Contract> contracts = await _context.Contracts.ToListAsync();
            foreach (Contract contract in contracts)
            {
                var customerContract = new CustomerContractDTO();
                var customer = await _context.Customers.Where(c => c.ID == contract.CustomerID).SingleOrDefaultAsync();
                var service = await _context.Services.Where(s => s.ID == contract.ServiceID).SingleOrDefaultAsync();
                customerContract.ContractDate = contract.ContractDate;
                customerContract.ContractExpiryDate = contract.ContractExpiryDate;
                customerContract.CustomerName = customer.Name;
                customerContract.ServiceType = service.Type;
                customerContracts.Add(customerContract);
            }
            customerContracts = customerContracts.Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToList();

            return customerContracts;

        }

        //Get Customers where their contract is expired
        [HttpGet("/ExpiredContracts")]
        public async Task<ActionResult<IEnumerable<CustomerContractDTO>>> GetExpiredContracts()
        {
            List<CustomerContractDTO> customerContracts = new List<CustomerContractDTO>();

            List<Contract> contracts = await _context.Contracts.Where(y => y.ContractExpiryDate < DateTime.Now).ToListAsync();
            foreach (Contract contract in contracts)
            {
                var customerContract = new CustomerContractDTO();
                var customer = await _context.Customers.Where(c => c.ID == contract.CustomerID).SingleOrDefaultAsync();
                var service = await _context.Services.Where(s => s.ID == contract.ServiceID).SingleOrDefaultAsync();
                customerContract.ContractDate = contract.ContractDate;
                customerContract.ContractExpiryDate = contract.ContractExpiryDate;
                customerContract.CustomerName = customer.Name;
                customerContract.ServiceType = service.Type;
                customerContracts.Add(customerContract);
            }
            return customerContracts;
        }


        //Get Customers where their contact will be expired within 1 month
        [HttpGet("/NextMonthExpiredContracts")]
        public async Task<ActionResult<IEnumerable<CustomerContractDTO>>> GetNextMonthExpiredContracts()
        {
            List<CustomerContractDTO> customerContracts = new List<CustomerContractDTO>();

            List<Contract> contracts = await _context.Contracts.Where(y => y.ContractExpiryDate < DateTime.Now.AddYears(1) && y.ContractExpiryDate > DateTime.Now).ToListAsync();
            foreach (Contract contract in contracts)
            {
                var customerContract = new CustomerContractDTO();
                var customer = await _context.Customers.Where(c => c.ID == contract.CustomerID).SingleOrDefaultAsync();
                var service = await _context.Services.Where(s => s.ID == contract.ServiceID).SingleOrDefaultAsync();
                customerContract.ContractDate = contract.ContractDate;
                customerContract.ContractExpiryDate = contract.ContractExpiryDate;
                customerContract.CustomerName = customer.Name;
                customerContract.ServiceType = service.Type;
                customerContracts.Add(customerContract);
            }
            return customerContracts;
        }



        //Get customers counts according the service type 
        [HttpGet("/CustomerPerService")]
        public async Task<ActionResult<int>> CustomerPerService(int ServiceID)
        {
            int customersCount = _context.Contracts.Where(x => x.ServiceID == ServiceID).Count();
            return customersCount;
        }



        //	Get customers count per month per year
        [HttpGet("/CustomerPerMonthPerYear")]
        public async Task<ActionResult<int>> CustomerPerMonthPerYear(int m, int y)
        {
            int customersCount = _context.Contracts.Where(x => x.ContractDate.Month == m && x.ContractDate.Year == y).Count();
            return customersCount;
        }









        //// GET: api/Contract
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        //{
        //  if (_context.Contracts == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Contracts.ToListAsync();
        //}

        // GET: api/Contract/5
        //    [HttpGet("{id}")]
        //public async Task<ActionResult<Contract>> GetContract(int id)
        //{
        //  if (_context.Contracts == null)
        //  {
        //      return NotFound();
        //  }
        //    var contract = await _context.Contracts.FindAsync(id);

        //    if (contract == null)
        //    {
        //        return NotFound();
        //    }

        //    return contract;
        //}


        //private bool ContractExists(int id)
        //{
        //   return (_context.Contracts?.Any(e => e.ID == id)).GetValueOrDefault();
        //}
    }
}
