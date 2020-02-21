using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MasterWF
{
    public enum SqlCommand
    {
        Select,
        Insert,
        Update,
        MergeInsert
    }


    public class MakeETL
    {
        private Table table;
        private Table descTable;

        public MakeETL(Table table, Table descTable) {
            this.table = table;
            this.descTable = descTable;
        }

        public void MappByName()
        {
            HlpTable hlp = null;
            foreach(var item in table.ColumnsList)
            {
                item.Mapping = item.Mapping == null || item.Mapping == "" ? item.Name : item.Mapping;
                hlp = descTable.ColumnsList.Where(z => z.Name.ToUpper() == item.Mapping.ToUpper()).FirstOrDefault();
                if (hlp != null)
                    item.MapDestColumn = hlp.Name;

            }

        }

        public void MappByOrader()
        {
            HlpTable hlp = null;
            int i = 0;
            
            foreach (var item in table.ColumnsList)
            {
                if (i < descTable.ColumnsList.Count)
                {
                    item.Mapping = item.Mapping == null || item.Mapping == "" ? item.Name : item.Mapping;
                    hlp = descTable.ColumnsList[i];
                    if (hlp != null)
                        item.MapDestColumn = hlp.Name;
                    i++;
                }
            }

        }


        public string CreateStm()
        {
            RTTgenerator gen = new RTTgenerator();
            gen.table = this.table;
            gen.descTable = this.descTable;

            return gen.TransformText();

        }

        public string CreateCommand(SqlCommand sc)
        {
            var column = String.Join(", ", descTable.ColumnsList.Where(z => z.Mapping != null).Select(z => z.Name));
            var result = from d in descTable.ColumnsList
                         join s in table.ColumnsList on d.Name equals s.MapDestColumn
                         select new { v1 = d.Name, v2 = s.Mapping, v3 = s.Name, key=d.Key };
            var r1 = result.ToList().Select(z => String.Format("{0}", z.v1)).ToArray();
            var r2 = result.ToList().Select(z => z.v2 != z.v3 && sc != SqlCommand.MergeInsert ? String.Format("{0} AS {1}", z.v3, z.v2) : String.Format("{0}", z.v2)).ToArray();
            var r3 = result.Where(z=> !z.key).ToList().Select(z => String.Format("{0} = sc.{1}", z.v1, z.v2)).ToArray();
            string descCols = String.Join(",", r1);
            string selectCols = String.Join("\n\t,", r2);
            string updateCols = String.Join("\n,", r3);

            switch (sc)
            {
                case SqlCommand.MergeInsert:
                    return String.Format("INSERT ({0}) \n VALUES ({1}) ", descCols, selectCols);
                case SqlCommand.Insert:    
                    if (!table.Name.Contains("query"))
                       return String.Format("TRUNCATE TABLE {0};\nINSERT INTO {0} ({1}) \nSELECT\n\t {2} \nFROM\n {3}", descTable.Name, descCols, selectCols, table.Name);
                    else
                        return String.Format("TRUNCATE TABLE {0};\nINSERT INTO {0} ({1}) \nSELECT\n\t {2} \nFROM\n ({3}) A", descTable.Name, descCols, selectCols, table.SqlDefinition);
                case SqlCommand.Select:
                    if (!table.Name.Contains("query"))
                        return String.Format("SELECT\n {0} \nFROM {1}", selectCols, table.Name);
                    else
                        return String.Format("SELECT {0} FROM ({1}) A", selectCols, table.SqlDefinition);
                case SqlCommand.Update:
                    return updateCols;
                default:
                    return "";

            }
        }

        internal string SelectWithAliases()
        {
            string command = "SELECT \n";

            foreach(var item in table.ColumnsList)
            {
                command += item.Name + ",\n";

            }

            command += "FROM " + table.SqlDefinition;

            return command;
        }

        internal string createMerge()
        {
            var srcColumn = String.Join(", ", table.ColumnsList.Where(z => z.Mapping != null).Select(z => z.Name));
            var descColumn = String.Join(", ", descTable.ColumnsList.Select(z => z.Name));
            var column = String.Join(", ", descTable.ColumnsList.Select(z => z.Name));
            var result = from d in descTable.ColumnsList.Where(z => z.Key == true).ToList()
                         join s in table.ColumnsList on d.Name equals s.MapDestColumn
                         select new { v1 = d.Name, v2 = s.Mapping };
            var r = result.ToList().Select(z => String.Format("{0}.{1} = sc.{2}", descTable.Name, z.v1, z.v2)).ToArray();
            string join = String.Join(" AND ", r);
            return String.Format(@"MERGE INTO {0} USING(
{1}
) sc ON ({2})
WHEN MATCHED THEN UPDATE SET
{3} 
WHEN NOT MATCHED THEN 
{4}
", descTable.Name, CreateCommand(SqlCommand.Select), join, CreateCommand(SqlCommand.Update), CreateCommand(SqlCommand.MergeInsert));

        }
    }
}
