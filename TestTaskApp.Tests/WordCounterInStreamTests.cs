using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskApp.Tests
{
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
}
