using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace probny
{
    /// <summary>
    /// Logika interakcji dla klasy OknoGry.xaml
    /// </summary>
    public partial class OknoGry : Window
    {
        //-------------Variables-------------

        /// <summary>
        /// Path to project folder
        /// </summary>
        static string project_path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Path to images folder
        /// </summary>
        static string imagePath = System.IO.Path.Combine(project_path, @"obrazki");

        /// <summary>
        /// Path to XML file
        /// </summary>
        static string xmlPath = System.IO.Path.Combine(project_path, @"cards_info.xml");

        /// <summary>
        /// Counter of the number of lost games
        /// </summary>
        
        int liczba_przegranych_komputer;
        int liczba_przegranych_gracz;

        Dictionary <Card, BitmapImage> pulaKart;
        Dictionary<Card, BitmapImage> pulaKartMulligan;
        Deck deckGracz = new Deck();
        Deck deckKomputer = new Deck();


        Gracz gracz;
        Cursor cursor;

        // NULL bitmap. Needed for cleaning.
        BitmapImage nullBitmap = new BitmapImage();

        // Picture of a crystal heart and the back of the card
        BitmapImage bitmapImageKrysztal = new BitmapImage(new Uri(imagePath + "\\serce.png"));
        BitmapImage bitmapImageBack = new BitmapImage(new Uri(imagePath + "\\back.png"));

        //Empty array for cards
        BitmapImage[] images = new BitmapImage[24];

        //Music
        MediaPlayer mediaPlayer = new MediaPlayer();


        //-------------Accessor methods-------------
        internal Dictionary<Card, BitmapImage> PulaKart { get => pulaKart; set => pulaKart = value; }
        internal Dictionary<Card, BitmapImage> PulaKartMulligan { get => pulaKartMulligan; set => pulaKartMulligan = value; }
        public Deck DeckGracz { get => deckGracz; set => deckGracz = value; }
        public Deck DeckKomputer { get => deckKomputer; set => deckKomputer = value; }
        internal Gracz Gracz { get => gracz; set => gracz = value; }

        //-------------db_entity-------------
        /// <summary>
        /// Connection with the entity. And write to the database.
        /// </summary>
        public void Write_to_DB()
        {
            using (var db = new db_entity())
            {
                db.GraczBaza.Add(gracz);
                db.SaveChanges();
            }
        }


        //-------------Constructor methods------------
        /// <summary>
        /// The nonparametric constructor of class OknoGry.
        /// Initialize Component.
        /// Initializes music in the background.
        /// Changes the appearance of the cursor.
        /// Sets the initial variables to zero.
        /// Generates a deck.
        /// </summary>
        public OknoGry()
        {
            InitializeComponent();



            // Start music
            mediaPlayer.Open(new Uri(imagePath + "\\Neverland.mp3"));
            mediaPlayer.Play();


            // Get a path to the custom cursor
            string cursorDirectory = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"obrazki");
            //Changes the cursor style
            cursor = new Cursor($"{cursorDirectory}\\myCursor.cur");
            this.Cursor = cursor;


            // Set counters to 0
            liczba_przegranych_komputer = 0;
            liczba_przegranych_gracz = 0;

            // Create empty Dict
            PulaKart = new Dictionary<Card, BitmapImage>();

            //Hides the lables
            labelStatus.Visibility = Visibility.Hidden;
            label_punkty_komputer.Content = "";
            label_punkty_gracz.Content = "";

            // Reads data from an XML file and generates a deck of cards
            Deck_Generator();

            // Set hp points img
            Image[] k_arr = { k1_gracz, k2_gracz, k3_gracz, k4_gracz, k5_gracz, k1_komp, k2_komp, k3_komp, k4_komp, k5_komp };

            foreach (var item in k_arr)
            {
                item.Source = bitmapImageKrysztal;
            }

        }


        //-------------Custom methods------------
        /// <summary>
        /// Function generates a deck of cards using data contained in XML.
        /// The XML file is divided into identical blocks that have the same ending nodes (leafs ).
        /// Using the loop, the program passes to all nodes and writes each leafs in a variable.
        /// The structure of the node looks like this:
        ///     name - the name of the card
        ///     text - card description
        ///     hp - the amount of health card
        ///     attak - the number of attacks card
        ///     index - node numbering
        ///     img - the name of the card image file located in the folder *obrazki*
        ///     counter - this indicator is needed for the program to know how many duplicates of the card should be made
        /// </summary>
        public void Deck_Generator()
        {
            //Open XML doc
            XmlDocument XML_doc = new XmlDocument();
            XML_doc.Load(xmlPath);

            // go to root element
            XmlElement xRoot = XML_doc.DocumentElement;

            // get all *card* nodes
            XmlNodeList card_nodes = xRoot.SelectNodes("//card");

            //Extracts data from each leaf in an XML file and writes to a variables.
            //Later, we use these variables to generate loops(Counter)
            //get path to image(img) and to transfer data to the Card constructor.
            foreach (XmlNode n in card_nodes)
            {
                string name = n.SelectSingleNode(".//name").InnerText;
                string text = n.SelectSingleNode(".//text").InnerText;
                int hp = short.Parse(n.SelectSingleNode(".//hp").InnerText);
                int attak = short.Parse(n.SelectSingleNode(".//attak").InnerText);

                int index = short.Parse(n.SelectSingleNode(".//id").InnerText);

                string img = n.SelectSingleNode(".//img").InnerText;
                // Convert img to the Bitmap
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath + "\\"+img));
                images[index] = bitmapImage;

                // get number of cards
                int counter = short.Parse(n.SelectSingleNode(".//counter").InnerText);

                // Creates the number of cards specified in the file
                for (int i = 0; i < counter; i++)
                {
                    PulaKart.Add(new Card(name, hp, attak, text), bitmapImage);
                }
            }

        }

        /// <summary>
        /// The function randomly selects cards from the deck and inserts them at  the card slots
        /// </summary>
        /// <param name="img_array"> Array of *places* for cards.</param>
        /// <param name="deck">A deck of cards from which cards will be randomly generated</param>
        public void Shuffle(Image[] img_array, Deck deck)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Card myKey;

            // The cycle runs through each place for the card
            foreach (var item in img_array)
            {
                // Cleans the item
                item.Source = nullBitmap;

                // Generates a random number
                int tmp = rand.Next(0, images.Length - 1);

                // Randomly selects a new card from the deck
                item.Source = images[tmp];

                //???
                myKey = PulaKart.FirstOrDefault(x => x.Value == images[tmp]).Key;

                // Puts card in hand
                deck.CardList.Add(myKey);
            }
        }

        /// <summary>
        /// If there is a card in the deck with an attack equal to one,
        /// then this card is replaced by a random other card.
        /// (Also, this card may have an attack of 1).
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="img_arr"></param>
        /// <returns></returns>
        public async void MulliganAsync(Deck deck, Image[] img_arr)
        {
            // Check each card in hand
            foreach (var (value, i) in img_arr.Select((value, i) => (value, i)))
            {
                //If you have a card in your hand with attack == 1
                if (DeckGracz.CardList[i].Attack == 1)
                {
                    // off the ability to use other buttons
                    btnMulligan.IsEnabled = false;
                    btnPlay.IsEnabled = false;

                    // Random number generation
                    Random rand = new Random(Guid.NewGuid().GetHashCode());
                    int tmp = rand.Next(0, images.Length - 1);

                    // Change the cart to the back img. Looks like turned upside down
                    value.Source = bitmapImageBack;

                    // Timer for the user to understand what happened
                    await Task.Delay(250);

                    // Removes a card from a hand that has 1 attack
                    deck.CardList.Remove(deck.CardList[i]);

                    // Randomly selects a new card from the deck
                    value.Source = images[tmp];

                    //??
                    var myKey = PulaKart.FirstOrDefault(x => x.Value == images[tmp]).Key;

                    // Puts card in hand
                    deck.CardList.Add(myKey);


                    // Highlights the message if the computer changes the card in hand
                    if (deck == DeckKomputer)
                    {
                        labelStatus.Content = "Przeciwnik przetasowal 1 karte";
                        labelStatus.Visibility = Visibility.Visible;
                        await Task.Delay(2000);
                        labelStatus.Visibility = Visibility.Hidden;
                    }

                    btnMulligan.IsEnabled = true;
                    btnPlay.IsEnabled = true;

                    break;
                }
            }

        }





        //-------------Buttons methods------------
        /// <summary>
        /// When pressed, the function changes the cards on the table to new ones randomly selected from the deck of cards.
        /// </summary>
        /// <param name="sender">~</param>
        /// <param name="e">~</param>
        private void Button_Shuffle(object sender, RoutedEventArgs e)
        {
            // Eliminates the possibility of using the *btnShuffle* button again. And allows you to continue the next steps.
            btnShuffle.IsEnabled = false;
            btnMulligan.IsEnabled = true;
            btnPlay.IsEnabled = true;

            //Sets the counters to empty
            label_punkty_gracz.Content = "";
            label_punkty_komputer.Content = "";


            // Set of cards in the player's and AI hands
            Image[] img_array_USER = { img_1, img_2, img_3, img_4 };
            Image[] img_array_PC = { img_5, img_6, img_7, img_8 };

            
            Shuffle(img_array_PC, DeckKomputer);
            Shuffle(img_array_USER, DeckGracz);

            // Allows the computer to change the card with the attack == 1
            MulliganAsync(DeckKomputer, img_array_PC);

        }


        /// <summary>
        /// After clicking, the program calculates the number of points for the player and the computer. 
        ///If the player has more points, the program reports it and takes one unit of health from the computer.
        ///If there are more points in the computer, the player is deprived of 1 unit of health.
        ///if there is a draw (ie points are equal) then no one is wasting their heart.
        ///The game continues until one of the players has lost all his hearts.
        ///There are also motivational messages for the user. :)
        /// </summary>
        /// <param name="sender">~</param>
        /// <param name="e">~</param>
        private async void Button_Play(object sender, RoutedEventArgs e)
        {
            //Variables for counting the number of points
            int sumaGracz = 0;
            int sumaKomputer = 0;

            // Includes the ability to use other buttons
            btnShuffle.IsEnabled = true;
            btnMulligan.IsEnabled = false;
            btnPlay.IsEnabled = false;

            // Hides the message box(label)
            labelStatus.Visibility = Visibility.Hidden;


            if (liczba_przegranych_gracz == 5)
            {
                string message = "Koniec gry, twój wstyd zapiano w bazie danych !";
                string title = "Koniec gry!";
                MessageBox.Show(message, title);

                Write_to_DB();

                return;
            }

            if (liczba_przegranych_komputer == 5)
            {
                string message = "Koniec gry, wielka sprawa wygrać w AI na 600 liniek, Brawo!";
                string title = "Koniec gry!";
                MessageBox.Show(message, title);

                Write_to_DB();

                return;
            }

            // Sums the cost of all cards
            foreach (var (val_a, val_b) in DeckGracz.CardList.Zip(DeckKomputer.CardList,(val_a, val_b) => (val_a, val_b)))
            {
                sumaGracz += val_a.Attack;
                sumaKomputer += val_b.Attack;
            }

            // Show the amount of points
            label_punkty_gracz.Content = sumaGracz.ToString();
            label_punkty_komputer.Content = sumaKomputer.ToString();


            DeckGracz.CardList.Clear();
            DeckKomputer.CardList.Clear();


            // Checking who won
            if (sumaGracz > sumaKomputer)
            {

                labelStatus.Content = "Wygrał gracz...You're just lucky " + gracz.Nick+".";
                labelStatus.Visibility = Visibility.Visible;
                await Task.Delay(2500);
                labelStatus.Visibility = Visibility.Hidden;

                ++liczba_przegranych_komputer;

                switch (liczba_przegranych_komputer)
                {
                    case 1:
                        k1_komp.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        k2_komp.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        k3_komp.Visibility = Visibility.Hidden;
                        break;
                    case 4:
                        k4_komp.Visibility = Visibility.Hidden;
                        break;
                    case 5:
                        k5_komp.Visibility = Visibility.Hidden;
                        break;
                }
            }
            else if (sumaGracz < sumaKomputer)
            {
                labelStatus.Content = "Wygrał MEGA AI BIG BRAIN.";
                labelStatus.Visibility = Visibility.Visible;
                await Task.Delay(2500);
                labelStatus.Visibility = Visibility.Hidden;

                --gracz.Liczba_krysztalow_many;
                ++liczba_przegranych_gracz;
                switch (liczba_przegranych_gracz)
                {
                    case 1:
                        k1_gracz.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        k2_gracz.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        k3_gracz.Visibility = Visibility.Hidden;
                        break;
                    case 4:
                        k4_gracz.Visibility = Visibility.Hidden;
                        break;
                    case 5:
                        k5_gracz.Visibility = Visibility.Hidden;
                        break;
                }
            }
            else
            {
                labelStatus.Content = "Remis";
                labelStatus.Visibility = Visibility.Visible;
                await Task.Delay(2500);
                labelStatus.Visibility = Visibility.Hidden;

            }




        }


        /// <summary>
        /// If the user presses this button, the player's table will be cleared of 
        /// cards and his health will be set to the maximum value, as well as the 
        /// health of the computer. That is, this button starts the game again.
        /// </summary>
        /// <param name="sender">~</param>
        /// <param name="e">~</param>
        private void Button_Restart(object sender, RoutedEventArgs e)
        {
            // Includes the ability to use other buttons
            btnShuffle.IsEnabled = true;
            btnMulligan.IsEnabled = false;
            btnPlay.IsEnabled = false;

            gracz.Liczba_krysztalow_many = 5;
            liczba_przegranych_komputer = 0;
            liczba_przegranych_gracz = 0;
            label_punkty_komputer.Content = "";
            label_punkty_gracz.Content = "";
            DeckGracz.CardList.Clear();
            DeckKomputer.CardList.Clear();

            Image[] images_arr = { img_1, img_2, img_3, img_4, img_5, img_6, img_7, img_8 };
            Image[] k_arr = { k1_gracz, k2_gracz, k3_gracz, k4_gracz, k5_gracz, k1_komp, k2_komp, k3_komp, k4_komp, k5_komp };

            foreach (var item in images_arr)
            {
                item.Source = nullBitmap;
            }

            foreach (var item in k_arr)
            {
                item.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// The function hides the window *OknoGry*, and go to a new window *MainWindow*.
        /// In addition, the function sets the value *gracz.Liczba_krysztalow_many* as 5.
        /// And liczba_przegranych_komputer as 0.
        /// </summary>
        /// <param name="sender">~</param>
        /// <param name="e">~</param>
        private void Button_Main_Menu(object sender, RoutedEventArgs e)
        {
            //When you go to the window *MainWindow* sets these values anew
            gracz.Liczba_krysztalow_many = 5;
            liczba_przegranych_komputer = 0;

            // Start new window *MainWindow*
            MainWindow main = new MainWindow();
            main.Show();

            // Stop music
            mediaPlayer.Stop();

            // Hides the window *Okno gry*
            this.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// If a player presses this button and has a card with an attack equal to one,
        /// he uses a mulligan and draws one card with that strength again. 
        /// A card is removed from its list of cards on the table,
        /// an image of the back of the card is displayed, and a new card is then drawn.
        /// 
        /// A mulligan is an optional process by which any player may attempt to draw a better opening hand before starting the game.
        /// </summary>
        private void Button_Mulligan(object sender, RoutedEventArgs e)
        {
            // Eliminates the possibility of pressing the *btnMulligan* button again
            btnMulligan.IsEnabled = false;

            // Set of cards in the player's hand
            Image[] img_arr = { img_1, img_2, img_3, img_4 };

            // Replace the card with attack 1 if it exists
            MulliganAsync(DeckGracz, img_arr);


        }


    }
}
