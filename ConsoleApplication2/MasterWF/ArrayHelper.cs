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

        public static string findAlias(string text)
        {
            int a = text.IndexOf('.');
            if (a <= 0)
                return text;

            string workingtxt = text.Substring(0, a);
            int bracket = 0;
            int quote = 0;

            for (int i = a-1; i > 0; i--)
            {
                switch (workingtxt[i])
                {
                    case '(':
                        return text.Substring(i+1, a - i-1);
                    case ' ':
                        return text.Substring(i + 1, a - i - 1);
                    case '\t':
                        return text.Substring(i + 1, a - i - 1);
                    case '"':
                        if(quote==1)
                            return text.Substring(i+1, a - i-1);
                        else
                            quote++;
                        break;
                        


                }

            }

            return text.Substring(0,a);

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

        public static string[] GetColumns(string query)
        {

            string[] arr = GetColumnAndAlias(query,',');// query.ToUpper().Split(',');
            var t1 = arr[0];
            arr[0] = t1.ToUpper().Replace("SELECT", "").Replace("DISTINCT", "").Trim();
            string tn = arr[arr.Length - 1];
            int a = Array.FindIndex(arr, 0, x => x.ToUpper().Contains("FROM"));
            String[] carr = new String[a + 1];
            Array.Copy(arr, carr, a + 1);
            carr[carr.Length - 1] = carr[carr.Length - 1].Substring(0, tn.ToUpper().LastIndexOf("FROM"));

            return carr;
        }

        public static string[] GetColumnAndAlias(string column, char delimiter=' ')
        {
            
            column = column.Trim();
            List<int> l = new List<int>();
            int bracket = 0;
            int quote = 0;
            int caseint = 0;
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
                    case 'C':
                        if(column.Length >= i+4)
                            if (column.Substring(i, 4) == "CASE")
                                caseint++;
                        break;
                    case 'E':
                        if (column.Length >= i + 3)
                            if (column.Substring(i-1, 4) == " END")
                                caseint--;
                        break;
                    default:
                        if (bracket == 0 && quote == 0 && caseint ==0 && column[i] == delimiter)
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
