//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static VulkanGameEngineLevelEditor.VulkanAPI;
//using static VulkanGameEngineLevelEditor.GameEngineAPI.VulkanRenderer;

//namespace VulkanGameEngineLevelEditor.GameEngineAPI
//{
//    public unsafe class TestRenderPass2 : Renderpass
//    {
//        private void CreateRenderPass()
//        {
//            var attachment = new VkAttachmentDescription
//            {
//                format = VkFormat.VK_FORMAT_B8G8R8A8_UNORM,
//                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
//                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
//                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
//                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
//                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
//                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
//                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
//            };

//            var colorAttachmentRef = new VkAttachmentReference
//            {
//                attachment = 1,
//                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
//            };

//            var subpass = new VkSubpassDescription
//            {
//                pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
//                colorAttachmentCount = 1,
//                pColorAttachments = &colorAttachmentRef,
//            };

//            var renderPassInfo = new VkRenderPassCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
//                attachmentCount = 1,
//                pAttachments = &attachment,
//                subpassCount = 1,
//                pSubpasses = &subpass,
//            };

//            VkRenderPass renderPass = new VkRenderPass();
//            vkCreateRenderPass(Device, &renderPassInfo, null, &renderPass);
//        }

//        private void CreateGraphicsPipeline()
//        {
//            var shaderStages = new List<VkPipelineShaderStageCreateInfo>()
//            {
//                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/vertex_shader.spv",  VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT),
//                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/fragment_shader.spv", VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT)
//            };

//            var vertexInputInfo = new VkPipelineVertexInputStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO
//            };

//            var inputAssembly = new VkPipelineInputAssemblyStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
//                topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
//            };

//            var viewport = new VkViewport
//            {
//                x = 0.0f,
//                y = 0.0f,
//                width = (float)VulkanRenderer.SwapChainResolution.Width, // Ensure this is not zero or negative
//                height = (float)VulkanRenderer.SwapChainResolution.Height, // Ensure this is not zero or negative
//                minDepth = 0.0f,
//                maxDepth = 1.0f
//            };

//            var scissor = new VkRect2D
//            {
//                Offset = new VkOffset2D
//                {
//                    X = 0,
//                    Y = 0
//                },
//                Extent = new VkExtent2D
//                {
//                    Width = VulkanRenderer.SwapChainResolution.Width, // Ensure this is correct
//                    Height = VulkanRenderer.SwapChainResolution.Height // Ensure this is correct
//                }
//            };

//            var viewportState = new VkPipelineViewportStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
//                viewportCount = 1,
//                pViewports = &viewport,
//                scissorCount = 1,
//                pScissors = &scissor,
//            };

//            var rasterizer = new VkPipelineRasterizationStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
//                depthClampEnable = VulkanConsts.VK_FALSE,
//                rasterizerDiscardEnable = VulkanConsts.VK_FALSE,
//                polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
//                cullMode = VkCullModeFlags.VK_CULL_MODE_NONE,
//                frontFace = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
//                depthBiasEnable = VulkanConsts.VK_FALSE,
//                lineWidth = 1.0f
//            };

//            var multisampling = new VkPipelineMultisampleStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
//                rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
//            };

//            var colorBlendAttachment = new VkPipelineColorBlendAttachmentState
//            {
//                blendEnable = VulkanConsts.VK_FALSE,
//                colorWriteMask = (VkColorComponentFlagBits)(
//                    VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT |
//                    VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT |
//                    VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT |
//                    VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT)
//            };

//            var colorBlending = new VkPipelineColorBlendStateCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
//                attachmentCount = 1,
//                pAttachments = &colorBlendAttachment,
//            };

//            var pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
//            };

//            VkPipelineLayout shaderPipelineLayout = new VkPipelineLayout();
//            vkCreatePipelineLayout(VulkanRenderer.Device, &pipelineLayoutInfo, null, &shaderPipelineLayout);

//            var pipelineInfo = new VkGraphicsPipelineCreateInfo
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
//                stageCount = 2,
//                //pStages = shaderStages,
//                pVertexInputState = &vertexInputInfo,
//                pInputAssemblyState = &inputAssembly,
//                pViewportState = &viewportState,
//                pRasterizationState = &rasterizer,
//                pMultisampleState = &multisampling,
//                pColorBlendState = &colorBlending,
//                layout = ShaderPipelineLayout,
//                renderPass = RenderPass,
//            };

//            VkPipeline pipeline;
//            VulkanAPI.vkCreateGraphicsPipelines(VulkanRenderer.Device, IntPtr.Zero, 1, &pipelineInfo, null, &pipeline);
//            ShaderPipeline = pipeline;
//        }
//    }
//}
