using System.Globalization;
using System.Reflection.Metadata;

namespace TestTaskApp {
    public static class Program
    {
        
        public static async Task Main(string[] args)
        {
            string inPath = "input.txt";
            string outPath = "output.txt";

            StreamReader reader = new StreamReader(inPath);
            WordCounterInStream wordCounterInStream = new WordCounterInStream(reader);

            await WriteDictionaryWordsToFile(wordCounterInStream.GetDictionaryWords(),outPath);
        }

        public static async Task WriteDictionaryWordsToFile(Dictionary<string, int> dictionary,string fileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                foreach (var (word, count) in dictionary.OrderByDescending(kv => kv.Value))
                {
                    await streamWriter.WriteAsync(word + " " + count + "\n");
                }
                await streamWriter.FlushAsync();
            }
        }

        
    }
}

