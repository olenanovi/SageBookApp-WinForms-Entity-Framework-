using System;
using System.Windows.Forms;

namespace SageBook
{
    public partial class AddSage : Form
    {
        public AddSage()
        {
            InitializeComponent();
        }

        private void buttonOpenFileDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a photo";
            openFile.Filter = "JPG(*.jpg)|*.jpg|PNG(*.png)|*.png|JPEG(*.jpeg)|*.jpeg";
            openFile.Multiselect = false;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                textBoxImage.Text = openFile.FileName;
            }

        }
    }
}
