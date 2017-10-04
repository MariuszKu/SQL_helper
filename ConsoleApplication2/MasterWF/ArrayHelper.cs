using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterWF
{
    class ArrayHelper
    {
        public static string Contraint(string[] arr){
            if (arr.Length > 2)
            {
                string text = String.Join(" ", arr.Skip(2).ToArray());
                return text;
            }
            return "";
        }

        public static string[] GetColumns(string query){

            string[] arr = query.ToUpper().Split(',');
            var t1 = arr[0];
            arr[0] = t1.ToUpper().Replace("SELECT","").Replace("DISTINCT", "").Trim();
            string tn = arr[arr.Length-1];
            arr[arr.Length-1] = tn.Substring(0, tn.ToUpper().LastIndexOf("FROM"));

            return arr;
        }

        public static List<string> SplitAtPositions(string input, List<int> delimiterPositions)
        {
            var output = new List<string>();

            for (int i = 0; i < delimiterPositions.Count; i++)
            {
                int index = i == 0 ? 0 : delimiterPositions[i - 1] + 1;
                int length = delimiterPositions[i] - index;
                string s = input.Substring(index, length);
                if(s.Trim()!="AS")
                    output.Add(s.Replace("\"", ""));
            }

            if (delimiterPositions.Count > 0)
            {
                string lastString = input.Substring(delimiterPositions.Last() + 1);
                output.Add(lastString.Replace("\"", ""));
            }else
            {
                output.Add(input.Replace("\"",""));
                output.Add(input.Replace("\"", ""));
            }

            return output;
        }

        public static string[] GetColumnAndAlias(string column)
        {
            
            column = column.Trim();
            List<int> l = new List<int>();
            int bracket = 0;
            int quote = 0;
            for (int i = 0; i < column.Length; i++)
            {
                switch (column[i])
                {
                    case '(':
                        bracket++;
                        break;
                    case ')':
                        bracket--;
                        break;
                    case '"':
                        if(quote==0)
                            quote++;
                        else
                            quote--;
                        break;
                    default:
                        if (bracket == 0 && quote == 0 && column[i] == ' ' )
                        {
                            l.Add(i);
                        }
                        break;

                }


            }            

            return SplitAtPositions(column,l).ToArray();
        }


    }



}
