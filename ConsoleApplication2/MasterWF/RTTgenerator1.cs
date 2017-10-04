using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterWF
{
    public partial class RTTgenerator
    {
        public Table table { get; set; }
        public Table descTable { get; set; }
        public string getSelectList(Guid id) {
            return String.Join(",", table.ColumnsList.Where(z => z.JonId == id).Select(z => z.Mapping).ToArray()) +
                " FROM " + table.ColumnsList.Where(z => z.JonId == id).First().TableAlias + " WHERE " + table.JoinsList.Where(z => z.JoinId == id).Single().JoinValue;


        }
    }

}