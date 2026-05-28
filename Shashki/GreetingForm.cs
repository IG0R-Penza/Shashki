using System;
using System.Windows.Forms;

namespace Shashki
{
    /// <summary>
    /// Приветственное окно
    /// </summary>
    public partial class GreetingForm : Form
    {
        public GreetingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Закрытие окна по кнопке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Закрытие окна по таймеру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerClosing(object sender, EventArgs e)
        {
            Close();
        }
    }
}
