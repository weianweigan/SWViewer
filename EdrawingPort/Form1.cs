using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuEDrawingControl;

namespace EdrawingPort
{
    public partial class Form1 : Form
    {
        private readonly List<SaveAsItem> _saveAsItems;
        private readonly Timer _timer = new Timer();
        private EDrawingComponent _host;

        public delegate void OnFinishedSavingDocumentEventHandler();

        public Form1(List<SaveAsItem> saveAsItems)
        {
            InitializeComponent();            

            if (saveAsItems == null)
                Application.Exit();

            this.Load += Form1_Load;
            _saveAsItems = saveAsItems;

            _timer.Tick += _timer_Tick;

            _timer.Interval = 6000 * _saveAsItems.Count+1000;
            _timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _host = new EDrawingComponent();
            _host.OnControlLoaded += Edrawing_ControlLoaded;
            this.Controls.Add(_host);
        }

        private void Edrawing_ControlLoaded(
            dynamic obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Cannot get edrawing instance");
            }

            if (_saveAsItems == null || _saveAsItems.Count < 1)
            {
                Application.Exit();
                return;
            }

            Console.WriteLine("OutPut Begins");
           
            //var hander = obj.OnFinishedSavingDocumentEventHandler as OnFinishedSavingDocumentEventHandler;
            foreach (var saveAsItem in _saveAsItems)
            {
                _host.OpenDoc(saveAsItem.FilePathName, true, false, true);
                _host.Save(saveAsItem.OutPutPathName, false, "");
                Console.WriteLine($"Save As:{saveAsItem.OutPutPathName}");
            }
            Application.Exit();
        }

        private int _saveCount;
        private void Obj_OnFinishedSavingDocument()
        {
            _saveCount++;
            if (_saveCount == _saveAsItems.Count)
            {
                _timer.Stop();
                Application.Exit();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("超时");
            Application.Exit();
        }

    }
}
