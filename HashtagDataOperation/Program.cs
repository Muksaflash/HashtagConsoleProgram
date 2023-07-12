using HashtagDataOperation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Logic = HashtagsWebApplication.Service.Implementations.GeneralService.Logic;
using HashtagsWebApplication.Domain.ViewModels.Funnel;

string newLine = Environment.NewLine;

string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

string jsonString = string.Empty;
JObject jsonConfig = new JObject();
var lastUserIndex = 0;
Console.WriteLine("Добро пожаловать в удивительный, компьютеризированный мир хэштегов!");
var instaParserAdress = "instaparser.ru";
var parserImAdress = "parser.im";

if (!File.Exists("config.json"))
    CreateUser(jsonConfig, ref lastUserIndex);

jsonString = File.ReadAllText("config.json");
jsonConfig = JObject.Parse(jsonString);
lastUserIndex = int.Parse(jsonConfig["lastUserIndex"].ToString());
GreetUser(jsonConfig, lastUserIndex);
var parserImKey = jsonConfig["users"][lastUserIndex]["parserImKey"].ToString();
var instaParserKey = jsonConfig["users"][lastUserIndex]["instaParserKey"].ToString();

var menuVariants = "Если вы хотите скачать законченные задания Parser.Im, то отправьте 1." + newLine +
        "Если вы хотите скачать законченные задания InstaParser, то отправьте 2." + newLine +
        "Если вы хотите из файлов с фильтрацией от Parser.Im или InstaParser собрать хештеги и частоту," +
        " то отправьте 3." + newLine +
        "Если вы хотите создать воронку, то отправьте 4." + newLine +
        "Если вы хотите сменить пользователя или добавить ключи, то отправьте 5.";

while (true)
{
    parserImKey = jsonConfig["users"][lastUserIndex]["parserImKey"].ToString();
    instaParserKey = jsonConfig["users"][lastUserIndex]["instaParserKey"].ToString();
    Console.WriteLine();
    Console.WriteLine(menuVariants);
    var enteredNumber = Console.ReadLine();

    if (enteredNumber == "1")
    {
        if (parserImKey == "")
        {
            Console.WriteLine("Добавьте API ключ Parser.Im к вашему пользователю");
            continue;
        }
        TakeCompletedFiltrTasks(parserImKey, parserImAdress);
    }

    if (enteredNumber == "2")
    {
        if (instaParserKey == "")
        {
            Console.WriteLine("Добавьте API ключ InstaParser к вашему пользователю");
            continue;
        }
        TakeCompletedFiltrTasks(instaParserKey, instaParserAdress);
    }

    if (enteredNumber == "3")
    {
        Console.WriteLine("Вы положили только нужные файлы в папку parser на рабочем столе?");
        Console.WriteLine("Когда положите, нажмите любую кнопку.");
        Console.ReadKey();
        try { Logic.RedoFiles(); }
        catch { Console.WriteLine("Данные неправильного формата"); }
    }

    if (enteredNumber == "4")
    {
        var minFreq = 0;
        var maxFreq = 0;
        var freqInterval = 0;
        var numberInFunnel = 0;
        try
        {
            CreateFunnelViewModel model = new CreateFunnelViewModel();
            Console.WriteLine("Введите необходимую минимальную частотность хештегов");
            model.MinFreq = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите необходимую максимальную частотность хештегов");
            model.MaxFreq = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите минимально допустимый интервал частотности");
            model.MinFreqInterval = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите необходимое количество хештегов в паках");
            model.FunnelHashtagsAmount = int.Parse(Console.ReadLine());

            Console.WriteLine("Засуньте хештеги с частотностью в файл Хештеги.txt на рабочем столе " +
               "и нажмите любую кнопку");
            Console.ReadKey();
            Console.WriteLine();
            var hashtagTXTPath = Path.Combine(desktopPath, "Хештеги.txt");
            var fileName = "Воронка.txt";
            StreamReader sr = new StreamReader(hashtagTXTPath);
            List<string?> listHashtags = new();
            while (!sr.EndOfStream)
            {
                listHashtags.Add(sr.ReadLine());
            }
            sr.Close();
            fileName = Logic.CreateNameNewFile(fileName, desktopPath);
            Logic.CreateFile(desktopPath, fileName);
            var createFunnelsOutput = Logic.CreateFunnels(model, listHashtags);
            Logic.FileAppendLines(createFunnelsOutput.Item1, desktopPath, fileName);
            Console.WriteLine(createFunnelsOutput.Item2);
        }
        catch { Console.WriteLine("Введите цифры правильно"); }
    }

    if (enteredNumber == "5")
    {
        var userNumber = 0;
        Console.WriteLine("Если вы хотите создать нового пользователя, то отправьте 0");
        Console.WriteLine("Если вы хотите редактировать ваши даннные, то отправьте 1");
        var menuNumber = Console.ReadLine();
        if (menuNumber == "0")
        {
            foreach (var user in jsonConfig["users"])
            {
                userNumber++;
                Console.WriteLine(userNumber.ToString() + ") " + user["nickName"]);
            }
            Console.WriteLine();
            Console.WriteLine("Отправьте номер выбранного пользователя, или отправьте 0 для добавления" +
                " нового");
            var enteredUserNumber = Console.ReadLine();
            if (enteredUserNumber == "0")
            {
                CreateUser(jsonConfig, ref lastUserIndex);
                GreetUser(jsonConfig, lastUserIndex);
            }
            else
            {
                lastUserIndex = int.Parse(enteredUserNumber) - 1;
                ChangeLastUserIndex(jsonConfig, lastUserIndex);
                GreetUser(jsonConfig, lastUserIndex);
            }
        }
        if (menuNumber == "1")
        {
            var itemNumber = 0;
            Console.WriteLine();
            foreach (var item in jsonConfig["users"][lastUserIndex])
            {
                itemNumber++;
                Console.WriteLine(itemNumber.ToString() + ") " + item);
            }
            Console.WriteLine("Введите название поля, которое хотите изменить, либо 0 для выхода");
            var itemName = Console.ReadLine();
            if (itemName == "0") continue;
            try
            {
                Console.WriteLine("Введите новое значение " + jsonConfig["users"][lastUserIndex][itemName].Path);
            }
            catch
            {
                Console.WriteLine("Неправильно введено название поля");
                continue;
            }
            jsonConfig["users"][lastUserIndex][itemName] = Console.ReadLine();
            File.WriteAllText("config.json", jsonConfig.ToString());
            GreetUser(jsonConfig, lastUserIndex);
            Console.WriteLine();
        }
    }
}

static void CreateUser(JObject jsonConfig, ref int lastUserIndex)
{
    Console.WriteLine("Введите ваше имя");
    var userName = Console.ReadLine();
    var parserImAPIKey = string.Empty;
    var instaParserAPIKey = string.Empty;
    Console.WriteLine("Если вы хотите добавить возможность работать с Parser.Im, отправьте 1");
    if (Console.ReadLine() == "1")
    {
        Console.WriteLine("Введите API ключ Parser.Im");
        parserImAPIKey = Console.ReadLine();
    }
    Console.WriteLine("Если вы хотите добавить возможность работать с InstaParser, отправьте 1");
    if (Console.ReadLine() == "1")
    {
        Console.WriteLine("Введите API ключ InstaParser");
        instaParserAPIKey = Console.ReadLine();
    }
    var user = new User(userName, parserImAPIKey, instaParserAPIKey);
    var usersList = new List<User>() { user };
    string jsonString1 = JsonConvert.SerializeObject(usersList);
    var jsonObject1 = JArray.Parse(jsonString1);

    if (!jsonConfig.TryAdd("users", jsonObject1))
        jsonConfig["users"][0].AddBeforeSelf(jsonObject1[0]);

    jsonConfig.Remove("lastUserIndex");
    jsonConfig.Add("lastUserIndex", "0");
    File.WriteAllText("config.json", jsonConfig.ToString());
}

static void ChangeLastUserIndex(JObject jsonConfig, int lastUserIndex)
{
    jsonConfig.Remove("lastUserIndex");
    jsonConfig.Add("lastUserIndex", lastUserIndex.ToString());
    File.WriteAllText("config.json", jsonConfig.ToString());
}

static void GreetUser(JObject jsonConfig, int lastUserIndex)
{
    Console.WriteLine("Здравствуйте, всемогущий бог " +
        jsonConfig["users"][lastUserIndex]["nickName"].ToString());
}

static void TakeCompletedFiltrTasks(string parserKey, string parserArdress)
{
    var response = Logic.ParseCompletedTasks(parserKey, parserArdress);
    var jsonData = JObject.Parse(response);
    var filtrTaskList = jsonData["tasks"].Where(task => task["type"].ToString() == "f1")
        .OrderBy(task => task["add_time"]).ToList();
    var taskNumber = filtrTaskList.Count() + 1;
    try
    {
        foreach (var task in filtrTaskList) 
            Console.WriteLine((--taskNumber).ToString() + ") " + task["name"]);
    }
    catch
    {
        Console.WriteLine(jsonData);
        return;
    }
    Console.WriteLine();
    Console.WriteLine("Выберите файлы для скачивания в папку Parser на рабочем столе и напишите их номера через пробел");
    Console.WriteLine();
    Console.WriteLine("Если вы маньяк и хотите скачать все фильтрации, то отправьте слово \"маньяк\"");
    var message = Console.ReadLine();
    if (message == "маньяк")
        foreach (var task in filtrTaskList)
            {
                var taskName = task["name"].ToString();
                var taskID = int.Parse(task["tid"].ToString());
                Logic.SaveDataFile(taskID, taskName, parserKey, parserArdress);
            }
    var selectedNumbersString = message;
    IEnumerable<int>? selectedNumbers;
    try
    {
        selectedNumbers = selectedNumbersString.Split(' ').Select(int.Parse);
        foreach (var number in selectedNumbers)
        {
            var selectorTask = filtrTaskList.Count() - number;
            try
            {

                var taskName = filtrTaskList[selectorTask]["name"].ToString();
                var taskID = int.Parse(filtrTaskList[selectorTask]["tid"].ToString());
                Logic.SaveDataFile(taskID, taskName, parserKey, parserArdress);
            }
            catch { Console.WriteLine("Точно есть такие задания? Проверьте номера."); }
        }
    }
    catch
    {
        Console.WriteLine("Неправильно ввели цифры");
        return;
    }
}