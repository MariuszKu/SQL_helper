using ConsoleApplication2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlHelper
{
    class SqlHelper
    {
        public List<HlpTable> Columns { get; set; }

        public string CreateCast(string q1)
        {
            string q = "";
            Columns = new List<HlpTable>();
            q = Regex.Match(q1, @"\(([\s\S]*)\)[^(]*$", RegexOptions.Multiline).Groups[1].Value;
            var a = q.Split(',');
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim();
                var w = a[i].Split(new char[] { ' ', '\t' });
                Columns.Add(new HlpTable { Name = w[0].ToUpper(), DataType = w[1].ToUpper() });
            }
            q = String.Join(",", a);
            //q = q.Trim(new char[] { '\t', '\n',' ' });
            

            return q;
        }

        public string Create(string q2)
        {
            string query = "";
            query = Regex.Match(q2.ToUpper(), @"SELECT([\s\S]*)FROM", RegexOptions.Multiline).Groups[1].Value;

            string REGEX_MATCH_TABLE_NAME = @"(?<=(?:FROM|JOIN)[\s(]+)(?>\w+)(?=[\s)]*(?:\s+(?:AS\s+)?\w+)?(?:$|\s+(?:WHERE|ON|(?:LEFT|RIGHT)?\s+(?:(?:OUTER|INNER)\s+)?JOIN)))";
            string table = Regex.Match(q2, REGEX_MATCH_TABLE_NAME, RegexOptions.IgnoreCase | RegexOptions.Multiline).Groups[0].Value;

            var a = query.Split(',');
            string[] arr = new string[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i].Trim();
                var w = Columns.Where(z => z.Name == a[i]).First();
                a[i] = String.Format("CAST({0} AS {1}) {2}\n", a[i], w.DataType, w.Name);
            }

            query = String.Join(",", a);
            return String.Format("SELECT \n{0} FROM \n{1}",query, table);
        }



    }
}
