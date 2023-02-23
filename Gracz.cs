using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace probny
{
    /// <summary>
    /// Klasa reprezentująca gracza
    /// </summary>
    [Serializable]
    public class Gracz : IGrajacy
    {
        public static int liczba_krysztalow_many;
        public String nick;
        public int liczba_pkt;

        /// <summary>
        /// Konstruktor statyczny przypisujący liczbę kryształów many - tzn. liczbę żyć
        /// </summary>
        static Gracz() => liczba_krysztalow_many = 5;
        /// <summary>
        /// Konstruktor nieparamertyczny do ewentualnej seralizacji
        /// </summary>
        public Gracz()
        {

        }
        /// <summary>
        /// Kosntruktor parametryczny
        /// </summary>
        /// <param name="nick">Nazwa gracza</param>
        /// <param name="liczba">Liczba punktów</param>
        public Gracz(string nick, int liczba) : this()
        {
            this.Nick = nick;
            this.liczba_pkt = liczba;
        }
        /// <summary>
        /// Getter oraz setter liczby kryształów many
        /// </summary>
        public int Liczba_krysztalow_many { get => liczba_krysztalow_many; set => liczba_krysztalow_many = value; }
        /// <summary>
        /// Getter oraz setter nicku gracza
        /// </summary>
        public string Nick { get => nick; set => nick = value; }
        /// <summary>
        /// Metoda interfejsu IGrajacy
        /// </summary>
        public void Graj()
        {
        }



        //-------------db_entity-------------
        [Key]
        public int GraczId { get; set; }






    }
}
