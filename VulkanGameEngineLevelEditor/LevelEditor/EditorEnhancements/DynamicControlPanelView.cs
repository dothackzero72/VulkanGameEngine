using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using GlmSharp;
using VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents;
using VulkanGameEngineLevelEditor.GameEngine.Structs;
using VulkanGameEngineLevelEditor.GameEngine.Systems;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements
{
    public struct UpdateProperty
    {
        public object ParentObj;
        public object Obj;
    }

    [DesignerCategory("")]
    public unsafe class DynamicControlPanelView : Panel
    {
        private object _targetObject;
        private Panel _contentPanel;
        private int yOffset = 5;
        private List<UpdateProperty> UpdatePropertiesList = new List<UpdateProperty>();

        public DynamicControlPanelView()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (DesignMode) return;

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };
            this.Controls.Add(_contentPanel);
        }

        public object SelectedObject
        {
            get => _targetObject;
            set
            {
                if (DesignMode) return;
                if (_targetObject != value)
                {
                    _targetObject = value;
                    _contentPanel.Controls.Clear();
                    yOffset = 5;
                    CreateCustomControls(null, _targetObject);
                }
            }
        }

        public void UpdateOriginalObject()
        {
            foreach(var updateProperty in UpdatePropertiesList)
            {
                if(updateProperty.ParentObj is GameObject)
                {
                    var parentObj = (GameObject)updateProperty.ParentObj;
                    if(updateProperty.Obj is Transform2DComponent)
                    {
                        var obj  = (Transform2DComponent)updateProperty.Obj;
                        GameObjectSystem.Transform2DComponentMap[parentObj.GameObjectId] = obj;
                    }
                }
            }
            UpdatePropertiesList.Clear();
        }

        private void CreateCustomControls(object parentObject, object obj)
        {
            if (DesignMode || obj == null) return;

            foreach (var prop in obj.GetType().GetProperties())
            {
                var ignoreAttr = prop.GetCustomAttributes(typeof(IgnorePropertyAttribute), true)
                   .FirstOrDefault() as IgnorePropertyAttribute;
                if (ignoreAttr != null) continue;

                Label label = new Label { Text = prop.Name, Location = new Point(5, yOffset), AutoSize = true, ForeColor = Color.White };
                Control control = CreateControlForProperty(parentObject, obj, prop);

                if (control != null)
                {
                    _contentPanel.Controls.Add(label);
                    _contentPanel.Controls.Add(control);
                    yOffset += 50; 
                }
                else if (prop.PropertyType == typeof(vec2))
                {
                    _contentPanel.Controls.Add(label); 
                    yOffset += 50; 
                }
            }
        }

        private Control CreateControlForProperty(object parentObject, object obj, PropertyInfo prop)
        {
            var readOnlyAttr = prop.GetCustomAttributes(typeof(ReadOnlyAttribute), true).FirstOrDefault() as ReadOnlyAttribute;
            bool isReadOnly = readOnlyAttr?.IsReadOnly ?? false;

            int controlWidth = 45;
            int totalControlWidth = controlWidth * 2 + 10;
            int rightMargin = 10;
            int xPosition = _contentPanel.Width - totalControlWidth - rightMargin;

            if (prop.PropertyType == typeof(string))
            {
                return TypeOfString(obj, prop, xPosition, totalControlWidth);
            }
            else if (prop.PropertyType == typeof(int))
            {
                return TypeOfInt(obj, prop, xPosition, totalControlWidth, isReadOnly);
            }
            else if (prop.PropertyType == typeof(uint))
            {
                return TypeOfUint(obj, prop, xPosition, totalControlWidth, isReadOnly);
            }
            else if(prop.PropertyType == typeof(bool))
            {
                //return TypeOfBool(obj, prop, xPosition, totalControlWidth, isReadOnly);
            }
            if (prop.PropertyType == typeof(Guid))
            {
                return TypeOfGuid(obj, prop, xPosition, totalControlWidth);
            }
            else if (prop.PropertyType == typeof(List<ComponentTypeEnum>))
            {
                return HandleComponentList(obj, prop, xPosition, totalControlWidth);
            }
            else if (prop.PropertyType == typeof(vec2))
            {
                TypeOfVec2(parentObject, obj, prop, xPosition, controlWidth);
                return null;
            }
            else if (typeof(IList).IsAssignableFrom(obj.GetType()))
            {
                var list = (IList)obj;
                for (int x = 0; x < list.Count; x++)
                {
                    if (list[x] is string)
                    {
                        var textBox = new TextBox
                        {
                            Location = new Point(xPosition, yOffset),
                            Size = new Size(xPosition, 30),
                            Text = (string)list[x] ?? "",
                            TextAlign = HorizontalAlignment.Left,
                            BackColor = Color.FromArgb(60, 60, 60),
                            ForeColor = Color.White
                        };

                        textBox.TextChanged += (s, e) =>
                        {
                            list[x] = ((TextBox)s).Text;
                        };
                        _contentPanel.Controls.Add(textBox);
                        yOffset += 50;
                    }
                    else
                    {
                        foreach (var subObjectProp in list[x].GetType().GetProperties())
                        {
                            CreateControlForProperty(obj, list[x], subObjectProp);
                            yOffset += 50;
                        }
                    }
                }
            }
            else
            {
                //var childObj = prop.GetValue(obj);
                //CreateControlForProperty(obj, childObj, prop);
            }
            return null;
        }

        private Control TypeOfString(object obj, PropertyInfo property, int xPosition, int width)
        {
            var textBox = new TextBox
            {
                Location = new Point(xPosition, yOffset),
                Size = new Size(width, 30),
                Text = property.GetValue(obj)?.ToString() ?? "",
                TextAlign = HorizontalAlignment.Left,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };
            textBox.TextChanged += (s, e) => property.SetValue(obj, ((TextBox)s).Text);
            return textBox;
        }

        private Control TypeOfGuid(object obj, PropertyInfo property, int xPosition, int width)
        {
            string guid = ((Guid)property.GetValue(obj)).ToString();
            var textBox = new TextBox
            {
                Location = new Point(xPosition, yOffset),
                Size = new Size(width, 30),
                Text = guid ?? "",
                TextAlign = HorizontalAlignment.Left,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                ReadOnly = true
            };
            textBox.TextChanged += (s, e) => property.SetValue(obj, ((TextBox)s).Text);
            return textBox;
        }

        private Control TypeOfBool(object obj, PropertyInfo property, int xPosition, int width, bool readOnly)
        {
            int value = (int)property.GetValue(obj);
            if (readOnly)
            {
                var labelDisplay = new Label
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                    Text = value.ToString() ?? "N/A",
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.FromArgb(60, 60, 60),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White
                };
                return labelDisplay;
            }
            else
            {
                var checkBox = new CheckBox
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                };

                checkBox.CheckedChanged += (s, e) =>
                {
                    try
                    {
                        property.SetValue(obj, (bool)((CheckBox)s).Checked);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting {property.Name}: {ex.Message}");
                    }
                };
                return checkBox;
            }
        }

        private Control TypeOfInt(object obj, PropertyInfo property, int xPosition, int width, bool readOnly)
        {
            int value = (int)property.GetValue(obj);
            if (readOnly)
            {
                var labelDisplay = new Label
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                    Text = value.ToString() ?? "N/A",
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.FromArgb(60, 60, 60),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White
                };
                return labelDisplay;
            }
            else
            {
                var numeric = new NumericUpDown
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                    Minimum = (decimal)int.MinValue,
                    Maximum = (decimal)int.MaxValue,
                    Value = (decimal)Math.Max(int.MinValue, Math.Min(int.MaxValue, value))
                };
                numeric.ValueChanged += (s, e) =>
                {
                    try
                    {
                        property.SetValue(obj, (int)((NumericUpDown)s).Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting {property.Name}: {ex.Message}");
                    }
                };
                return numeric;
            }
        }

        private Control TypeOfUint(object obj, PropertyInfo property, int xPosition, int width, bool readOnly)
        {
            uint value = (uint)property.GetValue(obj);
            if (readOnly)
            {
                var labelDisplay = new Label
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                    Text = value.ToString() ?? "N/A",
                    TextAlign = ContentAlignment.MiddleRight,
                    BackColor = Color.FromArgb(60, 60, 60),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White
                };
                return labelDisplay;
            }
            else
            {
                var numeric = new NumericUpDown
                {
                    Location = new Point(xPosition, yOffset),
                    Size = new Size(width, 30),
                    Minimum = (decimal)0,
                    Maximum = (decimal)uint.MaxValue,
                    Value = (decimal)Math.Max(int.MinValue, Math.Min(int.MaxValue, value))
                };
                numeric.ValueChanged += (s, e) =>
                {
                    try
                    {
                        property.SetValue(obj, (int)((NumericUpDown)s).Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting {property.Name}: {ex.Message}");
                    }
                };
                return numeric;
            }
        }

        private Control HandleComponentList(object parentObj, PropertyInfo prop, int xPosition, int width)
        {
            var gameObject = parentObj as GameObject;
            if (gameObject != null)
            {
                var componentList = gameObject.GameObjectComponentTypeList;
                foreach (var component in componentList)
                {
                    switch (component)
                    {
                        case ComponentTypeEnum.kTransform2DComponent:
                            if (GameObjectSystem.Transform2DComponentMap.TryGetValue(gameObject.GameObjectId, out var transformComponent))
                            {
                                CreateCustomControls(parentObj, transformComponent);
                            }
                            break;
                        case ComponentTypeEnum.kInputComponent:
                            if (GameObjectSystem.InputComponentMap.TryGetValue(gameObject.GameObjectId, out var inputComponent))
                            {
                                CreateCustomControls(parentObj, inputComponent);
                            }
                            break;
                        case ComponentTypeEnum.kSpriteComponent:
                            var spriteComponent = SpriteSystem.FindSprite(gameObject.GameObjectId);
                            {
                                CreateCustomControls(parentObj, spriteComponent);
                            }
                            break;
                    }
                }
            }
            return null;
        }

        private void TypeOfVec2(object parentObject, object obj, PropertyInfo property, int xPosition, int controlWidth)
        {
            var vec2Value = (vec2)property.GetValue(obj);
            var numericX = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000,
                Width = 100,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Location = new Point(xPosition - controlWidth - 100, yOffset),
                TextAlign = HorizontalAlignment.Left,
                Value = (decimal)vec2Value.x,
            };

            var numericY = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000,
                Width = 100,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                Location = new Point(xPosition, yOffset),
                TextAlign = HorizontalAlignment.Left,
                Value = (decimal)vec2Value.y,
            };

            numericX.ValueChanged += (s, e) =>
            {
                var newVec2 = new vec2((float)((NumericUpDown)s).Value, vec2Value.y);
                property.SetValue(obj, newVec2);
                vec2Value = newVec2;
                UpdatePropertiesList.Add(new UpdateProperty
                {
                    ParentObj = parentObject,
                    Obj = obj
                });
            };

            numericY.ValueChanged += (s, e) =>
            {
                var newVec2 = new vec2(vec2Value.x, (float)((NumericUpDown)s).Value);
                property.SetValue(obj, newVec2);
                vec2Value = newVec2;
                UpdatePropertiesList.Add(new UpdateProperty
                {
                    ParentObj = parentObject,
                    Obj = obj
                });
            };

            _contentPanel.Controls.Add(numericX);
            _contentPanel.Controls.Add(numericY);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!DesignMode && _targetObject != null)
            {
                _contentPanel.Controls.Clear();
                yOffset = 5;
                CreateCustomControls(null, _targetObject);
            }
        }

        private bool IsSimpleType(Type type)
        {
            return type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(int)
                   || type == typeof(uint)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(bool)
                   || type == typeof(byte)
                   || type == typeof(sbyte)
                   || type == typeof(short)
                   || type == typeof(ushort)
                   || type == typeof(long)
                   || type == typeof(ulong)
                   || type == typeof(char);
        }
    }
}