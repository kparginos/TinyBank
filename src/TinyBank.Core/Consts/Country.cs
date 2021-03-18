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
        public const string CyprusCountryCode = "GR";
        public const string ItalyCountryCode = "GR";

        public static readonly IReadOnlyCollection<string> SupportedCountryCodes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                GreekCountryCode,
                CyprusCountryCode,
                ItalyCountryCode
            };
    }
}
