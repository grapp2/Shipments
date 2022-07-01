using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.Utility
{
    internal static class Functions
    {
        public static string GetInitials(string input)
        {
            string initials = "";
            var a = input.Split(' ');
            foreach(var s in a)
            {
                initials += s[0];
            }
            return initials;
        }
    }
}
