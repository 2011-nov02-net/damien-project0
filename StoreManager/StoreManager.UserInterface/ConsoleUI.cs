using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public enum TimeFrame
        {
            // In the past
            Past,
            // In the future
            Future,
            // Doesn't matter
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
        private static readonly int[] s_daysInMonth = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

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

            string quitString = $"Quit";
            List<string> optionsList = new List<string>(options);
            
            if (withQuit)
                optionsList.Insert(0, quitString);

            int maxWidth = optionsList.Max(str => str.Length) + 4;
            string prompt = "";
            string newLine = consoleOptions.SingleLine ? "" : "\n";
            for (int i = 0; i < optionsList.Count - 1; i++) {
                int currentLength = optionsList[i].Length;
                prompt += BorderOption(border.OptionsBorderChar, $"{i - 1} {border.SeparatingChar} {optionsList[i]}".PadRight(maxWidth, ' ')) + newLine;
            }
            int final = optionsList.Count - 1;
            prompt += BorderOption(border.OptionsBorderChar, $"{final - 1} {border.SeparatingChar} {optionsList[final]}".PadRight(maxWidth, ' '));

            // Display the prompts here
            if (border.BorderWidth > 0) {
                string borderingString = new string(border.BorderChar, border.BorderWidth);
                Console.WriteLine(borderingString);
                Console.WriteLine(prompt);
                Console.WriteLine(borderingString);
            } else {
                Console.WriteLine(prompt);
            }

            return PromptRange("Enter your choice: ", withQuit ? -1 : 0, options.Length - 1);
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

        /// <summary>
        /// Prompts the user for a datetime, and parses it
        /// </summary>
        /// <param name="prompt">The prompt to show the user</param>
        /// <param name="timeFrame">The timeframe context that is targeted</param>
        /// <param name="oneLine">Determines if the prompts for the different points are in one line or split up</param>
        /// <returns>A datetime that the user passes in, if successfully parsed</returns>
        /// <remarks>
        /// Based upon the number of retries, the method could throw a MaximumRetriesMetException
        /// </remarks>
        public static DateTime PromptForDateTime(string prompt, TimeFrame timeFrame, bool oneLine) {
            int tries = 0;

            do {
                DateTime result;
                if (oneLine) {
                    string userInput = ConsoleUI.PromptForInput("Enter the date", false);
                    if (!DateTime.TryParse(userInput, out result)) {
                        ++tries;
                    }
                } else {
                    // Year
                    int year = ConsoleUI.PromptRange("Enter the year", 0, int.MaxValue);
                    // Month
                    int month = ConsoleUI.PromptRange("Enter the month", 1, 12) - 1;
                    // Day
                    // Now for some clever trickery based upon what they last entered
                    bool leapYear = year % 4 == 0;
                    int numberOfDaysInMonth = s_daysInMonth[month];
                    if (month == 1 && leapYear)
                        ++numberOfDaysInMonth;
                    int day = ConsoleUI.PromptRange("Enter the day: ", 1, numberOfDaysInMonth);

                    // Seconds
                    int seconds = ConsoleUI.PromptRange("Enter the number of seconds: ", 0, 59);
                    // Minutes
                    int minutes = ConsoleUI.PromptRange("Enter the number of minutes: ", 0, 59);
                    // Hour
                    int hours = ConsoleUI.PromptRange("Enter the number of hours: ", 0, 23);

                    result = new DateTime(year, month, day, hours, minutes, seconds);
                }

                var now = DateTime.Now;

                switch (timeFrame) {

                    case TimeFrame.Past:
                        // Time should be less than now
                        if (result <= now)
                            return result;
                        else {
                            Console.WriteLine($"Time passed in is invalid; it is after the current time (now: {now}, got: {result}).");
                            break;
                        }

                    case TimeFrame.Future:
                        if (result >= now)
                            return result;
                        else {
                            Console.WriteLine($"Time passed in is invalid; it is before the current time (now: {now}, got: {result}).");
                            break;
                        }

                    case TimeFrame.None:
                        return result;
                }

            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }

        public static string PromptForEmail(string prompt) {
            int tries = 0;

            do {
                // domain name length: 253 characters
                string userInput = PromptForInput("Enter the email", false);
                // Use Regex magic to match
                var regexString = @"^[a-zA-Z]+[a-zA-Z0-9]*\.[a-zA-Z0-9]+\@[a-zA-Z]+[a-zA-Z0-9]+\.([a-zA-Z]+[a-zA-Z0-9]+){0,253}";
                var match = Regex.Match(userInput, regexString);
                if (match.Success) {
                    return userInput;
                } else {
                    ++tries;
                }

            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }

        public static string PromptForPhoneNumber(string prompt) {
            int tries = 0;

            do {
                string userInput = PromptForInput("Enter the phone number", false);
                var regexString = @"^(\+[0-9]{0,3})?[ -]?[0-9]{3}[ -]?[0-9]{3}[ -]?[0-9]{4}";
                var match = Regex.Match(userInput, regexString);
                if (match.Success) {
                    return userInput;
                } else {
                    ++tries;
                }
            } while (tries != s_retryCount);

            throw new MaximumRetriesMetException();
        }
    }
}
