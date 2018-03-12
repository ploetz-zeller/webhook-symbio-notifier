using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SymbioNotifier.Models
{
    public class IndexModel
    {
        public bool IsSubscribed { get; set; }

        public List<string> Messages { get; } = new List<string>();
    }
}
