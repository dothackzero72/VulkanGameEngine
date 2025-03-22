using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Level2DRenderer : JsonRenderPass<Vertex2D>
    {
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; }
        public List<GameObject> GameObjectList { get; private set; }
        public List<Texture> TextureList { get; private set; }
        public List<Material> MaterialList { get; private set; }
    }
}
