using System;
using System.ComponentModel;
using System.IO;

namespace ProfitCalculator
{
    public class InputManager
    {
        public T AskUser<T>(string prompt, bool mustCheckFilePath = false)
        {
            T result;
            string userInput;
            bool isValid;

            do
            {
                Console.WriteLine(prompt);
                userInput = Console.ReadLine();
                Console.Clear();

                isValid = CheckInput<T>(userInput, mustCheckFilePath, out result);

                if (!isValid)
                {
                    Console.WriteLine("Something's wrong! Check it and try again.");
                }

            } while (!isValid);

            return result;
        }

        public bool CheckInput<T>(string userInput, bool mustCheckFilePath, out T result)
        {
            bool isValid = TryParseParameter<T>(userInput, out result);

            if (result.Equals(default(T)) && isValid)
            {
                isValid = false;
            }

            if (mustCheckFilePath && isValid)
            {
                if (!File.Exists(userInput) || !userInput.ToLower().EndsWith(".csv"))
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private bool TryParseParameter<T>(string userInput, out T result)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T)converter.ConvertFromString(userInput);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

    }
}
