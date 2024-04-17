using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Railway_MiniProject
{
    class Program
    {
        static RailwayReservation_ProjectEntities db = new RailwayReservation_ProjectEntities();
        static Train tr = new Train();
        static Class_Detail cd = new Class_Detail();

        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO RAILWAY RESERVATION SYSTEM");

            while (true)
            {
                Console.WriteLine("╔═════════════════════════════════════╗");
                Console.WriteLine("║ Option |         Description        ║");
                Console.WriteLine("║═════════════════════════════════════║");
                Console.WriteLine("║   1    |        Admin Login         ║");
                Console.WriteLine("║   2    |        User Login          ║");
                Console.WriteLine("║   3    |            Exit            ║");
                Console.WriteLine("╚═════════════════════════════════════╝");
                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AdminLogin();
                        break;
                    case 2:
                        UserLogin();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        //AdminLogin------------------------------------------------------------------//
        static void AdminLogin()
        {
            Console.WriteLine("Admin Login");
            Console.Write("Enter Admin Name: ");
            string Admin_Name = Console.ReadLine();
            Console.Write("Enter Password: ");

            // Hide password input
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                // Ignore any key other than Backspace and Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*"); // Print asterisk for each character entered
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Clear the character and move the cursor back
                }
            } while (key.Key != ConsoleKey.Enter);

            int Admin_Password;
            if (int.TryParse(password.ToString(), out Admin_Password))
            {
                Console.WriteLine("\n----------------------------------------------------------------------------------");

                if (Admin_Name == "Riya" && Admin_Password == 1234)
                {
                    // Admin operations menu
                    Console.WriteLine();
                    Console.WriteLine("╔═════════════════════════════════╗");
                    Console.WriteLine("║  Option        | Description    ║");
                    Console.WriteLine("╠═════════════════════════════════╣");
                    Console.WriteLine("║  1             | Add Train      ║");
                    Console.WriteLine("║  2             | Modify Train   ║");
                    Console.WriteLine("║  3             | Delete Train   ║");
                    Console.WriteLine("║  4             | Activate Train ║");
                    Console.WriteLine("║  5             | Show All Trains║");
                    Console.WriteLine("║  6             | All Bookings   ║");
                    Console.WriteLine("╚═════════════════════════════════╝");
                    Console.WriteLine("Admin operations go here...");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    switch (choice)
                    {
                        case 1:
                            AddTrain();
                            Console.WriteLine("Train have been successfully added");
                            Display_Train();
                            break;
                        case 2:
                            Console.WriteLine("Please modify the train");
                            Display_Train();
                            Modify_Train();
                            break;
                        case 3:
                            Console.WriteLine("Please soft delete the train");
                            Delete_Train();
                            Display_Train();
                            break;
                        case 4:
                            Console.WriteLine("Please enter the Train Number to activate:");
                            int trainNoToActivate = int.Parse(Console.ReadLine());
                            Activate_Train(trainNoToActivate);
                            Display_Train();
                            break;
                        case 5:
                            ShowAllTrains();
                            break;
                        case 6:
                            ShowAllBookings();
                            break;

                        default:
                            Console.WriteLine("Enter a valid number");
                            break;
                    }

                    Console.WriteLine("Returning to main menu...");
                }
                else
                {
                    Console.WriteLine("\nInvalid admin credentials, you can't access the admin control.");
                    Console.WriteLine("Returning to main menu...");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid password format.");
                Console.WriteLine("Returning to main menu...");
            }
        }


        //---------------------AddTrain--------------------------
        static void AddTrain()
        {

            Console.Write("Enter Train No.: ");
            tr.Train_No = int.Parse(Console.ReadLine());
            Console.Write("Enter Train Name: ");
            tr.Train_Name = Console.ReadLine();
            Console.Write("Enter Source Station: ");
            tr.Source_Station = Console.ReadLine();
            Console.Write("Enter Destination Station: ");
            tr.Destination_Station = Console.ReadLine();
            tr.Status = "Active";
            db.Trains.Add(tr);
            db.SaveChanges();
            var classname = new string[] { "1AC", "2AC", "Sleeper" };
            foreach (var ClassName in classname)
            {
                cd.Train_No = tr.Train_No;
                cd.Class_Name = ClassName;
                Console.Write($"Enter Total seats {ClassName} :");
                cd.Total_Seats = int.Parse(Console.ReadLine());
                Console.Write("Availble seats: ");
                cd.Available_Seats = int.Parse(Console.ReadLine());
                Console.Write("Enter Fare Amount: ");
                cd.Fare = int.Parse(Console.ReadLine());

                db.Class_Detail.Add(cd);
                db.SaveChanges();
                Console.WriteLine();
            }
        }




        //----------------------------Display Trains-------------------------------------------------------------------------------
        static void Display_Train()
        {
            var trains = db.Trains.ToList();

            // Display the table header
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("| Train No |    Train Name        | Source Station     | Destination Station     | Status   |");
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════");

            foreach (var tr in trains)
            {
                Console.WriteLine($"| {tr.Train_No,-20} | {tr.Train_Name,-20} | {tr.Source_Station,-20} | {tr.Destination_Station,-20} | {tr.Status,-8} |");
            }

            // End the table with a bottom border
            Console.WriteLine("═════════════════════════════════════════════════════════════════════════════════════════════");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");

        }







        static void Activate_Train(int trainNo)
        {
            // Retrieve the train from the database
            var trainToActivate = db.Trains.FirstOrDefault(t => t.Train_No == trainNo);
            if (trainToActivate != null)
            {
                if (trainToActivate.Status == "Active")
                {
                    Console.WriteLine("Train is already active.");
                }
                else
                {
                    // Activate the train by setting its status to "Active"
                    trainToActivate.Status = "Active";
                    db.SaveChanges();
                    Console.WriteLine("Train activated successfully.");
                }
            }
            else
            {
                Console.WriteLine("Train with the provided number does not exist.");
            }
        }



        static void UserLogin()
        {
            Console.WriteLine("User Operations Menu:");
            Console.WriteLine("╔═════════════════════════╗");
            Console.WriteLine("║  Option  | Description  ║");
            Console.WriteLine("╠══════════╪══════════════╣");
            Console.WriteLine("║  1       | Book Ticket  ║");
            Console.WriteLine("║  2       | Print Ticket ║");
            Console.WriteLine("║  3       | Cancel Ticket║");
            Console.WriteLine("╚══════════╧══════════════╝");
            Console.Write("Enter your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Display_Train();
                    Console.WriteLine();
                    BookingTicket();
                    break;
                case 2:
                    DisplayBooking_Details();
                    Console.WriteLine("Booking has been successfully done");
                    break;
                case 3:
                    ShowAllBookings();
                    CancelTicket();
                    break;
                default:
                    Console.WriteLine("Enter valid number");
                    break;
            }
            Console.WriteLine("User operations go here...");
            Console.WriteLine("Returning to main menu...");
            Console.WriteLine();
        }

        
       


        static void displaytickeDisplayTicketClass_Detail()
        {

            Console.Write("Enter Train No.: ");
            tr.Train_No = int.Parse(Console.ReadLine());
            var trainVal = db.Class_Detail.Where(t => t.Train_No == tr.Train_No).ToList();


            // Display the result in the console
            foreach (var Class_Detail in trainVal)
            {
                Console.WriteLine($"Train_No: {Class_Detail.Train_No}, Class_Name: {Class_Detail.Class_Name}, Total_Seats: {Class_Detail.Total_Seats}, Available_Seats: {Class_Detail.Available_Seats}, Fare: {Class_Detail.Fare}");
            }

        }


        public static void Modify_Train()
        {
            Console.Write("Enter Train No.: ");
            int train_No = int.Parse(Console.ReadLine());

            // Retrieve the train details from the database
            var trainToUpdate = db.Trains.FirstOrDefault(t => t.Train_No == train_No);
            if (trainToUpdate != null)
            {
                Console.Write("Enter Train Name: ");
                trainToUpdate.Train_Name = Console.ReadLine();
                Console.Write("Enter Train Source Station: ");
                trainToUpdate.Source_Station = Console.ReadLine();
                Console.Write("Enter Train Destination Station: ");
                trainToUpdate.Destination_Station = Console.ReadLine();
                Console.Write("Enter Status: ");
                trainToUpdate.Status = Console.ReadLine();
                // Update the train in the database
                db.SaveChanges();
                Console.WriteLine("Updated Tains Successfully");

                Display_Train();
                Console.ReadLine();
            }

            else
            {
                Console.WriteLine("Train do not exists ");
                AdminLogin();
            }
        }
        public static void Delete_Train()
        {
            Console.Write("Enter Train No: ");
            int train_No = int.Parse(Console.ReadLine());

            // Retrieve the train details from the database
            var trainToDelete = db.Trains.FirstOrDefault(t => t.Train_No == train_No);
            if (trainToDelete != null)
            {
                // Soft delete by updating the status to "Inactive"
                trainToDelete.Status = "InActive";
                db.SaveChanges();
                Console.WriteLine("Train  Soft deleted successfully.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Train do not exists");
                AdminLogin();
            }
        }



        static void ShowAllTrains()
        {
            var allTrains = db.Trains.ToList();

            Console.WriteLine("All Trains:");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("| Train No | Train Name        | Source       | Destination      | Status   |");
            Console.WriteLine("----------------------------------------------------------------------------");

            foreach (var train in allTrains)
            {
                Console.WriteLine($"| {train.Train_No,-9} | {train.Train_Name,-17} | {train.Source_Station,-12} | {train.Destination_Station,-16} | {train.Status,-8} |");
            }

            Console.WriteLine("----------------------------------------------------------------------------");
        }

        public static void BookingTicket()
        {
            try
            {
                Console.Write("Enter Train Number: ");
                int Train_No = Convert.ToInt32(Console.ReadLine());

                // Check if the train is active before proceeding with the booking
                var train = db.Trains.FirstOrDefault(t => t.Train_No == Train_No && t.Status == "Active");
                if (train == null)
                {
                    Console.WriteLine("Sorry! This train is not running at this time");
                    return;
                }

                Console.Write("Enter Passenger Name: ");
                string Passenger_Name = Console.ReadLine();

                Console.Write("Enter Class Name: ");
                string Class_Name = Console.ReadLine();

                Console.Write("Enter Date of Travel (YYYY-MM-DD): ");
                DateTime Travel_Date = Convert.ToDateTime(Console.ReadLine());

                Console.Write("Enter Number of Tickets: ");
                int Tickets_Count = Convert.ToInt32(Console.ReadLine());

                db.BookTrain_Ticket(Train_No, Passenger_Name, Class_Name, Travel_Date, Tickets_Count);
                db.SaveChanges();

                // Retrieve the booking ID after successful booking
                var booking = db.Bookings.OrderByDescending(b => b.Booking_ID).FirstOrDefault();
                if (booking != null)
                {
                    Console.WriteLine("Ticket Booked Successfully! Your Booking ID is: " + booking.Booking_ID);
                }
                else
                {
                    Console.WriteLine("Ticket Booked Successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }



        static void Cancel_train()
        {
            Console.Write("Enter Booking ID:");
            int Booking_ID = int.Parse(Console.ReadLine());

            // Retrieve the booking details from the database
            var bookingToDelete = db.Bookings.FirstOrDefault(b => b.Booking_ID == Booking_ID);
            if (bookingToDelete != null)
            {
                Console.Write("Enter Passenger Name:");
                string Passenger_Name = Console.ReadLine();

                Console.Write("Enter Train Number:");
                int Train_No = int.Parse(Console.ReadLine());

                Console.Write("Enter Class Name:");
                string Class_Name = Console.ReadLine();

                Console.Write("Enter Number of Tickets to Cancel:");
                int Tickets_Count = int.Parse(Console.ReadLine());

                try
                {
                    // Call the stored procedure to cancel the ticket
                    db.Cancel_Ticket(Booking_ID, Passenger_Name, Train_No, Class_Name, Tickets_Count);
                    db.SaveChanges();
                    Console.WriteLine("Ticket Cancelled Successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Booking not found with this Booking ID.");
            }
        }



        static void ShowAllBookings()
        {
            var allBookings = db.Bookings.ToList();

            Console.WriteLine("All Bookings:");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Booking ID | Passenger Name       | Train No | Class Name | Travel Date | Tickets Count | Total Amount |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");

            foreach (var booking in allBookings)
            {
                Console.WriteLine($"| {booking.Booking_ID,-11} | {booking.Passenger_Name,-20} | {booking.Train_No,-8} | {booking.Class_Name,-10} | {booking.Travel_Date,-12:d} | {booking.Tickets_Count,-13} | {booking.Total_Amount,-13} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }



        static void CancelTicket()
        {
            Console.Write("Enter Booking ID:");
            int Booking_ID = int.Parse(Console.ReadLine());

            // Retrieve the booking details from the database
            var bookingToDelete = db.Bookings.FirstOrDefault(b => b.Booking_ID == Booking_ID);
            if (bookingToDelete != null)
            {
                Console.Write("Enter Passenger Name:");
                string Passenger_Name = Console.ReadLine();

                Console.Write("Enter Train Number:");
                int Train_No = int.Parse(Console.ReadLine());

                Console.Write("Enter Class Name:");
                string Class_Name = Console.ReadLine();

                Console.Write("Enter Number of Tickets to Cancel:");
                int Tickets_Count = int.Parse(Console.ReadLine());

                try
                {
                    // Call the stored procedure to cancel the ticket
                    db.Cancel_Ticket(Booking_ID, Passenger_Name, Train_No, Class_Name, Tickets_Count);
                    db.SaveChanges();
                    Console.WriteLine("Ticket Cancelled Successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Booking not found with this Booking ID.");
            }
        }









        static void DisplayBooking_Details()
        {
            Console.Write("Enter Booking ID:");
            int Booking_ID = int.Parse(Console.ReadLine());
            Console.WriteLine("Ticket");

            var booking = db.Bookings.FirstOrDefault(b => b.Booking_ID == Booking_ID);
            if (booking != null)
            {
                Console.WriteLine(" ╔══════════════════════════════════════════════╗");
                Console.WriteLine(" ║             BOOKING DETAILS                  ║");
                Console.WriteLine(" ╠══════════════════════════════════════════════╣");
                Console.WriteLine($"║ Booking ID: {booking.Booking_ID}             ║");
                Console.WriteLine($"║ Passenger Name: {booking.Passenger_Name}     ║");
                Console.WriteLine($"║ Train Number: {booking.Train_No}             ║");
                Console.WriteLine($"║ Class Name: {booking.Class_Name}             ║");
                Console.WriteLine($"║ Date of Travel: {booking.Travel_Date}        ║");
                Console.WriteLine($"║ Number of Tickets: {booking.Tickets_Count}   ║");
                Console.WriteLine($"║ Total Amount: {booking.Total_Amount}         ║");
                Console.WriteLine($"║ Status: {booking.Status}                     ║");
                Console.WriteLine(" ╚══════════════════════════════════════════════╝");


            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }

       





    }
}




