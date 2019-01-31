using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Text;

namespace MySql.Connection
{
    [Flags]
    public enum LogOptions { None, LogToFile, PromptTheUser }

    public sealed partial class ErrorTrapper : Form
    {

        private static TextWriter writer;

        public static void Log(Exception ex, LogOptions logOption)
        {
            if (ex != null)
            {
                switch(logOption) 
                {
                    case LogOptions.PromptTheUser:
                        Log(ex.ToString());
                        if (logOption == LogOptions.PromptTheUser)
                        {
                            ErrorTrapper trapper = new ErrorTrapper(ex);
                            trapper.ShowDialog();
                            trapper.Dispose();
                        }
                        break;
                    case LogOptions.LogToFile:
                        Log(ex.ToString());
                        break;
                    case LogOptions.None:
                        break;
                }
            }
        }

        public static void Log(string message)
        {
            string datetime = DateTime.Now.ToString("HHmmss", CultureInfo.CurrentCulture);
            if (writer == null)
            {
                string date = DateTime.Now.ToString("ddMMMMyyyy", CultureInfo.CurrentCulture);
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + Assembly.GetEntryAssembly().GetName().Name;
                Directory.CreateDirectory(folder);
                writer = new StreamWriter(folder + "/" + date + ".err", true, Encoding.UTF8);
            }
            writer.WriteLine("#" + datetime + "# " + message);
            writer.Flush();
        }

        public ErrorTrapper(Exception ex)
        {
            InitializeComponent();
            if (ex != null)
            {
                this.source = ex.Source;
                this.message = ex.Message;
            }
            else
            {
                this.source = "";
                this.message = "";
            }
        }

        private string source;
        private string message;

        private void Error_Load(object sender, EventArgs e)
        {
            Size = new Size(300, 200);
            lblSourceValue.Text = source;
            lblDetailsValue.Text = message;
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt", CultureInfo.CurrentCulture);
        }
        
    }
}
