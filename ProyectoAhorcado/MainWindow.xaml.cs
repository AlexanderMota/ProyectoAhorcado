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
        /*private void CreaGrid()
        {
            Grid g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
        }*/
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String l =e.Key.ToString();

            if (CompruebaTecla(l))
            {
                ApagaTeclaTF(l);
                if (Plb.Contains(l))
                {
                    VisorPalabra.Text = AñadeLetraAcertada(Plb, VisorPalabra.Text.ToString(), char.Parse(l));
                    FeedbackA.Text = "Si está";
                }
                else
                {
                    FeedbackA.Text = "No está";
                    if (int.Parse(Ahorcado.Tag.ToString()) < 9)
                        CambiaFoto(int.Parse(Ahorcado.Tag.ToString()) + 1);
                    else Perdiste();
                }
                if (!VisorPalabra.Text.Contains("_")) Ganaste();
            }
        }
        private void Button_Click_Teclado(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            char l = char.Parse(((Button)sender).Tag.ToString());
            if(Plb.Contains(l))
            {
                VisorPalabra.Text = AñadeLetraAcertada(Plb, VisorPalabra.Text.ToString(), char.Parse(((Button)sender).Tag.ToString()));
                FeedbackA.Text = "Si está";
            }
            else
            {
                FeedbackA.Text = "No está";
                if (int.Parse(Ahorcado.Tag.ToString()) < 9) 
                    CambiaFoto(int.Parse(Ahorcado.Tag.ToString()) + 1);
                else Perdiste();
            }
            if (!VisorPalabra.Text.Contains("_")) Ganaste();
        }
        private void Button_Click_Opciones(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Equals(BRen)) this.Close();
            else Reinicia();
        }
        public void Ganaste()
        {
            CambiaFoto(11);
            SwitchTeclado(false);
            Ahorcado.Tag = 0;
            FeedbackA.Text = "Victoria!";
            BRen.Background = Brushes.Green;
            BRei.Background = Brushes.Green;
            Contenedor.Style = (Style)Resources["StGVictoria"];
        }
        public void Perdiste()
        {
            CambiaFoto(10);
            SwitchTeclado(false);
            Ahorcado.Tag = 0;
            FeedbackA.Text = "Has perdido.";
            BRen.Background = Brushes.Red;
            BRei.Background = Brushes.Red;
            Contenedor.Style = (Style)Resources["StGDerrota"];
        }
        public void Reinicia()
        {
            CambiaFoto(0);
            CreaBotonesTeclado(3, 9);
            Plb = PalabraRandom();
            PlbMostrar = EscondePalabra(Plb);
            VisorPalabra.Text = PlbMostrar;
            FeedbackA.Text = "";
            BRen.Background = Brushes.White;
            BRei.Background = Brushes.White;
            Contenedor.Style = (Style)Resources["StG"];
        }
        private void CambiaFoto(int numFoto)
        {
            Ahorcado.Tag = numFoto;
            BitmapImage dirFoto = new BitmapImage();
            dirFoto.BeginInit();
            dirFoto.UriSource = new Uri(@"pack://application:,,,/img/" + numFoto + ".jpg");
            dirFoto.EndInit();
            
            Ahorcado.Background = new ImageBrush(dirFoto);
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
        public void ApagaTeclaTF(String c)
        {
            foreach (Button b in teclado.Children)
                if (b.Tag.ToString().Equals(c))
                    b.IsEnabled = false;
        }
        public bool CompruebaTecla(String c)
        {
            foreach (Button b in teclado.Children)
                if (b.Tag.ToString().Equals(c))
                    return b.IsEnabled;
            return false;
        }
        public void CreaBotonesTeclado(int fil, int col)
        {
            teclado.Children.Clear();
            int l = 65;
            TextBlock tb;
            Viewbox vb;
            Button btt;
            for (int i = 0; i < fil; i++)
            {
                for (int e = 0; e < col; e++)
                {
                    tb = new TextBlock()
                    {
                        Text = (l == 79 && e == 5 ? 'Ñ' : (char)l).ToString()
                    };
                    vb = new Viewbox();
                    vb.Child = tb;
                    btt = new Button()
                    {
                        Style = (Style)Resources["StBTeclado"],
                        Content = vb,
                        Tag = (l == 79 && e == 5 ? 'Ñ' : (char)l).ToString()
                    };
                    teclado.Children.Add(btt);
                    if (!btt.Tag.Equals("Ñ")) l++;
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
