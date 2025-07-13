using GlmSharp;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VulkanGameEngineLevelEditor.GameEngine.Structs;

namespace VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents
{
    public class SpriteModel : INotifyPropertyChanged
    {
        private Sprite sprite = new Sprite();

        public event PropertyChangedEventHandler PropertyChanged;

        public uint GameObjectId
        {
            get => sprite.GameObjectId;
            private set
            {
                if (sprite.GameObjectId != value)
                {
                    sprite.GameObjectId = value;
                    OnPropertyChanged();
                }
            }
        }

        public uint SpriteID
        {
            get => sprite.SpriteID;
            private set
            {
                if (sprite.SpriteID != value)
                {
                    sprite.SpriteID = value;
                    OnPropertyChanged();
                }
            }
        }

        public uint CurrentAnimationID
        {
            get => sprite.CurrentAnimationID;
            set
            {
                if (sprite.CurrentAnimationID != value)
                {
                    sprite.CurrentAnimationID = value;
                    OnPropertyChanged();
                }
            }
        }

        public uint CurrentFrame
        {
            get => sprite.CurrentFrame;
            set
            {
                if (sprite.CurrentFrame != value)
                {
                    sprite.CurrentFrame = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid SpriteVramId
        {
            get => sprite.SpriteVramId;
            set
            {
                if (sprite.SpriteVramId != value)
                {
                    sprite.SpriteVramId = value;
                    OnPropertyChanged();
                }
            }
        }

        public float CurrentFrameTime
        {
            get => sprite.CurrentFrameTime;
            set
            {
                if (sprite.CurrentFrameTime != value)
                {
                    sprite.CurrentFrameTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SpriteAlive
        {
            get => sprite.SpriteAlive;
            set
            {
                if (sprite.SpriteAlive != value)
                {
                    sprite.SpriteAlive = value;
                    OnPropertyChanged();
                }
            }
        }

        public ivec2 FlipSprite
        {
            get => sprite.FlipSprite;
            set
            {
                if (sprite.FlipSprite != value)
                {
                    sprite.FlipSprite = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 LastSpritePosition
        {
            get => sprite.LastSpritePosition;
            set
            {
                if (sprite.LastSpritePosition != value)
                {
                    sprite.LastSpritePosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 LastSpriteRotation
        {
            get => sprite.LastSpriteRotation;
            set
            {
                if (sprite.LastSpriteRotation != value)
                {
                    sprite.LastSpriteRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 LastSpriteScale
        {
            get => sprite.LastSpriteScale;
            set
            {
                if (sprite.LastSpriteScale != value)
                {
                    sprite.LastSpriteScale = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 SpritePosition
        {
            get => sprite.SpritePosition;
            set
            {
                if (sprite.SpritePosition != value)
                {
                    sprite.SpritePosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 SpriteRotation
        {
            get => sprite.SpriteRotation;
            set
            {
                if (sprite.SpriteRotation != value)
                {
                    sprite.SpriteRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        public vec2 SpriteScale
        {
            get => sprite.SpriteScale;
            set
            {
                if (sprite.SpriteScale != value)
                {
                    sprite.SpriteScale = value;
                    OnPropertyChanged();
                }
            }
        }

        public SpriteModel()
        {
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}