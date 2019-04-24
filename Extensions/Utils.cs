using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestingFramework.Models;

namespace TestingFramework.Extensions
{
    public class Utils
    {
        public static Guid GetUserID(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static bool ValidateGuid(Guid guid)
        {
            if (guid == default(Guid))
            {
                return false;
            }

            return true;
        }

        public static string GetPercentageString(double value, double total, int places = 2)
        {
            try
            {
                var percentage = Math.Round((value / total), places);

                if (percentage < 1)
                {
                    var percentageString = percentage.ToString().Split('.')[1];
                    if (percentageString.Length == 1)
                    {
                        percentageString += "0";
                    }

                    return $"{percentageString}%";
                }
                else
                {
                    return "100%";
                }
            }
            catch
            {
                return "0%";
            }
        }
    }
}
