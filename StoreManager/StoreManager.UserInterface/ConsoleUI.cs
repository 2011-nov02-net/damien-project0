﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    public class MaximumRetriesMetException : Exception { }

    /// <summary>
    /// A static class containing methods that assist with prompting user input
    /// </summary>
    public class ConsoleUI
    {
        public enum OptionBorderChar
        {
            Brackets,
            Angles,
            Braces,
            Pipes,
            None
        }

        /// <summary>
        /// Class that represents the options that the prompts can be formatted with
        /// </summary>
        public struct ConsoleFormattingOptions
        {
            /// <summary>
            /// Struct that holds the data of the console border
            /// </summary>
            public struct Border
            {
                public Border(int borderWidth, char borderChar, char separatingChar, int? optionsBreak = null, OptionBorderChar optionsBorderChar = OptionBorderChar.None) {
                    BorderWidth = borderWidth;
                    BorderChar = borderChar;
                    SeparatingChar = separatingChar;
                    OptionsBreak = optionsBreak;
                    OptionsBorderChar = optionsBorderChar;
                }

                public int? OptionsBreak { get; set; }
                public OptionBorderChar OptionsBorderChar { get; set; }
                public int BorderWidth { get; private set; }
                public char BorderChar { get; private set; }
                public char SeparatingChar { get; private set; }
            }

            public ConsoleFormattingOptions(Border? borderSettings = null, bool singleLine = false) {
                BorderSettings = borderSettings ?? new Border(0, '\0', '-');
                SingleLine = singleLine;
            }

            /// <summary>
            /// The Border instance that determines what the borders look like
            /// </summary>
            public Border BorderSettings { get; private set; }
            /// <summary>
            /// Sets whether there should be newlines present or not
            /// </summary>
            public bool SingleLine { get; private set; }
        }

        private static int s_retryCount = 3;

        private static string BorderOption(OptionBorderChar optionBorderChar, string option) {
            return optionBorderChar switch
            {
                OptionBorderChar.Brackets => $"[{option}]",
                OptionBorderChar.Angles => $"<{option}>",
                OptionBorderChar.Braces => $"{{{option}}}",
                OptionBorderChar.Pipes => $"|{option}|",
                _ => option,
            };
        }

        /// <summary>
        /// Set the maximum number of tries the user can attempt before quitting
        /// </summary>
        /// <param name="retryCount">The retry count</param>
        public static void SetRetryCount(int retryCount) => s_retryCount = retryCount >= 0 ? retryCount : s_retryCount;

        /// <summary>
        /// Show a menu to the user, and ask for their input
        /// </summary>
        /// <param name="options">A list of strings that represents the options that the user is prompted with</param>
        /// <param name="withQuit">Allow an option to quit out of the menu</param>
        /// <returns>An int value that the user entered</returns>
        public static int PromptForMenuSelection(string[] options, bool withQuit, ConsoleFormattingOptions? consoleFormattingOptions = null) {
            if (options.Length <= 1)
                throw new ArgumentException("Options must have more than 1 passed in.");

            ConsoleFormattingOptions consoleOptions = consoleFormattingOptions ?? new ConsoleFormattingOptions();
            ConsoleFormattingOptions.Border border = consoleOptions.BorderSettings;

            string withQuitOption = withQuit ? $"-1 = Quit,{(consoleOptions.SingleLine ? " " : "\n")}" : "";
            string prompt = $"{withQuitOption}";
            string newLine = consoleOptions.SingleLine ? "" : "\n";
            for (int i = 0; i < options.Length - 1; i++) {
                prompt += BorderOption(border.OptionsBorderChar, $"{i} {border.SeparatingChar} {options[i]}{newLine}");
            }
            int final = options.Length - 1;
            prompt += BorderOption(border.OptionsBorderChar, $"{final} {border.SeparatingChar} {options[final]}");

            // Display the prompts here
            if(border.BorderWidth > 0) {
                string borderingString = new string(border.BorderChar, border.BorderWidth);
                Console.WriteLine(borderingString);
                Console.WriteLine(prompt);
                Console.WriteLine(borderingString);
            } else {
                Console.WriteLine(prompt);
            }

            return PromptRange("Enter your choice: ", withQuit ? -1 : 0, options.Length);
        }

        /// <summary>
        /// Prompt the user for a bool
        /// </summary>
        /// <param name="prompt">The prompt that the user is shown</param>
        /// <param name="trueString">A string that returns true when the user enters it</param>
        /// <param name="falseString">A string that returns false when the user enters it</param>
        /// <returns>A boolean value that the user answered with</returns>
        public static bool PromptForBool(string prompt, string trueString, string falseString) {
            int tries = 0;

            do {
                string userInput = PromptForInput($"{prompt} (enter '{trueString}' or '{falseString}')", false);

                if (userInput.ToLower() == trueString.ToLower())
                    return true;
                else if (userInput.ToLower() == falseString.ToLower())
                    return false;
                else Console.WriteLine($"Invalid input (got '{userInput}', expects: '{trueString}' or '{falseString}').");

                ++tries;

            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }

        /// <summary>
        /// Prompt the user for a value between a specified range
        /// </summary>
        /// <typeparam name="T">The type that is trying to be retrieved</typeparam>
        /// <param name="prompt">The prompt that the user is shown</param>
        /// <param name="min">The minimum value that is accepted</param>
        /// <param name="max">The maximum value that is accepted</param>
        /// <returns>A value between the min and max specified</returns>
        public static T PromptRange<T>(string prompt, T min, T max)
            where T : IComparable<T>, IConvertible {
            if (min.CompareTo(max) >= 0)
                throw new ArgumentException($"Improper arguments entered; max is less than min passed in (min: {min}, max: {max})");

            int tries = 0;

            do {
                string userInput = PromptForInput($"{prompt} (min: {min}, max: {max})", false);

                try {
                    T result = (T)Convert.ChangeType(userInput, typeof(T));

                    if (result.CompareTo(min) < 0)
                        Console.WriteLine($"Value entered is below the min (got: {result}, min: {min})");
                    else if (result.CompareTo(max) > 0)
                        Console.WriteLine($"Value entered is above the max (got: {result}, min: {max})");
                    else return result;

                    ++tries;
                } catch (Exception) {
                    Console.WriteLine($"Invalid input entered (got '{userInput}')");
                    ++tries;
                }

            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }

        /// <summary>
        /// Get user input
        /// </summary>
        /// <param name="message">The prompt the user is shown</param>
        /// <param name="allowEmpty">Allow the user to enter nothing</param>
        /// <returns>Input that the user entered</returns>
        public static string PromptForInput(string message, bool allowEmpty) {
            int tries = 0;
            string orLeaveEmpty = allowEmpty ? " (or leave empty)" : "";
            do {
                Console.Write($"{message}{orLeaveEmpty}: ");
                string userInput = Console.ReadLine();

                if (!allowEmpty && string.IsNullOrWhiteSpace(userInput))
                    Console.WriteLine($"Value entered is whitespace; please enter valid input");
                else return userInput;

                ++tries;

            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }
    }
}