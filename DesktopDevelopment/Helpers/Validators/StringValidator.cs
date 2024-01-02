using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopDevelopment.Helpers.Validators
{
    public static class StringValidator 
    {
        public static bool ContainsOnlyNumbers(string text)
        {
          return Regex.IsMatch(text, @"^[0-9]+$");
        }
    }
}
