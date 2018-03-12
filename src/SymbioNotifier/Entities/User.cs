using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SymbioNotifier.Entities
{
    public class User
    {
        public bool IsSubscribed { get; set; }

        public List<string> CollectedMessages { get; } = new List<string>();
    }
}
