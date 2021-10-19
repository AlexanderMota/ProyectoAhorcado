using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Path = System.IO.Path;

namespace ProyectoAhorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static String Plb;
        private static String PlbMostrar;
        public MainWindow()
        {
            InitializeComponent();
            Plb = PalabraRandom();
            PlbMostrar = EscondePalabra(Plb);

            Respuestas.Text = PlbMostrar;
            CreaBotonesTeclado(teclado, (Style)Resources["StBTeclado"], 3, 9);
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
                //Hacer que el muñeco cambie. Han surgido muchos problemas, no puedo hacerlo ahora mismo
            }
            if (!Respuestas.Text.Contains("_"))
            {
                feedback.Content = "Has Ganado!";
            }
        }
        private void Button_Click_Opciones(object sender, RoutedEventArgs e)
        {
            if ((((Button)sender).Equals(BRen)))
            {
                this.Close();
            }
            else
            {
                Plb = PalabraRandom();
                PlbMostrar = EscondePalabra(Plb);
                Respuestas.Text = PlbMostrar;
                feedback.Content = "";
                CreaBotonesTeclado(teclado, (Style)Resources["StBTeclado"], 3, 9);
            }
        }
        public static String PalabraRandom()
        {
            Random gen = new Random();
            String[] palabras = File.ReadAllText(@"./datos/palabras.txt").Split('\n');
            return palabras[gen.Next(0, palabras.Length)];

        }
        public static String EscondePalabra(String cadena)
        {
            string cadenaMostrar = "";
            for (int i = 0; i < cadena.Length - 1; i++)
                cadenaMostrar += i < cadena.Length - 1 ? "_ " : "_";
            return cadenaMostrar;
        }
        public static void CreaBotonesTeclado(Grid g, Style r,int fil, int col)
        {
            int l = 65;
            Button btt;
            for (int i = 0; i < fil; i++)
            {
                for (int e = 0; e < col; e++)
                {
                    if (l == 80)
                    {
                        btt = new Button()
                        {
                            Style = (Style)r,
                            Content = 'Ñ',
                        };
                        g.Children.Add(btt);
                        Grid.SetColumn(btt, e);
                        Grid.SetRow(btt, i);
                        e++;
                    }
                    btt = new Button()
                    {
                        Style = (Style)r,
                        Content = (char)l
                    };
                    g.Children.Add(btt);
                    l++;
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
