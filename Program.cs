using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _4digit_guess
{
    internal class Program
    {
        static void Main()
        {
            int secretNumber = GenNumber();
            Console.WriteLine(secretNumber);
            int attempts = 0;

            Auth userAuth = new Auth("logins.json");
            userAuth.Authenth();

            Console.WriteLine("Угадайте четырехзначное число (без 0 и повторяющихся цифр):");
            while (true)
            {                
                Console.Write("Введите ваше число: ");
                string userInput = Console.ReadLine();

                if (HasZero(int.Parse(userInput)) || (userInput.Length != 4))
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите четырехзначное число без 0 и повторяющихся цифр.");
                    continue;
                }

                attempts++;
                int correctPosition = 0, correctDigit = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (userInput[i] == secretNumber.ToString()[i])
                    {
                        correctPosition++;
                    }
                    if (secretNumber.ToString().Contains(userInput[i]))
                    {
                        correctDigit++;
                    }
                }

                if (correctPosition == 4)
                {
                    Console.WriteLine($"Вы угадали число {secretNumber} за {attempts} попыток.");
                    UpdateLeaderboard(userAuth.GetCurrentUsername(), attempts);
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine($"Цифры на своих позициях: {correctPosition}, цифры в числе: {correctDigit}");
                }
            }
        }

        public static int GenNumber()
        {
            Random random = new Random();
            int number;
            do
            {
                number = random.Next(1000, 10000);
            } while (HasZero(number) || HasDuplicateDigits(number));

            return number;
        }

        public static bool HasZero(int number)
        {
            return number.ToString().Contains('0');
        }

        public static bool HasDuplicateDigits(int number)
        {
            string numStr = number.ToString();
            return numStr.Length != new HashSet<char>(numStr).Count;
        }

        public static void UpdateLeaderboard(string playerName, int attempts)
        {
            string filePath = "Leaderboard.txt";
            List<LeaderInfo> leaderboard = new List<LeaderInfo>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.Any())
                    {
                        string[] values = line.Split(',');
                        leaderboard.Add(new LeaderInfo { Name = values[0], Score = int.Parse(values[1]), Date = DateTime.Parse(values[2])});
                    }                                    
                }               
            }

            leaderboard.Add(new LeaderInfo { Name = playerName, Score = attempts, Date = DateTime.Now });

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (LeaderInfo leader in leaderboard)
                {
                    double topPercentage = CalculateTopPercentage(leaderboard, leader.Score);
                    writer.WriteLine($"{leader.Name}, {leader.Score}, {leader.Date}, {topPercentage}%.");
                }
            }

            Console.WriteLine($"Ваш результат записан в таблице лидеров.");
        }

        public static double CalculateTopPercentage(List<LeaderInfo> leaderboard, int newAttempts)
        {
            double totalPlayers = leaderboard.Count;
            double betterPlayers = leaderboard.Count(x => x.Score < newAttempts);
            double tiePlayers = leaderboard.Count(x => x.Score == newAttempts);
            double percentage = 0;
            if (totalPlayers > 1)
            {
                percentage = ((betterPlayers+tiePlayers) / (totalPlayers)) * 100;
            }
            else { percentage = 1; }
            return percentage;
        }

        public class LeaderInfo
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
