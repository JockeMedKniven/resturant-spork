using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace WPF_Labb_3
{
    public abstract class FileHandlerMall : IFileHandler
    {
        public abstract List<Booker> PrintAll();
        public abstract void Save(List<Booker> bookings);

    }
    //Filhanteraren med override och asynkron filhantering
    public class FileHandler : FileHandlerMall
    {
        public override List<Booker> PrintAll()
        {
            string fileName = File.ReadAllText("Bookings.json");
            return JsonSerializer.Deserialize<List<Booker>>(fileName);
        }
        public override async void Save(List<Booker> bookings)
        {
            string fileName = "Bookings.json";
            using FileStream fileStream = new FileStream(fileName, FileMode.Create);
            await JsonSerializer.SerializeAsync(fileStream, bookings);
            await fileStream.DisposeAsync();
        }
    }
}
