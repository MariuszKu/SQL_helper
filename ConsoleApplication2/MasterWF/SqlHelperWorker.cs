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

namespace MasterWF
{
    class SqlHelperWorker
    {

        public List<Table> GetSelectTable { get; set; }
        private List<string> tablesWithDef { get; set; }

        public SqlHelperWorker()
        {
            GetSelectTable = new List<Table>();
        }

        public void ParseBatch(string query)
        {
            var qs = query.Split(';');
            int i = 0;
            foreach(var item in qs)
            {
                if (Regex.IsMatch(item.ToUpper(), @"SELECT([\s\S]*)FROM.*"))
                {
                    ParseSelect(item);
                }
                else
                {
                    ParseDDLCreate(item, i);
                    i++;
                }
            }

        }


        public string ParseDDLCreate(string q1, int xid=1)
        {
            string q = "";
            bool key=false;
            Table t = new Table();
            t.ColumnsList = new ObservableCollection<HlpTable>();
            q = Regex.Match(q1, @"\(([\s\S]*)\)[^(]*$", RegexOptions.Multiline).Groups[1].Value;
            t.Name = Regex.Match(q1, @"table\s+([a-z,0-9,\.,\[,\]]+).*\n?.*\(", RegexOptions.Multiline | RegexOptions.IgnoreCase).Groups[1].Value;
            var a = Regex.Split(q, @",(?![^\(]+\))");
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim();
                if (a[i].Length>0)
                {
                    //var w = Regex.Split(a[i], @"\s(?![^\(]+\))(?![^\s][^\(]+\))",RegexOptions.IgnorePatternWhitespace);
                    var w = a[i].Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var constraint = ArrayHelper.Contraint(w);
                    if (w.Length > 2)
                        if (w[2].StartsWith("("))
                        {
                            w[1] += w[2];
                            w[2] = w[3];
                        }
                    w[0] = w[0].Replace("[", "").Replace("]", "");
                    key = constraint.ToUpper().Contains("PRIMARY KEY");
                    if(!new String[] { "CONSTRAINT" }.Contains(w[0]))
                        t.ColumnsList.Add(new HlpTable { Name = w[0].ToUpper(), DataType = w[1].ToUpper(), Constrain = ArrayHelper.Contraint(w), Key = key, TableAlias = t.Name });
                    key = false;
                }
            }
            q = String.Join(",", a);
            this.GetSelectTable.Add(t);
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
                Table t = new Table();
                t.ColumnsList = new ObservableCollection<HlpTable>();
                tablesWithDef = new List<string>();
                string REGEX_MATCH_TABLE_NAME = @"(?<=(?:FROM|JOIN)[\s(]+)(?>\w+)(?=[\s)]*(?:\s+(?:AS\s+)?\w+)?(?:$|\s+(?:WHERE|ON|(?:LEFT|RIGHT)?\s+(?:(?:OUTER|INNER)\s+)?JOIN)))";
                t.Name = "query-" +Regex.Match(q2, REGEX_MATCH_TABLE_NAME, RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value;

                foreach(var item in Regex.Match(q2, REGEX_MATCH_TABLE_NAME, RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups)
                {
                    if(GetSelectTable.Count>0)
                        if(GetSelectTable.Where(z => z.Name == item.ToString()).FirstOrDefault()!= null)
                            tablesWithDef.Add(GetSelectTable.Where(z => z.Name == item.ToString()).First().Name);
                }

                t.SqlDefinition = q2;

                var a = ArrayHelper.GetColumns(q2);
                string[] arr = new string[a.Length];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = a[i].Trim();
                    var calies = ArrayHelper.GetColumnAndAlias(a[i]);
                    if (calies[0].Split('.').Length == 1)
                        t.ColumnsList.Add(new HlpTable { Name = calies[0], Mapping = calies[1], TableAlias = t.Name });
                    else
                    {
                        string mapname = calies[1].Split('.').Length > 1 ? calies[1].Split('.')[1] : calies[1];
                        t.ColumnsList.Add(new HlpTable { Name = calies[0], Mapping = mapname, TableAlias = ArrayHelper.findAlias(calies[0]) });
                    }
                }
                this.GetSelectTable.Add(t);
            }
        }

        private string ExtractDef(string q2)
        {

            int i = 0;
            int s = q2.ToUpper().IndexOf("FROM");
            string txt = q2.Substring(s, q2.Length-s);
            if (txt.Contains("openquery"))
            {
                i = txt.IndexOf("(");
                int bracket = 0;
                for (; i < txt.Length; i++)
                {
                    switch (txt[i])
                    {
                        case '(':
                            bracket++;
                            break;
                        case ')':
                            bracket--;
                            break;


                    }
                    if (bracket == 0)
                        break;
                }
            }
            return txt.Substring(4, i+1);
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
