using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public abstract class RenderPassEditorBaseModel : INotifyPropertyChanged
    {
        public string _name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public RenderPassEditorBaseModel() { }
        public RenderPassEditorBaseModel(string name)
        {
            _name = name;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
