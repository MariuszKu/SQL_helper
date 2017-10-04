using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SqlHelper
{
    class SqlHelperWorker
    {

        public Table[] GetSelectTable { get; set; }


        public SqlHelperWorker()
        {
            GetSelectTable = new Table[2] { new Table(), new Table() };
        }


        public string ParseDDLCreate(string q1, int xid=1)
        {
            string q = "";
            bool key=false;

            GetSelectTable[xid].ColumnsList = new ObservableCollection<HlpTable>();
            q = Regex.Match(q1, @"\(([\s\S]*)\)[^(]*$", RegexOptions.Multiline).Groups[1].Value;
            GetSelectTable[xid].Name = Regex.Match(q1, @"table\s+([a-z,0-9]+).*\n?.*\(", RegexOptions.Multiline | RegexOptions.IgnoreCase).Groups[1].Value;
            var a = Regex.Split(q, @",(?![^\(]+\))");
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim();
                var w = Regex.Split(a[i], @"\s(?![^\(]+\))(?![^\s][^\(]+\))");
                var constraint = ArrayHelper.Contraint(w);
                key = constraint.ToUpper().Contains("PRIMARY KEY");
                GetSelectTable[xid].ColumnsList.Add(new HlpTable { Name = w[0].ToUpper(), DataType = w[1].ToUpper(), Constrain=ArrayHelper.Contraint(w), Key=key, TableAlias= GetSelectTable[xid].Name });
                key = false;
            }
            q = String.Join(",", a);
            
            

            return q;
        }

        public void ParseSelect(string q2)
        {
            if (!Regex.IsMatch(q2.ToUpper(), @"SELECT([\s\S]*)FROM.*"))
            {
                ParseDDLCreate(q2, 0);

            }
            else
            {
               
                GetSelectTable[0].ColumnsList = new ObservableCollection<HlpTable>();
               
                string REGEX_MATCH_TABLE_NAME = @"(?<=(?:FROM|JOIN)[\s(]+)(?>\w+)(?=[\s)]*(?:\s+(?:AS\s+)?\w+)?(?:$|\s+(?:WHERE|ON|(?:LEFT|RIGHT)?\s+(?:(?:OUTER|INNER)\s+)?JOIN)))";
                GetSelectTable[0].Name = Regex.Match(q2, REGEX_MATCH_TABLE_NAME, RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value;
                
                var a = ArrayHelper.GetColumns(q2);
                string[] arr = new string[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = a[i].Trim();
                    var calies = ArrayHelper.GetColumnAndAlias(a[i]);
                    if (calies[0].Split('.').Length == 1)
                        GetSelectTable[0].ColumnsList.Add(new HlpTable { Name = calies[0], Mapping = calies[1] });
                    else
                    {
                        string mapname = calies[1].Split('.').Length > 1 ? calies[1].Split('.')[1] : calies[1];
                        GetSelectTable[0].ColumnsList.Add(new HlpTable { Name = calies[0].Split('.')[1], Mapping = mapname, TableAlias = calies[0].Split('.')[0] });
                    }
                }

            }
        }

        /*public string GetSelect(bool? IgnoreChars)
        {
            bool ignoreChars = IgnoreChars ?? false;
            string query = "";
            string tableAlies = "";
            string[] a = new string[this.GetSelectTable[0].ColumnsList.Count];
            int i = 0;
            foreach (var item in this.GetSelectTable[0].ColumnsList) {
                var w = GetSelectTable[0].ColumnsList.Where(z => z.Name == item.Name).FirstOrDefault();
                tableAlies = item.TableAlias != null ? item.TableAlias + "." : "";
                if (w != null && !(ignoreChars && w.DataType.Contains("CHAR"))) {
                    
                    a[i] = String.Format("CAST({3}{0} AS {1}) {2}\n", item.Name, w.DataType, item.Mapping, tableAlies);
                }
                else {
                    
                    a[i] = String.Format("{3}{0} AS {1}\n", item.Name, item.Mapping, item.TableAlias, tableAlies);
                }

                i++;
            }
            query = String.Join(",", a);
            return String.Format("SELECT \n{0} FROM \n{1}", query, GetSelectTable[0].Name);

        }*/

        internal string getInsert(bool ignoreChar)
        {

            var column = String.Join(", ", GetSelectTable[1].ColumnsList.Where(z=> z.Mapping != null).Select(z=> z.Name  ));
            return String.Format("INSERT INTO {0} ({1})\n {2}", GetSelectTable[1].Name, column, getMapping(ignoreChar));
        }

        internal string getUpsert(bool ignoreChar)
        {
            var srcColumn = String.Join(", ", GetSelectTable[1].ColumnsList.Where(z => z.Mapping != null).Select(z => z.Name));
            var descColumn = String.Join(", ", GetSelectTable[0].ColumnsList.Select(z => z.Name));
            var column = String.Join(", ", GetSelectTable[1].ColumnsList.Select(z => z.Name));
            var result = from d in GetSelectTable[1].ColumnsList.Where(z => z.Key == true).ToList()
                         join s in GetSelectTable[0].ColumnsList on d.Name equals s.Name
                         select new { v1 = d.Name, v2 = s.Name };
            var r = result.ToList().Select(z => String.Format("{0}.{1} = sc.{2}", GetSelectTable[1].Name, z.v1, z.v2)).ToArray();
            string join = String.Join(" AND ", r);
            return String.Format(@"MERGE INTO {0} USING(
{2}
) sc ON ({3})
WHEN MATCHED THEN UPDATE SET
{4} 
WHEN NOT MATCHED THEN INSERT
({5}) VALUES {6}
", GetSelectTable[1].Name, column, getMapping(ignoreChar), join, getUpdateMapping(), srcColumn, getInsertMapping());
        }

        public string getInsertMapping()
        {
            var column = String.Join(", ", GetSelectTable[1].ColumnsList.Where(z=> z.Mapping!=null).Select(z => z.Name));
            var result = from d in GetSelectTable[1].ColumnsList
                         join s in GetSelectTable[0].ColumnsList on d.Mapping equals s.Mapping
                         select new { v1 = d.Mapping, v2 = s.Mapping };
            var r = result.ToList().Select(z => String.Format("sc.{0}", z.v2)).ToArray();
            string join = String.Join(",", r);
            return String.Format(@"({0})",join);

        }


        public string getUpdate(bool ignoreChar)
        {
            var column = String.Join(", ", GetSelectTable[1].ColumnsList.Where(z=> z.Mapping != null).Select(z => z.Name));
            var result = from d in GetSelectTable[1].ColumnsList.Where(z => z.Key==true).ToList()
                         join s in GetSelectTable[0].ColumnsList on d.Mapping equals s.Mapping
                         select new { v1 = d.Mapping, v2= s.Mapping };
            var r = result.ToList().Select(z => String.Format("{0}.{1} = sc.{2}", GetSelectTable[1].Name, z.v1, z.v2)).ToArray();
            string join = String.Join(" AND ", r);
            return String.Format(@"MERGE INTO {0} USING(
{2}
) sc ON ({3})
WHEN MATCHED THEN UPDATE 
SET {4} ", GetSelectTable[1].Name, column, getMapping(ignoreChar), join, getUpdateMapping());

        }

        internal void MapByName(ListView lvColumns)
        {
            foreach(var item in GetSelectTable[1].ColumnsList)
            {

                var c = GetSelectTable[0].ColumnsList.Where(z => z.Mapping == item.Name).FirstOrDefault();
                if (c != null)
                    item.Mapping = c.Mapping;

            }
            ICollectionView view = CollectionViewSource.GetDefaultView(GetSelectTable[1].ColumnsList);
            view.Refresh();

        }

       

        internal string getMapping(bool? IgnoreChars)
        {
            bool ignoreChars = IgnoreChars ?? false;
            var arr = new List<string>();
            var i = 0;
            foreach(var row in this.GetSelectTable[1].ColumnsList)
            {
                var item = this.GetSelectTable[0].ColumnsList.Where(z => z.Mapping == row.Mapping).FirstOrDefault();
                if (item != null)
                {
                    if (!(ignoreChars && row.DataType.Contains("CHAR")))
                        arr.Add(String.Format("CAST({0} AS {1}) {2}\n", item.Name, row.DataType, row.Name));
                    else
                        arr.Add(String.Format("{0} AS  {1}\n", item.Mapping, row.Name));
                    i++;
                }
            }

            var query = String.Join(",", arr.ToArray());
            return String.Format("SELECT \n{0} FROM \n{1}", query, GetSelectTable[0].Name);
        }

        internal string getUpdateMapping()
        {
            var arr = new List<string>();//new string[this.GetSelectTable[1].ColumnsList.Count];
            
            foreach (var row in this.GetSelectTable[1].ColumnsList)
            {
                if (!row.Key && row.Mapping != null)
                {
                    arr.Add(String.Format("{0} = sc.{1}\n", row.Name, row.Mapping));
                }
                
            }

            var query = String.Join(",", arr.ToArray());
            return String.Format("{0}", query);
        }
    }
}
