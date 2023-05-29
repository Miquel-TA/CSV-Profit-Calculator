using System;
using System.ComponentModel;
using System.IO;

namespace ProfitCalculator
{
    public static class InputManager
    {
        public static T Ask<T>(string prompt, bool mustCheckPath = false)
        {
            T result;
            string input;
            bool isValid;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                isValid = TryParseParameter<T>(input, out result);

                if (isValid)
                {
                    if (result.Equals(default(T)))
                    {
                        isValid = false;
                    }

                    if (mustCheckPath)
                    {
                        if (!File.Exists(input) || !input.ToLower().EndsWith(".csv"))
                        {
                            isValid = false;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Something's wrong! Check it and try again.");
                }
            } while (!isValid);

            Console.Clear();
            return result;
        }

        private static bool TryParseParameter<T>(string input, out T result)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T)converter.ConvertFromString(input);
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
