//using Moq;
//using NUnit.Framework;
//using System;
//using System.Threading;
//using System.Windows.Forms
//using VulkanGameEngineLevelEditor;
//using VulkanGameEngineLevelEditor.GameEngineAPI;

//namespace LevelEditorUnitTests
//{
//    [TestFixture]
//    public class DllTests
//    {
//        VulkanRenderer renderer;
//        private VkInstance instance;
//        private VkPhysicalDevice physicalDevice;
//        private VkSurfaceKHR surface;
//        private VkPhysicalDeviceFeatures physicalDeviceFeatures;
//        private uint graphicsFamily;
//        private uint presentFamily;
//        [SetUp]
//        public void Setup()
//        {
//            mockPictureBox.Setup(m => m.Handle).Returns(new IntPtr(123456));
//            renderer.SetUpRenderer()
//        }

//        [Test]
//        public void Test_CreateVulkanInstance()
//        {
//            Assert.IsNotNull(GameEngineDLL.DLL_Renderer_CreateVulkanInstance());
//        }

//        [Test]
//        public void Test_SetUpPhysicalDevice()
//        {
//            VkResult result = GameEngineDLL.DLL_Renderer_SetUpPhysicalDevice(instance, ref physicalDevice, surface, ref physicalDeviceFeatures, out graphicsFamily, out presentFamily);
//            Assert.AreEqual(VkResult.VK_SUCCESS, result);
//            Assert.IsNotNull(physicalDevice);
//        }

//        [Test]
//        public void Test_SetUpDevice()
//        {
//            VkDevice device = GameEngineDLL.DLL_Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
//            Assert.IsNotNull(device);
//        }

//        [Test]
//        public void Test_SetUpCommandPool()
//        {
//            VkDevice device = GameEngineDLL.DLL_Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
//            VkCommandPool commandPool = GameEngineDLL.DLL_Renderer_SetUpCommandPool(device, graphicsFamily);
//            Assert.IsNotNull(commandPool);
//        }

//        [Test]
//        public void Test_GetDeviceQueue()
//        {
//            VkDevice device = GameEngineDLL.DLL_Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
//            VkQueue graphicsQueue, presentQueue;
//            VkResult result = GameEngineDLL.DLL_Renderer_GetDeviceQueue(device, graphicsFamily, presentFamily, out graphicsQueue, out presentQueue);
//            Assert.AreEqual(VkResult.VK_SUCCESS, result);
//            Assert.IsNotNull(graphicsQueue);
//            Assert.IsNotNull(presentQueue);
//        }

//        [Test]
//        public void Test_SetUpSemaphores()
//        {
//            VkDevice device = GameEngineDLL.DLL_Renderer_SetUpDevice(physicalDevice, graphicsFamily, presentFamily);
//            VkFence[] inFlightFences;
//            VkSemaphore[] acquireImageSemaphoreList;
//            VkSemaphore[] presentImageSemaphoreList;
//            VkResult result = GameEngineDLL.DLL_Renderer_SetUpSemaphores(device, out inFlightFences, out acquireImageSemaphoreList, out presentImageSemaphoreList, 2);
//            Assert.AreEqual(VkResult.VK_SUCCESS, result);
//            Assert.IsNotNull(inFlightFences);
//            Assert.IsNotNull(acquireImageSemaphoreList);
//            Assert.IsNotNull(presentImageSemaphoreList);
//        }
//    }
//}