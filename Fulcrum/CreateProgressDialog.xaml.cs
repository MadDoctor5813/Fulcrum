using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CreateProgressDialog.xaml
    /// </summary>
    public partial class CreateProgressDialog : Window
    {
        public CreateProgressDialog(BackgroundWorker worker, string outputFileName)
        {
            InitializeComponent();
            progLabel.Content = "Creating " + outputFileName;
            worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
            {
                Close();
            };
            worker.RunWorkerAsync();
            progBar.IsIndeterminate = true;
        }
    }
}
