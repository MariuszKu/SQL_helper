using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string q1 = @"create table a 
                        (id int,id2 numeric(28),

                        id3 varchar(12) not null,
                        id4 numeric(28)

                        )";
            string q2 = "SELECT id, id2, id3, id4 from dual";


            SqlHelper sh = new SqlHelper();
            sh.CreateCast(q1);
            Console.WriteLine(sh.Create(q2));
            Console.Read();
        }
    }
}
