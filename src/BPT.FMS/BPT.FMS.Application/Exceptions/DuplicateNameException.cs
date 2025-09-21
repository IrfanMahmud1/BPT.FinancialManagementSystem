using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.FMS.Application.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException(string name) :
            base($"{name} name can't be duplicate")
        { }
    }
}
