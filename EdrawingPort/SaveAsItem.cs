using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdrawingPort
{
    public class SaveAsItem
    {
        public SaveAsItem(string filePathName, string outputFormat)
        {
            FilePathName = filePathName;
            OutputFormat = outputFormat;

            OutPutPathName = Path.ChangeExtension(filePathName,outputFormat);
        }

        public string FilePathName { get; set; }

        public string OutputFormat { get; set; }

        public string OutPutPathName { get;private set; }

        public bool Finished { get; set; }
    }
}
