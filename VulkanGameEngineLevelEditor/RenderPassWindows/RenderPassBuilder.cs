using Silk.NET.Core;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassWindows
{
    public partial class RenderPassBuilder : Form
    {
        public MessengerModel RenderPassMessager { get; set; }
        public RenderedTextureInfoModel RenderedTextureAttachments { get; set; } = new RenderedTextureInfoModel();
        public RenderPassBuilder()
        {
            InitializeComponent();
            this.FormClosing += OnClose;

            RenderPassMessager = new MessengerModel()
            {
                IsActive = true,
                richTextBox = RenderPassBuilderDebug,
                TextBoxName = RenderPassBuilderDebug.Name
            };

            GlobalMessenger.AddMessenger(RenderPassMessager);
            listBox1.Items.Add(RenderedTextureAttachments);
        }

        public void ShowRenderPassBuilder()
        {
            using (RenderPassBuilder popup = new RenderPassBuilder())
            {
                DialogResult result = popup.ShowDialog();
               
                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Popup closed.");
                }
            }
        }
        private void ImageTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  var asdf = (ImageType)Enum.Parse(typeof(ImageType), ImageTypeBox.SelectedItem.ToString());
         //   var asd = ImageTypeBox.SelectedItem.ToString();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void Accept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            GlobalMessenger.messenger.Remove(RenderPassMessager);
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = listBox1.SelectedIndex;
            propertyGrid1.SelectedObject = RenderedTextureAttachments;
     
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {

        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            GlobalMessenger.messenger.Remove(RenderPassMessager);
        }

    }
}
