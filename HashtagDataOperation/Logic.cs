
namespace HashtagDataOperation
{
    static public class Logic
    {
        static readonly char separator = Path.DirectorySeparatorChar;
        static readonly string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        static readonly string parserFolderPath = Path.Combine(desktopPath, $"Parser{separator}");
        static readonly string newLine = Environment.NewLine;
        
        public static void RedoFiles()
        {
            var freqDict = new Dictionary<string, int>();
            Console.WriteLine($"{newLine}Введите минимальную частотность встречи хэштегов");
            var MinFreq = int.Parse(Console.ReadLine());

            var files = FindFiles(parserFolderPath, ".txt");

            var listHashtags = GetDataParser(files);

            IEnumerable<IEnumerable<string>> hashtagFreq = listHashtags
                .Where(line => line.Split(':').Length == 5)
                .Where(line => line.Split(':')[4] != 0.ToString())
                .Select(line => line.Split(':')[4].Split(','));

            var text1 = hashtagFreq.SelectMany(line => line);
            var text2 = text1.Select(x => "#" + x);

            foreach (var word in text2)
            {
                if (!freqDict.ContainsKey(word))
                    freqDict[word] = 1;
                else
                    freqDict[word] += 1;
            }

            Console.WriteLine();

            var fileName = "Частота.txt";
            fileName = CreateNameNewFile(fileName, desktopPath);

            foreach (var word in freqDict)
            {
                if (word.Value >= MinFreq)
                {
                    Console.WriteLine(word.Key + '\t' + word.Value);
                    File.AppendAllText(Path.Combine(desktopPath, fileName), word.Key + '\t' + word.Value + newLine);
                }

            }
            Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
            Console.WriteLine(newLine + $"Создан файл \"{fileName}\" на рабочем столе!");
            Console.ResetColor(); // сбрасываем в стандартный
        }

        private static string CreateNameNewFile(string fileName, string folder)
        {
            string newFileName = fileName;
            var fileCount = 1;
            while (File.Exists(Path.Combine(folder, newFileName)))
            {
                fileCount++;
                newFileName = fileName.Substring(0, fileName.Length-4) + " (" + fileCount.ToString() + ")" + ".txt";
            }
            return newFileName;
        }

        static IEnumerable<string> FindFiles(string folderName, string desiredExtension)
        {
            List<string> salesFiles = new List<string>();

            var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

            foreach (var file in foundFiles)
            {
                var extension = Path.GetExtension(file);
                if (extension == ".txt")
                {
                    salesFiles.Add(file);
                }
            }

            return salesFiles;
        }
        static IEnumerable<string?> GetDataParser(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                StreamReader sr = new StreamReader(file);
                while (!sr.EndOfStream)
                {
                    yield return sr.ReadLine();
                }
                sr.Close();
            }
        }
        public static void SaveDataFile(int tid, string fileName, string key, string adress)
        {
            var filePath = Path.Combine(parserFolderPath, fileName + ".txt");

            var request = new GetRequest($"https://instaparser.ru/api.php?key=E87oaGdl6yu9kv3C&mode=result&tid={tid}");
            request.Run();
            var fileData = ParseCompletedTasks(key, adress);
            File.AppendAllText(filePath, fileData);
        }

        public static void CreateFunnels(int minFreq, int maxFreq, int freqInterval, int numberInFunnel)
        {
            Console.WriteLine("Засуньте хештеги с частотностью в файл Хештеги.txt на рабочем столе " +
                "и нажмите любую кнопку");
            Console.ReadKey();
            Console.WriteLine();

            var hashtagTXTPath = Path.Combine(desktopPath, "Хештеги.txt");
            var outPath = desktopPath;
            StreamReader sr = new StreamReader(hashtagTXTPath);
            List<string?> listHashtags = new();
            while (!sr.EndOfStream)
            {
                listHashtags.Add(sr.ReadLine());
            }
            sr.Close();
            ILookup<int, string> hashtagFreq = listHashtags
                .Where(line => !string.IsNullOrEmpty(line))
                .ToLookup
                (line =>
                {
                    int key;
                    try
                    {
                        key = int.Parse(string.Join("", line.Split('\t')[1].Where(symbol => char.IsDigit(symbol))));
                    }
                    catch
                    {
                        return -1;
                    }
                    return key;
                },
                line =>
                {
                    string value;
                    value = line.Split('\t')[0];
                    return value;
                });

            var floorFreq = minFreq;
            var topFreq = maxFreq;
            var freqStep = freqInterval;
            var hashtagFunnelNumber = numberInFunnel;

            var hashtagFreq1 = hashtagFreq.Where(x => x.Key >= floorFreq && x.Key <= topFreq);
            var count = hashtagFreq1.ToList().Count;
            var hashtagFreq2 = hashtagFreq1.ToDictionary(group => group.Key, group => group.ToList())
                .OrderBy(group => group.Key);
            int nextBound;

            int hashtagFunnelCount;
            int fullFunnelCount = 0;
            int funnelCount = 0;
            bool isDictEmpty = false;
            if (count == 0) isDictEmpty = true;

            var fileName = "Воронка.txt";
            fileName = CreateNameNewFile(fileName, outPath);

            while (!isDictEmpty)
            {
                hashtagFunnelCount = hashtagFunnelNumber;
                nextBound = floorFreq;
                File.Exists(Path.Combine(outPath, fileName));
                File.AppendAllText(Path.Combine(outPath, fileName), Environment.NewLine);
                foreach (var item in hashtagFreq2)
                {
                    if (item.Key >= nextBound)
                    {
                        if (item.Value.Count != 0)
                        {
                            File.AppendAllText(Path.Combine(outPath, fileName), item.Value[0] + '\t' + item.Key + newLine);
                            item.Value.RemoveAt(0);
                            nextBound = item.Key + freqStep;
                            if (--hashtagFunnelCount == 0)
                            {
                                fullFunnelCount++;
                                break;
                            }
                        }
                    }
                }
                foreach (var item in hashtagFreq2)
                {
                    if (item.Value.Count != 0)
                    {
                        isDictEmpty = false;
                        break;
                    }
                    isDictEmpty = true;
                }
                funnelCount++;
            }

            if (count == 0)
            {
                Console.WriteLine("В указанной папке отсутствовали записи нужной частотности " +
                    "либо неправильный формат!" +
                    " Работа завершена!");
            }
            else
                Console.WriteLine($"Создано " + fullFunnelCount + " воронок по " + hashtagFunnelNumber
                    + " хэштегов и ещё " +
                    +funnelCount + " поменьше" + newLine + newLine +
                    "Хештеги записаны в файл \"" + fileName + "\"",
                    "Работа успешно завершена!");
        }

        public static string ParseCompletedTasks(string apiKey, string adress)
        {
            var request = new GetRequest($"https://{adress}/api.php?key={apiKey}&mode=status&tid=&status=3");
            request.Run();
            return request.Responce;
        }
    }
}
