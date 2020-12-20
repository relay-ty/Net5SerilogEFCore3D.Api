using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Model.ViewModels
{
   public class SerilogView: ViewModel<int>
    {
        public string Message { get;  set; }
        public string Level { get;  set; }
        public DateTimeOffset TimeStamp { get;  set; }
        public string Exception { get;  set; }
        public string LogEvent { get;  set; }
    }
}
