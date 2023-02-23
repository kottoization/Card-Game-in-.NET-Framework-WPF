using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace probny
{
    /// <summary>
    /// Klasa reprezentująca talię kart 
    /// </summary>
    public class Deck
        {
        string name;
        List<Card> cardList;

        /// <summary>
        /// Nazwa talii
        /// </summary>
        public string Name { get => name; set => name = value; }
        internal List<Card> CardList { get => cardList; set => cardList = value; }
        /// <summary>
        /// Konstruktor inicjujący kolekcję jako pulę kart
        /// </summary>
        public Deck()
            {
                name = null;
                cardList = new List<Card>();
            }
        /// <summary>
        /// Konstruktor parametryczny do przypisania nazwy do talii
        /// </summary>
        /// <param name="name"></param>
            public Deck(string name)
            {
                Name = name;
            }

            internal void AddCard(Card c)
            {
            cardList.Add(c);
            }
        /// <summary>
        /// Metoda usuwająca kartę wg podanej nazwy o ile karta o takiej nazwie istnieje w talii
        /// </summary>
        /// <param name="name"></param>
            public void RemoveCard(string name)
            {
                Card card = cardList.FirstOrDefault(x => x.Name == name);
                if (card != null) cardList.Remove(card);
            }
        /// <summary>
        /// Klasa reperzentująca generator liczb losowych
        /// </summary>
            public Random rnd = new Random();
        /// <summary>
        /// Metoda służąca do tasowania obiektów klasy
        /// </summary>
            public void Shuffle()
            {
                var randomized = cardList.OrderBy(item => rnd.Next()).ToList();
                this.CardList = randomized;
            }
    }
}

