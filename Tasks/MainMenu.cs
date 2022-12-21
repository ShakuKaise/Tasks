using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    internal class MainMenu
    {
        static public void Menu()
        {
            ConsoleKeyInfo key;
            int i = 0;
            int pos = 0;
            Process[] processes = Process.GetProcesses();
            List<Task> tasks = UpdateClass(processes);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Task name");
            Console.SetCursorPosition(70, 0);
            Console.WriteLine("ID");
            Console.SetCursorPosition(80, 0);
            Console.WriteLine("Using memory");

            foreach (Task task in tasks)
            {
                Console.SetCursorPosition(3, 2 + i);
                Console.WriteLine(task.name);
                Console.SetCursorPosition(70, 2 + i);
                Console.WriteLine(task.id);
                Console.SetCursorPosition(80, 2 + i);
                Console.WriteLine(task.memory);
                i++;
            }
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(0, pos + 3);
                Console.Write("   ");
                Console.SetCursorPosition(0, pos + 2);
                Console.Write("-->");
                Console.SetCursorPosition(0, pos + 1);
                Console.Write("   ");

                key = Console.ReadKey(true);
                
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (pos + 2 < tasks.Count)
                        {
                            pos++;
                            if (pos == tasks.Count)
                            {
                                pos = 0;
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                        {
                            pos--;
                        }
                        break;
                    case ConsoleKey.D:
                        try
                        {
                            processes[pos].Kill(true);
                            Console.Clear();
                            Menu();
                        }
                        catch(Exception)
                        {

                            Console.SetCursorPosition(0, pos + 2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("   У вас нет прав доступа к этому файлу");

                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Info(processes, pos);
                        Console.WriteLine();
                        Console.WriteLine("Press any key to restart Task manager");
                        Console.ReadKey();
                        Menu();
                        break;
                }

                
            }

        }
        private static void Info(Process[] processes, int pos)
        {
            try
            {
                Console.WriteLine($"  Physical memory usage     : {processes[pos].WorkingSet64}");
                Console.WriteLine($"  Base priority             : {processes[pos].BasePriority}");
                Console.WriteLine($"  Priority class            : {processes[pos].PriorityClass}");
                Console.WriteLine($"  Total processor time      : {processes[pos].TotalProcessorTime}");
                if (processes[pos].Responding)
                {
                    Console.WriteLine("  Status = Running");
                }
                else
                {
                    Console.WriteLine("  Status = Not Responding");
                }
            }
            catch(Exception)
            {
                Console.Clear();
                Console.WriteLine("Отазано в доступе");
            }
            
        }
        private static List<Task> UpdateClass(Process[] processes)
        {
            List<Task> tasks = new List<Task>();

            foreach (Process process in processes)
            {
                tasks.Add(new Task(process.ProcessName, process.Id, process.PagedMemorySize64));
            }

            return tasks;
        }
    }
}
