using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _4digit_guess
{
    /// <summary>
    /// Класс для реализации логики авторизации в приложении
    /// </summary>
    internal class Auth
    {
        private List<User> users;
        private string filePath;
        private User currentUser;
        private bool loging = true;

        /// <summary>
        /// Конструктор класса, автоматически загружающий всех пользователей
        /// при помощи метода LoadUsers()
        /// </summary>
        /// <param name="_filePath">Путь к списку пользователей</param>
        public Auth(string _filePath)
        {
            filePath = _filePath;
            users = LoadUsers(); 
        }

        /// <summary>
        /// Основной метод, в котором происходит цикличная
        /// авторизация пользователя пока не произойдет процесс входа
        /// или регистрации
        /// </summary>
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

        /// <summary>
        /// Метод, необходимый для регистрации
        /// нового пользователя
        /// </summary>
        public void Register()
        {
            Console.Write("Введите имя пользователя: ");
            string _username = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string _password = Console.ReadLine();

            if (users != null)
            {
                // Если такой пользователь уже существует
                if (users.Exists(u => u.Username == _username))
                {
                    Console.WriteLine("Пользователь с таким именем уже существует.");
                    return;
                }
            }
            // Создаем и добавляем нового пользователя
            User _user = new User(_username, _password);
            currentUser = _user;
            users.Add(_user);
            SaveUsers();

            Console.WriteLine("Регистрация прошла успешно!");
            loging = false;
        }

        /// <summary>
        /// Метод, реализующий авторизацию для уже
        /// существующего пользователя
        /// </summary>
        public void Login()
        {
            Console.Write("Введите имя пользователя: ");
            string _username = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string _password = Console.ReadLine();

            // Находим пользователя по логину и паролю
            User _user = users.Find(u => u.Username == _username && u.Password == _password);
            if (_user != null)
            {
                currentUser = _user;
                Console.WriteLine("Авторизация прошла успешно!");
                loging = false;
            }
            else
            {
                Console.WriteLine("Неверные имя пользователя или пароль.");
            }
        }

        /// <summary>
        /// Метод, реализующий десериализацию пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <exception cref="FileNotFoundException">Ошибка: файл не найден</exception>
        private List<User> LoadUsers()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            string _json = File.ReadAllText(filePath);
            string text = File.ReadAllText(filePath);
            if (text.Length == 0)
            {
                return new List<User>();
            }
            return JsonConvert.DeserializeObject<List<User>>(text);
        }

        /// <summary>
        /// Метод, реализующий сериализацию списка
        /// пользователей в JSON файл
        /// </summary>
        private void SaveUsers()
        {
            JsonSerializer _serializer = new JsonSerializer();
            using (StreamWriter _sw = new StreamWriter(filePath))
            using (JsonWriter _writer = new JsonTextWriter(_sw))
            {
                _serializer.Serialize(_writer, users);
            }
        }

        /// <summary>
        /// Получение имени текущего пользователя
        /// </summary>
        /// <returns>Имя текущего игрока</returns>
        public string GetCurrentUsername()
        {
            return currentUser.Username;
        }
    }
}
