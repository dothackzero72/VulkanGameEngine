using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.EditorEnhancements
{
    public class FlagEnumUIEditor : UITypeEditor
    {
        private FlagCheckedListBox FlagEnumCheckBoxList;

        public FlagEnumUIEditor()
        {
            FlagEnumCheckBoxList = new FlagCheckedListBox();
            FlagEnumCheckBoxList.BorderStyle = BorderStyle.None;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && 
                context.Instance != null && 
                provider != null)
            {

                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {

                    Enum e = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
                    FlagEnumCheckBoxList.EnumValue = e;
                    editorService.DropDownControl(FlagEnumCheckBoxList);
                    return FlagEnumCheckBoxList.EnumValue;

                }
            }
            return null;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }

    public class FlagCheckedListBox : CheckedListBox
    {
        private System.ComponentModel.Container components = null;
        private bool isUpdatingCheckStates = false;
        Type enumType;
        Enum enumValue;

        public FlagCheckedListBox()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.CheckOnClick = true;
        }
        #endregion

        public FlagCheckedListBoxItem Add(int v, string c)
        {
            FlagCheckedListBoxItem item = new FlagCheckedListBoxItem(v, c);
            Items.Add(item);
            return item;
        }

        public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
        {
            Items.Add(item);
            return item;
        }

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);

            if (isUpdatingCheckStates)
            {
                return;
            }

            FlagCheckedListBoxItem item = Items[e.Index] as FlagCheckedListBoxItem;
            UpdateCheckedItems(item, e.NewValue);
        }

        protected void UpdateCheckedItems(int value)
        {

            isUpdatingCheckStates = true;
            for (int x = 0; x < Items.Count; x++)
            {
                FlagCheckedListBoxItem item = Items[x] as FlagCheckedListBoxItem;

                if (item.value == 0)
                {
                    SetItemChecked(x, value == 0);
                }
                else
                {

                    if ((item.value & value) == item.value && item.value != 0)
                    {
                        SetItemChecked(x, true);
                    }
                    else
                    {
                        SetItemChecked(x, false);
                    }
                }
            }

            isUpdatingCheckStates = false;

        }

        protected void UpdateCheckedItems(FlagCheckedListBoxItem composite, CheckState cs)
        {
            if (composite.value == 0)
            {
                UpdateCheckedItems(0);
            }


            int sum = 0;
            for (int x = 0; x < Items.Count; x++)
            {
                FlagCheckedListBoxItem item = Items[x] as FlagCheckedListBoxItem;

                if (GetItemChecked(x))
                {
                    sum |= item.value;
                }
            }

            if (cs == CheckState.Unchecked)
            {
                sum = sum & (~composite.value);
            }
            else
            {
                sum |= composite.value;
            }

            UpdateCheckedItems(sum);

        }

        public int GetCurrentValue()
        {
            int sum = 0;

            for (int x = 0; x < Items.Count; x++)
            {
                FlagCheckedListBoxItem item = Items[x] as FlagCheckedListBoxItem;

                if (GetItemChecked(x))
                {
                    sum |= item.value;
                }
            }

            return sum;
        }

        private void FillEnumMembers()
        {
            foreach (string name in Enum.GetNames(enumType))
            {
                object val = Enum.Parse(enumType, name);
                int intVal = (int)Convert.ChangeType(val, typeof(int));

                Add(intVal, name);
            }
        }

        private void ApplyEnumValue()
        {
            int intVal = (int)Convert.ChangeType(enumValue, typeof(int));
            UpdateCheckedItems(intVal);

        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public Enum EnumValue
        {
            get
            {
                object enumObj = Enum.ToObject(enumType, GetCurrentValue());
                return (Enum)enumObj;
            }
            set
            {

                Items.Clear();
                enumValue = value;
                enumType = value.GetType(); 
                FillEnumMembers();
                ApplyEnumValue();

            }
        }
    }

    public class FlagCheckedListBoxItem
    {
        public int value;
        public string caption;

        public FlagCheckedListBoxItem(int value, string caption)
        {
            this.value = value;
            this.caption = caption;
        }

        public override string ToString()
        {
            return caption;
        }

        public bool IsFlag
        {
            get
            {
                return ((value & (value - 1)) == 0);
            }
        }

        public bool IsMemberFlag(FlagCheckedListBoxItem composite)
        {
            return (IsFlag && ((value & composite.value) == value));
        }
    }
}
