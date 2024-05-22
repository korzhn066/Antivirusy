using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirusy
{
    internal class Signatura
    {
        public int Id { get; set; }
        public List<Bytee> Bytees { get; set; } = null!;
    }
}
