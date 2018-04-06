using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormLog : Form
    {
        
        Service.Core core ;
        Service.Setting setting ;

        int maxNumberLines; 
        delegate void PushLogDelegate(string content);

        public FormLog()
        {
            core = Service.Core.Instance;
            setting = Service.Setting.Instance;

            maxNumberLines = setting.maxLogLines;

            InitializeComponent();

            core.OnLog += LogReceiver;

            this.FormClosed += (s, e) =>
            {
                core.OnLog -= LogReceiver;
            };

            this.Show();
        }

        void LogReceiver(object sender, Model.DataEvent e)
        {
            PushLogDelegate pushLog = new PushLogDelegate(PushLog);
            textBoxLogger.Invoke(pushLog, e.Data);
        }

        public void PushLog(string content)
        {
            if (textBoxLogger.Lines.Length >= maxNumberLines - 1)
            {
                textBoxLogger.Lines = textBoxLogger.Lines.Skip(textBoxLogger.Lines.Length - maxNumberLines).ToArray();
            }
            textBoxLogger.AppendText(content + "\r\n");
        }
    }
}
