﻿using System;
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
                Respuestas.Text = AñadeLetraAcertada(Plb, Respuestas.Text, char.Parse(((Button)sender).Content.ToString()));
            }
            else
            {
                // hacer que cambie el muñeqioto
            }
            if (!Respuestas.Text.Contains("_"))
            {
                feedback.Content = "Has Ganado!";
                //detectar cuando se gane
            }
        }
        private void Button_Click_Opciones(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            if ((((Button)sender).Equals(BRen)))
            {
                this.Close();
            }
            else
            {
                InitializeComponent();
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
        static bool HaAcertado(string palabraMostrar, string palabraSecreta)
        {
            int i = 0; bool flag = false;
            do
            {
                flag = palabraSecreta[i] == (i == 0 ? palabraMostrar[0] : palabraMostrar[i * 2]);
                i++;
            } while (flag == true && i < palabraSecreta.Length);
            return flag;
        }
        static String AñadeLetraAcertada(string palabraSecreta, String palabraMostrar, char letra)
        {
            StringBuilder pal = new StringBuilder(palabraMostrar);
            for (int i = 0; i < palabraSecreta.Length; i++)
                if (palabraSecreta[i] == letra) pal[i * 2] = letra;
            return pal.ToString();
        }

    }
}