using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Labb_3
{
    public abstract class Booking
    {
        public string Name { get; set; }
        public int Table { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
    public class Booker : Booking
    {
        public Booker(string name, int table, string date, string time)
        {
            Name = name;
            Table = table;
            Date = date;
            Time = time;
        }
    }
}
