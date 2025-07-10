using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VulkanGameEngineLevelEditor.LevelEditor.Commands
{
    public class PropertyChangeCommand : ICommand
    {
        private object target;
        private PropertyInfo property;
        private object oldValue;
        private object newValue;

        public PropertyChangeCommand(object target, PropertyInfo property, object oldValue, object newValue)
        {
            this.target = target;
            this.property = property;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public void Execute() => property.SetValue(target, newValue);
        public void Undo() => property.SetValue(target, oldValue);
    }

}
