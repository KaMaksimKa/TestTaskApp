# Тестовое задание от компании Digital Design
## Задание №1
>Дана БД, имеющая  две таблицы: сотрудники и подразделение.
Необходимо написать 4 запроса.
1. Сотрудника с максимальной заработной платой. 
2. Отдел, с самой высокой заработной платой между сотрудниками.
3. Отдел, с максимальной суммарной зарплатой сотрудников. 
4. Сотрудника, чье имя начинается на «Р» и заканчивается на «н»

### Ответы
1. Select id from employee where salary = (select max(salary) from employee)
2. Select department_id from employee group by department_id having avg(salary)=(select max(s) from (select avg(salary) as s from employee group by department_id) as x)
3. Select department_id from employee group by department_id having sum(salary)=(select max(s) from (select sum(salary) as s from employee group by department_id) as x)
4. Select id from employee where name like 'Р%н'

## Задание №2
>Напишите консольное приложение на C#, которое на вход принимает большой текстовый файл (например «Война и мир», можно взять отсюда http://az.lib.ru/). На выходе создает текстовый файл с перечислением всех уникальных слов, встречающихся в тексте, и количеством их употреблений, отсортированный в порядке убывания количества употреблений.

### Решение (TestTaskApp: написал консольное приложение TestTaskApp для выполнения этого задания, но при выполнении я взял за основу несколько условностей:
* Слова отличающиеся только регистром являются разными словами
* Комбинация цифр является словом
* Наличие знака пунктуации в начале или в конце слова не влияет на уникальность самого слова, то есть, мы удаляем знаки препенания окружающие слово, если такие имеются.


#### Реализация проэкта

#### Класс WordCounterInStream
Я релиозавал класс WordCounterInStream,который реализует интерфейс IDisponsable и в котором релиозованно два поля и 7 функций, не считая конструктор и метод Disponse.
##### Поля
1. Первое поле это `Dictionary<string, int> _dictionary` - это словарь куда будут записываться слова и их количество вхождений в текст.
```C#
private readonly Dictionary<string, int> _dictionary = new Dictionary<string, int>();
```
2. Второе поле это `StreamReader _streamReader` - это поток от куда будет считываться текст.
```C#
private readonly StreamReader _streamReader;
```
##### Функции
1. Функция CheckPunctuationMarksAtEnd на вход принимает строку (string), а возвращает bool. Эта функция проверяет последний символ на то, что он является знаком пунктуации, если да, возвращает true,если нет, возвращает false.
```C#
private bool CheckPunctuationMarksAtEnd(string world)
        {
            if (world != String.Empty)
            {
                char endChar = world.Last();
                return char.IsPunctuation(endChar);
            }

            return false;
        }
 ```
 2 Функция CheckPunctuationMarksAtStart на вход принимает строку (string), а возвращает bool. Эта функция проверяет первый символ на то, что он является знаком пунктуации, если да, возвращает true,если нет, возвращает false.
```C#
private bool CheckPunctuationMarksAtStart(string world)
        {
            if (world != String.Empty)
            {
                char startChar = world.First();
                return char.IsPunctuation(startChar);
            }

            return false;
        }
 ```
3. Функция DeletePunctuationMarksAtEnd принимает на вход string и возвращает тоже string, при этом удаляя все подряд идущие знаки пунктуации с конца.
```C#
 private string DeletePunctuationMarksAtEnd(string world)
        {
            while (CheckPunctuationMarksAtEnd(world))
            {
                world = world.Substring(0,world.Length-1);
            }

            return world;
        }
```
4. Функция DeletePunctuationMarksAtStart принимает на вход string и возвращает тоже string, при этом удаляя все подряд идущие знаки пунктуации с начала.
```C#
 private string DeletePunctuationMarksAtStart(string world)
        {
            while (CheckPunctuationMarksAtStart(world))
            {
                world = world.Substring(1);
            }

            return world;
        }
```
5. Функция DeletePunctuationMarksAtStartAndEnd  принимает на вход string и возвращает тоже string просто на просто объединяет функционал двух функций (DeletePunctuationMarksAtEnd и DeletePunctuationMarksAtStart), то есть, удаляет все подряд идущие знаки пунктуации с начала и с конца.
```C#
 private string DeletePunctuationMarksAtStartAndEnd(string world)
        {
            world = DeletePunctuationMarksAtStart(world);
            world = DeletePunctuationMarksAtEnd(world);
            return world;
        }
```
6. Функция AddWorldToDictionary принимает на вход string и увеличивает счетчик вхождений этого слова в словаре(`Dictionary<string, int> _dictionary`).
```C#
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
```
7. Функция GetDictionaryWords является единственным открытым методом, не считая Disponse, и возвращает копию словаря(`Dictionary<string, int> _dictionary`).
```C#
 public Dictionary<string, int> GetDictionaryWords()
{
    return _dictionary.ToDictionary(kv => kv.Key, kv => kv.Value);
}
```
8. Функция Disponse.
```C#
 public void Dispose()
        {
            _streamReader.Dispose();
        }
```
9. И наконец конструктор WordCounterInStream на вход принимает экземпляр класса StreamReader, после чего присваивает его своей закрытой переменной. После присвоения построчно читает поток(чтобы в случае огромного файла не читать его целиком в память), разбивая каждую строку на слова и удаляя у них знаки пунктуации с начала и с конца, потом добавляет получившиеся слова в словарь.
```C#
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
```

#### Класс Program
В классе Program где находится метод Main, который является точкой входа консольного приложения, создаю экземпляр класса WordCounterInStream и передаю ему StreamReader от файла input.txt, после чего вызываю функцию WriteDictionaryWordsToFile для записи в файл и передаю ей два параметра `Dictionary<string, int>`(вызываю метод GetDictionaryWords на экземпляре класса WordCounterInStream, который создал ранее) и имя файла, куда будет записан результат, в нашем случае output.txt(примечание: при запуске в режиме откладки файл output.txt создается в папке Debug)
* Функция Main.
```C#
public static async Task Main(string[] args)
        {
            string inPath = "input.txt";
            string outPath = "output.txt";

            WordCounterInStream wordCounterInStream = new WordCounterInStream(new StreamReader(inPath));

            await WriteDictionaryWordsToFile(wordCounterInStream.GetDictionaryWords(),outPath);
        }
```
* Функция WriteDictionaryWordsToFile.
```C#
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
 ```
 #### Unit Tests
 Также для класса WordCounterInStream я написал несколько Unit тестов (использовал xUnit)
 ```C#
 public class WordCounterInStreamTests
    {
        private string _firstTestString =
            "Капитан... А вы....слышали о море?.... Об этом огромном, почти бескрайнем озере, уходящем за горизонт?\r\nМы хотели однажды взглянуть на него, на то самое море, которое лежит за стенами. Но я... я давно о нём позабыл... Мы мечтали об этом ещё в детстве. Я хотел убить титанов и отомстить за свою мать. Меня вела вперед только ненависть. Но Армин... он совершенно другой. Он думает не только о войне... У НЕГО ЕСТЬ МЕЧТА";

        private string _secondTestString =
            "Eh bien, mon prince. Gênes et Lucques ne sont plus que des apanages, des поместья, de la famille Buonaparte. Non, je vous préviens que si vous ne me dites pas que nous avons la guerre, si vous vous permettez encore de pallier toutes les infamies, toutes les atrocités de cet Antichrist (ma parole, j'y crois) — je ne vous connais plus, vous n'êtes plus mon ami, vous n'êtes plus мой верный раб, comme vous dites 1. Ну, здравствуйте, здравствуйте. Je vois que je vous fais peur 2, садитесь и рассказывайте.\r\nТак говорила в июле 1805 года известная Анна Павловна Шерер, фрейлина и приближенная императрицы Марии Феодоровны, встречая важного и чиновного князя Василия, первого приехавшего на ее вечер. Анна Павловна кашляла несколько дней, у нее был грипп, как она говорила (грипп был тогда новое слово, употреблявшееся только редкими). В записочках, разосланных утром с красным лакеем, было написано без различия во всех:\r\n«Si vous n'avez rien de mieux à faire, Monsieur le comte (или mon prince), et si la perspective de passer la soirée chez une pauvre malade ne vous effraye pas trop, je serai charmée de vous voir chez moi entre 7 et 10 heures. Annette Scherer» 3.\r\n— Dieu, quelle virulente sortie! 4 — отвечал, нисколько не смутясь такою встречей, вошедший князь, в придворном, шитом мундире, в чулках, башмаках и звездах, с светлым выражением плоского лица.\r\nОн говорил на том изысканном французском языке, на котором не только говорили, но и думали наши деды, и с теми, тихими, покровительственными интонациями, которые свойственны состаревшемуся в свете и при дворе значительному человеку. Он подошел к Анне Павловне, поцеловал ее руку, подставив ей свою надушенную и сияющую лысину, и покойно уселся на диване.\r\n— Avant tout dites-moi, comment vous allez, chère amie? 5 Успокойте меня, — сказал он, не изменяя голоса и тоном, в котором из-за приличия и участия просвечивало равнодушие и даже насмешка.";
        [Fact]
        public void ContentDictionaryWorld()
        {
            WordCounterInStream firstWordCounterInStream =
                new WordCounterInStream(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(_firstTestString))));
            Dictionary<string, int> fistDictionary = firstWordCounterInStream.GetDictionaryWords();
            Assert.Equal(2, fistDictionary["Мы"]);
            Assert.Equal(2, fistDictionary["море"]);
            Assert.DoesNotContain<string,int>("Море",(IDictionary<string, int>)fistDictionary);
            Assert.Equal(71,fistDictionary.Sum(kv=>kv.Value));



            WordCounterInStream secondWordCounterInStream =
                new WordCounterInStream(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(_secondTestString))));
            Dictionary<string, int> secondDictionary = secondWordCounterInStream.GetDictionaryWords();
            Assert.Equal(4, secondDictionary["je"]);
            Assert.Equal(2, secondDictionary["был"]);
            Assert.Equal(3, secondDictionary["не"]);
        }
    }
 ```
 Примечание: все тесты проходят успешно.
