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
