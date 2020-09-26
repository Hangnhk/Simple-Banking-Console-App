using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net.Mail;

namespace Simple_Banking_System
{
    public class Bank
    {
        // Declare the static variables
        static string inputNumber;
        static string path;
        static string fileName;
        static string inputYesNo;
        static string[] foundAccount;
        static double oldBalance;
        static double newBalance;
        // Accessor
        public List<Account> Accounts { get; set; }

        // Default Bank Constructor
        public Bank()
        {
            Accounts = new List<Account>();
        }

        // Check if Input Number is a Valid Integer that is larger than 0 & has less than 10 digits
        public bool CheckIntegerInput(string input)
        {
            // If Input Number has more than 10 characters, Display Message & Return False
            if (input.Length > 10)
            {
                Console.WriteLine("\n\n\n\t\tInvalid input number!...Please enter a number with less than 10 digits!");
                return false;
            }

            // If Input Number has more than 10 characters, Continue Checking
            else
            {
                // If Input Number is Integer, Continue Checking
                try
                {
                    int integerInput = int.Parse(input);
                    // If Input Number <= 0, Display Message & Return False 
                    if (integerInput <= 0)
                    {
                        Console.WriteLine("\n\n\n\t\tInvalid input number!...Please enter a positive number!");
                        return false;
                    }
                    // If Input Number > 0, return True
                    return true;
                }

                // If Input Number is Integer, Display Message & Return False 
                catch
                {
                    Console.WriteLine("\n\n\n\t\tInvalid input number!...Please try again!");
                    return false;
                }
            }
        }

        // Check if User Input for Account is Valid
        public bool CheckUserAccountInput(Account account)
        {
            // If Fields for First Name, Last Name, Address are Empty, Display Message & Return False
            if (String.IsNullOrEmpty(account.FirstName) || String.IsNullOrEmpty(account.LastName) || String.IsNullOrEmpty(account.Address))
            {
                Console.WriteLine("\n\n\t\tEmpty fields are not allowed. Please try again!");
                return false;
            }

            // If Fields are not Empty, Continue Checking
            else
            {
                // If Phone Number is Valid Integer, Continue Checking
                if (CheckIntegerInput(Account.inputPhone))
                {
                    // If Valid Email Address, Return True
                    if (account.Email.Contains("@") && (account.Email.Contains("gmail.com") || account.Email.Contains("outlook.com") || account.Email.Contains("uts.edu.au")))
                        return true;

                    // If Invalid Email Address, Display Message & Return False
                    else
                    {
                        Console.WriteLine("\n\n\t\tInvalid email address. Please try again!");
                        return false;
                    }
                }

                // If Invalid Phone Number, Return False
                else
                    return false;
            }
        }

        // Check if User Wants to Retry the Activity
        public bool Retry(string input)
        {
            if (input == "y")
                return true;
            else
                Console.WriteLine("\n\t\tPress any key to return to Main Menu");
            Console.ReadKey();
            return false;
        }

        // Display Menu for Create Account
        public void CreateAccountMenu(Account account)
        {
            Console.Clear();
            Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
            Console.WriteLine("\t\t║           CREATE A NEW ACCOUNT            ║");
            Console.WriteLine("\t\t╠═══════════════════════════════════════════╣");
            Console.WriteLine("\t\t║                                           ║");
            Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
            Console.Write("\t\t║  First Name: ");
            int FirstNameCursorX = Console.CursorTop;
            int FirstNameCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║  Last Name: ");
            int LastNameCursorX = Console.CursorTop;
            int LastNameCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║  Address: ");
            int AddressCursorX = Console.CursorTop;
            int AddressCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║  Phone: ");
            int PhoneCursorX = Console.CursorTop;
            int PhoneCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║  Email: ");
            int EmailCursorX = Console.CursorTop;
            int EmailCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
            Console.SetCursorPosition(FirstNameCursorY, FirstNameCursorX);
            // Get User Input for First Name
            account.FirstName = Console.ReadLine();
            Console.SetCursorPosition(LastNameCursorY, LastNameCursorX);
            // Get User Input for Last Name
            account.LastName = Console.ReadLine();
            Console.SetCursorPosition(AddressCursorY, AddressCursorX);
            // Get User Input for Address
            account.Address = Console.ReadLine();
            Console.SetCursorPosition(PhoneCursorY, PhoneCursorX);
            // Get User Input for Phone
            Account.inputPhone = Console.ReadLine();
            Console.SetCursorPosition(EmailCursorY, EmailCursorX);
            // Get User Input for Email
            account.Email = Console.ReadLine();
        }

        // Create Account 
        public void CreateAccount()
        {
            bool createAccount = true;
            while (createAccount)
            {
                // Create a new Account instance
                var newAccount = new Account();

                // Display Menu for Create Account
                CreateAccountMenu(newAccount);

                // If Valid Account Input, Proceed with Create Account 
                if (CheckUserAccountInput(newAccount))
                {
                    // Convert Input String to Integer for Phone
                    newAccount.Phone = int.Parse(Account.inputPhone);

                    // Ask & Get User's Confirmation of the Input Information
                    Console.Write("\n\n\t\tIs the information correct (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();

                    // If User choose Yes, Proceed with Create Account
                    if (inputYesNo == "y")
                    {
                        Console.WriteLine("\t\tAccount Created! Details will be provided via email.\n");

                        // If Account Number has Existed, Generate a New One 
                        do
                        {
                            Random rnd = new Random();
                            newAccount.AccountNo = rnd.Next(100000, 99999999);
                            path = @"Accounts\" + newAccount.AccountNo + ".txt";
                        }
                        while (File.Exists(path));

                        // If Account Number is Unique, Create a New Text File with Details of New Account
                        if (!File.Exists(path))
                        {
                            // Create Content for the File
                            string fileContent = "First Name:" + newAccount.FirstName + Environment.NewLine + "Last Name:" + newAccount.LastName + Environment.NewLine +
                                                 "Address:" + newAccount.Address + Environment.NewLine + "Phone:" + newAccount.Phone + Environment.NewLine +
                                                 "Email:" + newAccount.Email + Environment.NewLine + "Account Number:" + newAccount.AccountNo + Environment.NewLine +
                                                 "Balance:$" + newAccount.Balance + Environment.NewLine + Environment.NewLine + "Last 5 transactions" + Environment.NewLine +
                                                 newAccount.FirstTransaction + "$0" + Environment.NewLine + newAccount.SecondTransaction + "$0" +
                                                 Environment.NewLine + newAccount.ThirdTransaction + "$0" + Environment.NewLine +
                                                 newAccount.FourthTransaction + "$0" + Environment.NewLine + newAccount.FifthTransaction + "$0";

                            // Add Content to the File
                            File.WriteAllText(path, fileContent);

                            // Display Message for Account Number
                            Console.WriteLine("\t\tYour Account number is: {0}\n", newAccount.AccountNo);
                        }

                        // Add New Account to Accounts List
                        Accounts.Add(newAccount);

                        // Send Account Details to User Email
                        try
                        {
                            // Set up Mail Message content
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("hangnhk.rec@gmail.com");
                            mail.To.Add(newAccount.Email);
                            mail.Subject = "New Account Created";
                            mail.Body = "You have just successfully created a new account! The details of your account is as below."
                                         + Environment.NewLine + Environment.NewLine + "First Name: " + newAccount.FirstName
                                         + Environment.NewLine + "Last Name: " + newAccount.LastName
                                         + Environment.NewLine + "Address: " + newAccount.Address
                                         + Environment.NewLine + "Phone: " + newAccount.Phone
                                         + Environment.NewLine + "Email: " + newAccount.Email
                                         + Environment.NewLine + "Account Number: " + newAccount.AccountNo
                                         + Environment.NewLine + "Balance: $" + newAccount.Balance;

                            // Set up SMTP Client content
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential("hangnhk.rec@gmail.com", "@Password1234");
                            client.EnableSsl = true;

                            // Send Email via SMTP server
                            client.Send(mail);

                            // Display Message if Email has been sent
                            Console.WriteLine("\t\tEmail has been succesfully sent!");
                        }

                        // Display Message if Unable to Send Email
                        catch (Exception)
                        {
                            Console.WriteLine("\t\tUnable to send email! Please try again");
                        }

                        // Break the Loop & Return to Main Menu
                        createAccount = false;
                        Console.WriteLine("\n\t\tPress any key to return to Main Menu");
                    }

                    // If User Choose No, Display Message & Return to Account Menu
                    else if (inputYesNo == "n")
                    {
                        Console.WriteLine("\t\tPlease re-enter the details!");
                    }
                }

                //If Invalid Account Input, Display Message & Return to Account Menu
                else
                    createAccount = true;
                Console.ReadKey();
            }

        }

        // Check if Account Exists
        public bool CheckAccountExist(string fileName)
        {
            path = @"Accounts\" + fileName;
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        // Get Account
        public string[] GetAccount(string fileName)
        {
            string[] files = Directory.GetFiles(@"Accounts\", fileName);
            string[] lines = File.ReadAllLines(files[0]);
            return lines;
        }

        // Display Account Details
        public void DisplayAccountDetails(string[] account)
        {
            Console.Write("\t\t║   Account Number: ");
            int AccountNumCursorX = Console.CursorTop;
            int AccountNumCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.Write("\t\t║   Account Balance: $");
            int BalanceCursorX = Console.CursorTop;
            int BalanceCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.Write("\t\t║   First Name: ");
            int FirstNameCursorX = Console.CursorTop;
            int FirstNameCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.Write("\t\t║   Last Name: ");
            int LastNameCursorX = Console.CursorTop;
            int LastNameCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║   Address: ");
            int AddressCursorX = Console.CursorTop;
            int AddressCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║   Phone: ");
            int PhoneCursorX = Console.CursorTop;
            int PhoneCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.Write("\t\t║   Email: ");
            int EmailCursorX = Console.CursorTop;
            int EmailCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t\t    ║");
            Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
            Console.SetCursorPosition(AccountNumCursorY, AccountNumCursorX);
            Console.Write(account[5].Split(':')[1]); // Display Account No
            Console.SetCursorPosition(BalanceCursorY, BalanceCursorX);
            Console.Write(account[6].Split('$')[1]); // Display Balance
            Console.SetCursorPosition(FirstNameCursorY, FirstNameCursorX);
            Console.Write(account[0].Split(':')[1]); // Display First Name
            Console.SetCursorPosition(LastNameCursorY, LastNameCursorX);
            Console.Write(account[1].Split(':')[1]); // Display Last Name
            Console.SetCursorPosition(AddressCursorY, AddressCursorX);
            Console.Write(account[2].Split(':')[1]); // Display Address          
            Console.SetCursorPosition(PhoneCursorY, PhoneCursorX);
            Console.Write(account[3].Split(':')[1]); // Display Phone
            Console.SetCursorPosition(EmailCursorY, EmailCursorX);
            Console.Write(account[4].Split(':')[1]);  // Display Email
        }

        // Display Search Account Menu
        public void SearchAccountMenu()
        {
            Console.Clear();
            Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
            Console.WriteLine("\t\t║             SEARCH AN ACCOUNT             ║");
            Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
            Console.WriteLine("\t\t║                                           ║");
            Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
            Console.WriteLine("\t\t║                                           ║");
            Console.Write("\t\t║    Account Number: ");
            int AccountNoCursorX = Console.CursorTop;
            int AccountNoCursorY = Console.CursorLeft;
            Console.WriteLine("\t\t\t    ║");
            Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
            Console.SetCursorPosition(AccountNoCursorY, AccountNoCursorX);
            // Get User Input for Account Number
            inputNumber = Console.ReadLine().Trim();
        }

        // Search Account
        public void SearchAccount()
        {
            bool searchAccount = true;
            while (searchAccount)
            {
                // Display Search Account Menu
                SearchAccountMenu();
                fileName = inputNumber + ".txt";

                // If Valid Account Number, Display Account Details
                if (CheckIntegerInput(inputNumber) && CheckAccountExist(fileName))
                {
                    // Get Account 
                    foundAccount = GetAccount(fileName);

                    // Display Account Details
                    Console.WriteLine("\n\n\t\tAccount Found!\n");
                    Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                    Console.WriteLine("\t\t║              ACCOUNT DETAILS              ║");
                    Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                    Console.WriteLine("\t\t║                                           ║");
                    DisplayAccountDetails(foundAccount);
                }

                // If Invalid Account Number, Display Message
                else
                {
                    Console.WriteLine("\n\n\t\tAccount Not Found!\n");
                }

                // Display Option to Retry
                Console.Write("\n\n\t\tCheck Another Account (y/n)? ");
                inputYesNo = Console.ReadLine().ToLower();
                searchAccount = Retry(inputYesNo);
            }
        }

        // Deposit   
        public void Deposit()
        {
            bool deposit = true;
            while (deposit)
            {
                // Display Deposit Menu
                Console.Clear();
                Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                Console.WriteLine("\t\t║                  DEPOSIT                  ║");
                Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                Console.WriteLine("\t\t║                                           ║");
                Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
                Console.WriteLine("\t\t║                                           ║");
                Console.Write("\t\t║    Account Number: ");
                int AccountNoCursorX = Console.CursorTop;
                int AccountNoCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t    ║");
                Console.Write("\t\t║    Amount: $");
                int AmountCursorX = Console.CursorTop;
                int AmountCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t\t    ║");
                Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
                Console.SetCursorPosition(AccountNoCursorY, AccountNoCursorX);
                // Get User Input for Account Number
                inputNumber = Console.ReadLine().Trim();
                fileName = inputNumber + ".txt";

                // If Valid Account Number, Display Message & Proceed with Deposit
                if (CheckIntegerInput(inputNumber) && CheckAccountExist(fileName))
                {
                    // Get Account 
                    foundAccount = GetAccount(fileName);

                    // Ask & Get User Input for Deposit Amount
                    Console.WriteLine("\n\n\n\t\tAccount Found! Enter the amount...");
                    Console.SetCursorPosition(AmountCursorY, AmountCursorX);
                    inputNumber = Console.ReadLine().Trim();
                    Console.WriteLine("\n\n\n");

                    // If Valid Deposit Amount, Proceed with Deposit
                    if (CheckIntegerInput(inputNumber))
                    {
                        // Convert Input String to Integer
                        int depositAmount = int.Parse(inputNumber);

                        // Get the Old Balance from the Text File
                        oldBalance = int.Parse(foundAccount[6].Split('$')[1]);

                        // Update new Balance in the Text File
                        newBalance = oldBalance + depositAmount;
                        foundAccount[6] = "Balance:$" + newBalance.ToString();

                        // Update last 5 transactions
                        foundAccount[13] = foundAccount[12];
                        foundAccount[12] = foundAccount[11];
                        foundAccount[11] = foundAccount[10];
                        foundAccount[10] = foundAccount[9];
                        foundAccount[9] = DateTime.Now.ToShortDateString() + "| Deposit $" + depositAmount.ToString("#,##0");

                        // Update Text File
                        path = @"Accounts\" + fileName;
                        File.WriteAllLines(path, foundAccount, Encoding.UTF8);

                        // Display Success Message
                        Console.WriteLine("\t\tDeposit Successful!");

                        // Break the Loop & Return to Main Menu
                        deposit = false;
                        Console.WriteLine("\n\t\tPress any key to return to Main Menu");
                    }

                    // If Invalid Deposit Amount, Display Message & Return to Deposit Menu
                    else
                    {
                        deposit = true;
                    }
                    Console.ReadKey();
                }

                // If Invalid Account Number, Display Message and Option to Retry
                else
                {
                    Console.Write("\n\n\t\tAccount Not Found!\n\t\tRetry (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();
                    deposit = Retry(inputYesNo);
                }
            }
        }

        //  Withdraw
        public void Withdraw()
        {
            bool withdraw = true;
            while (withdraw)
            {
                // Display Withdraw Menu
                Console.Clear();
                Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                Console.WriteLine("\t\t║                 WITHDRAW                  ║");
                Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                Console.WriteLine("\t\t║                                           ║");
                Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
                Console.WriteLine("\t\t║                                           ║");
                Console.Write("\t\t║    Account Number: ");
                int AccountNoCursorX = Console.CursorTop;
                int AccountNoCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t    ║");
                Console.Write("\t\t║    Amount: $");
                int AmountCursorX = Console.CursorTop;
                int AmountCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t\t    ║");
                Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
                Console.SetCursorPosition(AccountNoCursorY, AccountNoCursorX);
                // Get User Input for Account Number
                inputNumber = Console.ReadLine().Trim();
                fileName = inputNumber + ".txt";

                // If Valid Account Number, Display Message & Proceed with WithDraw
                if (CheckIntegerInput(inputNumber) && CheckAccountExist(fileName))
                {
                    // Get Account 
                    foundAccount = GetAccount(fileName);

                    // Ask & Get User Input for Withdraw Amount
                    Console.WriteLine("\n\n\n\t\tAccount Found! Enter the amount...");
                    Console.SetCursorPosition(AmountCursorY, AmountCursorX);
                    inputNumber = Console.ReadLine().Trim();
                    Console.WriteLine("\n\n\n");

                    // If Valid Withdraw Amount, Proceed with Withdraw
                    if (CheckIntegerInput(inputNumber))
                    {
                        // Convert Input String to Integer for Withdraw Amount
                        int withdrawAmount = int.Parse(inputNumber);

                        // Get the Old Balance from the Text File
                        oldBalance = int.Parse(foundAccount[6].Split('$')[1]);

                        // If Withdraw Amount > Balance, Display message & Return to Withdraw Menu
                        if (withdrawAmount > oldBalance)
                        {
                            Console.WriteLine("\t\tUnable to withdraw due to Insufficient Fund. Please try again!");
                            withdraw = true;
                        }

                        // If Withdraw Amount < Balance, Proceed with Withdraw
                        else
                        {
                            // Update new Balance in the Text File
                            newBalance = oldBalance - withdrawAmount;
                            foundAccount[6] = "Balance:$" + newBalance.ToString();

                            // Update last 5 transactions
                            foundAccount[13] = foundAccount[12];
                            foundAccount[12] = foundAccount[11];
                            foundAccount[11] = foundAccount[10];
                            foundAccount[10] = foundAccount[9];
                            foundAccount[9] = DateTime.Now.ToShortDateString() + "| Withdraw $" + withdrawAmount.ToString("#,##0");

                            // Update text file
                            path = @"Accounts\" + fileName;
                            File.WriteAllLines(path, foundAccount, Encoding.UTF8);

                            // Display Success Message
                            Console.WriteLine("\t\tWithdraw Successful!");

                            // Break the Loop & Return to Main Menu
                            withdraw = false;
                            Console.WriteLine("\n\t\tPress any key to return to Main Menu");
                        }
                    }

                    // If Invalid Withdraw Amount, Display Message & Return to Withdraw Menu
                    else
                    {
                        withdraw = true;
                    }
                    Console.ReadKey();
                }

                // If Invalid Account Number, Display Message & Option to Retry
                else
                {
                    Console.Write("\n\n\t\tAccount Not Found!\n\t\tRetry (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();
                    withdraw = Retry(inputYesNo);
                }
            }
        }

        // Display & Email Account Statement
        public void DisplayAccountStatement()
        {
            bool displayAccountStatement = true;
            while (displayAccountStatement)
            {
                // Display Menu for Account Statement 
                Console.Clear();
                Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                Console.WriteLine("\t\t║                 STATEMENT                 ║");
                Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                Console.WriteLine("\t\t║                                           ║");
                Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
                Console.WriteLine("\t\t║                                           ║");
                Console.Write("\t\t║    Account Number: ");
                int AccountNoCursorX = Console.CursorTop;
                int AccountNoCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t    ║");
                Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
                Console.SetCursorPosition(AccountNoCursorY, AccountNoCursorX);
                inputNumber = Console.ReadLine().Trim();
                fileName = inputNumber + ".txt";

                // If Valid Account Number, Display Account Statement 
                if (CheckIntegerInput(inputNumber) && CheckAccountExist(fileName))
                {
                    // Get Account 
                    foundAccount = GetAccount(fileName);

                    // Display Account Statement
                    Console.WriteLine("\n\n\n\t\tAccount Found! The statement is displayed below...");
                    Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                    Console.WriteLine("\t\t║           SIMPLE BANKING SYSTEM           ║");
                    Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                    Console.WriteLine("\t\t║                                           ║");
                    Console.WriteLine("\t\t║             Account Statement             ║");
                    Console.WriteLine("\t\t║                                           ║"); ;
                    DisplayAccountDetails(foundAccount);

                    // Display last 5 transactions
                    Console.WriteLine("\n\n\n\t\t╔═══════════════════════════════════════════╗");
                    Console.WriteLine("\t\t║            Last 5 transactions            ║");
                    Console.WriteLine("\t\t║                                           ║");
                    Console.Write("\t\t║   1. ");
                    int FirstTransCursorX = Console.CursorTop;
                    int FirstTransCursorY = Console.CursorLeft;
                    Console.WriteLine("\t\t\t\t\t    ║");
                    Console.Write("\t\t║   2. ");
                    int SecondTransCursorX = Console.CursorTop;
                    int SecondTransCursorY = Console.CursorLeft;
                    Console.WriteLine("\t\t\t\t\t    ║");
                    Console.Write("\t\t║   3. ");
                    int ThirdTransCursorX = Console.CursorTop;
                    int ThirdTransCursorY = Console.CursorLeft;
                    Console.WriteLine("\t\t\t\t\t    ║");
                    Console.Write("\t\t║   4. ");
                    int FourthTransCursorX = Console.CursorTop;
                    int FourthTransCursorY = Console.CursorLeft;
                    Console.WriteLine("\t\t\t\t\t    ║");
                    Console.Write("\t\t║   5. ");
                    int FifthTransCursorX = Console.CursorTop;
                    int FifthTransCursorY = Console.CursorLeft;
                    Console.WriteLine("\t\t\t\t\t    ║");
                    Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
                    Console.SetCursorPosition(FirstTransCursorY, FirstTransCursorX);
                    Console.WriteLine(foundAccount[9]); // Display 1st Transaction
                    Console.SetCursorPosition(SecondTransCursorY, SecondTransCursorX);
                    Console.WriteLine(foundAccount[10]); // Display 2nd Transaction
                    Console.SetCursorPosition(ThirdTransCursorY, ThirdTransCursorX);
                    Console.WriteLine(foundAccount[11]); // Display 3rd Transaction
                    Console.SetCursorPosition(FourthTransCursorY, FourthTransCursorX);
                    Console.WriteLine(foundAccount[12]); // Display 4th Transaction
                    Console.SetCursorPosition(FifthTransCursorY, FifthTransCursorX);
                    Console.WriteLine(foundAccount[13]); // Display 5th Transaction

                    // Ask & Get User Input for Option to Email Statement
                    Console.Write("\n\n\t\tEmail Statement (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();

                    // If User chooses Yes, Send Account Statement to User Email
                    if (inputYesNo == "y")
                    {
                        try
                        {
                            // Set up Mail Message content
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("hangnhk.rec@gmail.com");
                            string receiverEmail = foundAccount[4].Split(':')[1];
                            mail.To.Add(receiverEmail);
                            mail.Subject = "Account Statement";
                            mail.Body = "The details of your Account Statement is as below." + Environment.NewLine + Environment.NewLine +
                                        foundAccount[5] + Environment.NewLine + foundAccount[6] + Environment.NewLine +
                                        foundAccount[0] + Environment.NewLine + foundAccount[1] + Environment.NewLine +
                                        foundAccount[2] + Environment.NewLine + foundAccount[3] + Environment.NewLine +
                                        foundAccount[4] + Environment.NewLine + Environment.NewLine +
                                        foundAccount[8] + Environment.NewLine + "1. " + foundAccount[9] + Environment.NewLine +
                                        "2. " + foundAccount[10] + Environment.NewLine + "3. " + foundAccount[11] + Environment.NewLine +
                                        "4. " + foundAccount[12] + Environment.NewLine + "5. " + foundAccount[13];

                            // Set up SMTP Client content
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential("hangnhk.rec@gmail.com", "@Password1234");
                            client.EnableSsl = true;

                            // Send Email via SMTP Server
                            client.Send(mail);

                            // Display Message if Email has been Sent
                            Console.WriteLine("\t\tEmail has been succesfully sent!");
                        }

                        // Display Message if Unable to Send Email
                        catch (Exception)
                        {
                            Console.WriteLine("\t\tUnable to send email! Please try again");
                        }
                    }

                    // Break the Loop & Return to Main Menu
                    displayAccountStatement = false;
                    Console.WriteLine("\n\t\tPress any key to return to Main Menu");
                    Console.ReadKey();
                }

                // If Invalid Account Number, Display Message & Option to Retry
                else
                {
                    Console.Write("\n\n\t\tAccount Not Found!\n\t\tRetry (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();
                    displayAccountStatement = Retry(inputYesNo);
                }
            }
        }

        // Delete Account
        public void DeleteAccount()
        {
            bool deleteAccount = true;
            while (deleteAccount)
            {
                // Display Delete Menu  
                Console.Clear();
                Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                Console.WriteLine("\t\t║             DELETE AN ACCOUNT             ║");
                Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                Console.WriteLine("\t\t║                                           ║");
                Console.WriteLine("\t\t║             ENTER THE DETAILS             ║");
                Console.WriteLine("\t\t║                                           ║");
                Console.Write("\t\t║    Account Number: ");
                int AccountNoCursorX = Console.CursorTop;
                int AccountNoCursorY = Console.CursorLeft;
                Console.WriteLine("\t\t\t    ║");
                Console.WriteLine("\t\t╚═══════════════════════════════════════════╝");
                Console.SetCursorPosition(AccountNoCursorY, AccountNoCursorX);
                // Get User Input for Account Number
                inputNumber = Console.ReadLine().Trim();
                fileName = inputNumber + ".txt";

                // If Valid Account Number, Display Account Details 
                if (CheckIntegerInput(inputNumber) && CheckAccountExist(fileName))
                {
                    // Get Account 
                    foundAccount = GetAccount(fileName);

                    // Display Account Details
                    Console.WriteLine("\n\n\n\t\tAccount Found! Details is displayed below...");
                    Console.WriteLine("\t\t╔═══════════════════════════════════════════╗");
                    Console.WriteLine("\t\t║              ACCOUNT DETAILS              ║");
                    Console.WriteLine("\t\t╠───────────────────────────────────────────╣");
                    Console.WriteLine("\t\t║                                           ║");
                    DisplayAccountDetails(foundAccount);
                    // Ask & Get User Input for Delete
                    Console.Write("\n\n\t\tDelete (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();

                    // If User chooses Yes, Delete the Account
                    if (inputYesNo == "y")
                    {
                        path = @"Accounts\" + fileName;
                        File.Delete(path);
                        Console.WriteLine("\n\n\t\tAccount Deleted!...");
                    }

                    // Break the Loop & Return to Main Menu
                    deleteAccount = false;
                    Console.WriteLine("\n\t\tPress any key to return to Main Menu");
                    Console.ReadKey();
                }

                // If Invalid Account Number, Display Message & Option to Retry
                else
                {
                    Console.Write("\n\n\t\tAccount Not Found!\n\t\tRetry (y/n)? ");
                    inputYesNo = Console.ReadLine().ToLower();
                    deleteAccount = Retry(inputYesNo);
                }
            }
        }
    }
}
