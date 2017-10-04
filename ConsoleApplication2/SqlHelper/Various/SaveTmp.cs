using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Various
{
    public class SaveTmp
    {
        public static void Save(string text,string name)
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString()+@"\SqlHelper\";
            }

            if (Directory.Exists(path))
            {
                File.WriteAllText(path + name, text);
            }
            else
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(path + name, text);
            }

        }


        public static void LoadText(System.Windows.Controls.TextBox tx,string name)
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString() + @"\SqlHelper\";
            }

            if (Directory.Exists(path))
            {
                tx.Text =  File.ReadAllText(path + name);
            }


        }

    }
}
