using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdrawingPort
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            List<SaveAsItem> items = new List<SaveAsItem>();
            if (args != null && args.Length >= 2)
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    var item = new SaveAsItem(args[i],args[i+1]);
                    items.Add(item);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(items));
        }
    }
}
