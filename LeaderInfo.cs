using System;

namespace _4digit_guess
{
    /// <summary>
    /// Класс, содержащий информацию об
    /// игроках из таблицы лидеров
    /// </summary>
    public class LeaderInfo
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
