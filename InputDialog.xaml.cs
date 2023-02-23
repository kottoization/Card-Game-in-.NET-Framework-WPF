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
using System.Windows.Shapes;

namespace probny
{
    /// <summary>
    /// Logika interakcji dla klasy InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        Cursor cursor;
        OknoGry oknoGry;
        bool clicked = false;
        public InputDialog()
        {
            InitializeComponent();
            string cursorDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(
         Assembly.GetExecutingAssembly().Location), @"obrazki");
            cursor = new Cursor($"{cursorDirectory}\\myCursor.cur");
            this.Cursor = cursor;
        }

        public InputDialog(OknoGry oknoGry) : this()
        {
            this.OknoGry = oknoGry;
        }

        public OknoGry OknoGry { get => oknoGry; set => oknoGry = value; }
        public bool Clicked { get => clicked; set => clicked = value; }

        public void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            Clicked = true;
            Gracz gracz = new Gracz(this.txtAnswer.Text,0);
            OknoGry newOkno = new OknoGry();
            newOkno.Gracz = gracz;
            newOkno.Gracz.Nick = this.txtAnswer.Text;
            this.Visibility = Visibility.Hidden;
            newOkno.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            OknoGry.Hide();
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
