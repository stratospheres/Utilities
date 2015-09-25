using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerArgs;

namespace DirDump
{
    public class Arguments
    {
        [ArgPosition(0)] // default arg...
        public string RootDirectory { get; set; }
    }
}
