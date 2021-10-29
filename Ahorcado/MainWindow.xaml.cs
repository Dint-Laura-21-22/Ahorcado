﻿using System;
using System.Collections.Generic;
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

namespace Ahorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock palabra = new TextBlock();
        String palabraAdivinar;
        String rallitasPalabra;
        char[] palabraSecretaArray;
        public MainWindow()
        {

            InitializeComponent();
            // Creamos una lista para los botones //
            List<Char> CaracteresTeclado = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char)i).ToList<Char>();
            CaracteresTeclado.Insert(14, 'Ñ');
            
           
            // Añadimos los controles y rellenamos //
            foreach (var letraCaracter in CaracteresTeclado)
            {
                Button letrasBoton = new Button();
                letrasBoton.Tag = letraCaracter;
                TextBlock texto = new TextBlock();
                texto.Text = letraCaracter.ToString();
                Viewbox box = new Viewbox();
                box.Child = texto;
                letrasBoton.Content = box;
                ContenedorLetrasUniformGrid.Children.Add(letrasBoton);
                letrasBoton.Click += Button_Click;
            }

            // Creamos el contenedor de la palabra //
            ScrollViewer scroll = new ScrollViewer();
            scroll.Content = palabra;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            PalabrasWrapPanel.Children.Add(scroll);
            palabra.Style = (Style)Application.Current.Resources["PalabrasTexto"];
            palabraAdivinar = PalabraSecretaRandom();
            rallitasPalabra = RallitasPalabra(palabraAdivinar);
            palabra.Text = rallitasPalabra;
            palabra.Width = 800;
            palabra.Height = 300;
            palabraSecretaArray = palabraAdivinar.ToCharArray();



        }

        private int estado = 4;
        public BitmapImage GetStageImage()
        {
            return new BitmapImage(new Uri(System.IO.Path.Combine(
                Environment.CurrentDirectory,"../../assets/" + estado + ".jpg")));
        }
        public String PalabraSecretaRandom()
        {
            List<String> Palabras = new List<string>() { "PAMPLONA"/*,"PROGRAMADOR", "HIELO", "OSO", "JAVA", "GALLINA", "CONEJO","ÑORA"*/ };
            Random rand = new Random();
            return Palabras[rand.Next(0, Palabras.Count())];
        }

        private String RallitasPalabra(String cadena)
        {
            StringBuilder PalabraRallitas = new StringBuilder();
            for (int i = 0; i < cadena.Length; i++)
            {
                palabra.Text = PalabraRallitas.Append(" _ ").ToString();
            }
            return PalabraRallitas.ToString();

        }

        // Cambio de texto si la palabra es correcta //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;
            boton.IsEnabled = false;
            char letraPulsada = (char)boton.Tag;
            Comprobar(letraPulsada);

        }

        public void Comprobar(char letra)
        {
         
           
            for (int i = 0; i < palabraAdivinar.Length; i++)
            {
                if (palabraSecretaArray[i].Equals(letra))
                {
                    palabra.Text= rallitasPalabra.Replace(rallitasPalabra[i], letra).ToString();
                }
                else
                {
                    MessageBox.Show("Letra incorrecta");
                }
            }
             
        }

       

        // Añadir al XAML//
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            foreach (Button b in ContenedorLetrasUniformGrid.Children)
            {
                if (b.Tag.ToString() == e.Key.ToString())
                {
                    b.IsEnabled = false;
                    Comprobar((char)b.Tag);
                }
            }
        }



        private void ReiniciarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Te has rendido" + palabraAdivinar);
            estado = 10;
            palabraAdivinar = PalabraSecretaRandom();
            ImagenAhorcadoImage.Source = GetStageImage();
            foreach (Button b in ContenedorLetrasUniformGrid.Children)
            {
                b.IsEnabled = true;
                
            }

        }

        private void NuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Has iniciado una nueva partida");
            foreach (Button b in ContenedorLetrasUniformGrid.Children)
            {
                b.IsEnabled = true;
                
            }
            estado = 4;
            ImagenAhorcadoImage.Source = GetStageImage();
            palabraAdivinar = PalabraSecretaRandom();
        }
    }
}
