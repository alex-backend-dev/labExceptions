using System;

namespace labExceptions
{
    public class MyException : ApplicationException
    {
        public MyException() { }
        public MyException(string message) : base(message) { }
        public MyException(string message, Exception ex) : base(message) { }

        // Конструктор для обработки сериализации типа
        protected MyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext contex)
            : base(info, contex) { }
    }

    class Program
    {
        // метод для получения матрицы из консоли
        static int[,] MatrixFromConsole(string name)
        {

            Console.Write("Количество строк матрицы {0}: ", name);
            var n = int.Parse(Console.ReadLine());
            Console.Write("Количество столбцов матрицы {0}: ", name);
            var m = int.Parse(Console.ReadLine());

            var matrix = new int[n, m];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    Console.Write("{0}[{1},{2}] = ", name, i, j);
                    matrix[i, j] = int.Parse(Console.ReadLine());
                }
            }

            return matrix;
        }

        // метод для печати матрицы в консоль
        static void PrintMatrix(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j].ToString().PadLeft(4));
                }

                Console.WriteLine();
            }
        }
        // метод возвращения нулевой матрицы
        static int[,] GetEmpty(int[,] matrix)
        {
            var resultMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    int k = 0;
                    resultMatrix[i, j] = matrix[i, j] * k;
                }
            }
            return resultMatrix;
        }


        // метод для умножения матриц
        static int[,] MatrixMultiplication(int[,] matrixA, int[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
            {
                MyException exc = new MyException("ОШИБКА: Умножение невозможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
                throw exc;
            }

            var matrixC = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

            for (var i = 0; i < matrixA.GetLength(0); i++)
            {
                for (var j = 0; j < matrixB.GetLength(1); j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < matrixA.GetLength(1); k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;
        }

        // Mетод для сложения двух матриц
        static int[,] MatrixSumm(int[,] matrixA, int[,] matrixB)
        {
            if ((matrixA.GetLength(0) != matrixB.GetLength(0)) || (matrixA.GetLength(1) != matrixB.GetLength(1)))
            {
                MyException exc = new MyException("ОШИБКА! Для матриц с разным размером сложение/вычитание невозможно!");
                throw exc;
            }

            var matrixC = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

            for (var i = 0; i < matrixA.GetLength(0); i++)
            {
                for (var j = 0; j < matrixB.GetLength(1); j++)
                {
                    matrixC[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }

            return matrixC;
        }

        // Метод вычитания двух матриц
        static int[,] MatrixMinus(int[,] matrixA, int[,] matrixB)
        {
            if ((matrixA.GetLength(0) != matrixB.GetLength(0)) || (matrixA.GetLength(1) != matrixB.GetLength(1)))
            {
                MyException exc = new MyException("ОШИБКА! Для матриц с разным размером сложение/вычитание невозможно!");
                throw exc;
            }

            var matrixC = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

            for (var i = 0; i < matrixA.GetLength(0); i++)
            {
                for (var j = 0; j < matrixB.GetLength(1); j++)
                {
                    matrixC[i, j] = matrixA[i, j] - matrixB[i, j];
                }
            }
            return matrixC;
        }

        static void Main()
        {

            try
            {
                var a = MatrixFromConsole("A");
                var b = MatrixFromConsole("B");
                Console.WriteLine("Матрица A:");
                PrintMatrix(a);

                Console.WriteLine("Матрица B:");
                PrintMatrix(b);

                var result1 = MatrixMultiplication(a, b);
                Console.WriteLine("Результат произведения двух матриц:");
                PrintMatrix(result1);

                var result2 = MatrixSumm(a, b);
                Console.WriteLine("Результат сложения двух матриц: ");
                PrintMatrix(result2);

                var result3 = MatrixMinus(a, b);
                Console.WriteLine("Результат вычитания двух матриц:");
                PrintMatrix(result3);

                // нулевая матрица

                Console.WriteLine("Нулевая матрица A");
                var matrix01 = GetEmpty(a);
                PrintMatrix(matrix01);
                Console.WriteLine("Нулевая матрица B");
                var matrix02 = GetEmpty(b);
                PrintMatrix(matrix02);
            }

            // некорректный ввод числа в консоль
            catch (FormatException ex)
            {
                Console.WriteLine("Это НЕ число!!!\n");
                Console.WriteLine("ОШИБКА: " + ex.Message + "\n\n");
                Main();
            }

            // исключение при сложении/вычитании
            catch (MyException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Перепроверьте столбцы и строки матриц");
                Main();
            }
            // Обрабатываем все исключения
            catch
            {
                Console.WriteLine("Возникла непредвиденная ошибка");
                Main();
            }

            Console.ReadLine();
        }
    }
}