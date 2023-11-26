using System;
using System.Diagnostics;
using System.Threading;

class BrownianMotionSync
{
    static int N = 100; // Розмір кристалу
    static int K = 500;  // Кількість атомів домішки
    static double p = 0.5; // Поріг ймовірності

    private static readonly object _lock = new object(); // Об'єкт блокування для синхронізації

    private static readonly int[] Cells = new int[N]; // Кристал - масив клітин

    static void Main()
    {
        Initialize(); // Ініціалізація кристалу

        var threads = new Thread[K]; // Масив потоків для кожного атома домішки
        // Запуск потоків для кожного атома домішки
        for (var i = 0; i < K; i++)
        {
            threads[i] = new Thread(new ParameterizedThreadStart(Move));
            threads[i].Start(i);
        }

        // Виведення стану масиву кожну секунду
        for (int i = 0; i < 10; i++) // 10 секунд моделювання
        {
            DisplayArrayState();
            Thread.Sleep(1000); // Очікування 1 секунди
        }


        // Зупинка всіх потоків
        foreach (var thread in threads)
        {
            thread.Interrupt(); // Переривання потоку
        }
        CalculateTotalAtoms(); // Обчислення загальної кількості атомів домішки
    }

    static void DisplayArrayState()
    {
        Console.WriteLine("Стан кристалу:");
        for (int i = 0; i < N; i++)
        {
            Console.Write(Cells[i] + " ");
        }
        Console.WriteLine();
    }

    static void Initialize()
    {
        // Початковий стан: всі атоми домішки в крайній лівій клітинці
        Cells[0] = K;
    }

    static void Move(object indexObj)
    {
        int index = (int)indexObj;

        Random rand = new Random();
        try
        {
            int currentPosition = 0;
            while (true)
            {
                Thread.Sleep(1000); // Очікування 1 секунди

                double m = rand.NextDouble(); // Генерація випадкового числа m у діапазоні [0, 1]

                lock (_lock) // Блокуємо критичну секцію для синхронізації доступу до даних
                {
                    if (m > p && currentPosition + 1 < N) // Рух вправо, якщо m > p
                    {
                        Cells[currentPosition]--;
                        Cells[currentPosition + 1]++;
                        currentPosition++;
                    }
                    else if (currentPosition - 1 >= 0) // Рух вліво, якщо умова не виконується
                    {
                        Cells[currentPosition]--;
                        Cells[currentPosition - 1]++;
                        currentPosition--;
                    }
                }
            }
        }
        catch (ThreadInterruptedException)
        {
            // Обробка ThreadInterruptedException при перериванні потоку
            Console.WriteLine($"Потiк {index} перервано");
        }
    }

    //static void Move(object indexObj)
    //{
    //    int index = (int)indexObj;
    //    Random rand = new Random();
    //    try
    //    {
    //        int currentPosition = 0;
    //        while (true)
    //        {
    //            Thread.Sleep(1000); // Очікування 1 секунди

    //            double m = rand.NextDouble(); // Генерація випадкового числа m у діапазоні [0, 1]

    //            if (m > p && currentPosition + 1 < N) // Рух вправо, якщо m > p
    //            {
    //                lock (_lock) // Блокуємо окрему клітину при її зміні
    //                {
    //                    Cells[currentPosition]--;
    //                    Cells[currentPosition + 1]++;
    //                }
    //                currentPosition++;
    //            }
    //            else if (currentPosition - 1 >= 0) // Рух вліво, якщо умова не виконується
    //            {
    //                lock (_lock) // Блокуємо окрему клітину при її зміні
    //                {
    //                    Cells[currentPosition]--;
    //                    Cells[currentPosition - 1]++;
    //                }
    //                currentPosition--;
    //            }
    //        }
    //    }
    //    catch (ThreadInterruptedException)
    //    {
    //        // Обробка ThreadInterruptedException при перериванні потоку
    //        //Console.WriteLine($"Потік {index} перервано");
    //    }
    //}


    static void CalculateTotalAtoms()
    {
        int totalAtoms = 0;
        foreach (int atoms in Cells)
        {
            totalAtoms += atoms;
        }

        Console.WriteLine("Загальна кiлькiсть атомiв: " + totalAtoms);
    }

    // Метод для отримання кількості атомів домішки в клітці з індексом i
    public static int GetCell(int i)
    {
        if (i >= 0 && i < N)
        {
            return Cells[i];
        }
        else
        {
            Console.WriteLine("Неправильний індекс клітини");
            return -1; // Повертаємо -1 у випадку неправильного індексу
        }
    }
}
