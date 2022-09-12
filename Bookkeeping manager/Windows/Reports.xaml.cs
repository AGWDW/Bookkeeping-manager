using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bookkeeping_manager.Windows
{
    using AMLS = Tuple<List<DateTime>, string>;
    using Client = src.Clients.Client;

    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        private readonly string[] Reports_ = new string[]
        {
            "Anti Money Laundring",
            //"Services provided"
        };
        private readonly bool[] ReportsToCreate, CreateForClients;
        // used if all is clicked
        public bool Selected_L { get; set; }
        public bool Selected_R { get; set; }
        public Reports()
        {
            ReportsToCreate = new bool[Reports_.Length];
            for (int i = 0; i < ReportsToCreate.Length; i++)
            {
                ReportsToCreate[i] = false;
            }
            CreateForClients = new bool[ClientManager.AllClients.Count];
            for (int i = 0; i < CreateForClients.Length; i++)
            {
                CreateForClients[i] = false;
            }
            InitializeComponent();
            DataContext = this;
            PopulateGrid();

        }
        private void PopulateGrid()
        {
            DockLeft.Children.Clear();
            DockRight.Children.Clear();
            StackPanel stackL = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            for (int i = 0; i < ReportsToCreate.Length; i++)
            {
                string report = Reports_[i];
                stackL.Children.Add(CreateRow(report, ReportsToCreate, i));
            }
            DockLeft.Children.Add(stackL);

            StackPanel stackR = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };
            for (int i = 0; i < CreateForClients.Length; i++)
            {
                string client = ClientManager.AllClients[i].Name;
                stackR.Children.Add(CreateRow(client, CreateForClients, i));
            }
            Grid.SetColumn(stackR, 1);
            DockRight.Children.Add(stackR);
        }
        private StackPanel CreateRow(string text, bool[] collection, int index)
        {
            StackPanel res = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5, 0, 5, 0),
            };
            CheckBox cb = new CheckBox()
            {
                IsChecked = collection[index],
                // VerticalAlignment = VerticalAlignment.Center,
                // Margin = new Thickness(0, 0, 5, 0),
                // LayoutTransform = new ScaleTransform(1.5, 1.5)
            };
            cb.Click += (o, e) =>
            {
                collection[index] = cb.IsChecked.GetValueOrDefault();
            };
            res.Children.Add(cb);
            TextBlock tb = new TextBlock()
            {
                Text = text
            };
            res.MouseUp += (o, e) =>
            {
                cb.IsChecked = !cb.IsChecked;
                collection[index] = cb.IsChecked.GetValueOrDefault();
            };
            res.Children.Add(tb);

            return res;
        }

        private void GenerateReportsButton_Click(object sender, RoutedEventArgs e)
        {
            List<Client> clients = new List<Client>();
            for (int i = 0; i < CreateForClients.Length; i++)
            {
                if (!CreateForClients[i])
                    continue;
                clients.Add(ClientManager.AllClients[i]);
            }
            if (clients.Count == 0)
                return;
            for (int i = 0; i < Reports_.Length; i++)
            {
                if (!ReportsToCreate[i])
                    continue;
                CreateReport(i, clients);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ReportsToCreate.Length; i++)
            {
                ReportsToCreate[i] = Selected_L;
            }
            PopulateGrid();
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < CreateForClients.Length; i++)
            {
                CreateForClients[i] = Selected_R;
            }
            PopulateGrid();
        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AllLeft.IsChecked = !Selected_L;
            CheckBox_Click(null, null);
        }

        private void StackPanel_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            AllRight.IsChecked = !Selected_R;
            CheckBox_Click_1(null, null);
        }

        private void CreateReport(int reportIndex, List<Client> clients)
        {
            string text = "";
            switch (reportIndex)
            {
                case 0:
                    List<AMLS> amls = new List<AMLS>();
                    foreach (Client client in clients)
                    {
                        amls.Add(new AMLS(new List<DateTime>(), client.Name));
                        foreach (ContactInfomation_Data contact in client.Contacts)
                        {
                            if (contact.AML_ReviewDue != "")
                            {
                                DateTime aml = contact.AML_ReviewDue.ToDate();
                                amls.Last().Item1.Add(aml);
                            }
                            else
                            {
                                amls.Last().Item1.Add(new DateTime(year: 1, month: 1, day: 1));
                            }
                        }
                        amls.Last().Item1.Sort((a, b) => a.CompareTo(b));
                    }
                    amls.Sort((a, b) => a.Item1[0].CompareTo(b.Item1[0]));
                    text = "AML\nClient Name,AML reveiw date\n";
                    foreach (AMLS amls_ in amls)
                    {
                        string msg = $"{amls_.Item2}";
                        foreach (DateTime aml in amls_.Item1)
                        {
                            msg += $",{(aml.Year < 1900 ? "NAN" : aml.GetString())}";
                        }
                        text += msg + "\n";
                    }
                    break;
                case 1:
                    /*text = "Services\nClient Name";
                    foreach (KeyValuePair<string, object> pair in ClientManager.AllClients[0].ServiceInfomation.ToDictionary())
                    {
                        if (pair.Key.Contains("Enabled"))
                            continue;
                        text += $",{pair.Key}";
                    }
                    text += "\n";
                    foreach (Client client in clients)
                    {
                        string msg = $"{client.Name}";
                        Dictionary<string, object> services = client.Services.ToDictionary();
                        foreach (KeyValuePair<string, object> pair in services)
                        {
                            if (pair.Value as string is null)
                                continue;
                            msg += $",{(pair.Value as string == "" ? "NAN" : pair.Value)}";
                        }
                        text += msg + "\n";
                    }*/
                    break;
            }

            try
            {
                using (var w = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Reports_[reportIndex]} Report.csv"))
                {
                    w.Write(text);
                    w.Flush();
                }
                MessageBox.Show("Reports generated");
            }
            catch
            {
                MessageBox.Show("Please close the previous report");
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
