using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PecMemberSearch.Common
{
    public class CommonFunctions
    {
        public static string GetIPAddress()
        {
            var strHostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] address = ipHostEntry.AddressList;
            return address[1].ToString();
        }
    }
}
