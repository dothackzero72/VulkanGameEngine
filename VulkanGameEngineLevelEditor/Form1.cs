using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{
    public unsafe partial class Form1 : Form
    {
        public delegate void TextCallback(string message);
        public delegate void RichTextCallback(string message);

        public VulkanRenderer vulkanRenderer = new VulkanRenderer();

        public void GetCText(string text)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action(() => { textBox1.AppendText(text + Environment.NewLine); }));
            }
            else
            {
                textBox1.AppendText(text + Environment.NewLine);
            }
        }

        public Form1()
        {
            try
            {
                InitializeComponent();
                vulkanRenderer.SetUpRenderer(this.Handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Console.SetOut(new RichTextCallback(richTextBox1));
        }
    }
}
