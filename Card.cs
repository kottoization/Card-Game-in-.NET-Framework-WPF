using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace probny
{
    /// <summary>
    /// Klasa reprezentująca kartę którą posługują się gracze
    /// </summary>
        public class Card : IComparable<Card>
        {
            string name;
            int hp;
            int attack;
            string text;
        /// <summary>
        /// Nazwa karty
        /// </summary>
            public string Name { get => name; set => name = value; }
        /// <summary>
        /// Liczba punktów zdrowia, do wykorzystania w kolejnych wersjach gry
        /// </summary>
            public int Hp { get => hp; set => hp = value; }
        /// <summary>
        /// Liczba reprezentująca moc karty - liczba punktów ataku
        /// </summary>
            public int Attack { get => attack; set => attack = value; }
        /// <summary>
        /// Opis karty
        /// </summary>
            public string Text { get => text; set => text = value; }
        /// <summary>
        /// Konstruktor domyślny, przygotowany na przyszłość w razie serializacji do pliku .xml
        /// </summary>
            public Card()
            {

            }
        /// <summary>
        /// Konstruktor parametryczny nadający wartośći propertom klasy
        /// </summary>
        /// <param name="name"> nazwa karty</param>
        /// <param name="hp">liczba pkt. zdrowia </param>
        /// <param name="attack">liczba pkt. ataku </param>
        /// <param name="text">opis katrty</param>
            public Card(string name, int hp, int attack, string text)
            {
                this.Name = name;
                this.Hp = hp;
                this.Attack = attack;
                this.Text = text;
            }
        /// <summary>
        /// Metoda implemetująca interfejs IComparable, przyrównująca daną kartę do przekazanej w argumencie metody 
        /// </summary>
        /// <param name="other">przekazana jako argument do porównania </param>
        /// <returns>int jako typ zwracany operacji porównania</returns>
        public int CompareTo(Card other)
        {
            if (other == null)
                return 1;
            int wynik = this.Attack.CompareTo(other.Attack);
            if (wynik == 0)
            {
                wynik = this.Attack.CompareTo(other.Attack);
            }
            return wynik;
        }
    }

}