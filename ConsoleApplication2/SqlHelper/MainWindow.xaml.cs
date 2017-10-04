using SqlHelper.Various;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SqlHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        SqlHelperWorker sh;

        public MainWindow()
        {
            InitializeComponent();
            // this.AddChild();

            
            string result = System.IO.Path.GetTempPath();
            SaveTmp.LoadText(tbSqlDesc, "desc.tmp");
            SaveTmp.LoadText(tbSqlSource, "source.tmp");

            if (tbSqlDesc.Text == "")
            {
                tbSqlDesc.Text = @"create table a 
                        (id int primary key,id2 numeric(28,0),

                        id3 varchar(12) not null,
                        id4 numeric (28 , 0)

                        )";
                tbSqlSource.Text = "SELECT DISTINCT t.id, t.id2, t.id3, t.id4 from dual";
            } 
           
        }

        private void tbSqlSource1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F5)
            {
               
                

            }
        }

        private void LvColumns_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
               ( Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    // Find the data behind the ListViewItem
                    HlpTable row = (HlpTable)listView.ItemContainerGenerator.
                         ItemFromContainer(listViewItem);

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("myFormat", row);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private static T FindAnchestor<T>(DependencyObject current)
        where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                HlpTable contact = e.Data.GetData("myFormat") as HlpTable;
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                HlpTable row = (HlpTable)listView.ItemContainerGenerator.
                     ItemFromContainer(listViewItem);

                row.Mapping = contact.Mapping;
                listView.ItemsSource = sh.GetSelectTable[1].ColumnsList;
                ICollectionView view = CollectionViewSource.GetDefaultView(sh.GetSelectTable[1].ColumnsList);
                view.Refresh();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool ignoreChar = this.ChBoxChar.IsChecked ?? false;
            switch (e.Key)
            {
                case Key.F5:
                    sh = new SqlHelperWorker();
                    sh.ParseDDLCreate(tbSqlDesc.Text);
                    sh.ParseSelect(tbSqlSource.Text);

                    LvScColumns.ItemsSource = sh.GetSelectTable[0].ColumnsList;
                    LvColumns.ItemsSource = sh.GetSelectTable[1].ColumnsList;
                    sh.MapByName(this.LvColumns);
                    tbSql.Text = sh.getMapping(ignoreChar);
                    Various.SaveTmp.Save(tbSqlDesc.Text,"desc.tmp");
                    Various.SaveTmp.Save(tbSqlSource.Text, "source.tmp");

                    break;
                case Key.F7:
                    tbSql.Text = sh.getUpsert(ignoreChar);
                    break;
                case Key.F6:
                    tbSql.Text = sh.getMapping(ignoreChar);
                    break;
                case Key.F2:
                    tbSql.Text = sh.getInsert(ignoreChar);
                    break;
                case Key.F3:
                    tbSql.Text = sh.getUpdate(ignoreChar);
                    break;

            }
        }

       
    }
}
