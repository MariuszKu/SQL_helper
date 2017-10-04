using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterWF
{
    public class HlpTable
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool Key { get; set; }

        public string Constrain { get; set; }
        public string Mapping { get; set; }
        
        public string MapDestColumn { get; set; }
        public string TableAlias { get; set; }
        public string JoinCondition { get; set; }
        public Guid JonId { get; set; }
    }

    public class Joins
    {
        
        public string JoinValue { get; set; }
        public Guid JoinId { get; set; }
    }

    public class Table
    {
        public Table()
        {
            this.ColumnsList = new ObservableCollection<HlpTable>();
            JoinsList = new List<Joins>();
        }
        public string SqlDefinition { get; set; }
        public List<Joins> JoinsList { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public ObservableCollection<HlpTable> ColumnsList { get; set; }

    }
}
