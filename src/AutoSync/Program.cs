using System;
using System.Threading;

namespace AutoSync
{
    class Program
    {
        static CancellationTokenSource cts = new CancellationTokenSource();

        static bool interactiveMode = true;

        static void Main(string[] args)
        {
            //default in interactive mode
            for(int i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith('-'))
                {
                    continue;
                }
                switch (args[i])
                {
                    case "-l":
                    case "--log":
                        interactiveMode = false;
                        Console.WriteLine("Enable log mode.");
                        break;
                    default:
                        Console.WriteLine("Unknwon argument '{0}'", args[i]);
                        break;
                }
            }

            EventBus.StartSpider(cts.Token);

            if (interactiveMode)
            { 
                do
                {
                    string k = Console.ReadLine();
                    if (k == null)
                    {
                        break;
                    }
                    string[] cmd = k.Split(' ');
                    if (cmd[0] == "exit")
                    {
                        break;
                    }
                    switch (cmd[0])
                    {
                        case "dl":
                        case "download":
                            Console.WriteLine("Create a new download task");
                            break;
                        case "tasks":
                            Console.WriteLine("{0,-5}{1,-15}{2,-10}{3,-60}", "ID", "TaskName", "Progress","Description");
                            Console.WriteLine("".PadLeft(90, '-'));
                            foreach (var v in EventBus.activeTasks)
                            {
                                Console.WriteLine("{0,-5}{1,-15}{2,-10}{3,-60}", v.ID, v.TaskType, v.Progress+"%",v.Description);
                            }
                            break;
                        default:
                            Console.WriteLine("Command not found: {0}", cmd[0]);
                            break;
                    }
                }
                while (true);
                Cleanup(false);
            }
            else
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    Cleanup(true);
                };
                while (Console.Read() > 0)
                {
                    //wait
                }
            }
        }

        static void Cleanup(bool force)
        {
            cts.Cancel();
            if (!force)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
