using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsInput;

namespace PS3ApplicationControllerProject
{
    /*
     * User Control that can hold a key inputted by the user
     */

    public partial class keyInputUC : UserControl
    {
        public VirtualKeyCode vKeyCode = new VirtualKeyCode();

        public int index = 0;
        public int keyInt = 0;
        public bool isMouse = false;

        public keyInputUC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyValue.Text = "";
            keyInt = 0;
            vKeyCode = new VirtualKeyCode();
        }

        private void keyValue_KeyDown(object sender, KeyEventArgs e)
        {
            keyValue.Text = e.KeyCode.ToString();

            keyInt = e.KeyValue;
            vKeyCode = ParseKeyCode(e.KeyValue);
            //sKeyCode = ParseScanKeyCode(e.KeyValue);

            e.SuppressKeyPress = true;
        }

        public static VirtualKeyCode ParseKeyCode(int KeyValue)
        {
            VirtualKeyCode ret = new VirtualKeyCode();

            ret = (VirtualKeyCode)KeyValue;

            return ret;
        }

    }
}
