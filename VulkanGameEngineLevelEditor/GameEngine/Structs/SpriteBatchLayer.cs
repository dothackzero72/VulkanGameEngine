using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public struct SpriteBatchLayer
    {
        public Guid RenderPassId { get; set; }
        public uint SpriteBatchLayerId { get; set; }
        public uint SpriteLayerMeshId { get; set; }
        public SpriteBatchLayer() { }
        public SpriteBatchLayer(Guid renderPassId)
        {
            RenderPassId = renderPassId;
        }
    }
}
