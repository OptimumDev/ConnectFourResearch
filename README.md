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
[для записи в файл](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Logging/FileBoardLogger.cs) - кроме поля они логирую варинты ходов и счет для удобного дебага  
Реализовали универсальный
[Controller](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/ConnectFour/Controller.cs)
который принимает два
[ISolver](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Algorithms/ISolver.cs) и умеет симулировать игру  
Так же сделали консольный интерфейс для пользователя в виде
[реализации](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Solvers/ConsoleSolver.cs)
`ISolver`

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
[такого теста](https://github.com/OptimumDev/ConnectFourResearch/blob/master/ConnectFourResearch/Tests/SpeedTests.cs#L19):

| Solver | Games | Wins | Score |
| --- | --- | --- | --- |
| MiniMax with cache | 112567 | 112567 | 11369267 |
| MiniMax with sorting with cache | 100670 | 100670 | 10167670 |
| MiniMax | 75 | 75 | 7575 |
| MiniMax with sorting | 75 | 75 | 7575 |
| NegaMax | 31 | 31 | 3131 |

Оказалось, что Альфа-бета отсечение увеличивает скорость примерно в 2.5 раза, а приоритет ходов (sorting) не имеет смысла для такого "глупого" противника. При этом кэширование увеличивает скорость 1500 раз

### Победы

Мы так же провели тесты, в которых каждый алгоритм играл против каждого. В следующей таблице по строкам расположены алгоритмы которые играли за желтого игрока, а по столбцам - за красного. В ячейках таблицы указано, кто победил. Данные тесты проводились с тремя различными ограничениями по времени и без ограничения по глубине

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
