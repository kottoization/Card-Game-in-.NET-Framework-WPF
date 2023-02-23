using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace probny
{
    /// <summary>
    /// Klasa reprezentująca okno główne gry
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Klasa reprezentująca kursor myszki
        /// </summary>
        Cursor cursor;
        /// <summary>
        /// Metoda inicjalizująca okno główne
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();
            RegisterEvents();
            string cursorDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(
        Assembly.GetExecutingAssembly().Location), @"obrazki");
            cursor = new Cursor($"{cursorDirectory}\\myCursor.cur");
            this.Cursor = cursor;
        }
        /// <summary>
        /// Metoda zapewniająca działanie przycisków w oknie głównym tak aby wywoływały odpowiednie metody jednostkowe
        /// </summary>
        private void RegisterEvents()
        {
            btn_nowaGra.MouseEnter += btn_nowaGra_MouseEnter;
            btn_zasady.MouseEnter += btn_zasady_MouseEnter;
            btn_wyjscieGra.MouseEnter += btn_wyjscieGra_MouseEnter;

            btn_nowaGra.MouseLeave += MainMenu_MouseLeave;
            btn_zasady.MouseLeave += MainMenu_MouseLeave;
            btn_wyjscieGra.MouseLeave += MainMenu_MouseLeave;
        }
        /// <summary>
        /// Metoda zapewniająca nie wyświetlanie się podświetlanego podpisu gdy kursor znajduje sie poza zasięgiem przycisku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            txtBlk_Info.Text = string.Empty;
        }
        /// <summary>
        ///  Metoda zapewniająca wyświetlanie się podświetlanego podpisu z pytaniem czy wyjść z gry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_wyjscieGra_MouseEnter(object sender, MouseEventArgs e)
        {
            txtBlk_Info.Text = "Wyjść z gry ? ";
            txtBlk_Info.Foreground = Brushes.AntiqueWhite;
            txtBlk_Info.FontWeight = FontWeights.Bold;
        }
        /// <summary>
        /// Metoda zapewniająca wyświetlanie się podświetlanego podpisu z pytaniem czy wyświetlić zasady
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_zasady_MouseEnter(object sender, MouseEventArgs e)
        {
            txtBlk_Info.Text = " Wyświetlić zasady ? ";
            txtBlk_Info.Foreground = Brushes.AntiqueWhite;
            txtBlk_Info.FontWeight = FontWeights.Bold;
        }
        /// <summary>
        /// Metoda zapewniającawyświetlanie się podświetlanego podpisu z pytaniem czy Rozpocząć nową grę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_nowaGra_MouseEnter(object sender, MouseEventArgs e)
        {
            txtBlk_Info.Text = "Rozpocząć nową grę ? ";
            txtBlk_Info.Foreground = Brushes.AntiqueWhite;
            txtBlk_Info.FontWeight = FontWeights.Bold ;
        }
        /// <summary>
        /// Przycisk obsługujący rozpoczęcie nowej gry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_nowaGra_Click(object sender, RoutedEventArgs e)
        {
            InputDialog newOkno = new InputDialog();
            this.Visibility = Visibility.Hidden;
            newOkno.Show();
        }
        /// <summary>
        /// Przycisk obsługujący wyjście z gry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_wyjscieGra_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Przycisk obsługujący pojawienie się okna zasad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_zasady_Click(object sender, RoutedEventArgs e)
        {
            OknoZasady newOkno = new OknoZasady();
            this.Visibility = Visibility.Hidden;
            newOkno.Show();
        }
    }
}


