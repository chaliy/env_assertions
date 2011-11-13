namespace EnvAssertions.Utilities
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Management;
    using System.Linq;
    using System;

    public static class Wmi
    {        
        public static IEnumerable<dynamic> SearchDefaultPath(string serverName, string query)
        {
            var searcher = new ManagementObjectSearcher(@"\\" + serverName + @"\root\cimv2", query);

            foreach (ManagementObject managementObj in searcher.Get())
            {

                var exp = (IDictionary<string, object>)new ExpandoObject();
                foreach (var item in managementObj.Properties)
                {
                    exp[item.Name] = item.Value;
                }

                yield return exp;
            }
        }

    }    
}
