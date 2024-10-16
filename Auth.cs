using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _4digit_guess
{
    internal class Auth
    {
        List<User> users;
        string filePath;
        User currentUser;
        bool loging = true;

        public Auth(string filePath)
        {
            this.filePath = filePath;
            users = LoadUsers();
        }

        public void Authenth()
        {
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Авторизация");
            Console.WriteLine("0. Выход");
            string choice = Console.ReadLine();
            while (loging)
            {
                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "0":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }           
        }

        public void Register()
        {
            Console.Write("Введите имя пользователя: ");
            string username = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (users != null)
            {
                if (users.Exists(u => u.Username == username))
                {
                    Console.WriteLine("Пользователь с таким именем уже существует.");
                    return;
                }
            }
            User user = new User(username, password);
            currentUser = user;
            users.Add(user);
            SaveUsers();

            Console.WriteLine("Регистрация прошла успешно!");
            loging = false;
        }

        public void Login()
        {
            Console.Write("Введите имя пользователя: ");
            string username = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            User user = users.Find(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine("Авторизация прошла успешно!");
                loging = false;
            }
            else
            {
                Console.WriteLine("Неверные имя пользователя или пароль.");
            }
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            var json = File.ReadAllText(filePath);
            string text = File.ReadAllText(filePath);
            if (text.Length == 0)
            {
                return new List<User>();
            }
            return JsonConvert.DeserializeObject<List<User>>(text);
        }

        private void SaveUsers()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, users);
            }
        }

        public string GetCurrentUsername()
        {
            return currentUser.Username;
        }
    }

    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
