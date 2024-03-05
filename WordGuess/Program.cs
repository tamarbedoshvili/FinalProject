using System.Text;
using System.Xml;

namespace WordGuess
{
    public class User
    {
        public string UserName { get; set; }
        public int Score { get; set; }

        public string Password { get; set; }
        public override string ToString()
        {
            return $"{UserName} - {Score}";

        }


    }
    public static class WordsGame
    {
        public static List<string> Words = new List<string>
        {
            "apple", "banana", "orange", "grape", "kiwi", "strawberry", "pineapple", "blueberry", "peach", "watermelon"
        };
        public static string RandomWord()
        {
            Random random = new Random();
            int index = random.Next(0,9) ;
            return Words[index];
        }
        public static string Visualize(string text)
        {
            string result = string.Empty;
            foreach (char letter in text) 
            {
                result += "- ";
            }
            return result;
        }

    }

    internal class Program
    {
        public const string fileLocation = "C:\\Users\\User\\source\\repos\\FinalProjects\\WordGuess\\UsersHistory.xml";
        public static List<User> data = new();

        public static void Read()
        {
            data = Parse(File.ReadAllText(fileLocation));
        }
        static void ReplaceUsersInXml(string filePath)
        {
            
            XmlDocument doc = new XmlDocument();

          
            doc.Load(filePath);

           
            XmlElement root = doc.DocumentElement;

           
            root.RemoveAll();

            foreach (User user in data)
            {
                XmlElement userElement = doc.CreateElement("User");

                XmlElement score = doc.CreateElement("Score");
                score.InnerText = user.Score.ToString();

                XmlElement userName = doc.CreateElement("UserName");
                userName.InnerText = user.UserName;
                XmlElement password = doc.CreateElement("Password");
                password.InnerText = user.Password;



                userElement.AppendChild(score);
                userElement.AppendChild(userName);
                userElement.AppendChild(password);


               

                root.AppendChild(userElement);
            }

            
            doc.Save(filePath);

            
        }
        public static string GeneratePassword()
        {
            Random random = new Random();

            while (true)
            {
                string first = random.Next(0, 9).ToString();
                string lastThree = random.Next(100, 999).ToString();
                string finalVersion = first + lastThree;
                if (!data.Select(x => x.Password).Contains(finalVersion))
                {
                    return finalVersion;
                    break;
                }

            }

        }
        private static List<User> Parse(string input)
        {
            List<User> result = new();

            XmlDocument xDoc = new();
            xDoc.LoadXml(input);

            XmlNodeList rowNodes = xDoc.SelectNodes("//User");

            foreach (XmlNode rowNode in rowNodes)
            {
                User User = new();
                User.UserName = rowNode.SelectSingleNode("UserName").InnerText;
                User.Score = int.Parse(rowNode.SelectSingleNode("Score").InnerText);
                User.Password = rowNode.SelectSingleNode("Password").InnerText;
               

                result.Add(User);
            }

            return result;
        }

        public static void ToXML(User User)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileLocation);
            XmlElement xmlUser = xDoc.CreateElement("User");
            XmlElement xmlUserName = xDoc.CreateElement("Name");
            xmlUserName.InnerText = User.UserName.ToString();
            xmlUser.AppendChild(xmlUserName);
            XmlElement xmlUserScore = xDoc.CreateElement("Score");
            xmlUserScore.InnerText = User.Score.ToString();
            xmlUser.AppendChild(xmlUserScore);
            XmlElement xmlUserPassword = xDoc.CreateElement("Password");
            xmlUserPassword.InnerText = User.Password.ToString();
            xmlUser.AppendChild(xmlUserPassword);


            XmlElement root = xDoc.DocumentElement;
            root.AppendChild(xmlUser);
            xDoc.Save(fileLocation);




        }
        public static void AddNewUser(User User)
        {

          
            data.Add(User);
            data = data.OrderByDescending(x => x.Score).DistinctBy(x=>x.Password).ToList();
            ReplaceUsersInXml(fileLocation);

           
        }
        public static void PrintFirstTen()
        {
            if (data.Count < 10)
            {
                foreach (var x in data)
                {
                    Console.WriteLine(x.ToString());
                }
            }
            else
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.WriteLine(data[k].ToString());
                }
            }
        }
        static void Main(string[] args)
        {
            Read(); 
            

            Console.WriteLine("Log in(1) or Registration(2):");
            string answer = Console.ReadLine();
            while (!(answer == "1" || answer == "2"))
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
                while (!data.Select(x => x.UserName).Contains(name))
                {
                    Console.WriteLine("This name is not valid. Please try again:");
                    name = Console.ReadLine();

                }

                List<User> possibleUsers = data.Where(user => user.UserName == name).ToList();
                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();
                while (!possibleUsers.Select(x => x.Password).Contains(password))
                {
                    Console.WriteLine("Incorrect password. Try again:");
                    password = Console.ReadLine();
                }
                user.UserName = possibleUsers.Where(x => x.Password == password).FirstOrDefault().UserName;
                user.Password = password;
            }
            Console.WriteLine("Guess the word. You have 6 tries.");
            string word = WordsGame.RandomWord();
            Console.WriteLine(WordsGame.Visualize(word));
            Console.WriteLine("Enter the letter:");
            int trial = 6;
            StringBuilder guessedWord = new StringBuilder(WordsGame.Visualize(word));
            while (trial > 0)
            {
                user.Score = 1000 - (6 - trial) * 100;
                string input = Console.ReadLine(); 
                if (input == word) { Console.WriteLine("YOU WON"); 
                    AddNewUser(user);
                    Console.WriteLine($"Your score is - {user.Score}");
                    PrintFirstTen();
                    return; }
                else if(input.Length>1 && input!= word) { Console.WriteLine("YOU LOST");
                    user.Score = 0;
                    AddNewUser(user);
                    Console.WriteLine($"Your score is - {user.Score}");
                    PrintFirstTen();
                    return; }
                while (input.Length == 0)
                {
                    Console.WriteLine("Please enter at least one letter");
                    input = Console.ReadLine();
                }
                if (word.Contains(input))
                {
                   for(int i = 0; i< word.Length; i++)
                    {
                        if (word[i].ToString() == input)
                        {
                            guessedWord[(2 * i)] = char.Parse(input);
                        }
                    }
                    Console.WriteLine(guessedWord);
                }
                else
                {
                    trial--;
                    Console.WriteLine($"You have {trial} more tries");
                }
                if (!guessedWord.ToString().Contains("-"))
                {
                    AddNewUser(user);
                    Console.WriteLine("YOU WON");
                    Console.WriteLine($"Your score is - {user.Score}");
                    PrintFirstTen();
                  
                    return;
                }

                
            }
            user.Score = 0;
            AddNewUser(user);
            Console.WriteLine("YOU LOST");
            Console.WriteLine($"Your score is - {user.Score}");
            PrintFirstTen();




        }
    }
}
