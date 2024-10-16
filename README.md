<h1 align="center">Работу выполнил <a href="https://daniilshat.ru/" target="_blank">Егор Профатило</a> </h1>
<h2 align="center">Тема работы: создание аналога игры "Быки и коровы" на C#</h2>
<h4 align ="center">Рассмотрим логику генерации числа. Мы будем играть рассматривать только те числа, у которых все цифры разные и отсутствует 0 в записи. Это поможет упростить игру для пользователя. Генерацию числа будем производить при помощи класса Random, а проверку на наличие 0 при помощи метода Contains. Наличие же повторяющихся цифр в числе реализуем с использованием HashSet для сравнения длины исходного числа и размера HashSet'а, полученного из данного числа (HashSet содержит только уникальные значения). Указанная логика представлена на рисунке:
<br> <img src="pictures/number.png" align="center"> <br>
<br>А на следующей картинке представлена логика непосредственного отгадывания числа:
<br> <img src="pictures/cycle.png" align="center"> <br>
Имеем бесконечный цикл, который проверяет правильность введенного числа и при вводе числа пользователем выводит количество цифр на своем месте и общее количество отгаданных цифр загаданного числа. Работа прекращается, когда пользователь отгадывает число. <br> <br>
Ниже представлена логика записи результата игрока в файл и чтения из файла результатов предыдущих игроков для составления статистики. Особый метод CalculateTopPercentage используется для подсчета "топа" игрока среди всех игроков.
<br> <img src="pictures/file.png" align="center"> <br> </h4>
<h3 align ="center"> Это основная логика данной игры! Группа: СО231КОБ. <h3> 
