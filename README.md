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
* При наличии знака препенания в начале или в конце слова не влияет на уникальность самого слова, то есть, мы удаляем знаки препенания окружающие слово, если такие имеются.
Я релиозавал класс WordCounterInStream, в котором релиозованно 7 функций, не считая конструктор.

#### Реализация проэкта

1. Функция CheckPunctuationMarksAtEnd на вход принимает строку (string), а возвращает bool. Эта функция проверяет последний символ на то, что он является знаком препинания, если да, возвращает true,если нет, возвращает false.
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
 2 Функция CheckPunctuationMarksAtStart на вход принимает строку (string), а возвращает bool. Эта функция проверяет первый символ на то, что он является знаком препинания, если да, возвращает true,если нет, возвращает false.
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


