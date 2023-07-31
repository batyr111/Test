using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TestFIle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Получаем путь к каталогу с текстовыми файлами и путь к файлу со списком слов
            string directoryPath = "";
            string wordsFilePath = "";            
            while (true)
            {
                Console.Write("Введите путь к каталогу с текстовыми файлами: ");
                directoryPath = Console.ReadLine();
                if (Directory.Exists(directoryPath))
                    break;
                else
                    Console.WriteLine("Указанный каталог не существует. Попробуйте еще раз.");
            }

            while (true)
            {
                Console.Write("Введите путь к файлу со списком слов: ");
                wordsFilePath = Console.ReadLine();
                if (File.Exists(wordsFilePath))
                    break;
                else
                    Console.WriteLine("Указанный файл не существует. Попробуйте еще раз.");
            }
           
            // Запрашиваем выбор регистра
            Console.Write("Учитывать регистр? (Да/Нет): ");
            bool ignoreCase = Console.ReadLine().Equals("Да", StringComparison.OrdinalIgnoreCase);


            // Читаем файл со списком слов
            string[] words = File.ReadAllLines(wordsFilePath);

            // Словарь для хранения статистики по файлам
            Dictionary<string, int> fileStatistics = new Dictionary<string, int>();

            // Перебираем все текстовые файлы в указанном каталоге
            foreach (string filePath in Directory.EnumerateFiles(directoryPath, "*.txt"))
            {
                // Читаем текстовый файл
                string text = File.ReadAllText(filePath);

                // Подсчитываем количество вхождений каждого слова
                Dictionary<string, int> wordCounts = new Dictionary<string, int>();
                foreach (string word in words)
                {
                    // Создаем регулярное выражение для поиска слова с учетом знаков препинания и пробелов
                    string pattern = @"\b" + Regex.Escape(word) + @"\b";
                    // Ищем все вхождения слова в тексте и учитываем регистр
                    MatchCollection matches = Regex.Matches(text, pattern, ignoreCase ? RegexOptions.IgnoreCase: RegexOptions.None);
                    wordCounts[word] = matches.Count;
                }

                // Выводим статистику для текущего файла
                Console.WriteLine($"Статистика для файла {Path.GetFileName(filePath)}:");
                foreach (var pair in wordCounts)
                {
                    Console.WriteLine($"Слово '{pair.Key}': {pair.Value} вхождений");
                }
                Console.WriteLine();

                // Добавляем количество вхождений каждого слова к общей статистике по файлам
                foreach (var pair in wordCounts)
                {
                    if (fileStatistics.ContainsKey(pair.Key))
                        fileStatistics[pair.Key] += pair.Value;
                    else
                        fileStatistics[pair.Key] = pair.Value;
                }
            }

            // Выводим суммарную статистику по словам без учета файлов
            Console.WriteLine("Суммарная статистика:");
            foreach (var pair in fileStatistics)
            {
                Console.WriteLine($"Слово '{pair.Key}': {pair.Value} вхождений");
            }

            Console.ReadLine();
        }
    }
}
