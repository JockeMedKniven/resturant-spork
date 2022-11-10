using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Labb_3
{
    //Skalskydd för filhanteraren
    internal interface IFileHandler
    {
        public List<Booker> PrintAll();
        public void Save(List<Booker> bookings);
    }
}
