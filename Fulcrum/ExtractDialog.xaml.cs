using System;
using Ookii.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Fulcrum
{
    /// <summary>
    /// Interaction logic for ExtractDialog.xaml
    /// </summary>
    public partial class ExtractDialog : Window
    {
        public ExtractDialog()
        {
            InitializeComponent();
        }

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.SelectedPath = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                extractLocation.Text = dlg.SelectedPath;
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog dlg = new VistaOpenFileDialog();
            dlg.Filter = "Fulcrum files (*.ful)|*.ful";
            dlg.DefaultExt = ".ful";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                arcFile.Text = dlg.FileName;
            }
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            string arcFileStr = arcFile.Text;
            worker.DoWork += delegate (object s, DoWorkEventArgs args)
            {
                FulcrumFile file = new FulcrumFile();
                file.LoadFromFile(arcFileStr);
            };
            string dlgText = string.Format("Extracting {0}", System.IO.Path.GetFileName(arcFileStr));
            ProgressDialog dlg = new ProgressDialog(worker, "Extract", dlgText);
        }
    }
}
