using System;
using System.Collections.Generic;
using System.Threading;

namespace PP_Producers_Consumers
{
    class Program
    {
        private static int PRODUCERS = 5;
        private static int CONSUMERS = 10;

        private static int ADDS = 5;
        private static int TAKES = 20;

        private static CustomQueue<Task> queue = new CustomQueue<Task>();
        private static Random _random = new Random();

        public class Task
        {
            public string Name;
            public Task()
            {
                Name = "";
            }
            public Task(int index)
            {
                Name = $"Task №{index}";
            }
        }

        static void Main(string[] args)
        {
            Thread[] consumers = new Thread[CONSUMERS];
            for (int i = 0; i < consumers.Length; i++)
            {
                consumers[i] = new Thread(TakeTask);
                consumers[i].Name = $"Consumer №{i + 1}";
            }
            Thread[] producers = new Thread[PRODUCERS];
            for (int i = 0; i < producers.Length; i++)
            {
                producers[i] = new Thread(AddTask);
                producers[i].Name = $"Producer №{i + 1}";
            }
            foreach (var c in consumers)
            {
                c.Start();
            }
            foreach (var p in producers)
            {
                p.Start();
            }
            foreach (var c in consumers)
            {
                c.Join();
            }
            foreach (var p in producers)
            {
                p.Join();
            }

            Console.WriteLine("Главный поток закончил выполнение.");
        }

        static void TakeTask()
        {
            for (int i = 0; i < TAKES; i++)
            {
                Task task = queue.Dequeue();
                if (task == default)
                {
                    ConsoleHelper.WriteToConsole("Поток " + Thread.CurrentThread.Name, " обнаружил, что пул задач пуст.");
                }
                else
                {
                    ConsoleHelper.WriteToConsole("Поток " + Thread.CurrentThread.Name, " забрал задачу " + task.Name);
                }

                Thread.Sleep(_random.Next(50, 100));
            }
        }

        static void AddTask()
        {
            for (int i = 0; i < ADDS; i++)
            {
                Task new_task = new Task(_random.Next(0, 1000));

                queue.Enqueue(new_task);
                ConsoleHelper.WriteToConsole("Поток " + Thread.CurrentThread.Name, " добавил " + new_task.Name);

                Thread.Sleep(_random.Next(300, 400));
            }
        }
    }
}
