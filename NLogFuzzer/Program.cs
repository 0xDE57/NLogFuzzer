using NLog;
using NLogFuzzer;

class Program {

    enum LogLevelArray 
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    };

    static void Main()
    {

        int port = GetLogPort(6666);

        NLogManager.SetupNLog(port);
        Logger log = LogManager.GetCurrentClassLogger();
        
        log.Info("--- Hello from fuzzer! ---");
        log.Info("info");
        log.Debug("debug");
        log.Trace("trace");
        log.Warn("warn");
        log.Error("error");
        log.Fatal("fatal");

        while (true)
        {
            DoFuzz(log);
        }
    }

    private static void DoFuzz(Logger log)
    {
        int numFuzz = 1;
        Console.Write("\nNumber of times to fuzz? (leave blank for 1): ");
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }
            if (int.TryParse(input, out numFuzz))
            {
                break;
            }
            Console.WriteLine("Invalid input.");
            Console.Write("Number of times to fuzz? : ");
        }

        Console.WriteLine($"Sending {numFuzz} lines of fuzz...");
        int totalChars = 0;
        for (int i = 0; i < numFuzz; i++)
        {
            //string fuzz = RandomStringGenerator.RandomAscii(10, 2000);
            string fuzz = RandomStringGenerator.RandomUnicode(0, 20000);
            int chars = fuzz.Length;
            totalChars+=chars;

            int level = i % 6;
            //Console.WriteLine($"i: {i}, level: {level} - {(LogLevelArray)level}");
            Console.WriteLine($"{(LogLevelArray)level} {chars} chars.");
            switch (level)
            {
                case (int)LogLevelArray.Info:
                    log.Info(fuzz);
                    break;
                case (int)LogLevelArray.Debug:
                    log.Debug(fuzz);
                    break;
                case (int)LogLevelArray.Trace:
                    log.Trace(fuzz);
                    break;
                case (int)LogLevelArray.Warn:
                    log.Warn(fuzz);
                    break;
                case (int)LogLevelArray.Error:
                    log.Error(fuzz);
                    break;
                case (int)LogLevelArray.Fatal:
                    log.Fatal(fuzz);
                    break;
            }
            Thread.Sleep(100);
        }
        Console.WriteLine($"\nFinished! Sent {totalChars} characters. Did the reciever break?\n");
    }

    public static int GetLogPort(int defaultPort = 6666)
    {
        Console.Write($"Port to log? (leave blank for default[{defaultPort}]): ");

        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"Using default port: {defaultPort}");
                return defaultPort;
            }

            if (int.TryParse(input, out int parsedPort))
            {
                if (parsedPort >= 1 && parsedPort <= 65535)
                {
                    Console.WriteLine($"Port set to: {parsedPort}");
                    return parsedPort;
                }
                else
                {
                    Console.Write("Invalid port range (1-65535). Port to log? : ");
                }
            }
            else
            {
                Console.Write("Invalid input. Port to log? : ");
            }
        }
    }

}