using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Sprite
    {
        [JsonIgnore]
        public GameObject ParentGameObject { get; set; }

        [JsonIgnore]
        public Transform2DComponent Transform2D { get; set; }

        [JsonIgnore]
        public uint CurrentAnimationID { get; set; } = 0;

        [JsonIgnore]
        public uint CurrentFrame { get; set; } = 0;

        [JsonIgnore]
        public bool SpriteAlive { get; set; } = true;

        [JsonIgnore]
        public Animation2D CurrentSpriteAnimation { get; set; }

        [JsonIgnore]
        public SpriteSheet Spritesheet { get; set; }

        [JsonIgnore]
        public SpriteInstanceStruct SpriteInstance;

        public vec2 SpriteSize { get; set; } = new vec2(50.0f);
        public vec4 SpriteColor { get; set; } = new vec4(0.0f, 0.0f, 0.0f, 1.0f);
        public vec2 SpritePosition { get; set; } = new vec2(0.0f);
        public vec2 SpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 SpriteScale { get; set; } = new vec2(1.0f);
        public uint SpriteLayer { get; set; } = 0;
        public Material SpriteMaterial { get; set; }
        public List<Animation2D> AnimationList { get; set; } = new List<Animation2D>();

        Sprite()
        {

        }

        public Sprite(GameObject parentGameObject, SpriteModel model)
        {
            this.ParentGameObject = parentGameObject;

        }

        public Sprite(GameObject parentObject, SpriteSheet spriteSheet)
        {
            ParentGameObject = parentObject;
            Transform2D = (Transform2DComponent)ParentGameObject.GetComponentByComponentType(ComponentTypeEnum.kGameObjectTransform2DComponent);

            Spritesheet = spriteSheet;
            SpriteLayer = spriteSheet.SpriteLayer;
            SpriteSize = new vec2(spriteSheet.SpritePixelSize.x * spriteSheet.SpriteScale.x, spriteSheet.SpritePixelSize.y * spriteSheet.SpriteScale.y);
            SpriteColor = new vec4(0.0f, 0.0f, 0.0f, 1.0f);
            SpriteMaterial = spriteSheet.SpriteMaterial;
            SpriteInstance = new SpriteInstanceStruct();

            List<ivec2> frameList = new List<ivec2>
            {
                new ivec2(0, 0),
                new ivec2(1, 0)
            };

            AnimationList.Add(new Animation2D("Standing", frameList, 0.2f));

            List<ivec2> frameList2 = new List<ivec2>
            {
               new ivec2(3, 0),
                new ivec2(4, 0),
                new ivec2(5, 0),
                new ivec2(4, 0)
            };
            AnimationList.Add(new Animation2D("Walking", frameList2, 0.5f));
            CurrentSpriteAnimation = AnimationList.Last();
        }

        public void Input(float deltaTime)
        {

        }

        public void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            if (Transform2D != null)
	{
                mat4 spriteMatrix = mat4.Identity;
                spriteMatrix = mat4.Translate(new vec3(Transform2D.GameObjectPosition.x, Transform2D.GameObjectPosition.y, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(Transform2D.GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(Transform2D.GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(0.0f), new vec3(0.0f, 0.0f, 1.0f));
                spriteMatrix = mat4.Scale(new vec3(Transform2D.GameObjectScale.x, Transform2D.GameObjectScale.y, 0.0f));


                SpriteInstance.SpritePosition = Transform2D.GameObjectPosition;
                SpriteInstance.SpriteSize = SpriteSize;
                SpriteInstance.UVOffset = new vec4(Spritesheet.SpriteUVSize.x * CurrentSpriteAnimation.FrameList[(int)CurrentFrame].x, Spritesheet.SpriteUVSize.y * CurrentSpriteAnimation.FrameList[(int)CurrentFrame].y, Spritesheet.SpriteUVSize.x, Spritesheet.SpriteUVSize.y);
                SpriteInstance.Color = SpriteColor;
                SpriteInstance.MaterialID = (SpriteMaterial != null) ? SpriteMaterial.MaterialBufferIndex : 0;
                SpriteInstance.InstanceTransform = spriteMatrix;
            }

            CurrentSpriteAnimation.CurrentFrameTime += deltaTime;
            if (CurrentSpriteAnimation.CurrentFrameTime >= CurrentSpriteAnimation.FrameHoldTime)
            {
                CurrentFrame += 1;
                CurrentSpriteAnimation.CurrentFrameTime = 0.0f;
                if (CurrentFrame > CurrentSpriteAnimation.FrameList.Count() - 1)
                {
                    CurrentFrame = 0;
                }
            }
        }

        public void Destroy()
        {

        }
    }
}
