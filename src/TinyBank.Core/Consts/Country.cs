using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBank.Core.Consts
{
    public static class Country
    {
        public const string GreekCountryCode = "GR";
        public const string CyprusCountryCode = "CY";
        public const string ItalyCountryCode = "IT";

        //public static readonly IReadOnlyCollection<string> SupportedCountryCodes =
        //    new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        //    {
        //        GreekCountryCode,
        //        CyprusCountryCode,
        //        ItalyCountryCode
        //    };
        public static readonly IReadOnlyDictionary<string, int> VatValidNumbers =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                {GreekCountryCode, 9 },
                {CyprusCountryCode, 11 },
                {ItalyCountryCode, 10 }
            };
    }
}
