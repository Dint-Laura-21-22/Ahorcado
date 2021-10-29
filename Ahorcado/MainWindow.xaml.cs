using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Ahorcado
{
    public partial class MainWindow : Window
    {
        List<Char> letras;
        List<BitmapImage> ahorcado;
        List<TextBlock> contenedorPalabra;
        string palabraAdivinar;
        int fallos = 0;
        
        public MainWindow()
        {

            InitializeComponent();
            CrearBotones();
            ahorcado = new List<BitmapImage>();
            EmpezarPartida();


        }



        public void CrearBotones()
        {
            letras = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (Char)i).ToList<Char>();
            letras.Insert(14, 'Ñ');

            foreach (var letra in letras)
            {
                Button letras = new Button();
                letras.Tag = letra;
                TextBlock texto = new TextBlock();
                texto.Text = letra.ToString();
                Viewbox box = new Viewbox();
                box.Child = texto;
                letras.Content = box;
                ContenedorLetrasUniformGrid.Children.Add(letras);
                letras.Click += Button_Click;
                
            }
        }

        private void EmpezarPartida()
        {

            CargarImagen();
            PalabrasWrapPanel.Children.Clear();
            Barras();
        }

        private void CargarImagen()
        {
            for (int i = 4; i < 11; i++)
            {
                var imagen = new BitmapImage(
                new Uri(System.IO.Path.Combine(
                    Environment.CurrentDirectory,
                    "../../assets/" + i.ToString() + ".jpg")));
                ahorcado.Add(imagen);
            }
        }

        private string PalabraSecretaRandom()
        {
            Random gen = new Random();
            List<String> listaPalabras = new List<string>() { "ÑORA", "ALMENDRA", "CABALLO", "CONEJO", "PERRO", "PROGRAMADOR", "JAVA", "OSO", "GALLINA","PAMPLONA" };
            return listaPalabras[gen.Next(0,listaPalabras.Count)];
        }

        private void Barras()
        {
            fallos = 0;
            this.palabraAdivinar = PalabraSecretaRandom();
            ImagenAhorcadoImage.Source = ahorcado[0];
            contenedorPalabra = new List<TextBlock>();

            for (int i = 0; i < this.palabraAdivinar.Length; i++)
            {
                TextBlock contenedor = new TextBlock()
                {
                    Text = "_",
                };
                contenedor.Style = (Style)this.Resources["letrasTextBlock"];
                PalabrasWrapPanel.Children.Add(contenedor);
                contenedorPalabra.Add(contenedor);
                contenedor.FontSize = 40;
            }
        }

        public void Juego(string letra)
        {
            bool acierto = false;

            for (int i = 0; i < this.palabraAdivinar.Length; i++)
            {
                if (palabraAdivinar[i].ToString().Equals(letra))
                {
                    acierto = true;
                    contenedorPalabra[i].Text = letra;
                }
            }
            if (acierto == false)
            {
                fallos++;
                ImagenAhorcadoImage.Source = ahorcado[fallos];
            }
            if (fallos == 6)
            {
                PerderPartida();
            }
            int contador = 0;
            for (int i = 0; i < palabraAdivinar.Length; i++)
            {
                if (contenedorPalabra[i].Text != "_")
                    contador++;
            }
            if (contador == palabraAdivinar.Length)
            {
                GanarPartida();
            }

        }

        public void GanarPartida()
        {
            MessageBox.Show("Biien la palabraAdivinar era " + palabraAdivinar);
        }
        public void PerderPartida()
        {

            ImagenAhorcadoImage.Source = ahorcado[6];

            MessageBox.Show(" =( la palabraAdivinar era " + palabraAdivinar);
        }

        //EVENTOS
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;

            string letra = boton.Tag.ToString();

            Juego(letra);
            boton.IsEnabled = false;
        }


        private void ReiniciarButton_Click(object sender, RoutedEventArgs e)
        {
            ImagenAhorcadoImage.Source = ahorcado[6];
            MessageBox.Show("Te has rendido \nLa palabraAdivinar era " + palabraAdivinar);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            foreach (Button b in ContenedorLetrasUniformGrid.Children)
            {
                if (b.Tag.ToString() == e.Key.ToString())
                {
                    b.IsEnabled = false;
                    Juego(b.Tag.ToString());
                }
            }

        }

        private void NuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            EmpezarPartida();
            
            foreach (Button b in ContenedorLetrasUniformGrid.Children)
            {
                b.IsEnabled = true;
            }
            MessageBox.Show("Nueva Partida");
        }
    }

}

