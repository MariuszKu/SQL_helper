using System;
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
using System.Windows.Shapes;

namespace MasterWF
{
    /// <summary>
    /// Interaction logic for PropertyWin.xaml
    /// </summary>
    public partial class PropertyWin : Window
    {
        public List<Table> GetSelectTable { get; set; }
        public Table SelectedTable { get; set; }

        public PropertyWin()
        {
            InitializeComponent();
            dataGrid.ItemsSource = GetSelectTable;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Guid g = Guid.NewGuid();
            foreach (var item in dataGrid.ItemsSource) {
                ((HlpTable)item).JonId = g;
                SelectedTable.ColumnsList.Add((HlpTable)item);
            }
            this.SelectedTable.JoinsList.Add(new Joins { JoinId = g, JoinValue = textBox.Text });
            this.Close();
        }
    }
}
