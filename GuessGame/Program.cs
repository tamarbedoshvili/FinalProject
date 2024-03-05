using System.ComponentModel.DataAnnotations;
using System.Net;

namespace GuessGame
{
    public class User
    {
        public string UserName {  get; set; }
      
        public int Score { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return $"{UserName} - {Score}";

        }


    }

        internal class Program

    {
        public const string FileLocation = "C:\\Users\\User\\source\\repos\\FinalProjects\\GuessGame\\UsersHistory.csv";
        public static List<User> Users { get; set; } = new List<User>();
        public static void Read() {
            using (StreamReader reader = new(FileLocation))
            {
                
                reader.ReadLine();

                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    User parsedUser = Parse(line);
                    Users.Add(parsedUser);
                }
            }
        }
        public static string GeneratePassword()
        {
            Random random = new Random();

            while (true)
            {
                string first = random.Next(0, 9).ToString();
                string lastThree = random.Next(100, 999).ToString();
                string finalVersion = first + lastThree;
                if (!Users.Select(x => x.Password).Contains(finalVersion))
                {
                   return finalVersion;
                   
                }

            }

        }
        public static bool Validation(string input)
        {
            try
            {
                int.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }



        }
        private static User Parse(string input)
        {
            var data = input.Split(',');

           

            var result = new User();
            result.UserName = data[0];
           
            result.Score = int.Parse(data[1]);
            result.Password= data[2];
           
           
            return result;
        }
        public static bool ValidateDifficulty(string input)
        {
            return (input == "easy" || input == "medium" || input == "hard");
           
        }
        private static string ToCSV(User model) => $"{model.UserName},{model.Score},{model.Password}";
        public static void Save()
        {
            File.WriteAllText(FileLocation, "Username,Score,Password");
            Users = Users.OrderByDescending(x => x.Score).DistinctBy(x => x.Password).ToList();
            foreach (var user in Users)
            {
                using (StreamWriter writer = new(FileLocation, append: true))
                {
                    
                    writer.Write($"\n{ToCSV(user)}");
                   
                }
            }
           
        }
       
        public static void AddNewUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Error");
            }
           
            
                Users.Add(user);
                Save();
            

           
           

        }

        static void Main(string[] args)
        {
            Read();

            Console.WriteLine("Log in(1) or Registration(2):");
            string answer = Console.ReadLine();
            while(!(answer =="1" || answer =="2"))
            {
                Console.WriteLine("Please choose between - Log in(1) or Registration(2): ");
                answer = Console.ReadLine();
            }
            User user = new User();
            if (answer == "2")
            {
                User newUser = new User();
                Console.WriteLine("Enter your name:");
                newUser.UserName = Console.ReadLine();



                newUser.Password = GeneratePassword();
                Console.WriteLine($"Your password is {newUser.Password}");


                user = newUser;
            }
            else if (answer == "1")
            {
                Console.WriteLine("Please Enter Your Name:");
                string name = Console.ReadLine();
                while (!Users.Select(x => x.UserName).Contains(name))
                {
                    Console.WriteLine("This name is not valid. Please try again:");
                    name = Console.ReadLine();

                }

                List<User> possibleUsers = Users.Where(user => user.UserName == name).ToList();
                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();
                while (!possibleUsers.Select(x => x.Password).Contains(password))
                {
                    Console.WriteLine("Incorrect password. Try again:");
                    password = Console.ReadLine();
                }
                user = possibleUsers.Where(x => x.Password == password).FirstOrDefault();

            }








            Console.WriteLine("Choose the difficulty of the game: easy, medium, hard.");
            string difficulty = Console.ReadLine();


            Random random = new Random();
            int actualNumber = 0;

            while (!ValidateDifficulty(difficulty.ToLowerInvariant()))
            {


                Console.WriteLine("Please choose from the levels given (easy, medium, hard)");
                difficulty = Console.ReadLine();

            }
            difficulty = difficulty.ToLowerInvariant();
            if (difficulty == "easy")
            {
                Console.WriteLine("Guess the number from the range - [1;15]:");
                actualNumber = random.Next(1, 15);
                //Console.WriteLine(actualNumber);
            }
            else if (difficulty == "medium")
            {
                Console.WriteLine("Guess the number from the range - [1;25]:");
                actualNumber = random.Next(1, 25);
                //Console.WriteLine(actualNumber);
            }
            else

            {
                Console.WriteLine("Guess the number from the range - [1;50]:");
                actualNumber = random.Next(1, 50);
                //Console.WriteLine(actualNumber);

            }
            

            int i = 0;
            for (i = 0; i < 10; i++)
            {
                string input = Console.ReadLine();
                while (!Validation(input))
                {
                    Console.WriteLine("Please enter numbers:");
                    input = Console.ReadLine();
                }
                int userNumber = int.Parse(input);
                if (difficulty == "easy")
                {
                    
                    while (userNumber > 15 || userNumber < 1)
                    {
                        Console.WriteLine("You should enter number from given range: [1;15]");
                        input = Console.ReadLine();
                        userNumber = int.Parse(input);
                    }
                }
                else if (difficulty == "medium")
                {
                    
                    while (userNumber > 25 || userNumber < 1)
                    {
                        Console.WriteLine("You should enter number from given range: [1;25]");
                        input = Console.ReadLine();
                        userNumber = int.Parse(input);
                    }
                }
                else if (difficulty == "hard")
                {
                    
                    while (userNumber > 50 || userNumber < 1)
                    {
                        Console.WriteLine("You should enter number from given range: [1;50]");
                        input = Console.ReadLine();
                        userNumber = int.Parse(input);
                    }
                }

                while (userNumber != actualNumber)
                {
                    if (userNumber < actualNumber && i < 9)
                    {
                        Console.WriteLine("try larger number");
                        break;
                    }
                    else if (userNumber > actualNumber && i < 9)
                    {
                        Console.WriteLine("try smaller number");
                        break;
                    }
                    else if (i == 9)
                    {
                        Console.WriteLine("YOU LOSE.");
                        break;
                    }
                }
                if (userNumber == actualNumber)
                {
                    Console.WriteLine("Congrats! YOU WIN.");

                    if (i == 0)
                    {
                        switch (difficulty)
                        {
                            case "easy":
                                user.Score = 300; break;

                            case "medium":
                                user.Score = 500; break;
                            case "hard":
                                user.Score = 1000; break;
                            default:
                                Console.WriteLine("error");
                                break;

                        }

                    }
                    if (i != 0)
                    {
                        switch (difficulty)
                        {


                            case "easy":
                                user.Score = 250 - 10 * i; break;

                            case "medium":
                                user.Score = 400 - 10 * i; break;
                            case "hard":
                                user.Score = 900 - 10 * i; break;
                            default:
                                Console.WriteLine("error");
                                break;

                        }


                    }

                    break;
                }





            }
            Console.WriteLine($"{user.UserName} - Your score is {user.Score}");

            AddNewUser(user);

            if (Users.Count < 10)
            {
                foreach (var x in Users)
                {
                    Console.WriteLine(x.ToString());
                }
            }
            else
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.WriteLine(Users[k].ToString());
                }
            }
        }
       
    }
}