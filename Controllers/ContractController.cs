using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContractsAPI.Models;

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


        //Select Customers with Pagination
        //[HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(int pagesize = 12, int pagenumber = 1)
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            customers = customers.Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToList();
            return customers;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
          if (_context.Contracts == null)
          {
              return NotFound();
          }
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        
        //private bool ContractExists(int id)
        //{
        //   return (_context.Contracts?.Any(e => e.ID == id)).GetValueOrDefault();
        //}
    }
}
