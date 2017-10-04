using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper
{
    public class HlpTable
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool Key { get; set; }

        public string Constrain { get; set; }
        public string Mapping { get; set; }
        public string TableAlias { get; set; }
    }


    public class Table
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public ObservableCollection<HlpTable> ColumnsList { get; set; }

    }
}
