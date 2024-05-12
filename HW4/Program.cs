namespace HW4
{
    internal class Program
    {
        static void Main2(string[] args)
        {
            Task1();
        }

        static void Task1()
        {
            Random rnd = new();

            // Сбор данных

            Console.Write("Введите кол-во строк: ");
            int rows = int.Parse(Console.ReadLine()!);

            Console.Write("Введите кол-во столбцов: ");
            int columns = int.Parse(Console.ReadLine()!);

            // Создание и заполнение матрицы

            int[,] matrix = new int[rows, columns];
            
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    matrix[i, j] = rnd.Next(0, 100);
                }
            }

            // Вывод матрицы 1 на экран

            Console.Write("[\n");
            for (int i = 0; i < rows; i++)
            {
                Console.Write("\t[");
                for (int j = 0; j < columns-1; j++)
                {
                    Console.Write(matrix[i, j] + ", ");
                }
                Console.WriteLine(matrix[i, columns - 1] + "], ");
            }
            Console.Write("\b\b\b");
            Console.WriteLine("]\n");

            // Подсчет и вывод суммы на экран

            int sum = 0;
            foreach(var num in matrix)
            {
                sum += num;
            }

            Console.WriteLine("Сумма всех элементов: " + sum + "\n");

            // Создание, заполнение и вывод второй матрицы

            int[,] matrix2 = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix2[i, j] = rnd.Next(0, 100);
                }
            }

            Console.WriteLine("Вторая матрица для сложения: ");
            Console.Write("[\n");
            for (int i = 0; i < rows; i++)
            {
                Console.Write("\t[");
                for (int j = 0; j < columns - 1; j++)
                {
                    Console.Write(matrix2[i, j] + ", ");
                }
                Console.WriteLine(matrix2[i, columns - 1] + "], ");
            }
            Console.Write("\b\b\b");
            Console.WriteLine("]\n");

            // Создание матрицы для подсчета суммы и вывод

            int[,] sumMatrix = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sumMatrix[i, j] = matrix[i, j] + matrix2[i, j];
                }
            }

            Console.WriteLine("Итог сложения: ");
            Console.Write("[\n");
            for (int i = 0; i < rows; i++)
            {
                Console.Write("\t[");
                for (int j = 0; j < columns - 1; j++)
                {
                    Console.Write(sumMatrix[i, j] + ", ");
                }
                Console.WriteLine(sumMatrix[i, columns - 1] + "], ");
            }
            Console.Write("\b\b\b");
            Console.WriteLine("]\n");
        }
    }
}

namespace GameOfLife
{

    public class LifeSimulation
    {
        private readonly int _heigth;
        private readonly int _width;
        private readonly bool[,] cells;
        private readonly int[,] neighbors;

        private uint _runs = 0;
        private readonly uint _maxRuns;

        readonly Random rnd = new();

        /// <summary>
        /// Создаем новую игру
        /// </summary>
        /// <param name="Heigth">Высота поля.</param>
        /// <param name="Width">Ширина поля.</param>

        public LifeSimulation(int Heigth, int Width, uint maxRuns)
        {
            _heigth = Heigth;
            _width = Width;
            cells = new bool[Heigth, Width];
            neighbors = new int[Heigth, Width];
            _maxRuns = maxRuns;
            GenerateField();
            SaveNeighbors();
        }

        /// <summary>
        /// Перейти к следующему поколению и вывести результат на консоль.
        /// </summary>
        public void DrawAndGrow()
        {
            DrawGame();
            Grow();
            _runs++;
        }

        private void SaveNeighbors()
        {
            for (int i = 0; i < _heigth; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    neighbors[i, j] = GetNeighbors(i, j);
                }
            }
        }

        /// <summary>
        /// Двигаем состояние на одно вперед, по установленным правилам
        /// </summary>

        private void Grow()
        {
            // Клетки требуют изменений. Если кол-во соседей осталось прежним, то клетка умирает от скуки. Если нет, значит она случайным
            // образом изменяет одного из соседей.

            for (int i = 0; i < _heigth; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if(GetNeighbors(i, j) == neighbors[i, j])
                    {
                        cells[i, j] = false;
                    } else
                    {
                        int rndX = i + rnd.Next(-1, 2), rndY = j + rnd.Next(-1, 2);
                        while (rndX >= _heigth || rndX < 0) rndX = i + rnd.Next(-1, 2);
                        while (rndY >= _width || rndY < 0) rndY = j + rnd.Next(-1, 2);
                        cells[rndX, rndY] = rnd.Next(2) == 1;
                    }
                }
            }
            SaveNeighbors();
        }

        /// <summary>
        /// Смотрим сколько живых соседий вокруг клетки.
        /// </summary>
        /// <param name="x">X-координата клетки.</param>
        /// <param name="y">Y-координата клетки.</param>
        /// <returns>Число живых клеток.</returns>

        private int GetNeighbors(int x, int y)
        {
            int NumOfAliveNeighbors = 0;

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (!(i < 0 || j < 0 || i >= _heigth || j >= _width))
                    {
                        if (cells[i, j] == true) NumOfAliveNeighbors++;
                    }
                }
            }
            return NumOfAliveNeighbors;
        }

        /// <summary>
        /// Нарисовать Игру в консоле
        /// </summary>

        private void DrawGame()
        {
            for (int i = 0; i < _heigth; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Console.Write(cells[i, j] ? "x" : " ");
                    if (j == _width - 1) Console.WriteLine("\r");
                }
            }
            Console.WriteLine();

            for(int i = 0; i < _width/2-1; i++)
            {
                Console.Write(' ');
            }
            Console.Write($"({_runs})");

            Console.SetCursorPosition(0, Console.WindowTop);
        }

        /// <summary>
        /// Инициализируем случайными значениями
        /// </summary>

        private void GenerateField()
        {
            Random generator = new();
            int number;
            for (int i = _heigth / 3; i < _heigth - _heigth / 3; i++)
            {
                for (int j = _width / 3; j < _width - _width / 3; j++)
                {
                    number = generator.Next(2);
                    cells[i, j] = number != 0;
                }
            }
        }

        public bool GameEnded()
        {
            if (_maxRuns == _runs) { return true; }
            foreach(var cell in cells)
            {
                if (cell) return false;
            }
            return true;
        }

        public uint GetRuns() => _runs;
    }

    internal class Program
    {

        // Ограничения игры
        private const int Heigth = 15;
        private const int Width = 50;
        private const uint MaxRuns = 100;

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            LifeSimulation sim = new(Heigth, Width, MaxRuns);

            while (!sim.GameEnded())
            {
                sim.DrawAndGrow();

                // Дадим пользователю шанс увидеть, что происходит, немного ждем
                Thread.Sleep(100);
            }

            Console.WriteLine($"Игра завершена! ({sim.GetRuns()}) ");
            return;
        }
    }
}

