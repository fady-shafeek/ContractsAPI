﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContractsAPI.Models;
using ContractsAPI.Models.NewFolder;
using System.Collections;
using ContractsAPI.Models.DTO;

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
            var contracts = from cntrct in _context.Contracts
                       from client in _context.Customers
                       from srvc in _context.Services
                       where cntrct.CustomerID == client.ID && cntrct.ServiceID == srvc.ID
                       select new
                       {
                           customerName = client.Name,
                           contractDate = cntrct.ContractDate,
                           contractExpiryDate = cntrct.ContractExpiryDate,
                           serviceType = srvc.Type
                       };
            foreach(var contract in contracts)
            {
                var customerContract = new CustomerContractDTO();
                customerContract.ContractDate = contract.contractDate;
                customerContract.ContractExpiryDate = contract.contractExpiryDate;
                customerContract.CustomerName = contract.customerName;
                customerContract.ServiceType = contract.serviceType;
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



        //Get customers counts per service type
        [HttpGet("/CustomerPerService")]
        public async Task<ActionResult<IEnumerable<ServiceUserCountDTO>>> CustomerPerService()
        {
            List<ServiceUserCountDTO> Srv = new List<ServiceUserCountDTO>();
            var services = _context.Services.ToList();
            foreach(Service service in services)
            {
                
                var customerCount = await _context.Contracts.Where(c => c.ServiceID == service.ID).CountAsync(); 
                var rec = new ServiceUserCountDTO();
                rec.Service = service.Type;
                rec.Count = customerCount;
                Srv.Add(rec);
            }

            return Srv;
        }



        //	Get customers count per month per year
        [HttpGet("/CustomerPerMonthPerYear")]
        public async Task<ActionResult<int>> CustomerPerMonthPerYear(int month, int year)
        {
            int customersCount = _context.Contracts.Where(x => x.ContractDate.Month == month && x.ContractDate.Year == year).Count();
            return customersCount;
        }

        //Pie-Chart
        [HttpGet("/ChartPerMonthPerYear")]
        public async Task<ActionResult<IEnumerable<ContractsPieChartDTO>>> GetPieChartData(int year =2020)
        {
            IDictionary<string, int> Months = new Dictionary<string, int>()
            {{"Jan",0},{"Feb",0},{"Mar",0},{"Apr",0},{"May",0},{"Jun",0},{"Jul",0},{"Aug",0},{"Sep",0},{"Oct",0},{"Nov",0},{"Dec",0}};
            List<ContractsPieChartDTO> customerContracts = new List<ContractsPieChartDTO>();
            List<Contract> contracts = await _context.Contracts.Where(c => c.ContractDate.Year == year).ToListAsync();
            foreach (Contract contract in contracts)
            {
                string ff = contract.ContractDate.ToString("MMM");
                Months[contract.ContractDate.ToString("MMM")]++;
            }

            foreach (var item in Months)
            {
                var pieChart = new ContractsPieChartDTO();
                pieChart.MonthName = item.Key;
                pieChart.Count = item.Value;
                customerContracts.Add(pieChart);
            }

            return customerContracts;
        }

    }
}
