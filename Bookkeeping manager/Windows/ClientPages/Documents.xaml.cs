using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Bookkeeping_manager.Scripts;
using Microsoft.Win32;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for Documents.xaml
    /// </summary>
    public partial class Documents : Page
    {
        private string ClientName { get; set; }
       private List<Document> Docs { get; set; }
        public Documents(List<Document> documents, string clientName)
        {
            Docs = documents;
            ClientName = clientName;
            InitializeComponent();
            CreateGrid();
        }
        private void CreateGrid()
        {
            DocumentsGrid.Children.RemoveRange(1, DocumentsGrid.Children.Count - 1);
            try
            {
                DocumentsGrid.RowDefinitions.RemoveRange(1, DocumentsGrid.RowDefinitions.Count - 1);
            }
            catch
            {

            }
            for (int i = 0; i < Docs.Count; i++)
            {
                Document document = Docs[i];
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(document.FilePath).ToImageSource();
                DocumentsGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
                DockPanel dock = new DockPanel();
                dock.MouseUp += (o, e) =>
                {
                    Process fileopener = new Process();
                    fileopener.StartInfo.FileName = "explorer";
                    fileopener.StartInfo.Arguments = "\"" + document.FilePath + "\"";
                    fileopener.Start();
                };
                dock.Children.Add(new Image()
                {
                    Source = icon
                });
                dock.Children.Add(new TextBlock()
                {
                    Text = System.IO.Path.ChangeExtension(document.FileName, "").Replace(".", "")
                });

                Button button = new Button()
                {
                    Content = "Remove",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 2, 5),
                    Style = FindResource("ActionButton") as  Style
                };
                button.Click += (o, e) =>
                {
                    Docs.Remove(document);
                    System.IO.File.Delete(document.FilePath);
                    CreateGrid();
                };
                DockPanel.SetDock(button, Dock.Right);

                dock.Children.Add(button);

                Grid.SetRow(dock, i + 1);

                DocumentsGrid.Children.Add(dock);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string fName;
            string fPath;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                fPath = dialog.FileName;
                fName = System.IO.Path.GetFileName(fPath);
            }
            else
            {
                return;
            }
            fPath = System.IO.Path.GetTempPath();
            fPath += fName;
            System.IO.File.Copy(dialog.FileName, fPath, true);
            Docs.Add(new Document()
            {
                FileName = fName,
                FileName_Cloud = $"{ClientName}|{fName}",
                FilePath = fPath
            });
            CreateGrid();
        }
    }
}
