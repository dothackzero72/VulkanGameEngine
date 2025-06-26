using GlmSharp;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Systems
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct VertexLoaderStruct
    {
        public BufferTypeEnum VertexType { get; set; }
        public uint MeshVertexBufferId { get; set; }
        public size_t SizeofVertex { get; set; }
        public size_t VertexCount { get; set; }
        public void* VertexData { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct IndexLoaderStruct
    {
        public uint MeshIndexBufferId { get; set; }
        public size_t SizeofIndex { get; set; }
        public size_t IndexCount { get; set; }
        public void* IndexData { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct TransformLoaderStruct
    {
        public uint MeshTransformBufferId { get; set; }
        public size_t SizeofTransform { get; set; }
        public void* TransformData { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct MeshPropertiesLoaderStruct
    {
        public uint PropertiesBufferId { get; set; }
        public size_t SizeofMeshProperties { get; set; }
        public void* MeshPropertiesData { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct MeshLoader
    {
        public uint ParentGameObjectID { get; set; }
        public uint MeshId { get; set; }
        public Guid MaterialId { get; set; }

        public VertexLoaderStruct VertexLoader { get; set; }
        public IndexLoaderStruct IndexLoader { get; set; }
        public TransformLoaderStruct TransformLoader { get; set; }
        public MeshPropertiesLoaderStruct MeshPropertiesLoader { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct MeshPropertiesStruct
    {
        public uint ShaderMaterialBufferIndex = 0;
        public mat4 MeshTransform = mat4.Identity;

        public MeshPropertiesStruct()
        {
        }
    };

    public unsafe struct Mesh
    {
        public uint MeshId { get; set; } = 0;
        public uint ParentGameObjectID { get; set; } = 0;
        public uint GameObjectTransform { get; set; } = 0;
        public uint VertexCount { get; set; } = 0;
        public uint IndexCount { get; set; } = 0;
        public Guid MaterialId { get; set; } = Guid.Empty;

        public BufferTypeEnum VertexType { get; set; }
        public vec3 MeshPosition { get; set; } = new vec3(0.0f);
        public vec3 MeshRotation { get; set; } = new vec3(0.0f);
        public vec3 MeshScale { get; set; } = new vec3(1.0f);

        public int MeshVertexBufferId { get; set; } = 0;
        public int MeshIndexBufferId { get; set; } = 0;
        public int MeshTransformBufferId { get; set; } = 0;
        public int PropertiesBufferId { get; set; } = 0;

        public MeshPropertiesStruct MeshProperties { get; set; } = new MeshPropertiesStruct();

        public Mesh()
        {
        }
    };

    public unsafe static class MeshSystem
    {
        public static uint NextMeshId { get; private set; } = 0;
        public static uint NextSpriteMeshId { get; private set; } = 0;
        public static uint NextLevelLayerMeshId { get; private set; } = 0;

        public static Dictionary<uint, Mesh> MeshMap { get; private set; } = new Dictionary<uint, Mesh>();
        public static Dictionary<int, Mesh> SpriteMeshMap { get; private set; } = new Dictionary<int, Mesh>();
        public static Dictionary<Guid, List<Mesh>> LevelLayerMeshListMap { get; private set; } = new Dictionary<Guid, List<Mesh>>();
        public static Dictionary<uint, ListPtr<Vertex2D>> Vertex2DListMap { get; private set; } = new Dictionary<uint, ListPtr<Vertex2D>>();
        public static Dictionary<uint, ListPtr<uint>> IndexListMap { get; private set; } = new Dictionary<uint, ListPtr<uint>>();

        public static uint CreateMesh<T>(ListPtr<T> vertexList, ListPtr<uint> indexList, Guid materialId) where T : unmanaged
        {
            if (typeof(T) != typeof(Vertex2D))
            {
                throw new ArgumentException("Vertex type must be Vertex2D", nameof(vertexList));
            }

            if (vertexList == null || vertexList.Ptr == null || indexList == null || indexList.Ptr == null)
            {
                throw new ArgumentNullException("Vertex or index list is null or disposed");
            }

            uint meshId = NextMeshId++;
            mat4 meshMatrix = mat4.Identity;

            ListPtr<Vertex2D> vertex2DList = vertexList as ListPtr<Vertex2D> ?? throw new InvalidCastException("Failed to cast vertexList to ListPtr<Vertex2D>");
            Vertex2DListMap[meshId] = vertex2DList;
            IndexListMap[meshId] = indexList;

            Mesh mesh = new Mesh();
            MeshMap[meshId] = mesh;

            GCHandle matrixHandle = GCHandle.Alloc(meshMatrix, GCHandleType.Pinned);
            GCHandle propertiesHandle = GCHandle.Alloc(mesh.MeshProperties, GCHandleType.Pinned);
            try
            {
                MeshLoader meshLoader = new MeshLoader
                {
                    ParentGameObjectID = 0,
                    MeshId = meshId,
                    MaterialId = materialId,
                    VertexLoader = new VertexLoaderStruct
                    {
                        VertexType = BufferTypeEnum.BufferType_Vector2D,
                        MeshVertexBufferId = ++BufferSystem.NextBufferId,
                        SizeofVertex = sizeof(T),
                        VertexCount = vertexList.Count,
                        VertexData = vertexList.Ptr
                    },
                    IndexLoader = new IndexLoaderStruct
                    {
                        MeshIndexBufferId = ++BufferSystem.NextBufferId,
                        SizeofIndex = sizeof(uint),
                        IndexCount = indexList.Count,
                        IndexData = indexList.Ptr
                    },
                    TransformLoader = new TransformLoaderStruct
                    {
                        MeshTransformBufferId = ++BufferSystem.NextBufferId,
                        SizeofTransform = sizeof(mat4),
                        TransformData = (void*)matrixHandle.AddrOfPinnedObject()
                    },
                    MeshPropertiesLoader = new MeshPropertiesLoaderStruct
                    {
                        PropertiesBufferId = ++BufferSystem.NextBufferId,
                        SizeofMeshProperties = sizeof(MeshPropertiesStruct),
                        MeshPropertiesData = (void*)propertiesHandle.AddrOfPinnedObject()
                    }
                };

                mesh = Mesh_CreateMesh(RenderSystem.renderer, meshLoader,
                    BufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId]);

                MeshMap[meshId] = mesh;
            }
            finally
            {
                matrixHandle.Free();
                propertiesHandle.Free();
            }

            return meshId;
        }

        public static int CreateSpriteLayerMesh(ListPtr<Vertex2D> vertexList, ListPtr<uint> indexList)
        {
            uint meshId = ++NextSpriteMeshId;
            mat4 meshMatrix = mat4.Identity;

            Vertex2DListMap[meshId] = vertexList;
            IndexListMap[meshId] = indexList;

            Mesh mesh = new Mesh();
            MeshMap[meshId] = mesh;

            GCHandle matrixHandle = GCHandle.Alloc(meshMatrix, GCHandleType.Pinned);
            GCHandle propertiesHandle = GCHandle.Alloc(mesh.MeshProperties, GCHandleType.Pinned);
            try
            {
                MeshLoader meshLoader = new MeshLoader
                {
                    ParentGameObjectID = 0,
                    MeshId = meshId,
                    MaterialId = Guid.Empty,
                    VertexLoader = new VertexLoaderStruct

                    {
                        VertexType = BufferTypeEnum.BufferType_Vector2D,
                        MeshVertexBufferId = ++BufferSystem.NextBufferId,
                        SizeofVertex = sizeof(Vertex2D),
                        VertexCount = vertexList.Count,
                        VertexData = vertexList.Ptr,
                    },
                    IndexLoader = new IndexLoaderStruct

                    {
                        MeshIndexBufferId = ++BufferSystem.NextBufferId,
                        SizeofIndex = sizeof(uint),
                        IndexCount = indexList.Count,
                        IndexData = indexList.Ptr,
                    },
                    TransformLoader = new TransformLoaderStruct

                    {
                        MeshTransformBufferId = ++BufferSystem.NextBufferId,
                        SizeofTransform = sizeof(mat4),
                        TransformData = (void*)matrixHandle.AddrOfPinnedObject()
                    },
                    MeshPropertiesLoader = new MeshPropertiesLoaderStruct

                    {
                        PropertiesBufferId = ++BufferSystem.NextBufferId,
                        SizeofMeshProperties = sizeof(MeshPropertiesStruct),
                        MeshPropertiesData = (void*)propertiesHandle.AddrOfPinnedObject()
                    }
                };

                mesh = Mesh_CreateMesh(RenderSystem.renderer, meshLoader,
                    BufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
                    BufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId]);

                SpriteMeshMap[(int)meshId] = mesh;
            }
            finally
            {
                matrixHandle.Free();
                propertiesHandle.Free();
            }


            return (int)meshId;
        }

        public static int CreateLevelLayerMesh(Guid levelId, ListPtr<Vertex2D> vertexList, ListPtr<uint> indexList)
        {
            uint meshId = ++NextLevelLayerMeshId;
            mat4 meshMatrix = mat4.Identity;

            Vertex2DListMap[meshId] = vertexList;
            IndexListMap[meshId] = indexList;

            Mesh mesh = new Mesh();
            MeshMap[meshId] = mesh;

            GCHandle matrixHandle = GCHandle.Alloc(meshMatrix, GCHandleType.Pinned);
            GCHandle propertiesHandle = GCHandle.Alloc(mesh.MeshProperties, GCHandleType.Pinned);
            try
            {
                MeshLoader meshLoader = new MeshLoader
                {
                    ParentGameObjectID = 0,
                    MeshId = meshId,
                    MaterialId = Guid.Empty,
                    VertexLoader = new VertexLoaderStruct

                    {
                        VertexType = BufferTypeEnum.BufferType_Vector2D,
                        MeshVertexBufferId = ++BufferSystem.NextBufferId,
                        SizeofVertex = sizeof(Vertex2D),
                        VertexCount = vertexList.Count,
                        VertexData = vertexList.Ptr,
                    },
                    IndexLoader = new IndexLoaderStruct

                    {
                        MeshIndexBufferId = ++BufferSystem.NextBufferId,
                        SizeofIndex = sizeof(uint),
                        IndexCount = indexList.Count,
                        IndexData = indexList.Ptr,
                    },
                    TransformLoader = new TransformLoaderStruct

                    {
                        MeshTransformBufferId = ++BufferSystem.NextBufferId,
                        SizeofTransform = sizeof(mat4),
                        TransformData = (void*)matrixHandle.AddrOfPinnedObject()
                    },
                    MeshPropertiesLoader = new MeshPropertiesLoaderStruct

                    {
                        PropertiesBufferId = ++BufferSystem.NextBufferId,
                        SizeofMeshProperties = sizeof(MeshPropertiesStruct),
                        MeshPropertiesData = (void*)propertiesHandle.AddrOfPinnedObject()
                    }
                };

                List<Mesh> meshList = new List<Mesh>
                {
                    Mesh_CreateMesh(RenderSystem.renderer, meshLoader, BufferSystem.VulkanBufferMap[meshLoader.VertexLoader.MeshVertexBufferId],
                                                                       BufferSystem.VulkanBufferMap[meshLoader.IndexLoader.MeshIndexBufferId],
                                                                       BufferSystem.VulkanBufferMap[meshLoader.TransformLoader.MeshTransformBufferId],
                                                                       BufferSystem.VulkanBufferMap[meshLoader.MeshPropertiesLoader.PropertiesBufferId])

                };
                LevelLayerMeshListMap[levelId] = meshList;
            }
            finally
            {
                matrixHandle.Free();
                propertiesHandle.Free();
            }

            return (int)meshId;
        }

        public static void Update(float deltaTime)
        {
            foreach (var meshPair in SpriteMeshMap)
            {
                VulkanBuffer propertiesBuffer = BufferSystem.VulkanBufferMap[(uint)meshPair.Value.PropertiesBufferId];
                uint shaderMaterialBufferIndex = (meshPair.Value.MaterialId != new Guid()) ? MaterialSystem.MaterialMap[meshPair.Value.MaterialId].ShaderMaterialBufferIndex : 0;
                Mesh_UpdateMesh(RenderSystem.renderer, meshPair.Value, propertiesBuffer, shaderMaterialBufferIndex, deltaTime);
            }
        }

        public static void DestroyMesh(uint meshId)
        {
            if (!MeshMap.TryGetValue(meshId, out Mesh mesh))
                return;

            // Use int keys directly, as Mesh stores buffer IDs as int
            Mesh_DestroyMesh(RenderSystem.renderer, mesh,
                BufferSystem.VulkanBufferMap[(uint)mesh.MeshVertexBufferId],
                BufferSystem.VulkanBufferMap[(uint)mesh.MeshIndexBufferId],
                BufferSystem.VulkanBufferMap[(uint)mesh.MeshTransformBufferId],
                BufferSystem.VulkanBufferMap[(uint)mesh.PropertiesBufferId]);

            if (Vertex2DListMap.TryGetValue(meshId, out var vertexList))
            {
                vertexList.Dispose();
                Vertex2DListMap.Remove(meshId);
            }
            if (IndexListMap.TryGetValue(meshId, out var indexList))
            {
                indexList.Dispose();
                IndexListMap.Remove(meshId);
            }

            MeshMap.Remove(meshId);
        }
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern Mesh Mesh_CreateMesh(GraphicsRenderer renderer, MeshLoader meshLoader, VulkanBuffer outVertexBuffer, VulkanBuffer outIndexBuffer, VulkanBuffer outTransformBuffer, VulkanBuffer outPropertiesBuffer);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Mesh_UpdateMesh(GraphicsRenderer renderer, Mesh mesh, VulkanBuffer meshPropertiesBuffer, uint shaderMaterialBufferIndex, float deltaTime);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Mesh_DestroyMesh(GraphicsRenderer renderer, Mesh mesh, VulkanBuffer vertexBuffer, VulkanBuffer indexBuffer, VulkanBuffer transformBuffer, VulkanBuffer propertiesBuffer);

    }
}
