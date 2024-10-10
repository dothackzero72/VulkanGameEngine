using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class BuildRenderPass : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public Device _device {  get; private set; }

        public BuildRenderPass()
        {
        }

        public void CreateRenderPass(RenderPassModel model)
        {
            List<AttachmentDescription> AttachmentList = new List<AttachmentDescription>();
            List<RenderedTextureInfoModel> colorAttachmentList = new List<RenderedTextureInfoModel>();
            List<RenderedTextureInfoModel> depthAttachmentList = new List<RenderedTextureInfoModel>();
            List<RenderedTextureInfoModel> inputAttachmentList = new List<RenderedTextureInfoModel>();
            List<RenderedTextureInfoModel> resolveAttachmentList = new List<RenderedTextureInfoModel>();

            foreach (var attachment in model.AttachmentList)
            {
                AttachmentList.Add(attachment.AttachmentDescription);
                switch (attachment.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture: colorAttachmentList.Add(attachment); break;
                    case RenderedTextureType.DepthRenderedTexture: depthAttachmentList.Add(attachment); break;
                    case RenderedTextureType.InputAttachmentTexture: inputAttachmentList.Add(attachment); break;
                    case RenderedTextureType.ResolveAttachmentTexture: resolveAttachmentList.Add(attachment); break;
                    default:
                        {
                            MessageBox.Show("Something went wrong building render pass: Attachment Problems.");
                            break;
                        }
                }
            }

            List<AttachmentReference> inputAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> colorAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> resolveAttachmentReferenceList = new List<AttachmentReference>();
            List<SubpassDescription> preserveAttachmentReferenceList = new List<SubpassDescription>();
            AttachmentReference depthAttachementReference = new AttachmentReference();
            for (int x = 0; x < colorAttachmentList.Count; x++)
            {
                colorAttachmentReferenceList.Add(new AttachmentReference
                {
                    Attachment = colorAttachmentList.UCount(),
                    Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                });
                resolveAttachmentReferenceList.Add(new AttachmentReference
                {
                    Attachment = (uint)(colorAttachmentList.UCount() + 1),
                    Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                });
            }
            for (int x = 0; x < inputAttachmentList.Count; x++)
            {
                inputAttachmentReferenceList.Add(new AttachmentReference
                {
                    Attachment = (uint)(x),
                    Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                });
            }
            if (depthAttachmentList.Any())
            {
                depthAttachementReference = new AttachmentReference
                {
                    Attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                    Layout = Silk.NET.Vulkan.ImageLayout.DepthAttachmentOptimal
                };
            }

            SubpassDescription subpassDescription = new SubpassDescription();
            var depthAttachment = &depthAttachementReference;
            fixed (AttachmentReference* colorAttachments = colorAttachmentReferenceList.ToArray())
            fixed (AttachmentReference* inputAttachments = inputAttachmentReferenceList.ToArray())
            fixed (AttachmentReference* resolveAttachments = resolveAttachmentReferenceList.ToArray())
            fixed (SubpassDescription* preserveAttachments = preserveAttachmentReferenceList.ToArray())
            {
                subpassDescription = new SubpassDescription
                {
                    PipelineBindPoint = PipelineBindPoint.Graphics,
                    ColorAttachmentCount = colorAttachmentList.UCount(),
                    PColorAttachments = colorAttachments,
                    PDepthStencilAttachment = depthAttachment,
                    PResolveAttachments = resolveAttachments,
                    InputAttachmentCount = inputAttachmentList.UCount(),
                    PInputAttachments = inputAttachments,
                    PreserveAttachmentCount = preserveAttachmentReferenceList.UCount(),
                    Flags = SubpassDescriptionFlags.None,
                    PPreserveAttachments = null
                };
            }

            List<SubpassDependency> subPassList = new List<SubpassDependency>();
            foreach (SubpassDependencyModel subpass in model.SubpassDependencyList )
            {
                //subPassList.Add(subpass.);
            }

            fixed (AttachmentDescription* attchments = AttachmentList.ToArray())
            fixed (SubpassDependency* subpasses = subPassList.ToArray())
            {
                RenderPassCreateInfo renderPassInfo = new RenderPassCreateInfo
                {
                    SType = StructureType.RenderPassCreateInfo,
                    AttachmentCount = AttachmentList.UCount(),
                    PAttachments = attchments,
                    SubpassCount = subPassList.UCount(),
                    PSubpasses = &subpassDescription,
                    DependencyCount = model.SubpassDependencyList.UCount(),
                    PDependencies = subpasses,
                    Flags = RenderPassCreateFlags.None,
                    PNext = null
                };

                var tempRenderPass = renderPass;
                vk.CreateRenderPass(_device, &renderPassInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }
        }
    }
}
