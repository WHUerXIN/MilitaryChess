using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarChess
{
    public partial class FormSet : Form
    {
        public FormSet()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                Form1.Music = true;
            }
            else
            {
                Form1.Music = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Form1.Music = false;
            }
            else
            {
                Form1.Music = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                checkBox2.Checked = false;
                Form1.Message = true;
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                Form1.Message = true;
            }
        }

        private void FormSet_Load(object sender, EventArgs e)
        {
            if (Form1.Music)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            if (Form1.Message)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = false;
            }
            else
            {
                checkBox2.Checked = true;
                checkBox1.Checked = false;
            }
            if(Form1.Wechat)
            {
                comboBox1.Text = "开启";
            }
            else
            {
                comboBox1.Text = "关闭";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "开启")
                Form1.Wechat=true;
            else if (comboBox1.Text == "关闭")
                Form1.Wechat = false; 
            else Form1.Wechat = true; ;
        }
    }
}
