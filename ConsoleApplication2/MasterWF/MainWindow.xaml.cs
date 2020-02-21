using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MasterWF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlHelperWorker sh;
        public Table SelectedTable { get; set; }
        public Table DestinationTable { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataGrid.AutoGenerateColumns = false;
            DataGridSource.AutoGenerateColumns = false;
            this.tbSqlSource.Text = @"Create table test (id int,
                city varchar(128),
                name varchar(123),
                Surname varchar(123),
                );
                Create table city (id int,
                name varchar(123)
                
                );
create table dupa(
a int,
b int,
c numeric(28,2)
)
;

select a,b,c from dupa;
Create table Desttest (id int,
                cityId varchar(128),
                name varchar(123),
                Surname varchar(123),
                );
                

";


            textBlock.Text = @"f5 - parse queries\n
f3 - set as dest
f2 - set as source
f4 - add alias
f6 - 
f7 - insert
f8 - merge

crt-1 - join based on name
crt-2 - join based on position

                ";

            this.tbSqlSource.Focus();
            this.tbSqlSource.Select(0, this.tbSqlSource.Text.Length);


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MakeETL me = new MakeETL(SelectedTable, DestinationTable);
            switch (e.Key)
            {
                case Key.F4:
                    tbSqlSource.Text = me.SelectWithAliases();
                    break;
                case Key.F5:
                    sh = new SqlHelperWorker();
                    sh.ParseBatch(this.tbSqlSource.Text);
                    trvMenu.ItemsSource = sh.GetSelectTable;
                    break;
                case Key.F6:
                    
                    tbSqlSource.Text = me.CreateStm();
                    break;
                case Key.F7:

                    tbSqlSource.Text = me.CreateCommand(SqlCommand.Insert);
                    break;
                case Key.F8:

                    tbSqlSource.Text = me.createMerge();
                    break;
            }
            if(Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.D1) {
                this.DataGridSource.CommitEdit();
                
                me.MappByName();
                DataGrid.Items.Refresh();
                this.DataGridSource.Items.Refresh();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.D2)
            {
                this.DataGridSource.CommitEdit();
                
                me.MappByOrader();
                DataGrid.Items.Refresh();
                this.DataGridSource.Items.Refresh();
            }
            
        }

       

        private void trvMenu_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    if (this.trvMenu.SelectedItem is Table)
                    {
                        SelectedTable = this.trvMenu.SelectedItem as Table;
                        this.addToLV(SelectedTable);
                    }
                    else
                        AddLookup();
                    break;
                case Key.F3:
                    this.DestinationTable = (this.trvMenu.SelectedItem as Table);
                    DataGrid.ItemsSource = this.DestinationTable.ColumnsList;
                    break;
            }
        }

        private void addToLV(Table t)
        {
            //this.LVcolumns.ItemsSource = t.ColumnsList;
           this.DataGridSource.ItemsSource = t.ColumnsList;
           
        }

        private void AddLookup()
        {
            var a = DataGridSource.SelectedItem as HlpTable;
            var b = trvMenu.SelectedItem as HlpTable;
            var x = trvMenu.ItemsSource as List<Table>;

            var columns = x.Where(z => z.Name == b.TableAlias).First();
            ObservableCollection<HlpTable> newList = new ObservableCollection<HlpTable>();

            foreach(var item in columns.ColumnsList)
            {
                newList.Add(item);
            };

            PropertyWin win = new PropertyWin();
            win.dataGrid.ItemsSource = newList;
            win.SelectedTable = this.SelectedTable;
            win.textBox.Text = a.Name + " = " + b.TableAlias +"."+ b.Name;
            win.Show();

        }
    }

    



}
