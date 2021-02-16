using System.IO;
using System.Collections.Generic;
using System.Linq;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Interfaces;

using NPOI;
using Npoi.Mapper;
using System.Threading.Tasks;
using TinyBank.Core.Services.Results;
using TinyBank.Core.Consts;
using System.Globalization;

namespace TinyBank.Core.Services
{
    public class CustomerFileService : IFileParser
    {
        public Result<List<CustomerFile>> LoadCustFile(string path)
        {
            if(File.Exists(path))
            {
                var mapper = new Mapper(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, path));
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
