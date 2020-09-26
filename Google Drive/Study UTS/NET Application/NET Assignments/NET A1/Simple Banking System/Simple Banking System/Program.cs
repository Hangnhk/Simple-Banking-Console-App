using System;
using System.Text;

namespace Simple_Banking_System
{
    class Program
    {
        // Declare the static variables
        static string inputMenuOption;
        static string inputUserName;
        static string inputPassword;

        // Declare enum variable for Menu Option
        enum MenuOption
        {
            CreateAccount = 1,
            SearchAccount = 2,
            Deposit = 3,
            Withdraw = 4,
            AccountStatement = 5,
            DeleteAccount = 6,
            Exit = 7
        }

        // Display Login Menu
        public static void LogIn()
        {
            Console.Clear();
            Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
            Console.WriteLine("\t\t║     WELCOME TO SIMPLE BANKING SYSTEM      ║");
            Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
            Console.WriteLine("\t\t║                                           ║");
            Console.WriteLine("\t\t║               LOGIN TO START              ║");
            Console.Write("\t\t║          User Name: ");
            int UserNameCursorX = Console.CursorTop;
            int UserNameCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.Write("\t\t║          Password: ");
            int PassCursorX = Console.CursorTop;
            int PassCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
            Console.SetCursorPosition(UserNameCursorY, UserNameCursorX);

            // Get User Input for User Name
            inputUserName = Console.ReadLine();
            Console.SetCursorPosition(PassCursorY, PassCursorX);

            // Get User Input for Password & Display Masked Password
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);

                // Password is set & Exit the Loop if Enter Key is Pressed
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }

                // Remove a Character if Backspace Key is Pressed
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }
                    continue;
                }

                // Mask Input Password with Asterisk *
                Console.Write("*");

                // Append Input Characters to Password
                sb.Append(key.KeyChar);
            }

            // Convert String Builder to String for Password
            inputPassword = sb.ToString();
            Console.WriteLine();
        }

        // Check Login Credententials
        public static bool checkLogInCredentials(string userName, string userPassword)
        {
            // Read the text file login.txt
            string[] lines = System.IO.File.ReadAllLines("login.txt");
            foreach (string set in lines)
            {
                // Split each line
                string[] splits = set.Split(',');

                // If userName and userPassword match data in login.txt, Return True
                if (userName.Equals(splits[0]) && userPassword.Equals(splits[1].Trim()))
                {
                    return true;
                }
            }

            // If not Match, Return False
            return false;
        }

        // Display Main Menu
        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
            Console.WriteLine("\t\t║     WELCOME TO SIMPLE BANKING SYSTEM      ║");
            Console.WriteLine("\t\t╠═══════════════════════════════════════════╣");
            Console.WriteLine("\t\t║                                           ║");
            Console.WriteLine("\t\t║          1. Create a New Account          ║");
            Console.WriteLine("\t\t║          2. Search for an Account         ║");
            Console.WriteLine("\t\t║          3. Deposit                       ║");
            Console.WriteLine("\t\t║          4. Withdraw                      ║");
            Console.WriteLine("\t\t║          5. A/C Statement                 ║");
            Console.WriteLine("\t\t║          6. Delete Account                ║");
            Console.WriteLine("\t\t║          7. Exit                          ║");
            Console.WriteLine("\t\t╟───────────────────────────────────────────╢");
            Console.Write("\t\t║        Enter your choice (1-7):   ");
            int ChoiceCursorX = Console.CursorTop;
            int ChoiceCursorY = Console.CursorLeft;
            Console.WriteLine("\t    ║");
            Console.WriteLine("\t\t╚═══════════════════════════════════════════╝"); ;
            Console.SetCursorPosition(ChoiceCursorY, ChoiceCursorX);
            // Get User Input for Menu Option
            inputMenuOption = Console.ReadLine();
        }

        // Check User Input for Menu Option
        public static bool CheckUserMenuOption(string input)
        {
            // If User Input is an Integer, Continue Checking
            try
            {
                // Convert Input String to Integer 
                int userChoice = int.Parse(input);

                // If User Input > 7 or <1, Display Message & Return False
                if (userChoice > 7 || userChoice < 1)
                {
                    Console.WriteLine("\n\n\t\tInvalid number!...Please enter a number from 1 to 7!");
                    Console.ReadKey();
                    return false;
                }

                // If User Input from 1-7, Return True
                else
                {
                    return true;
                }
            }

            // If User Input is not an Integer, Display Message & Return False
            catch (Exception)
            {
                Console.WriteLine("\n\n\t\tIncorrect format!...Please enter a number from 1 to 7!");
                Console.ReadKey();
                return false;
            }
        }

        static void Main(string[] args)
        {
            // Change Title of Console App
            Console.Title = "Simple Banking System";

            // Display Login Menu
            LogIn();

            // If Invalid Login Credentials, Display Message & Return to Login Menu 
            while (!checkLogInCredentials(inputUserName, inputPassword))
            {
                Console.WriteLine("\n\n\t\tInvalid credentials!... Please try again!");
                Console.ReadKey();
                LogIn();
            };

            // If Valid Login Credentials, Display message & Proceed to Main Menu
            if (checkLogInCredentials(inputUserName, inputPassword))
            {
                Console.WriteLine("\n\n\t\tValid credentials!... Please enter");
                Console.ReadKey();
                while (true)
                {
                    // Display Main Menu
                    MainMenu();

                    // Run the program if user input for menu option is correct
                    if (CheckUserMenuOption(inputMenuOption))
                    {
                        // Convert Input String to Integer
                        int userChoice = int.Parse(inputMenuOption);

                        // Create a new instance of Bank
                        Bank bank = new Bank();

                        // Navigate to Different Cases according to User Choice for Menu Option
                        switch ((MenuOption)userChoice)
                        {
                            case MenuOption.CreateAccount:
                                bank.CreateAccount();
                                break;
                            case MenuOption.SearchAccount:
                                bank.SearchAccount();
                                break;
                            case MenuOption.Deposit:
                                bank.Deposit();
                                break;
                            case MenuOption.Withdraw:
                                bank.Withdraw();
                                break;
                            case MenuOption.AccountStatement:
                                bank.DisplayAccountStatement();
                                break;
                            case MenuOption.DeleteAccount:
                                bank.DeleteAccount();
                                break;
                            case MenuOption.Exit:
                                Environment.Exit(0);
                                break;
                        }
                    }
                }
            }
        }
    }
}
