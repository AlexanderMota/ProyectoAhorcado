using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProyectoAhorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String Plb;
        private String PlbMostrar;
        public MainWindow()
        {
            InitializeComponent();
            Plb = PalabraRandom();
            PlbMostrar = EscondePalabra(Plb);
            Reinicia();
            CreaBotonesTeclado(3, 9);
        }
        private void Button_Click_Teclado(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            char l = char.Parse(((Button)sender).Content.ToString());
            if(Plb.Contains(l))
            {
                Respuestas.Text = AñadeLetraAcertada(Plb, Respuestas.Text.ToString(), char.Parse(((Button)sender).Content.ToString()));
                feedback.Content = "Si está";
            }
            else
            {
                feedback.Content = "No está";
                if (int.Parse(Ahorcado.Tag.ToString()) < 9) 
                    CambiaFoto(int.Parse(Ahorcado.Tag.ToString()) + 1);
                else Perdiste();
            }
            if (!Respuestas.Text.Contains("_")) Ganaste();
        }
        private void Button_Click_Opciones(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Equals(BRen)) this.Close();
            else Reinicia();
        }
        public void Ganaste()
        {
            CambiaFoto(11);
            Ahorcado.Tag = 0;
            SwitchTeclado(false);
            feedback.Content = "Victoria!";
            BRen.Background = Brushes.Green;
            BRei.Background = Brushes.Green;
        }
        public void Perdiste()
        {
            Ahorcado.Tag = 0;
            SwitchTeclado(false);
            feedback.Content = "Has perdido.";
            BRen.Background = Brushes.Red;
            BRei.Background = Brushes.Red;
            CambiaFoto(10);
        }
        public void Reinicia()
        {
            CambiaFoto(0);
            Plb = PalabraRandom();
            PlbMostrar = EscondePalabra(Plb);
            Respuestas.Text = PlbMostrar;
            feedback.Content = "";
            BRen.Background = Brushes.White;
            BRei.Background = Brushes.White;
            CreaBotonesTeclado(3, 9);
        }
        private void CambiaFoto(int foto)
        {
            Ahorcado.Tag = foto;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"pack://application:,,,/img/" + foto + ".jpg");
            logo.EndInit();
            Ahorcado.Source = logo;
        }
        public String PalabraRandom()
        {
            Random gen = new Random();
            String[] palabras = File.ReadAllText(@"./datos/palabras.txt").Split('\n');
            return palabras[gen.Next(0, palabras.Length)];
        }
        public String EscondePalabra(String cadena)
        {
            StringBuilder cadenaMostrar = new StringBuilder("");
            for (int i = 0; i < cadena.Length - 1; i++)
                cadenaMostrar.Append(i < cadena.Length - 1 ? "_ " : "_");
            return cadenaMostrar.ToString();
        }
        public void SwitchTeclado(bool s)
        {
            foreach (Button b in teclado.Children)
                b.IsEnabled = s;
        }
        public void CreaBotonesTeclado(int fil, int col)
        {
            int l = 65;
            Button btt;
            for (int i = 0; i < fil; i++)
            {
                for (int e = 0; e < col; e++)
                {
                    btt = new Button()
                    {
                        Style = (Style)Resources["StBTeclado"],
                        Content = (l == 79 && e == 5? 'Ñ' : (char)l)
                    };
                    teclado.Children.Add(btt);
                    if (!btt.Content.Equals('Ñ')) l++;
                    Grid.SetColumn(btt,e);
                    Grid.SetRow(btt, i);
                }
            }
        }
        static String AñadeLetraAcertada(string palabraSecreta, String palabraMostrar, char letra)
        {
            StringBuilder pal = new StringBuilder(palabraMostrar);
            for (int i = 0; i < palabraSecreta.Length - 1; i++)
                if (palabraSecreta[i] == letra) pal[i * 2] = letra;
            return pal.ToString();
        }
    }
}
