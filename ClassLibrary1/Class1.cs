using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts.CLI;


namespace VulkanGameEngineGameObjectScripts
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CustomAttribute : Attribute
    {
        public float Value;
    }

    public enum ComponentTypeEnum
    {
        kUndefined,
        kRenderMesh2DComponent,
        kGameObjectTransform2DComponent,
        kInputComponent
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public uint MeshBufferIndex;
        public ulong buffer;
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            MeshBufferIndex = uint.MaxValue;
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
            buffer = 0;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct GameObjectStruct
    {

        public String Name { get; set; }
        public List<GameObjectComponent> GameObjectComponentList { get; set; } = new List<GameObjectComponent>();

        public GameObjectStruct()
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ComponentListContainer
    {
        public int Count { get; set; }
        public GameObjectComponent* ptr { get; set; }
    }

    public interface IGameObject
    {
        public void Input(InputKey key, KeyState keyState);
        public void Update(float deltaTime);
        public void BufferUpdate(IntPtr commandBuffer, float deltaTime);
        public void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
        public int GetMemorySize();
    }

    public unsafe class GameObject : IGameObject
    {
        public String Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();


        public GameObject()
        {
        }

        private GameObject(String name)
        {
            Name = name;
        }

        private GameObject(String name, List<GameObjectComponent> gameObjectComponentList)
        {
            Name = name;
            GameObjectComponentList = gameObjectComponentList;
        }

        public void Initialize(String name)
        {
            Name = name;
        }

        public void Initialize(String name, List<GameObjectComponent> componentTypeList)
        {
            Name = name;
            GameObjectComponentList = componentTypeList;
        }

        public void AddComponent(GameObjectComponent newComponent)
        {
            Console.WriteLine("AddComponent()");
            GameObjectComponentList.Add(newComponent);
        }

        public void AddComponent(IntPtr blightPtr, int componentTypeEnum)
        {
           //var ablightPtr = (GameObjectComponentBlight*)blightPtr.ToPointer();
           // Console.WriteLine($"Initialized with Name: {ablightPtr->Name}, ComponentType: {ablightPtr->ComponentType}");

            Console.WriteLine("Type in " + componentTypeEnum);
            Console.WriteLine("AddComponent() in");

            string typeName = string.Empty;

            // Map component type enum to C# type name
            switch ((ComponentTypeEnum)componentTypeEnum)
            {
                case ComponentTypeEnum.kGameObjectTransform2DComponent:
                    typeName = "VulkanGameEngineGameObjectScripts.Transform2DComponent";
                    break;
                // Add more cases as necessary
                default:
                    Console.WriteLine("Unknown component type");
                    return;
            }

            Console.WriteLine("Looking for type: " + typeName);
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                Console.WriteLine($"Type '{typeName}' not found.");
                return;
            }

            // Create the component instance dynamically
            GameObjectComponent component = (GameObjectComponent)Activator.CreateInstance(type, blightPtr);

            if (component == null)
            {
                Console.WriteLine("Failed to create component instance.");
                return;
            }

            // Output the component type
            Console.WriteLine("GameObjectPtr after creation: 0x" + component.ParentGameObjectPtr.ToString("X"));
            Console.WriteLine("Component Type after creation: " + component.ComponentType);
            Console.WriteLine("name after creation: " + component.Name);

            // Add to the list
            GameObjectComponentList.Add(component);
            Console.WriteLine("AddComponent() added");
            Console.WriteLine("GameObjectPtr after creation: 0x" + GameObjectComponentList.First().ParentGameObjectPtr.ToString("X"));
            Console.WriteLine("Component Type after creation: " + GameObjectComponentList.First().ComponentType);
            Console.WriteLine("name after creation: " + GameObjectComponentList.First().Name);

            // Call Update on all components in the list
            // foreach (var components in GameObjectComponentList)
            // {
            //     components.Update(0.0f); // Call update for each component
            // }

            Console.WriteLine("AddComponent() update");
        }

        public virtual void Input(InputKey key, KeyState keyState)
        {
        }

        public virtual void Update(float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Update(deltaTime);
            }
        }

        public virtual void BufferUpdate(IntPtr commandBuffer, float deltaTime)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.BufferUpdate(commandBuffer, deltaTime);
            }
        }

        public virtual void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties)
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
            }
        }

        public virtual void Destroy()
        {
            foreach (GameObjectComponent component in GameObjectComponentList)
            {
                component.Destroy();
            }
        }

        public virtual int GetMemorySize()
        {
            return sizeof(GameObject);
        }

        public unsafe GameObjectComponent* GetGameObjectComponentListPtr()
        {
            GameObjectComponent* arrayPtr = (GameObjectComponent*)Marshal.AllocHGlobal(GameObjectComponentList.Count * sizeof(GameObjectComponent*));
            return arrayPtr;
        }

        public unsafe void FreeGameObjectComponentListPtr(GameObjectComponent* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
    }

    public unsafe struct GameObjectComponentBlight
    {
        public IntPtr ParentGameObjectPtr = IntPtr.Zero;
        public int ComponentType { get; set; }
        public NativeString Name { get; set; } = new NativeString();

        public GameObjectComponentBlight()
        {

        }
    }

    public interface IGameObjectComponent
    {
        public void Input(InputKey key, KeyState keyState);
        public void Update(float deltaTime);
        public void BufferUpdate(IntPtr commandBuffer, float deltaTime);
        public void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties);
        public void Destroy();
        public int GetMemorySize();
    }

    public unsafe class GameObjectComponent : IGameObjectComponent
    {
        protected GameObjectComponentBlight* blight;

        public NativeString Name
        {
            get => blight->Name;
            set => blight->Name = value;
        }

        public int ComponentType
        {
            get => blight->ComponentType;
            set => blight->ComponentType = value;
        }

        public IntPtr ParentGameObjectPtr
        {
            get => blight->ParentGameObjectPtr;
            set => blight->ParentGameObjectPtr = value;
        }

        public GameObjectComponent()
        {
            
        }

        public GameObjectComponent(IntPtr blightPtr)
        {
            blight = (GameObjectComponentBlight*)blightPtr.ToPointer();
        }

        public virtual void Input(InputKey key, KeyState keyState) 
        { 
        }

        public virtual void Update(float deltaTime) 
        { 
            Console.WriteLine("GameObjectComponent Updated"); 
        }

        public virtual void BufferUpdate(IntPtr commandBuffer, float deltaTime) 
        { 

        }

        public virtual void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties) 
        { 

        }

        public virtual void Destroy() 
        { 

        }

        public virtual int GetMemorySize() 
        { 
            return sizeof(GameObjectComponent); 
        }
    }

    public unsafe class Transform2DComponent : GameObjectComponent
    {
        public mat4 GameObjectTransform;
        public vec2 GameObjectPosition;
        public vec2 GameObjectRotation;
        public vec2 GameObjectScale;

        public Transform2DComponent() : base()
        {
            //Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr blight3) : base(blight3)
        {
        //    Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(IntPtr blight3, String name) : base(blight3)
        {
          //  Name = name;

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public override void Input(InputKey key, KeyState keyState)
        {
            Console.WriteLine("input called");
        }

        public override void Update(float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));

            Console.WriteLine("Transform2DComponent Update called");
        }

        public override void BufferUpdate(IntPtr commandBuffer, float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));

            Console.WriteLine("Transform2DComponent BufferUpdate called");
        }

        public override void Draw(IntPtr commandBuffer, IntPtr pipeline, IntPtr shaderPipelineLayout, IntPtr descriptorSet, SceneDataBuffer sceneProperties)
        {
            Console.WriteLine("Draw called");
        }

        public override void Destroy()
        {
            Console.WriteLine("Destroy called");
        }

        public override int GetMemorySize()
        {
            return (int)sizeof(Transform2DComponent);
        }

        public vec2* GetPositionPtr()
        {
            fixed (vec2* positionPointer = &GameObjectPosition)
            {
                return positionPointer;
            }
        }

        public vec2* GetRotationPtr()
        {
            fixed (vec2* rotationPointer = &GameObjectRotation)
            {
                return rotationPointer;
            }
        }

        public vec2* GetScalePtr()
        {
            fixed (vec2* scalePointer = &GameObjectScale)
            {
                return scalePointer;
            }
        }

        public mat4* GetTransformMatrixPtr()
        {
            fixed (mat4* transformPointer = &GameObjectTransform)
            {
                return transformPointer;
            }
        }
    }

    //[Custom(Value = -2500.0f)]
    //public unsafe class ExampleClass : GameObjectComponent
    //{

    //    public struct MyVec3
    //    {
    //        public float X;
    //        public float Y;
    //        public float Z;

    //        public MyVec3()
    //        {
    //            X = 1.0f;
    //            Y = 2.0f;
    //            Z = 3.0f;
    //        }

    //        public MyVec3(float x, float y, float z)
    //        {
    //            X = x;
    //            Y = y;
    //            Z = z;
    //        }
    //    }

    //    public NativeString Name { get; protected set; } = new NativeString();
    //    public ComponentTypeEnum ComponentType { get; set; }

    //    public mat4 GameObjectTransform;
    //    public vec2 GameObjectPosition;
    //    public vec2 GameObjectRotation;
    //    public vec2 GameObjectScale;

    //    internal static unsafe delegate*<ComponentTypeEnum*> EnumIcall;
    //    internal static unsafe delegate*<MyVec3*, MyVec3*, void> VectorAddIcall;
    //    internal static unsafe delegate*<NativeString, void> PrintStringIcall;
    //    internal static unsafe delegate*<NativeArray<float>, void> NativeArrayIcall;
    //    internal static unsafe delegate*<NativeArray<float>> ArrayReturnIcall;

    //    private int myPrivateValue;
    //    public MyVec3 vec32 = new MyVec3(2.2f, 3.3f, 4.4f);
    //    private ComponentTypeEnum vec3 = ComponentTypeEnum.kRenderMesh2DComponent;
    //    public ExampleClass(int someValue)
    //    {
    //        Console.WriteLine($"Example({someValue})");
    //    }


    //    public static void StaticMethod(float value)
    //    {
    //        Console.WriteLine($"StaticMethod: {value}");
    //    }


    //    public ComponentTypeEnum GetEnum()
    //    {
    //        return ComponentTypeEnum.kGameObjectTransform2DComponent;
    //    }


    //    public void MemberMethod(MyVec3 vec3)
    //    {
    //        MyVec3 anotherVector = new()
    //        {
    //            X = 10,
    //            Y = 20,
    //            Z = 30
    //        };

    //       // unsafe { VectorAddIcall(&vec3, &anotherVector); }

    //        Console.WriteLine($"X: {vec3.X}, Y: {vec3.Y}, Z: {vec3.Z}");
    //    }

    //    public void StringDemo()
    //    {
    //        NativeString str = "Hello, World?";
    //        unsafe { PrintStringIcall(str); }
    //    }

    //    public void ArrayDemo(float[] InArray)
    //    {
    //        NativeArray<float> arr = new(InArray);
    //        unsafe { NativeArrayIcall(arr); }

    //        unsafe
    //        {
    //            // We use "using" here so that nativeArr is automatically disposed of at
    //            // the end of this scope
    //            using var nativeArr = ArrayReturnIcall();

    //            foreach (var v in nativeArr)
    //                Console.WriteLine(v);
    //        }
    //    }

    //    public int PublicProp
    //    {
    //        get => myPrivateValue;
    //        set => myPrivateValue = value * 2;
    //    }

    //    public ComponentTypeEnum Publicvec3
    //    {
    //        get => vec3;
    //        set => vec3 = value;
    //    }

    //    public override void Update()
    //    {
    //        GameObjectTransform = mat4.Identity;
    //        GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
    //        GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
    //        GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
    //        GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
    //    }

    //    public vec2* GetPositionPtr()
    //    {
    //        fixed (vec2* positionPointer = &GameObjectPosition)
    //        {
    //            return positionPointer;
    //        }
    //    }

    //    public vec2* GetRotationPtr()
    //    {
    //        fixed (vec2* rotationPointer = &GameObjectRotation)
    //        {
    //            return rotationPointer;
    //        }
    //    }

    //    public vec2* GetScalePtr()
    //    {
    //        fixed (vec2* scalePointer = &GameObjectScale)
    //        {
    //            return scalePointer;
    //        }
    //    }

    //    public mat4* GetTransformMatrixPtr()
    //    {
    //        fixed (mat4* transformPointer = &GameObjectTransform)
    //        {
    //            return transformPointer;
    //        }
    //    }

    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct SceneDataBuffer
    //{
    //    public uint MeshBufferIndex;
    //    public ulong buffer;
    //    public mat4 Projection;
    //    public mat4 View;
    //    public vec3 CameraPosition;

    //    public SceneDataBuffer()
    //    {
    //        MeshBufferIndex = uint.MaxValue;
    //        Projection = new mat4();
    //        View = new mat4();
    //        CameraPosition = new vec3(0.0f);
    //        buffer = 0;
    //    }
    //};

    //[Custom(Value = -2500.0f)]
    //public unsafe class GameObjectComponent
    //{
    //    public IntPtr ParentGameObjectPtr = IntPtr.Zero;
    //    public NativeString Name { get; protected set; } = new NativeString();
    //    public ComponentTypeEnum ComponentType { get; set; }

    //    public mat4 GameObjectTransform;
    //    public vec2 GameObjectPosition;
    //    public vec2 GameObjectRotation;
    //    public vec2 GameObjectScale;

    //    public GameObjectComponent()
    //    {

    //    }

    //    public void Input(InputKey key, KeyState keyState) { }
    //    public void Update(float deltaTime) { }
    //    //public abstract void BufferUpdate(CommandBuffer commandBuffer, float deltaTime);
    //    //public abstract void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
    //    public void Destroy() { }
    //   // public int GetMemorySize() { }
    //}

    //[Custom(Value = -2500.0f)]
    //public unsafe class Transform2DComponent : GameObjectComponent
    //{
    //  //  private GameObject ParentGameObject { get; set; }

    //    //public mat4 GameObjectTransform;
    //    //public vec2 GameObjectPosition;
    //    //public vec2 GameObjectRotation;
    //    //public vec2 GameObjectScale;

    //    public Transform2DComponent()
    //    {
    //        ParentGameObjectPtr = IntPtr.Zero;
    //        Name = "GameObjectTransform2DComponent";

    //        GameObjectTransform = mat4.Identity;
    //        GameObjectPosition = new vec2(0.0f, 0.0f);
    //        GameObjectRotation = new vec2(0.0f, 0.0f);
    //        GameObjectScale = new vec2(1.0f, 1.0f);
    //    }

    //    public Transform2DComponent(IntPtr parentGameObject)
    //    {
    //        ParentGameObjectPtr = parentGameObject;

    //        GCHandle handle = GCHandle.FromIntPtr(ParentGameObjectPtr);
    //       // ParentGameObject = handle.Target as GameObject;

    //        Name = "GameObjectTransform2DComponent";

    //        GameObjectTransform = mat4.Identity;
    //        GameObjectPosition = new vec2(0.0f, 0.0f);
    //        GameObjectRotation = new vec2(0.0f, 0.0f);
    //        GameObjectScale = new vec2(1.0f, 1.0f);
    //    }

    //    public Transform2DComponent(IntPtr parentGameObject, String name)
    //    {
    //        ParentGameObjectPtr = parentGameObject;

    //        GCHandle handle = GCHandle.FromIntPtr(ParentGameObjectPtr);
    //       // ParentGameObject = handle.Target as GameObject;

    //        Name = name;

    //        GameObjectTransform = mat4.Identity;
    //        GameObjectPosition = new vec2(0.0f, 0.0f);
    //        GameObjectRotation = new vec2(0.0f, 0.0f);
    //        GameObjectScale = new vec2(1.0f, 1.0f);
    //    }

    //    public override void Input(InputKey key, KeyState keyState)
    //    {

    //    }

    //    public override void Update(float deltaTime)
    //    {
    //        GameObjectTransform = mat4.Identity;
    //        GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
    //        GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
    //        GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
    //        GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
    //    }

    //    //public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
    //    //{

    //    //}

    //    //public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
    //    //{

    //    //}

    //    public override void Destroy()
    //    {

    //    }

    //    public override int GetMemorySize()
    //    {
    //        return (int)sizeof(Transform2DComponent);
    //    }

    //}
}
