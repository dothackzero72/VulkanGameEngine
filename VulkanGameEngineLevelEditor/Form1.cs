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
        Texture texture;
        Mesh2D mesh;
        FrameBufferRenderPass renderPass;
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
                VulkanRenderer.SetUpRenderer(this.Handle, pictureBox1);
                texture = new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\awesomeface.png", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, TextureTypeEnum.kType_DiffuseTextureMap);
                mesh = new Mesh2D();
                renderPass = new FrameBufferRenderPass();
                renderPass.BuildRenderPass(texture);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
