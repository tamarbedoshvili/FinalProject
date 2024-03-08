using System.Text.Json;
using System.Text.RegularExpressions;

namespace ATM_account
{
    public class Operation
    {
      
        public string Message { get; set; }

    }
    public class Client
    {
        public const string FileLocationClientsHistory = @"../../../ClientsHistory.json";
        public const string FileLocationClients = @"../../../Clients.json";
        public static List<Client> Clients = new();
        public static List<Operation> Operations = new();
        

        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Id { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
        public string PrivateNumber { get; set; }

        public void AddNewClient(Client model)
        {
            model.Id = Clients.Max(x => model.Id) + 1;
            Clients.Add(model);
        }
        public static bool Validation(string input)


        {
            try
            {
                long.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }



        }

        public static void Read()
        {
            Operations = ParseOperation(File.ReadAllText(FileLocationClientsHistory));

            Clients = Parse(File.ReadAllText(FileLocationClients));
        }
        private static List<Operation> ParseOperation(string input)
        {
            List<Operation> result = JsonSerializer.Deserialize<List<Operation>>(input);

            if (result == null)
            {
                throw new FormatException("Invalid format while deserialization");
            }

            return result;
        }
        private static List<Client> Parse(string input)
        {
            List<Client> result = JsonSerializer.Deserialize<List<Client>>(input);

            if (result == null)
            {
                throw new FormatException("Invalid format while deserialization");
            }

            return result;
        }



        public static void SaveOperation(Operation operation)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            Operations.Add(operation);
            string json = JsonSerializer.Serialize(Operations,options);
            File.WriteAllText(FileLocationClientsHistory, json);
        }
        public static void Save(Client model)
        {
            if (model.Id != null)
            {
                Clients.Where(y => y.PrivateNumber == model.PrivateNumber).FirstOrDefault().Balance = model.Balance;

            }
            else
            {

                if (Clients.Count == 0)
                {
                    model.Id = 1;
                }
                else
                {
                    model.Id = Clients.Max(x => x.Id) + 1;
                }
                Clients.Add(model);
            }


            string json = JsonSerializer.Serialize(Clients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileLocationClients, json);

        }

        public void GeneratePassword()
        {
            Random random = new Random();

            while (true)
            {
                string first = random.Next(0, 9).ToString();
                string lastThree = random.Next(100, 999).ToString();
                string finalVersion = first + lastThree;
                if (!Clients.Select(x => x.Password).Contains(finalVersion))
                {
                    Password = finalVersion;
                    break;
                }

            }

        }
        public void Deposit(int amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"You have deposited {amount} Lari {DateTime.Now} ");
                Operation operation = new Operation();
                operation.Message = $"მომხმარებელმა სახელად {Name} {Surname} - შეავსო ბალანსი {amount} ლარით. : {DateTime.Now} -ში. მისი მოქმედი ბალანსი შეადგენს {Balance} ლარს.";

                SaveOperation(operation);
            }
            else { Console.WriteLine("you can deposit only positive amount."); }

        }
        public void Withdraw(int amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("you can withdraw only positive amount.");
            }
            else if (amount < Balance)
            {
                Balance -= amount;
                Console.WriteLine($"You have withdrawn {amount} Lari {DateTime.Now} ");
                Operation operation = new Operation();
                operation.Message = $"მომხმარებელმა სახელად {Name} {Surname} - გაანაღდა {amount} ლარი. : {DateTime.Now} -ში. მისი მოქმედი ბალანსი შეადგენს {Balance} ლარს.";

                SaveOperation(operation);
            }
            else { Console.WriteLine("insufficient amount of money."); }

           



        }
        public void CheckBalance()
        {
            Operation operation = new Operation();
            operation.Message = $"მომხმარებელმა სახელად {Name} {Surname} - შეამოწმა ბალანსი : {DateTime.Now} -ში";
            Console.WriteLine($"you have {Balance} Lari on your balance. {DateTime.Now}");
            SaveOperation(operation);
            
        }

    }
    internal class Program
    {


        static void Main(string[] args)
        {
            Client.Read();
            Console.WriteLine("Log in(1) or Registration(2):");
            string answer = Console.ReadLine();
            Client client = new Client();
            if (answer == "1")
            {
                Console.WriteLine("Enter your private number:");
                string privateNumber = Console.ReadLine();
                while (!Client.Clients.Select(x => x.PrivateNumber).Contains(privateNumber))
                {
                    Console.WriteLine("This private number is not valid. Please try again:");
                    privateNumber = Console.ReadLine();

                }
                Client existingClient = Client.Clients.Where(client => client.PrivateNumber == privateNumber).FirstOrDefault();

                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();
                while (password != existingClient.Password)
                {
                    Console.WriteLine("Incorrect password. Try again:");
                    password = Console.ReadLine();
                }

                client = existingClient;

            }
            else if (answer == "2")
            {
                Client newClient = new Client();
                Console.WriteLine("Enter your name:");
                newClient.Name = Console.ReadLine();
                Console.WriteLine("Enter your surname:");
                newClient.Surname = Console.ReadLine();
                Console.WriteLine("Enter your private number:");
                newClient.PrivateNumber = Console.ReadLine();
                while (newClient.PrivateNumber.Length != 11)
                {
                    Console.WriteLine("Please Enter 11-digit number:");
                    newClient.PrivateNumber = Console.ReadLine();
                }
                while (!Client.Validation(newClient.PrivateNumber))
                {
                    Console.WriteLine("Please Enter only numbers:");
                    newClient.PrivateNumber = Console.ReadLine();
                }
                if(Client.Clients.Select(x=>x.PrivateNumber).Contains(newClient.PrivateNumber))
                {
                    Console.WriteLine("Already registered with this Identification Number.");
                    return;
                }

                newClient.GeneratePassword();
                Console.WriteLine($"Your password is {newClient.Password}");
                newClient.Balance = 0;
                Client.Save(newClient);
                client = newClient;
            }

            while (true)
            {
                Console.WriteLine("Options: (1) Deposit; (2) Withdraw; (3) Check Balance; (4) Log out.");
                string option = Console.ReadLine();
                if (option == "1")
                {
                    Console.WriteLine("Enter amount of money:");
                    string amount = Console.ReadLine();
                    while (!Client.Validation(amount))
                    {
                        Console.WriteLine("Enter only digits:");
                        amount = Console.ReadLine();
                    }
                    int money = int.Parse(amount);
                    client.Deposit(money);
                    Client.Save(client);

                }
                else if (option == "2")
                {
                    Console.WriteLine("Enter amount of money:");
                    string amount = Console.ReadLine();
                    while (!Client.Validation(amount))
                    {
                        Console.WriteLine("Enter only digits:");
                        amount = Console.ReadLine();
                    }
                    int money = int.Parse(amount);
                    client.Withdraw(money);
                    Client.Save(client);
                }
                else if (option == "3")
                {
                    client.CheckBalance();

                }
                else if (option == "4")
                {
                    return;
                }


            }



        }
    }
}





