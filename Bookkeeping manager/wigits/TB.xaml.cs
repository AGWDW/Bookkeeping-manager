using System.Windows.Controls;
using System.Windows.Data;

namespace Bookkeeping_manager.wigits
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TB : UserControl
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public Binding Bind { get; set; }
        public ValidationRule[] Rules { get; set; } = new ValidationRule[0];
        public TB()
        {
            InitializeComponent();
            Grid.SetRow(this, Row);
            Grid.SetColumn(this, Column);
            DataContext = this;
            if (Bind is null)
            {
                return;
            }
            foreach (ValidationRule rule in Rules)
            {
                Bind.ValidationRules.Add(rule);
            }
            Base.SetBinding(TextBox.TextProperty, Bind);
        }
    }
}
