using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WPF_Labb_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BookingHandler bookingHandler = new BookingHandler();
        FileHandler filehandler = new FileHandler();
        public MainWindow()
        {
            //Program start
            InitializeComponent();
            //Lägger till objekt i comboboxarna
            ComboBoxItems();
            //Hämtar alla bokningar från fil till lista
            bookingHandler.GetBookings();

        }
        //Bekräfta bokning med asynkron metod för utskrift av bokningsmeddelande
        private async void btnBookConfirm_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                //RegEx för att det är kul
                var regEx = new Regex(@"^[a-öA-Ö]{1,14}$");
                bool Confirmation = false;
                while (!Confirmation)
                {
                    var nameInput = tbxNameInput.Text;
                    //Kollar så de inte skriver sitt namn med tecken eller siffror
                    bool nameIsValid = regEx.IsMatch(nameInput);
                    int tableInput = cbxTable.SelectedIndex + 1;
                    string timeInput = cbxTime.Text;
                    string dateInput = datePicker.SelectedDate.Value.ToShortDateString();
                    int tablecount = 0;
                    bool isBusy = false;
                    //Check if available
                    foreach (var item in filehandler.PrintAll()) 
                    {
                        //Kollar så alla fält är ifyllda
                        if (nameInput == null || tableInput == 0 || timeInput == null)
                        {
                            MessageBox.Show("You haven't entered all the required information");
                            Confirmation = true;
                            break;
                        }
                        //Kollar alla rum
                        foreach (var booking in filehandler.PrintAll())
                        {
                            if (bookingHandler.CheckIfAvailable(tableInput, dateInput, timeInput) == true)
                            {
                                tablecount++;
                            }
                            if (tablecount == 5)
                            {
                                MessageBox.Show("All Tables at this day and time is taken, please choose another day.");
                                isBusy = true;
                                //Metod för att rensa alla fält
                                ClearAllFields();
                                Confirmation = true;
                                break;
                            }
                        }
                        if (isBusy == true) { break; }
                        if (nameIsValid == true)
                        {
                            //Kollar så det är ledigt den tiden, den dagen och vid det bordet.
                            if (bookingHandler.CheckIfAvailable(tableInput, dateInput, timeInput) == true)
                            {
                                MessageBox.Show("The booking already exists, try another table, date or time");
                                Confirmation = true;
                                break;
                            }
                            //Bokning genomförs
                            else if (bookingHandler.CheckIfAvailable(tableInput, dateInput, timeInput) != true)
                            {
                                bookingHandler.AddNewBooking(nameInput, tableInput, dateInput, timeInput);
                                await Task.Delay(1000);
                                MessageBox.Show("Din bokning är genomförd!");
                                Confirmation = true;
                                break;
                            }
                        }
                        else
                        {
                            //Felhantering för RegEx
                            MessageBox.Show("How many letter do you have in your name? 14 (without spaces) is accepted.");
                            Confirmation = true;
                            break;
                        }
                    }
                }
                //Rensar alla fält efter bokning
                ClearAllFields();
            }
            //Felhantering, resta att lämna datumfältet blankt tex.
            catch 
            {
                MessageBox.Show("Something went wrong, try again!" + "\n(Maybe you haven't entered all the required information?)");

            }
        }
        //Skriver ut alla bokningar
        private void btnListBookings_Click(object sender, RoutedEventArgs e)
        {
            PrintBookings();
        }
        //Metod med Arrays för att skriva ut värden till ComboBox
        private void ComboBoxItems()
        {
            int[] tableForBooking = new int[] {1,2,3,4,5};
            foreach (var item in tableForBooking)
            {
                cbxTable.Items.Add(item);
            }
            string[] timesForBooking = new string[] {"16.00", "16.30", "17.00", "17.30", "18.00", "18.30", "19.00", "19.30", "20.00", "20.30", "21.00", "21.30", "22.00", "22.30"};
            foreach (var item in timesForBooking)
            {
                cbxTime.Items.Add(item);
            }
        }

        private void btnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            //Tar bort bokning
            try
            {
                if (lbxBookings.SelectedIndex == -1)
                {
                    MessageBox.Show("You have not choosen any booking to cancel, please choose in the list");
                }
                else
                {
                    //var query = bookingHandler.Bookers.Where(bookerName => bookerName.Name == lbxBookings.Name);
                    //var getList = bookingHandler.Bookers;
                    //var query = getList.Where(name => name.Equals(lbxBookings.SelectedIndex));
                    //foreach (var booker in query)
                    //{
                    //    getList.Remove(booker);
                    //}
                    //Tar bort vid valt index
                    bookingHandler.Bookers.RemoveAt(lbxBookings.SelectedIndex); //Försökte med LinQ men detta var 1000 gånger lättare.
                    filehandler.Save(bookingHandler.Bookers);
                    //Anropar metod för att printa listan utan den borttagna bokningen
                    PrintBookings(); 
                }
            }
            //Felmetod med meddelande till programmeraren
            catch (Exception ex)
            {
                MessageBox.Show("Något gick fel, försök igen!" + "\n(" + ex.Message +")");
            }
        }
        //Metod för att printa alla bokningar till listbox, hämtar från fil genom FileHandler-klassen
        private void PrintBookings()
        {
            lbxBookings.Items.Clear();
            foreach (var item in filehandler.PrintAll())
            {
                lbxBookings.Items.Add(item.Name + " Table: " + item.Table + ", " + item.Date + ", " + item.Time);
            }
        } 
        //Metoden för att tömma alla fält
        private void ClearAllFields()
        {
            cbxTable.SelectedItem = null;
            cbxTime.SelectedItem = null;
            datePicker.SelectedDate = null;
            tbxNameInput.Clear();
        }
    }
}
