using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Projektwoche.Umfrage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            YesDataGrid.ItemsSource = new List<string>
            {
                "Computerspiele sind mir wichtig",
                "Computerspiele haben eine große Auswirkung auf mein Sozialleben (positiv wie negativ)",
                "Computerspiele helfen mir bei Stress",
                "Ohne Computerspiele ist mein Leben eher langweilig",
                "Computerspiele sind an Schultagen mein Highlight",
                //"In der Schule muss ich häufig an Computerspiele denken",
                "Meine Eltern sehen meinen Umgang mit Computerspielen eher kritisch",
                "Erfolge in Computerspielen sind für mich genauso wichtig wie gute Noten",
                //"Mein Leben würde sich stark verändern, wenn ich keine Computerspiele mehr spielen dürfte",
                //"Ich verbringe viel Zeit mit meinen Freunden im wirklichen Leben"
            };

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

            Content = null;
            var viewBox = new Viewbox { Child = MainGrid, Width = 2480, Height = 3508 };
            ExportToPng("Umfrage.png", viewBox);
            Close();
        }

        public void ExportToPng(string path, FrameworkElement element)
        {
            if (path == null) return;

            // Save current canvas transform
            Transform transform = element.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            element.LayoutTransform = null;

            // Get the size of canvas
            Size size = new Size(element.Width, element.Height);
            // Measure and arrange the surface
            // VERY IMPORTANT
            element.Measure(size);
            element.Arrange(new Rect(size));
            
            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)(size.Width / 96d*300d), (int)(size.Height / 96d * 300d), 300, 300, PixelFormats.Pbgra32);
            renderBitmap.Render(element);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            element.LayoutTransform = transform;
        }
    }
}
