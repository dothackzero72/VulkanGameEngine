using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineGameObjectScripts;
using Silk.NET.Vulkan;
using System.Security.Cryptography.Xml;
using VulkanGameEngineGameObjectScripts.Import;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Sprite
    {
        uint CurrentAnimationID = 0;

        vec2 SpriteSize = new vec2(50.0f);
        vec4 SpriteColor = new vec4(0.0f, 0.0f, 0.0f, 1.0f);
        vec2 SpritePosition = new vec2(0.0f);
        uint SpriteLayer = 0;
        vec2 SpriteRotation = new vec2(0.0f);
        vec2 SpriteScale = new vec2(1.0f);

        GameObject ParentGameObject;
        Transform2DComponent Transform2D;

        SpriteSheet Spritesheet;
        Material SpriteMaterial;
        SpriteInstanceStruct SpriteInstance;
        Animation2D CurrentSpriteAnimation;

        List<Animation2D> AnimationList;
        uint CurrentFrame = 0;

        bool SpriteAlive = true;

        Sprite()
        {

        }

        Sprite(GameObject parentObject, SpriteSheet spriteSheet)
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
                spriteMatrix = mat4.Scale(new vec3(SpriteScale.x, SpriteScale.y, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(SpriteRotation.x), new vec3(1.0f, 0.0f, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(SpriteRotation.y), new vec3(0.0f, 1.0f, 0.0f));
                spriteMatrix = mat4.Rotate(CLIMath.DegreesToRadians(0.0f), new vec3(0.0f, 0.0f, 1.0f));
                spriteMatrix = mat4.Translate(new vec3(SpritePosition.x, SpritePosition.y, 0.0f));

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

       public SpriteInstanceStruct GetSpriteInstance() { return SpriteInstance; }
    }
}
