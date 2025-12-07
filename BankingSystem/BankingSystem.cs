using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace BankingSystem
{
    internal class BankingSystem
    {
        private static string dataFile = "accounts.json";
        private static List<Card> records = new List<Card>();

        private static void CreateAccount(List<Card> records)
        {
            Console.Write("Name And Surname> ");
            string? user = Console.ReadLine();

            var random = new Random();
            var cardId = new string(Enumerable.Repeat("0123456789", 12).Select(s => s[random.Next(s.Length)]).ToArray());
            string pin = new string(Enumerable.Repeat("0123456789", 4).Select(s => s[random.Next(s.Length)]).ToArray());
            records.Add(new Card() { AccountId = cardId, User = user!, Balance = 0.0m, Pin = pin });
            SaveAccounts();

            Console.WriteLine("Account Created. Details:");
            Console.WriteLine($"\tAccountId: {cardId}");
            Console.WriteLine("Press enter to continue...");
            Console.WriteLine($"Your Pin is: {pin}");
            Console.ReadLine();
            Console.Clear();
        }

        private static void AccountController(List<Card> records)
        {
            Console.Write("Enter Bank AccountId: ");
            string? input = Console.ReadLine();
            Console.Write("Enter your Pin: ");
            string? pintup = Console.ReadLine() ?? string.Empty;
            Card? account = records.Find(a => a.AccountId == input);
            Card? pin = records.Find(a => a.Pin == pintup);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                Console.WriteLine("Press Enter to go back...");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            if(pin == null)
            {
                Console.WriteLine("Pin is incorrect");
                Console.WriteLine("Press Enter to go back...");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Welcome {account.User}. Your Current Balance: {account.Balance}");
            bool atmClose = false;

            while (!atmClose)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("\t1. View Balance");
                Console.WriteLine("\t2. Deposit Money");
                Console.WriteLine("\t3. Withdraw Money");
                Console.WriteLine("\t4. Quit");
                Console.Write("Enter Operation> ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine($"Balance: {account.Balance}");
                        break;

                    case "2":
                        Console.Write("Enter Quantity> ");
                        input = Console.ReadLine();
                        if (decimal.TryParse(input, out decimal deposit))
                        {
                            account.Balance += deposit;
                            Console.WriteLine($"New Balance: {account.Balance}");
                            SaveAccounts();
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter Quantity> ");
                        input = Console.ReadLine();
                        if (decimal.TryParse(input, out decimal withdraw))
                        {
                            if (withdraw <= account.Balance)
                            {
                                account.Balance -= withdraw;
                                Console.WriteLine($"New Balance: {account.Balance}");
                                SaveAccounts();
                            }
                            else
                            {
                                Console.WriteLine("Not enough balance.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid amount.");
                        }
                        break;

                    case "4":
                        atmClose = true;
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private static void SaveAccounts()
        {
            string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dataFile, json);
        }

        private static void LoadAccounts()
        {
            if (File.Exists(dataFile))
            {
                try
                {
                    string json = File.ReadAllText(dataFile);

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        Console.WriteLine("The JSON file is empty.");
                        return;
                    }

                    var loaded = JsonSerializer.Deserialize<List<Card>>(json);

                    if (loaded != null)
                        records = loaded;
                    else
                        Console.WriteLine("Failed to deserialize the data.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading accounts: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No account data file found.");
            }
        }
    }
}



