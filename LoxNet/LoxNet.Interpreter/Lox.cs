using System;
using System.IO;
using LoxNet.Interpreter.Components;

namespace LoxNet.Interpreter
{
    class Lox
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
                Console.WriteLine("Usage: loxnet [script]");
            else if (args.Length == 1)
                RunFile(args[0]);
            else
                RunPrompt();
            
            if (_hadError)
                Environment.Exit(65);
            else
                Environment.Exit(0);
        }

        private static void RunFile(string path)
        {
            if (!File.Exists(path)) 
                Console.WriteLine("File does not exist!");
            else
                Run(File.ReadAllText(path));
        }

        private static void RunPrompt()
        {
            while(true)
            {
                Console.Write("> ");
                Run(Console.ReadLine());
                _hadError = false;
            }
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public static void ReportError(int line, string message)
        {
            ReportError(line, "", message);
        }

        private static bool _hadError;
        private static void ReportError(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            _hadError = true;
        }
    }
}
