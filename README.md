# HashtagDataOperation [version 0.9]

## Hashtags and Parsers Console Application
This console application is a tool designed to work with hashtags and data parsers from the web services Parser.Im and InstaParser.

### Key Features

*User Creation/Edit*

- When the program starts, users can either create a new user or select an existing one.
Users can provide their name and add API keys for Parser.Im and InstaParser access.
Downloading Completed Tasks from Parsers

- Users can choose to download completed tasks from either Parser.Im or InstaParser.
They are required to specify the file numbers they want to download, or they can download all files at once.
Data Filtering from Parsers

- Users can filter data from files obtained from Parser.Im or InstaParser.
To filter data, users must place the necessary files in the parser folder on their desktop.
After filtering, the results will be saved in a new file.
Creating Funnels

- Users can create funnels by selecting hashtags with specific frequencies.
To create a funnel, users need to specify the minimum and maximum hashtag frequencies, the minimum allowed frequency interval, and the number of hashtags per package.
The user should place hashtags with frequencies into a file named Hashtags.txt on their desktop, and after processing, the results will be saved in a new file.


### Dependencies

- The program utilizes the HashtagDataOperation, Newtonsoft.Json.

### Running the Program

- To run the program, a config.json file is required, containing user information and API keys. If the file is missing, the program will prompt the user to create the first user and add keys for working with parsers.

- After successful initialization, the user can select the desired action by entering the corresponding digit from the menu.

### Technologies Used

- C# Programming Language:
The entire program is written in C#, a versatile and object-oriented programming language developed by Microsoft. C# is widely used for building various types of applications, including console applications like this one.

- Newtonsoft.Json:
The Newtonsoft.Json library is used to handle JSON data in the program. It allows for easy serialization and deserialization of JSON objects, enabling efficient data exchange between the application and external files.

- Logic:
This is a module or namespace that contains classes related to funnel creation. It might include view models and data structures needed to manage funnel-related data and operations.

- Environment Class:
The Environment class from the .NET Framework is used to access system environment variables. In the program, it is used to determine the desktop directory and create file paths for storing and retrieving data.

- File I/O Operations:
The program makes use of file input/output (I/O) operations to read and write data from and to external files. File I/O is essential for saving user configurations and processed data.

- Console Interaction:
The program heavily relies on interactions with the console to display messages, menus, and receive user inputs. Console interactions make the application user-friendly and enable seamless communication with users.

- API Communication:
To interact with the web services Parser.Im and InstaParser, the program uses API calls to access and download completed tasks and filter data. The API keys provided by users are used to authenticate and access the services.

- Exception Handling:
The program incorporates exception handling mechanisms to gracefully handle potential errors or invalid user inputs. Proper exception handling ensures the program continues running smoothly and provides meaningful error messages.

- Data Structures:
Various data structures like lists and dictionaries are employed to store and manage data efficiently throughout the program's execution.

### Notes

- The program assumes that API keys for Parser.Im and InstaParser are stored in the config.json file.

- The program uses not the most profitable algorithm for creating funnels.
  
- Various methods for data handling are used in the program, including reading from files, creating new files, filtering, and saving results.

- Users should follow the program's instructions and provide correct inputs for seamless application execution.

- The application may display error messages in case of incorrect user actions or API key issues.

### Conclusion

- This convenient console application is designed to simplify hashtag management and data retrieval from parsers. With its user-friendly interface and intuitive commands, users can easily perform various tasks related to hashtag analysis and data from parsers.
