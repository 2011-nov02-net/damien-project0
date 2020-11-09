using System;
using System.Collections.Generic;
using System.Linq;

public class MaximumRetriesMetException : Exception { }

/// <summary>
/// A static class containing methods that assist with prompting user input
/// </summary>
public static class ConsoleUI
{
    private static int _retryCount;

    /// <summary>
    /// Set the maximum number of tries the user can attempt before quitting
    /// </summary>
    /// <param name="retryCount">The retry count</param>
    public static void SetRetryCount(int retryCount) => _retryCount = retryCount >= 0 ? retryCount : _retryCount;

    /// <summary>
    /// Show a menu to the user, and ask for their input
    /// </summary>
    /// <param name="options">A list of strings that represents the options that the user is prompted with</param>
    /// <param name="withQuit">Allow an option to quit out of the menu</param>
    /// <returns>An int value that the user entered</returns>
    public static int PromptForMenuSelection(string[] options, bool withQuit, bool singleLine) {
        if (options.Length <= 1)
            throw new ArgumentException("Options must have more than 1 passed in.");

        List<string> prompts = new List<string>(options);
        int current = 0;
        string prompt = $"{(singleLine ? "-1 = Quit, " : "")}{string.Join(singleLine ? ", " : ",\n", prompts.Select(prompt => $"{current++} - {prompt}"))}";

        Console.WriteLine(prompt);

        return PromptRange("Enter your choice: ", withQuit ? -1 : 0, options.Length + (withQuit ? 1 : 0));
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
            string userInput = PromptForInput(prompt, false);

            if (userInput.ToLower() == trueString.ToLower())
                return true;
            else if (userInput.ToLower() == falseString.ToLower())
                return false;
            else Console.WriteLine($"Invalid input (got '{userInput}', expects: '{trueString}' or 'falseString').");

            ++tries;

        } while (tries != _retryCount);

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
            string userInput = PromptForInput(prompt, false);

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

        } while (tries != _retryCount);

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

        do {
            Console.Write(message);
            string userInput = Console.ReadLine();

            if (!allowEmpty && string.IsNullOrWhiteSpace(userInput))
                Console.WriteLine($"Value entered is whitespace; please enter valid input");
            else return userInput;

            ++tries;

        } while (tries != _retryCount);

        throw new MaximumRetriesMetException();
    }
}
