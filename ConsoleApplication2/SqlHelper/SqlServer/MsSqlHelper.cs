using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.SqlServer
{
    public class MsSqlHelper : ISqlHelper
    {
        public string getUpdate()
        {
            //var column = String.Join(", ", Columns.Select(z => z.Name));
            //return String.Format("UPDATE {0} SET\n {2}", GetDescTable, column, getMapping());

            return "";
        }
    }
}
