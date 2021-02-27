using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Npoi.Mapper;

using TinyBank.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Results;
using TinyBank.Core.Consts;

namespace TinyBank.Core.Services
{
    public class CustomerFileService : IFileParser
    {
        //private readonly TinyBankDBContext _dbContext;
        private readonly ICustomerService _customer;

        public CustomerFileService(/*TinyBankDBContext dbContext,*/
            ICustomerService customer)
        {
            //_dbContext = dbContext;
            _customer = customer;
        }
        public Result<bool> ExportCustomerAccountsToFile(string exportPath, int customerID)
        {
            if (string.IsNullOrWhiteSpace(exportPath))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"File {exportPath} is empty or null !",
                    Data = false
                };
            }

            if (File.Exists(exportPath))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"File {exportPath} already exists !",
                    Data = false
                };
            }
            else
            {
                var customerResult = _customer.GetCustomerAccounts(customerID);

                if(customerResult.Code == ResultCodes.Success)
                {
                    var mapper = new Mapper();

                    mapper.Save<CustomerAccounts_V>(exportPath, customerResult.Data, "Customer Accounts");

                    return new Result<bool>()
                    {
                        Code = ResultCodes.Success,
                        Message = $"{customerResult.Data.Count} row(s) saved",
                        Data = true
                    };
                }
                else
                {
                    return new Result<bool>()
                    {
                        Code = customerResult.Code,
                        Message = customerResult.Message,
                        Data = false
                    };
                }
            }
        }
        public Result<bool> ExportCustomersToFile(string exportPath)
        {
            if(string.IsNullOrWhiteSpace(exportPath))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"File {exportPath} is empty or null !",
                    Data = false
                };
            }

            if (File.Exists(exportPath))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"File {exportPath} already exists !",
                    Data = false
                };
            }
            else
            {
                var result = _customer.GetAllCustomers();

                if(result != null)
                {
                    var mapper = new Mapper();

                    mapper.Format<Customer>("dd/MM/yyyy hh:mm:ss.00", d => d.Created);
                    mapper.Save<Customer>(exportPath, result.Data, "Customer Data");

                    return new Result<bool>()
                    {
                        Code = ResultCodes.Success,
                        Message = $"{result.Data.Count} row(s) found",
                        Data = true
                    };
                }
                else
                {
                    return new Result<bool>()
                    {
                        Code = ResultCodes.BadRequest,
                        Message = $"Could not retreive customer list",
                        Data = false
                    };
                }
            }
        }

        public Result<List<CustomerFile>> LoadCustFile(string path)
        {
            if(File.Exists(path))
            {
                var mapper = new Mapper(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
                mapper.Map<CustomerFile>(3, $"{nameof(CustomerFile.TotalGross)}",
                    (columnInfo, data) =>
                    {
                        var gross = decimal.Parse(columnInfo.CurrentValue.ToString(),
                            CultureInfo.InvariantCulture);

                        (data as CustomerFile).TotalGross = gross;

                        return true;
                    });

                var result = mapper.Take<CustomerFile>("sheet1")
                    .Select(row => row.Value)
                    .ToList();
                return new Result<List<CustomerFile>>() 
                {
                    Code = ResultCodes.Success,
                    Message = $"{result.Count} row(s) loaded",
                    Data = result
                };
            }
            else
            {
                return new Result<List<CustomerFile>>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"File {path} does not exist !"
                };
            }
        }
    }
}
