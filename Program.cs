using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _4digit_guess
{
    /// <summary>
    /// Программа, реализующая игру "Быки и Коровы"
    /// в консольном виде
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Основной метод, в котором происходит
        /// отгадывание загаданного числа
        /// </summary>
        static void Main()
        {
            // Генерация случайного числа
            int _secretNumber = GenNumber();

#if DEBUG
            Console.WriteLine(_secretNumber);
#endif
            Auth _userAuth = new Auth("Файлы/logins.json");
            _userAuth.Authenth();

            int _attempts = 0;
            Console.WriteLine("Угадайте четырехзначное число (без 0 и повторяющихся цифр):");
            while (true)
            {                
                Console.Write("Введите ваше число: ");
                string _userInput = Console.ReadLine();
                HashSet<string> _contains = new HashSet<string>(); 
                if (HasZero(int.Parse(_userInput)) || (_userInput.Length != 4))
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите четырехзначное число без 0 и повторяющихся цифр.");
                    continue;
                }

                _attempts++;
                int _correctPosition = 0, _correctDigit = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (_userInput[i] == _secretNumber.ToString()[i])
                    {
                        _correctPosition++;
                    }
                    if (_secretNumber.ToString().Contains(_userInput[i]))
                    {
                        _contains.Add(_userInput[i].ToString());
                    }
                }
                _correctDigit = _contains.Count;
                if (_correctPosition == 4)
                {
                    Console.WriteLine($"Вы угадали число {_secretNumber} за {_attempts} попыток.");
                    UpdateLeaderboard(_userAuth.GetCurrentUsername(), _attempts);
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine($"Цифры на своих позициях: {_correctPosition}, цифры в числе: {_correctDigit}");
                }
            }
        }

        /// <summary>
        /// Метод, реализующий генерацию числа без
        /// нулей и повторяющихся цифр от 1000 до 9999
        /// </summary>
        /// <returns>Число от 1000 до 9999 без 0 и повт. цифр</returns>
        public static int GenNumber()
        {
            Random _random = new Random();
            int _number;
            do
            {
                _number = _random.Next(1000, 10000);
            } while (HasZero(_number) || HasDuplicateDigits(_number));

            return _number;
        }

        /// <summary>
        /// Проверка числа на наличие нуля
        /// </summary>
        /// <param name="number">Загаданное число</param>
        /// <returns>True, если есть 0; false, если 0 отсутствует</returns>
        public static bool HasZero(int _number)
        {
            return _number.ToString().Contains('0');
        }

        /// <summary>
        /// Проверка числа на повторяющиеся цифры
        /// </summary>
        /// <param name="number">Загаданное число</param>
        /// <returns>True, если есть повторяющиеся цифры; false, 
        /// если нет повторяющихся цифр</returns>
        public static bool HasDuplicateDigits(int _number)
        {
            string _numStr = _number.ToString();
            return _numStr.Length != new HashSet<char>(_numStr).Count;
        }

        /// <summary>
        /// Метод, динамически обновляющий таблицу
        /// лидеров и записывающий ее в файл
        /// </summary>
        /// <param name="playerName">Имя текущего игрока</param>
        /// <param name="attempts">Количество попыток текущего игрока</param>
        public static void UpdateLeaderboard(string _playerName, int _attempts)
        {
            string _filePath = "Файлы/Leaderboard.txt";
            List<LeaderInfo> _leaderboard = new List<LeaderInfo>();

            // Чтение списка лидеров
            if (File.Exists(_filePath))
            {
                string[] _lines = File.ReadAllLines(_filePath);
                foreach (string _line in _lines)
                {
                    if (_line.Any())
                    {
                        string[] _values = _line.Split(',');
                        _leaderboard.Add(new LeaderInfo { Name = _values[0], Score = int.Parse(_values[1]), Date = DateTime.Parse(_values[2])});
                    }                                    
                }               
            }

            // Добавление текущего игрока в список лидеров
            _leaderboard.Add(new LeaderInfo { Name = _playerName, Score = _attempts, Date = DateTime.Now });

            // Запись обновленной таблицы лидеров в файл
            using (StreamWriter _writer = new StreamWriter(_filePath))
            {
                foreach (LeaderInfo _leader in _leaderboard)
                {
                    double _topPercentage = CalculateTopPercentage(_leaderboard, _leader.Score);
                    _writer.WriteLine($"{_leader.Name}, {_leader.Score}, {_leader.Date}, {_topPercentage}%.");
                }
            }

            Console.WriteLine($"Ваш результат записан в таблице лидеров.");
        }

        /// <summary>
        /// Метод, подсчитывающий топ %, в который
        /// попал игрок после завершения игры
        /// </summary>
        /// <param name="_leaderboard">Список всех лидеров</param>
        /// <param name="_newAttempts">Количество попыток текущего игрока</param>
        /// <returns>Топ % текущего игрока</returns>
        public static double CalculateTopPercentage(List<LeaderInfo> _leaderboard, int _newAttempts)
        {
            double _totalPlayers = _leaderboard.Count;

            // Подсчет игроков, у которых число попыток меньше
            double _betterPlayers = _leaderboard.Count(x => x.Score < _newAttempts);

            // Подсчет игроков, у которых такое же число попыток
            double _tiePlayers = _leaderboard.Count(x => x.Score == _newAttempts);

            // Подсчет процента топа
            double _percentage = 0;
            if (_totalPlayers > 1)
            {
                _percentage = ((_betterPlayers+_tiePlayers) / (_totalPlayers)) * 100;
            }
            else { _percentage = 1; }
            return _percentage;
        }
    }
}
