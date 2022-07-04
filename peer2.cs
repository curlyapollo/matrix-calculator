using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Channels;

namespace peer2
{
    /// <summary>
    /// Класс, в котором проходит работа с матрицами.
    /// </summary>
    class Program
    {
        static Random rnd = new Random();
        
        /// <summary>
        /// Вывод матрицы в виде таблицы.
        /// </summary>
        /// <param name="matrix">матрица, которую надо вывести.</param>
            static void PrintMatrix(double[,] matrix)
            {
                Console.WriteLine("Элементы матрицы:");
                for (int i = 0; i < matrix.GetLength(0); i++, Console.WriteLine())
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write("{0,10}", Math.Round(matrix[i, j], 2));
            }
        
        /// <summary>
        /// Получение следа матрицы.
        /// </summary>
        /// <param name="matrix">матрица, след которой мы ищем.</param>
        /// <returns>след матрицы</returns>
        static string TraceOfMatrix(double[,] matrix)
        {
            // Проверка на квадратность матрицы.
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                return @"Твоя матрица не квадратная, поэтому её след посчитать невозможно.
Если хочешь найти след, то введи характеристику квадратной матрицы.";
            }
            double trace = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                trace += matrix[i, i];
            }
            return $"След твоей матрицы: {trace}";
        }

        /// <summary>
        /// Нахождение транспонированной матрицы.
        /// </summary>
        /// <param name="matrix">матрица, которую надо транспонировать.</param>
        /// <returns>транспонированная матрица</returns>
        static double[,] TransposeMatrix(double[,] matrix)
        {
            double[,] newMatrix = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    newMatrix[j, i] = matrix[i, j];
                }
            }
            return newMatrix;
        }

        /// <summary>
        /// Нахождение суммы матриц.
        /// </summary>
        /// <param name="matrix1">первое слагаемое</param>
        /// <param name="matrix2">второе слагаемое</param>
        /// <returns>сумма матриц</returns>
        static double[,] SumOfMatrix(double[,] matrix1, double[,] matrix2)
        {
            double[,] resultMatrix = new double[matrix1.GetLength(0), matrix1.GetLength(1)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    resultMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            return resultMatrix;
        }
        
        /// <summary>
        /// Нахождение разности матриц.
        /// </summary>
        /// <param name="matrix1">уменьшаемое</param>
        /// <param name="matrix2">вычитаемое</param>
        /// <returns>разность</returns>
        static double[,] DifferenceOfMatrix(double[,] matrix1, double[,] matrix2)
        {
            double[,] resultMatrix = new double[matrix1.GetLength(0), matrix1.GetLength(1)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    resultMatrix[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return resultMatrix;
        }

        /// <summary>
        /// Умножение матрицы на число.
        /// </summary>
        /// <param name="matrix">умножаемая матрица</param>
        /// <param name="factor">множитель</param>
        /// <returns>полученная матрица</returns>
        static double[,] MultiplicationByNumber(double[,] matrix, double factor)
        {
            double[,] resultMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    resultMatrix[i, j] = matrix[i, j] * factor;
                }
            }
            return resultMatrix;
        }
        
        /// <summary>
        /// Произведение матриц.
        /// </summary>
        /// <param name="matrix1">первый множитель</param>
        /// <param name="matrix2">второй множитель</param>
        /// <returns>матрица, являющаяся произведением</returns>
        static double[,] CompositionOfMatrix(double[,] matrix1, double[,] matrix2)
        {
            double[,] resultMatrix = new double[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        resultMatrix[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            return resultMatrix;
        }
        
        /// <summary>
        /// Функция, узнающая, как и с чем хочет работать пользователь.
        /// </summary>
        /// <param name="optionNumber">вид работы с матрицами</param>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void BeginningOfWork(out uint optionNumber, out string choiceUser)
        {
            Console.Write("Введи номер опции(1-8), с которой хочешь работать: ");
            while (!uint.TryParse(Console.ReadLine(), out optionNumber) || optionNumber < 1 || optionNumber > 8)
            {
                Console.WriteLine("Ты ввёл некорректный номер, попробуй ещё раз.");
                Console.Write("Введи номер опции(1-8), с которой хочешь работать: ");
            }
            Console.Write("Ты сам введёшь данные матрицы(+), это делает компьютер(-) или мы считаем их с файла(=)? ");
            choiceUser = Console.ReadLine();
            while (choiceUser != "+" && choiceUser != "-" && choiceUser != "=")
            {
                Console.WriteLine("Ты ввёл некорректный ответ, попробуй ещё раз.");
                Console.Write("Введи '+', '-' или '=' в зависимости от своего выбора: ");
                choiceUser = Console.ReadLine();
            }                                                               
        }

        /// <summary>
        /// Взаимодействие программы со следом.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithTrace(string choiceUser)
        {
            switch (choiceUser)
            {
                case "+": 
                    double[,] matrix1 = EnterOfMatrix();
                    Console.WriteLine(TraceOfMatrix(matrix1));
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {
                        matrix1 = EnterOfMatrix();
                        Console.WriteLine(TraceOfMatrix(matrix1));
                    }
                    break; 
                case "-":
                    matrix1 = GenerateOfMatrix();
                    Console.WriteLine(TraceOfMatrix(matrix1));
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {
                        matrix1 = GenerateOfMatrix();
                        Console.WriteLine(TraceOfMatrix(matrix1));
                    }
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    Console.WriteLine(TraceOfMatrix(matrix1));          
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {                                                   
                        matrix1 = FileInputOfMatrix(out isMatrixCorrect);
                        Console.WriteLine(TraceOfMatrix(matrix1));      
                    } 
                    break;                                                  
            }
        }
        
        /// <summary>
        /// Взаимодействие программы с транспозицией.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithTransposition(string choiceUser)
        {
            switch (choiceUser)
            { 
                case "+": 
                    double[,] matrix1 = EnterOfMatrix();
                    PrintMatrix(TransposeMatrix(matrix1));
                    break;
                case "-": 
                    matrix1 = GenerateOfMatrix();
                    PrintMatrix(TransposeMatrix(matrix1));
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    PrintMatrix(TransposeMatrix(matrix1));
                    break;
            }
        }

        /// <summary>
        /// Взаимодействие программы с суммой.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithSum(string choiceUser)
        {
            switch (choiceUser)
            {
                case "+":
                    double[,] matrix1 = EnterOfMatrix();
                    double[,] matrix2 = EnterOfMatrix();
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))
                    {
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи матрицы одинаковых размеров.");
                        matrix1 = EnterOfMatrix();
                        matrix2 = EnterOfMatrix();
                    }
                    PrintMatrix(SumOfMatrix(matrix1, matrix2));
                    break;
                case "-": 
                    matrix1 = GenerateOfMatrix(); 
                    matrix2 = GenerateOfMatrix();
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))
                    {
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи одинаковые размеры.");
                        matrix1 = GenerateOfMatrix();
                        matrix2 = GenerateOfMatrix();
                    }
                    PrintMatrix(SumOfMatrix(matrix1, matrix2));
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))                                              
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))                                          
                    {                                                                                                  
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи одинаковые размеры.");         
                        matrix1 = FileInputOfMatrix(out isMatrixCorrect);
                        matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    }                                                                                                  
                    PrintMatrix(SumOfMatrix(matrix1, matrix2));
                    break;
            }     
        }

        /// <summary>
        /// Взаимодействие программы с разностью.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithDifference(string choiceUser)
        {
            switch (choiceUser)
            {
                case "+":
                    double[,] matrix1 = EnterOfMatrix();
                    double[,] matrix2 = EnterOfMatrix();
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))
                    {
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи матрицы одинаковых размеров.");
                        matrix1 = EnterOfMatrix();
                        matrix2 = EnterOfMatrix();
                    }
                    PrintMatrix(DifferenceOfMatrix(matrix1, matrix2));
                    break;
                case "-":
                    matrix1 = GenerateOfMatrix();
                    matrix2 = GenerateOfMatrix();
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))
                    {
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи одинаковые размеры.");
                        matrix1 = GenerateOfMatrix();
                        matrix2 = GenerateOfMatrix();
                    }

                    PrintMatrix(DifferenceOfMatrix(matrix1, matrix2));
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    while ((matrix1.GetLength(0) != matrix2.GetLength(0))
                           || (matrix1.GetLength(1) != matrix2.GetLength(1)))
                    {
                        Console.WriteLine("Матрицы твоих размеров нельзя сложить! Введи одинаковые размеры.");
                        matrix1 = FileInputOfMatrix(out isMatrixCorrect);
                        matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    }
                    PrintMatrix(DifferenceOfMatrix(matrix1, matrix2));
                    break;
            }
        }

        /// <summary>
        /// Взаимодействие программы с произведением матриц.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithComposition(string choiceUser)
        {
            switch (choiceUser)
            {
                case "+":
                    double[,] matrix1 = EnterOfMatrix();
                    double[,] matrix2 = EnterOfMatrix();
                    while (matrix1.GetLength(1) != matrix2.GetLength(0))
                    {
                        Console.WriteLine(
                            "Матрицы твоих размеров нельзя перемножить! Введи матрицы корректных размеров.");
                        matrix1 = EnterOfMatrix();
                        matrix2 = EnterOfMatrix();
                    }
                    PrintMatrix(CompositionOfMatrix(matrix1, matrix2));
                    break;
                case "-":
                    matrix1 = GenerateOfMatrix();
                    matrix2 = GenerateOfMatrix();
                    while (matrix1.GetLength(1) != matrix2.GetLength(0))
                    {
                        Console.WriteLine(
                            "Матрицы твоих размеров нельзя перемножить! Введи матрицы корректных размеров.");
                        matrix1 = GenerateOfMatrix();
                        matrix2 = GenerateOfMatrix();
                    }
                    PrintMatrix(CompositionOfMatrix(matrix1, matrix2));
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    while (matrix1.GetLength(1) != matrix2.GetLength(0))
                    {
                        Console.WriteLine(
                            "Матрицы твоих размеров нельзя перемножить! Введи матрицы корректных размеров.");
                        matrix1 = FileInputOfMatrix(out isMatrixCorrect);
                        matrix2 = FileInputOfMatrix(out isMatrixCorrect);
                    }
                    PrintMatrix(CompositionOfMatrix(matrix1, matrix2));
                    break;
            }
        }

        /// <summary>
        /// Взаимодействие программы с умножением на число.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithMultiplication(string choiceUser)
        {
            double factor;
            Console.Write("Введи множитель: ");
            while (!double.TryParse(Console.ReadLine(), out factor) || Math.Abs(factor) > 100)
            {
                Console.WriteLine("Ты ввёл некорректный множитель! Попробуй ещё раз!");
                Console.Write("Введи множитель: ");
            }

            switch (choiceUser)
            {
                case "+":
                    double[,] matrix1 = EnterOfMatrix();
                    PrintMatrix(MultiplicationByNumber(matrix1, factor));
                    break;
                case "-": 
                    matrix1 = GenerateOfMatrix();
                    PrintMatrix(MultiplicationByNumber(matrix1, factor));
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    PrintMatrix(MultiplicationByNumber(matrix1, factor));
                    break;
            }
        }

        /// <summary>
        /// Взаимодействие программы с определителем.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithDeterminant(string choiceUser)
        { 
            switch (choiceUser)
            {
                case "+":
                    double[,] matrix1 = EnterOfMatrix();
                    Console.WriteLine(Determinant(matrix1));
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {
                        matrix1 = EnterOfMatrix();
                        Console.WriteLine(Determinant(matrix1));
                    }
                    break;
                case "-": 
                    matrix1 = GenerateOfMatrix();
                    Console.WriteLine(Determinant(matrix1));
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {
                        matrix1 = GenerateOfMatrix();
                        Console.WriteLine(Determinant(matrix1));
                    }
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    Console.WriteLine(Determinant(matrix1));
                    while (matrix1.GetLength(0) != matrix1.GetLength(1))
                    {
                        matrix1 = FileInputOfMatrix(out isMatrixCorrect);
                        Console.WriteLine(Determinant(matrix1));
                    }
                    break;
            }
        }

        /// <summary>
        /// Основная работа программы, взаимодействие с операциями.
        /// </summary>
        static void MiddleOfWork()
        {
            uint optionNumber;
            string choiceUser;
            BeginningOfWork(out optionNumber, out choiceUser);
            switch (optionNumber)
            {
                case 1:
                    InteractionWithTrace(choiceUser);
                    break;
                case 2:
                    InteractionWithTransposition(choiceUser);
                    break;
                case 3:
                    InteractionWithSum(choiceUser);
                    break;
                case 4:
                    InteractionWithDifference(choiceUser);
                    break;
                case 5:
                    InteractionWithComposition(choiceUser);
                    break;
                case 6:
                    InteractionWithMultiplication(choiceUser);
                    break;
                case 7:
                    InteractionWithDeterminant(choiceUser);
                    break;
                default:
                    InteractionWithSLAU(choiceUser);
                    break;
            }

            Console.WriteLine(@"Славно потрудились! Если хочешь поработать ещё, напиши 'да'. 
Если же тебе надоело, то введи что-либо другое.");
        }
        
        /// <summary>
        /// Генерация рандомной матрицы.
        /// </summary>
        /// <returns>сгенерированная матрица</returns>
        static double[,] GenerateOfMatrix()
        {
            uint rows = 0, columns = 0;
            int lowerBound, upperBound;
            EnterOfSizes(ref rows, ref columns);
            Console.Write("Теперь введи нижнюю границу элементов твоей матрицы: ");                                        
            while (!int.TryParse(Console.ReadLine(), out lowerBound) || lowerBound < -100 || lowerBound > 100)                     
            {                                                                                                 
                Console.WriteLine("Ты ввёл некорректную нижнюю границу! Попробуй ещё раз.");                
                Console.Write("Введи нижнюю границу элементов твоей матрицы: ");                                    
            }                                                                                                 
            Console.Write("Введи верхнюю границу элементов твоей матрицы: ");                                     
            while (!int.TryParse(Console.ReadLine(), out upperBound) || upperBound < -100 || upperBound > 100)            
            {                                                                                                 
                Console.WriteLine("Ты ввёл некорректную верхнюю границу! Попробуй ещё раз.");             
                Console.Write("Введи верхнюю границу элементов твоей матрицы: ");                                 
            }                                                                                                 
            double[,] matrix = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = rnd.Next(lowerBound, upperBound) + rnd.NextDouble();
                }
            }
            Console.WriteLine("Сгенерировалась такая матрица:");
            PrintMatrix(matrix);
            return matrix;
        }

        static double[,] FileInputOfMatrix(out bool isMatrixCorrect)
        {
            uint rows = 0, columns = 0;
            EnterOfSizes(ref rows, ref columns);
            double[,] matrix = new double[rows, columns];
            isMatrixCorrect = true;
            FileInstruction();
            try
            {
                string line;
                int i = 0;
                using (StreamReader sr = new StreamReader(Console.ReadLine()))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] row = line.Split();
                        for (int j = 0; j < row.Length; j++)
                        {
                            double element;
                            if (!double.TryParse(row[j], out element))
                            {
                                Console.WriteLine("Введённые данные некорректны!");
                                isMatrixCorrect = false;
                            }
                            else
                            {
                                matrix[i, j] = element;
                            }
                        }
                        i++;
                    }
                    if (i != matrix.GetLength(0))
                    {
                        PrintMatrix(matrix);
                        Console.WriteLine("Ваша матрица не соответствует введённым размерам!");
                        isMatrixCorrect = false;
                    }
                }
            }
            catch (Exception e)
            {
                // Обработка исключения даст пользователю информацию, что пошло не так.
                Console.WriteLine("Этот файл не может быть прочитан:");
                Console.WriteLine(e.Message);
                isMatrixCorrect = false;
            }
            return FileOutput(isMatrixCorrect, matrix);
        }

        static double[,] FileOutput(bool isMatrixCorrect, double[,] matrix)
        {
            if (isMatrixCorrect)
            {
                return matrix;
            }
            else
            {
                Console.WriteLine("Ошибка ввода!");
                return FileInputOfMatrix(out isMatrixCorrect);
            }
        }

        /// <summary>
        /// Ввод размеров матрицы.
        /// </summary>
        /// <param name="rows">количество строк</param>
        /// <param name="columns">количество столбцов</param>
        static void EnterOfSizes(ref uint rows, ref uint columns)
        {
            Console.Write("Введи количество строк в твоей матрице: ");                                                                  
            while (!uint.TryParse(Console.ReadLine(), out rows) || rows < 1 || rows > 10)                                               
            {                                                                                                                           
                Console.WriteLine("Ты ввёл некорректное количество строк! Попробуй ещё раз.");                                          
                Console.Write("Введи количество строк в твоей матрице: ");                                                              
            }                                                                                                                           
            Console.Write("Введи количество столбцов в твоей матрице: ");                                                               
            while (!uint.TryParse(Console.ReadLine(), out columns) || columns < 1 || columns > 10)                                      
            {                                                                                                                           
                Console.WriteLine("Ты ввёл некорректное количество столбцов! Попробуй ещё раз.");                                       
                Console.Write("Введи количество столбцов в твоей матрице: ");                                                           
            }
        }

        /// <summary>
        /// Ввод матрицы пользователем.
        /// </summary>
        /// <returns>введённая матрица</returns>
        static double[,] EnterOfMatrix()
        {
            uint rows = 0, columns = 0;
            EnterOfSizes(ref rows, ref columns);
            Console.WriteLine("Теперь ты должен ввести элементы матрицы: элементы первой строчки, потом второй итд.");
            double[,] matrix = new double[rows, columns];
            double element;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write("Введи элемент матрицы: ");
                    while (!double.TryParse(Console.ReadLine(), out element) || Math.Abs(element) > 100)
                    {
                        Console.WriteLine("Ты ввёл некорректный элемент! Попробуй ещё раз!");
                        Console.Write("Введи элемент матрицы: ");
                    }
                    matrix[i, j] = element;
                }
            }
            Console.WriteLine("Ты ввёл такую матрицу:");
            PrintMatrix(matrix);
            return matrix;
        }

        
        /// <summary>
        /// Вычисление определителя.
        /// </summary>
        /// <param name="matrix">матрица, определитель которой вычисляем</param>
        /// <returns>определитель</returns>
        static string Determinant(double[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                return @"Твоя матрица не квадратная, поэтому её определитель посчитать невозможно.
Если хочешь найти определитель, то введи характеристику квадратной матрицы."; 
            }
            double determinant = 1;
            for (int row = 0, column = 0;
                row < matrix.GetLength(0) &&
                column < matrix.GetLength(1);
                column++)
            {
                int temp = row;
                for (int i = row; i < matrix.GetLength(0); i++)
                {
                    if (Math.Abs(matrix[i, column]) > Math.Abs(matrix[row, column]))
                    {
                        temp = i;
                    }
                }

                if (matrix[temp, column] == 0)
                {
                    return "Определитель твоей матрицы: 0";
                }
                SwapRows(ref matrix, row, temp);
                if (row != temp)
                {
                    determinant *= -1;
                }
                determinant *= matrix[row, column];     
                CompositonOfRowByNumber(ref matrix, row, 1 / matrix[row, column]);
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (i != row)
                    {
                        SumOfRows(ref matrix, i, row, -matrix[i, column]);
                    }
                    
                }
                row++;
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                determinant *= matrix[i, i];
            }
            return $"Определитель твоей матрицы: {Math.Round(determinant, 2)}";              
        }
        
        /// <summary>
        /// Взаимодействие программы с решением СЛАУ.
        /// </summary>
        /// <param name="choiceUser">выбор, генерировать или вводить хочет пользователь</param>
        static void InteractionWithSLAU(string choiceUser)
        {
            Console.WriteLine(@"Для того, чтобы решить твою систему уравнений, мы преобразуем её в матрицу.
Каждая строка матрицы будет представлять коэффициенты при x1, x2 итд. и свободный член.
Например: 5x1 + 6x2 - x3 = 8  ==> 5 6 -1 8");
            double[,] matrix1;
            switch (choiceUser)
            {
                case "+":
                    matrix1 = EnterOfMatrix();
                    break;
                case "-":
                    matrix1 = GenerateOfMatrix();
                    break;
                default:
                    matrix1 = FileInputOfMatrix(out bool isMatrixCorrect);
                    break;
            }
            double[] roots = new double[matrix1.GetLength(1) - 1];
            int result = GaussianAlgorithm(matrix1, out roots);
            if (result == 0)
            {
                Console.WriteLine("У этой системы нет решения!");
            }
            else if (result == -1)
            {
                Console.WriteLine("У этой системы бесконечно много решений!");            
            }
            else
            {
                Console.WriteLine("Решение системы:");
                for (int i = 0; i < roots.Length; i++)
                {
                    Console.WriteLine($"x{i + 1} = {Math.Round(roots[i]), 2}");
                }
            }
        }
        
        /// <summary>
        /// Сумма i-ой и j-ой строки, умноженной на какой-то коэффициент.
        /// </summary>
        /// <param name="matrix">матрица, со строками которой работаем</param>
        /// <param name="i">номер строки, к которой прибавляют</param>
        /// <param name="j">номер прибавляемой строки</param>
        /// <param name="coefficient">коэффициент, на который домножают j-ую строку</param>
        static void SumOfRows(ref double[,] matrix, int i, int j, double coefficient)
        {
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                matrix[i, k] += coefficient * matrix[j, k];
            }
        }
        
        /// <summary>                                                                        
        /// Умножение i-ой строки на какой-то коэффициент.                    
        /// </summary>                                                                       
        /// <param name="matrix">матрица, со строкой которой работаем</param>               
        /// <param name="i">номер строки, с которой работают</param>                             
        /// <param name="coefficient">коэффициент, на который домножают i-ую строку</param>  
        static void CompositonOfRowByNumber(ref double[,] matrix, int i, double coefficient)
        {
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                matrix[i, k] *= coefficient;
            }
        }
        
        /// <summary>
        /// Инструкция по использованию файла.
        /// </summary>
        static void FileInstruction()
        {
            Console.Write(@"В вашем файле должна быть записана матрица размеров, которые вы указали до этого.
Если ваша матрица состоит не из действительных чисел, разделённых пробелами - выдаст ошибку. Нужно переписать файл.
Теперь введите полный путь к файлу вместе с названием:");
        }

        /// <summary>
        /// Реализация метода Гаусса.
        /// </summary>
        /// <param name="matrix">СЛАУ</param>
        /// <param name="roots">корни</param>
        /// <returns>флаг для понимания, сколько корней</returns>
        static int GaussianAlgorithm(double[,] matrix, out double[] roots)
        {
            int[] locationRoot = new int[matrix.GetLength(1) - 1];
            roots = new double[matrix.GetLength(1) - 1]; 
            for (int i = 0; i < locationRoot.Length; i++)
            {
                locationRoot[i] = -1;
                roots[i] = 0;
            }
            for (int row = 0, column = 0; row < matrix.GetLength(0) && 
                                          column < matrix.GetLength(1) - 1; column++)
            {
                int temp = row;
                for (int i = row; i < matrix.GetLength(0); i++)
                {
                    if (Math.Abs(matrix[i, column]) > Math.Abs(matrix[row, column]))
                    {
                        temp = i;
                    }
                }
                if (matrix[temp, column] < Double.Epsilon)
                {
                    continue;
                }
                SwapRows(ref matrix, row, temp);
                locationRoot[column] = row;
                CompositonOfRowByNumber(ref matrix, row, 1 / matrix[row, column]);
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (i != row)
                    {
                        SumOfRows(ref matrix, i, row, -matrix[i, column]);
                    }
                }
                row++;
            }
            for (int i = 0; i < locationRoot.Length; i++)
            {
                if (locationRoot[i] != -1)
                {
                    roots[i] = matrix[i, matrix.GetLength(1) - 1];
                }
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                    sum += roots[j] * matrix[i, j];
                if (Math.Abs(sum - matrix[i, matrix.GetLength(1) - 1]) > Double.Epsilon)
                    return 0;
            }
            for (int i = 0; i < matrix.GetLength(1) - 1; ++i)
                if (locationRoot[i] == -1)
                    return -1;
            return 1;
        }
        
        /// <summary>
        /// Обмен строк.
        /// </summary>
        /// <param name="matrix">матрица, в которой происходит обмен</param>
        /// <param name="i">номер 1-й строки</param>
        /// <param name="j">номер 2-й строки/param>
        static void SwapRows(ref double[,] matrix, int i, int j)
        {
            double[,] newMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int k = 0; k < matrix.GetLength(0); k++)
            {
                for (int l = 0; l < matrix.GetLength(1); l++)
                {
                    newMatrix[k, l] = matrix[k, l];
                }
            }
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                newMatrix[j, k] = matrix[i, k];
                newMatrix[i, k] = matrix[j, k];
            }
            matrix = newMatrix;
        }
        
        /// <summary>
        /// инструкция
        /// </summary>
        /// <returns>правила</returns>
        static string Instruction()
        {
            string instruction;
            instruction = @"Привет, мой дорогой сокурсник! Ну или моя прекрасная сокурсница. Давай поиграем с матрицами!
Правила просты: ты выбираешь, какие действия выполнять с матрицей. Тебе будет предложено 8 опций: 
1. нахождение следа матрицы;
2. транспонирование матрицы;
3. сумма двух матриц;
4. разность двух матриц;
5. произведение двух матриц;
6. умножение матрицы на число;
7. нахождение определителя матрицы;
8. решение СЛАУ методом Гаусса;
Чтобы выбрать опцию, нужно ввести её номер и нажать Enter. 
Далее ты либо сам вводишь данные матрицы, либо элементы этой матрицы генерируются (параметры задаешь всё равно ты.)
Вводишь '+', если сам хочешь ввести, '-', если хочешь, чтобы сами сгенерировались. 
Важное замечание: в моей программе стоит ограничение на размеры - каждый аргумент не больше 10.
Все числа, вводимые пользователем, также ограничены: каждый по модулю не превосходит 100.
Ещё эти числа округлены до двух знаков после запятой, чтобы не было путаницы.
После каждого выполненного действия тебе будет предложено либо продолжить, введя 'да',
либо выйти из игры, введя что-то другое.
Удачи!";
            return instruction;
        }
        
        /// <summary>
        /// Точка входа.
        /// </summary>
        static void Main()
        {
            string yes;
            do
            {
                Console.WriteLine(Instruction());
                MiddleOfWork();
                yes = Console.ReadLine();
            } while (yes == "да");
            Console.WriteLine("Хорошего дня! Работа окончена.");
        }
    }
}