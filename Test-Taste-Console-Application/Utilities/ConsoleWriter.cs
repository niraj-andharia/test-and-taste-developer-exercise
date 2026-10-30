using System;
using System.IO;
using System.Text;
using Test_Taste_Console_Application.Constants;

namespace Test_Taste_Console_Application.Utilities
{
    public static class ConsoleWriter
    {
        // Generates a unique timestamped file per run in the project root folder (navigating up from bin/Debug/net5.0/)
        private static readonly string OutputDirectory = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, @"..\..\..\FileOutput"));

        private static readonly string OutputFilePath = Path.Combine(
            OutputDirectory,
            $"SolarSystemOutput_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt"
        );

        static ConsoleWriter()
        {
            if( !Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);
        }

        /// <summary>
        /// Writes text to both the console window and the unique timestamped text file on disk,
        /// while handling potential file I/O exceptions, printing warnings, and logging via log4net.
        /// </summary>
        private static void WriteLineToBoth(string text = "")
        {
            // 1. Output to Console
            Console.WriteLine(text);

            // 2. Output to Disk and handle/log any unexpected I/O failures
            try
            {
                File.AppendAllText(OutputFilePath, text + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                string errorMessage = $"{ExceptionMessage.ScreenOutputOperationFailed} Could not write output text to file path '{OutputFilePath}': {ex.Message}";

                // Keep the user informed on the console without crashing the application flow
                Console.WriteLine($"[Warning] {errorMessage}");

                // Safely log the error using the project's configured log4net Logger instance
                try
                {
                    Logger.Instance.Error(errorMessage);
                }
                catch (Exception logEx)
                {
                    Console.WriteLine($"[Critical] Failed to write to log4net: {logEx.Message}");
                }
            }
        }

        public static void CreateHeader(string[] columnLabels, int[] columnSizes)
        {
            CreateLine(columnSizes);
            CreateText(columnLabels, columnSizes);
            CreateLine(columnSizes);
        }

        public static void CreateText(string[] columnValues, int[] columnSizes)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('|');
            for (int i = 0; i < columnValues.Length; i++)
            {
                stringBuilder.Append(columnValues[i].PadRight(columnSizes[i]));
                stringBuilder.Append('|');
            }

            WriteLineToBoth(stringBuilder.ToString());
        }

        public static void CreateLine(int[] columnSizes)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('+');
            foreach (var columnSize in columnSizes)
            {
                stringBuilder.Append(new string('-', columnSize));
                stringBuilder.Append('+');
            }

            WriteLineToBoth(stringBuilder.ToString());
        }

        public static void CreateEmptyLines(int updateCount)
        {
            for (int i = 0; i < updateCount; i++)
            {
                WriteLineToBoth();
            }
        }
    }
}