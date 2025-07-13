using GlmSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents
{
    public class Transform2DComponentModel : INotifyPropertyChanged
    {
        private string name = "Transform2DComponent";
        private Transform2DComponent transform = new Transform2DComponent();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public Transform2DComponent Transform
        {
            get => transform;
            set
            {
                transform = value;
                OnPropertyChanged();
            }
        }

        public Transform2DComponentModel()
        {
        }

        public Transform2DComponentModel(string name, Transform2DComponent transform)
        {
            Name = name;
            Transform = transform;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}