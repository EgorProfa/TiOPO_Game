# **Тема работы: создание аналога игры "Быки и коровы" на C#**
## Работу выполнил: Профатило Егор, группа СО231КОБ
### Рассмотрим логику генерации числа. 
Мы будем играть рассматривать только те числа, у которых все цифры разные и отсутствует 0 в записи. Это поможет упростить игру для пользователя. Генерацию числа будем производить при помощи класса Random, а проверку на наличие 0 при помощи метода Contains. Наличие же повторяющихся цифр в числе реализуем с использованием HashSet для сравнения длины исходного числа и размера HashSet'а, полученного из данного числа (HashSet содержит только уникальные значения). Указанная логика представлена в коде:

```csharp
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
public static bool HasZero(int _number)
{
    return _number.ToString().Contains('0');
}
public static bool HasDuplicateDigits(int _number)
{
    string _numStr = _number.ToString();
    return _numStr.Length != new HashSet<char>(_numStr).Count;
}
```
В следующем блоке кода представлена логика непосредственного отгадывания числа:
```csharp
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
```
Имеем бесконечный цикл, который проверяет правильность введенного числа и при вводе числа пользователем выводит количество цифр на своем месте и общее количество отгаданных цифр загаданного числа. Работа прекращается, когда пользователь отгадывает число.

Ниже представлена логика записи результата игрока в файл и чтения из файла результатов предыдущих игроков для составления статистики. Особый метод CalculateTopPercentage используется для подсчета "топа" игрока среди всех игроков.
```csharp
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
```

##### Это основная логика данной игры!
