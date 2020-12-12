using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api.Configuration
{
    public class StartupConfiguration
    { 
        public string AppSourceName { get; set; } 

        public string LocalPort { get; set; } = "10100";

        public List<DatabaseConfiguration> DatabasesConfiguration { get; set; }
    }

   
}
