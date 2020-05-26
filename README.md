# ConnectFourResearch

Исследование игры [Connect Four](https://ru.wikipedia.org/wiki/%D0%A7%D0%B5%D1%82%D1%8B%D1%80%D0%B5_%D0%B2_%D1%80%D1%8F%D0%B4)

## Реализация

Для данного исследования мы написали свою
[реализацию игры](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs)
"Connect four". Мы использовали максимально эффективное использование данных для минимазии скорости работы логики (чтобы проверять именно скорость работы алгоритмов, а не логики игры). Вот несколько основных приемов, которые мы использовали:
 - Поле хранится в виде 2 `ulong`
 [полей](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs#L23): позиции фишек желтого игрока и позиции фишек красного игрока. Бит 1 означает наличие фишки, иначе - ее отсутствие. Такой подход
 [позволяет перебрать все линии](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs#L88)
из длины `N` за практически константное время (при учете, что нет смысла проверять длину меньше 2 и больше 4) - Делается `N` битовых сдвигов с `&` операцией и далее идет [подсчет количества установленых бит](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Extensions/UlongExtensions.cs#L5)
~~~
  6 13 20 27 34 41 48   55 62  <-  Дополнительный ряд для того, чтобы 
+---------------------+            не учитывать следующий столбец при битовом сдвиге
| 5 12 19 26 33 40 47 | 54 61      (всегда содержит 0)
| 4 11 18 25 32 39 46 | 53 60
| 3 10 17 24 31 38 45 | 52 59
| 2  9 16 23 30 37 44 | 51 58
| 1  8 15 22 29 36 43 | 50 57
| 0  7 14 21 28 35 42 | 49 56 63  
+---------------------+
~~~
 - Дополнительно используется [массив с указателями на верхнюю фишку в столбике](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs#L25), позволяет быстро определять место выставления фишки и возможность ее установления вообще.
 - Реализован
[хэш Зобриста](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs#L122)
и функция 
[Equals](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Board.cs#L50)
(которая тоже очень быстрая за счет хранения поля в двух числовых переменных)
 
## Проверка реализации

На реализацию алгоритма написаны 
[тесты](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Tests/BoardTests.cs)
 - Они покрывают большее количество игровых случаев
 - Поверяют функции сравнения
 - Тестируют правильное завершение игры
 
Они позволили нам быть уверенными в реализации логики и сосредоточить внимание на алгоритмах

## Интерфейс
 
Мы сделали 2 логгера: 
[для консоли](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Logging/ConsoleBoardLogger.cs)
и 
[для записи в файл](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Logging/FileBoardLogger.cs) - кроме поля они логируют варинты ходов и счет для удобного дебага  
Реализовали универсальный
[Controller](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Controller.cs)
который принимает два
[ISolver](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Algorithms/ISolver.cs) и умеет симулировать игру  
Так же сделали консольный интерфейс для пользователя в виде [реализации ISolver](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/ConsoleSolver.cs)

## Алгоритмы

Мы реализовали различные вариации MiniMax алгоритма
 1. [NegaMax](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/NegaMaxSolver.cs)
 2. [MiniMax](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/MiniMaxSolver.cs) с Альфа-Бета отсечением и двумя дополнительными стратегиями:
    - [Приоритет ходов](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/MiniMaxSolver.cs#L162)
    - [Кэширование](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/MiniMaxSolver.cs#L72)
 
В каждом из них используется [итеративная глубина поиска](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/MiniMaxSolver.cs#L35) (для ограничения по времени) с ограничением ее максимального знчаения (удобно для тестов)

Так же мы написали простой
[Жадный алгоритм](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/GreedySolver.cs)
для автоматических тестов на скорость алгоритма

## Тесты

### Скорость

Главным показателем мы решили считать счет, набранный за 10 секунд игр против жадного алгоритма. При этом алгоритмам дается 100 мс на ход и максимальная глубина ограничивается 5. Счет вычисляется простой формулой: `100 * <количество побед> + <количество игр>`

Вот результаты
[такого теста](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Tests/SpeedTests.cs#L16):

| Solver | Games | Wins | Score |
| --- | --- | --- | --- |
| MiniMax with cache | 112567 | 112567 | 11369267 |
| MiniMax with sorting with cache | 100670 | 100670 | 10167670 |
| MiniMax | 75 | 75 | 7575 |
| MiniMax with sorting | 75 | 75 | 7575 |
| NegaMax | 31 | 31 | 3131 |

Оказалось, что Альфа-бета отсечение увеличивает скорость примерно в 2.5 раза, а приоритет ходов (sorting) не имеет смысла для такого "глупого" противника. При этом кэширование увеличивает скорость в 1500 раз

### Победы

Мы так же провели
[тесты](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Tests/WinTests.cs#L14),
в которых каждый алгоритм играл против каждого. В следующей таблице по строкам расположены алгоритмы, которые играли за желтого игрока, а по столбцам - за красного. В ячейках таблицы указано, кто победил. Данные тесты проводились с тремя различными ограничениями по времени и без ограничения по глубине

#### 500 мс
Желтый \ Красный | NegaMax | MiniMax | MiniMax with sorting | MiniMax with cache | MiniMax with sorting with cache |
| --- | --- | --- | --- | --- | --- |
| NegaMax | Красный | Желтый | Красный | Красный | Красный |
| MiniMax | Красный | Красный | Красный | Красный | Красный |
| MiniMax with sorting | Красный | Красный | Красный | Красный | Красный |
| MiniMax with cache | Красный | Красный | Красный | Красный | Красный |
| MiniMax with sorting with cache | Красный | Красный | Красный | Красный | Красный |

#### 300 мс
Желтый \ Красный | NegaMax | MiniMax | MiniMax with sorting | MiniMax with cache | MiniMax with sorting with cache |
| --- | --- | --- | --- | --- | --- |
| NegaMax | Красный | Красный | Красный | Красный | Красный |
| MiniMax | Красный | Красный | Красный | Красный | Красный |
| MiniMax with sorting | Красный | Красный | Красный | Красный | Красный |
| MiniMax with cache | Красный | Желтый | Ничья | Красный | Красный |
| MiniMax with sorting with cache | Красный | Желтый | Желтый | Красный | Красный |

#### 100 мс
Желтый \ Красный | NegaMax | MiniMax | MiniMax with sorting | MiniMax with cache | MiniMax with sorting with cache |
| --- | --- | --- | --- | --- | --- |
| NegaMax | Красный | Красный | Красный | Красный | Красный |
| MiniMax | Красный | Красный | Красный | Красный | Красный |
| MiniMax with sorting | Желтый | Красный | Красный | Красный | Красный |
| MiniMax with cache | Желтый | Желтый | Желтый | Желтый | Ничья |
| MiniMax with sorting with cache | Ничья | Красный | Ничья | Красный | Красный |

### Против человека

Никто из нашей команды ни разу не смог победить лучший алгоритм, независимо от порядка ходов. Игра почти всегда сводилась к ситуации, когда у бота 2 выигрышных хода и мы можем перекрыть только один из них. Поиграть можно просто запустив программу - [выставлена игра](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Program.cs#L11) за желтого игрока против MiniMax алгоритма с приоритетом ходов и кэшированием

## Выводы

Любой из этих алгоритмов достаточно хорошо играет против человека (он просчитывает на 7-10 ходов вперед в самом начале игры и чем дольше игра, тем больше ходов), но кэширование делает бота очень "умным", даже при маленьком времени на ход. Данное время так же проявляется при игре алгоритмов друг против друга

Интересно, что при времени на ход в 500 мс, во всех случаях кроме одного выигрывает красный игрок. Мы сделали вывод, что такого времени хватает, чтобы все реализации играли хорошо, при этом его не достаточно, чтобы желтый игрок (который ходит первым) не ошибся на одном из первых ходов, что неминуемо приводит его к поражению

При этом 300 мс хвататет, чтобы алгоритмы с кэшированием, которые работают сильно быстрее, погружались на сильно большую глубину и они начали выигрывать

На ограничении по времени в 100 мс, кэширование раскрывается на полную, и данные алгоритмы начинают играть заметно лучше других. Так же становится ясно, что приоритет ходов имеет смысл при игре за красного
