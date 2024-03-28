using System.Text.RegularExpressions;

namespace SuperConsole
{
    public sealed class IO
    {
        private readonly Dictionary<string, ConsoleColor>? textColors;

        private static readonly Lazy<IO> lazy =
            new(() => new IO());

        public static IO Instance { get { return lazy.Value; } }

        public static bool InstanceCreated { get { return lazy.IsValueCreated; } }

        private IO()
        {
            textColors = new Dictionary<string, ConsoleColor>()
            {
                { "default", ConsoleColor.White },
                { "green", ConsoleColor.Green },
                { "blue", ConsoleColor.Blue },
                { "cyan", ConsoleColor.Cyan },
                { "magenta", ConsoleColor.Magenta },
                { "red", ConsoleColor.Red },
                { "yellow", ConsoleColor.Yellow },
                { "white", ConsoleColor.White },
                { "black", ConsoleColor.Black },
                { "gray", ConsoleColor.Gray }
            };
        }

        /// <summary>
        /// This method can clear the console and change colors of a given string directly.
        /// </summary>

        public void Write(
            string text,
            bool? clear = false,
            bool? newline = false,
            string? foreground = "white",
            string? background = null
        )
        {
            if (clear is true) Console.Clear();

            Console.ForegroundColor = textColors![foreground!];
            if (background is not null)
            {
                Console.BackgroundColor = textColors[background];
            }

            Console.Write(text);

            if (newline is true)
            {
                Console.Write(Environment.NewLine);
            }
            Console.ResetColor();
        }

        /// <summary>
        /// This method takes a preencoded string and writes colors the sequences based on the [$color] sequence signifier.
        /// </summary>

        public void WriteEncoded(string text, bool? clear = false, bool? newline = false)
        {
            if (textColors is null)
                throw new TextColorNotSetException();

            if (clear is true)
                Console.Clear();

            string encoderPattern = @"(\[\w+\])";
            string colorMatchPattern = @"blue|yellow|magenta|cyan|red|green|white|black|gray";

            string[] stringList = Regex.Split(text, encoderPattern);

            bool nextColored = false;

            foreach (string str in stringList)
            {
                Match match = Regex.Match(str, encoderPattern);

                if (nextColored)
                {
                    if (match.Success)
                    {
                        nextColored = !nextColored;
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(str);
                    }
                }
                else
                {
                    if (match.Success)
                    {
                        nextColored = !nextColored;

                        Match colorMatch = Regex.Match(str, colorMatchPattern);

                        string color = Regex.Replace(str, @"[\[\]]", "");

                        Console.ForegroundColor = textColors[color];
                    }
                    else
                    {
                        Console.Write(str);
                    }
                    if (newline is true)
                    {
                        Console.Write(Environment.NewLine);
                    }
                }
            }
        }

        public void ClearAll()
        {
            Console.Clear();
        }

        public string? ReadAndClear()
        {
            if (this is null) throw new Exception("An IO instance is required!");

            string input = Console.ReadLine()!;
            Console.Clear();
            return input;
        }
    }
}
