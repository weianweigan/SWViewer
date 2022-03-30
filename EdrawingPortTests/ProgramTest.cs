using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdrawingPortTests
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        [DataRow(".html")]
        [DataRow(".jpg")]
        [DataRow(".exe")]
        [DataRow(".stl")]
        public void Test(string format)
        {
            var dir = Path.Combine(
                Path.GetDirectoryName(typeof(ProgramTest).Assembly.Location));
            var testModelDir = Path.Combine(
                dir,
                "TestModel");
            var files = Directory.GetFiles(testModelDir);
            var models = Directory.GetFiles(testModelDir,"*.sld*");

            var spareFiles = files.ToList().Except(models).ToList();
            foreach (var spareFile in spareFiles)
            {
                File.Delete(spareFile);
            }

            var args = models.Select(x => $"{x} {format}")
                .Aggregate((a,b) => $"{a} {b}");

            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = dir,
                FileName = "EdrawingPort.exe",
                Arguments = args
            };

            process.Start();
            process.WaitForExit();

            var results = models
                .Select(p => Path.ChangeExtension(p, format))
                .ToList();

            foreach (var res in results)
            { 
                Assert.IsTrue(File.Exists(res));
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }
    }
}
