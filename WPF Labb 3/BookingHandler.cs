using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Labb_3
{
    internal class BookingHandler
    {
        public List<Booker> Bookers { get; set; }
        public FileHandler fileHandler { get; set; }
        public BookingHandler()
        {
            Bookers = new List<Booker>();
            fileHandler = new FileHandler();
        }
        //Metod för att hämta alla filer i början av programmet
        public void GetBookings()
        {
            foreach (var booker in fileHandler.PrintAll())
            {
                Bookers.Add(booker);
            }
        }
        //Metod för att lägga in nya bokningar
        public void AddNewBooking(string name, int table, string date, string time)
        {
            Bookers.Add(new Booker(name, table, date, time));
            fileHandler.Save(Bookers);
        }
        //Metod för att kolla så bokningen är genomförbar som skickar tillbaka en bool
        public bool CheckIfAvailable(int table, string date, string time)
        {
            foreach (var booker in fileHandler.PrintAll())
            {
                if (table == booker.Table && date == booker.Date && time == booker.Time)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
