using System;
using System.Windows.Forms;

namespace Shashki
{
    public partial class HelloForm : Form
    {
        public HelloForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TimerClosing(object sender, EventArgs e)
        {
            Close();
        }
    }
}
