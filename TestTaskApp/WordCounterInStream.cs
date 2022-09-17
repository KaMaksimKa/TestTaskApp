using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskApp
{
    public class WordCounterInStream:IDisposable
    {
        private readonly Dictionary<string, int> _dictionary = new Dictionary<string, int>();
        private readonly StreamReader _streamReader;
        public WordCounterInStream(StreamReader streamReader)
        {
            _streamReader = streamReader;
            while (streamReader.ReadLine() is { } line)
            {
                List<string> listWorld = line.Split(" ").ToList();
                foreach (var world in listWorld)
                {
                    string rightWorld = DeletePunctuationMarksAtStartAndEnd(world);
                    AddWorldToDictionary(rightWorld);
                }
            }
        }
        public Dictionary<string, int> GetDictionaryWords()
        {
            return _dictionary.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        private  void AddWorldToDictionary(string world)
        {
            if (world != String.Empty)
            {
                if (_dictionary.ContainsKey(world))
                {
                    _dictionary[world]++;
                }
                else
                {
                    _dictionary[world] = 1;
                }
            }
        }
        private bool CheckPunctuationMarksAtEnd(string world)
        {
            if (world != String.Empty)
            {
                char endChar = world.Last();
                return char.IsPunctuation(endChar);
            }

            return false;
        }
        private bool CheckPunctuationMarksAtStart(string world)
        {
            if (world != String.Empty)
            {
                char startChar = world.First();
                return char.IsPunctuation(startChar);
            }

            return false;
        }
        private string DeletePunctuationMarksAtEnd(string world)
        {
            while (CheckPunctuationMarksAtEnd(world))
            {
                world = world.Substring(0,world.Length-1);
            }

            return world;
        }
        private string DeletePunctuationMarksAtStart(string world)
        {
            while (CheckPunctuationMarksAtStart(world))
            {
                world = world.Substring(1);
            }

            return world;
        }
        private string DeletePunctuationMarksAtStartAndEnd(string world)
        {
            world = DeletePunctuationMarksAtStart(world);
            world = DeletePunctuationMarksAtEnd(world);
            return world;
        }
        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
