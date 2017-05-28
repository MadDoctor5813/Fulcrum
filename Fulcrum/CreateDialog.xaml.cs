using Ookii.Dialogs;
using System;
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

namespace Fulcrum
{
    /// <summary>
    /// Interaction logic for CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : Window
    {
        public CreateDialog()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.SelectedPath = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                inputDir.Text = dlg.SelectedPath;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            VistaSaveFileDialog dlg = new VistaSaveFileDialog();
            dlg.DefaultExt = ".ful";
            dlg.AddExtension = true;
            dlg.Filter = "Fulcrum files (*.ful)|*.ful";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputFile.Text = dlg.FileName;
            }
        }
    }
}
