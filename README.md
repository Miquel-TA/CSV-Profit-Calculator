# Profit Calculator by Miquel Trujillo

This .NET Framework 4.8 C# code will calculate stock balance progression over time based on user-defined parameters and data from a CSV file.
This code has been made for learning purposes at Vueling University.
This project uses MVC system. Different classses are separated into their respective folders depending on their usage.

# How to Use

1. Compile and run the program using a C# compiler.
2. The program will prompt you to enter various required parameters for reading, interactacting and displaying data.
3. Provide the required inputs as requested by the program.
4. Once all the parameters are provided, the program will read the data from the CSV file and perform the profit calculation.
5. The calculated results will be saved to an output text file with a timestamp.
6. The file path of the output text file will be displayed on the console.

# Program Structure

The program consists of several classes:

- `Program`: The main class that contains the entry point of the program. It interacts with the user, reads the CSV data, and performs the profit calculation.
- `ProfitCalculator`: This class performs the profit calculation based on the provided parameters and the data from the CSV file.
- `InputManager`: This class handles user input and validation.
- `DataPoint`: Represents a data point containing the date, opening price, and closing price.
- `CsvDataManager`: Responsible for reading the CSV file and extracting the necessary data points.

# Dependencies

The program relies on the following external libraries:

- `System`: Provides basic system and data manipulation functionalities.
  - `System.Collections.Generic`: Provides generic collection classes.
  - `System.Globalization`: Provides culture-specific information and formatting.
  - `System.IO`: Provides file input/output operations.
  - `System.Text`: Provides encoding and string manipulation.
  - `System.LinQ`: Provides query-like interaction with objects.
- `CsvHelper`: A library for reading and writing CSV files efficiently.

# Detailed Class Functionality

The `Program` class serves as the entry point of the program. Its main responsibilities include:

- Initializing the required variables and objects.
- Prompting the user for input parameters.
- Reading data from a CSV file.
- Calling the `ProfitCalculator` class to perform profit calculations.
- Saving the calculated results to an output text file.
- Displaying the file path of the output file on the console.

The `InputManager` class handles user input and validation. Its basic functionality includes:

- Providing a generic method, `Ask<T>`, to prompt the user for input and parse the input to the specified type `T`.
- Validating the input based on the specified requirements, such as checking file existence for file paths.
- Clearing the console and displaying error messages for invalid input until valid input is provided.
- Returning the parsed and validated input value of type `T`.

The `CsvDataManager` class handles the reading and management of CSV data. Its main functionality includes:

- Initializing the required variables based on the provided CSV file path and column indices.
- Reading the CSV file using `CsvHelper` library, with specified configuration options such as value separator and encoding.
- Mapping the CSV columns to the properties of the `DataPoint` class using a custom mapping.
- Sorting the data points by date in ascending order.
- Returning a list of `DataPoint` objects representing the data read from the CSV file.

The `DataPoint` class represents a single data point containing the date, opening price, and closing price. Its purpose is to store the data read from the CSV file.

The `ProfitCalculator` class is responsible for performing the profit calculation based on the provided parameters and data from the CSV file. Its main functionality includes:

- Initializing the required variables based on the input parameters.
- Iterating over the deposit dates and performing calculations for each deposit.
- Finding the closest data point to the deposit date.
- Calculating the current stock value, stocks to buy, stock count, and total deposited amount.
- Generating output logs for each deposit with relevant information.
- Calculating the final balance and generating the last output log.
- Returning a list of output logs containing the calculated results.
