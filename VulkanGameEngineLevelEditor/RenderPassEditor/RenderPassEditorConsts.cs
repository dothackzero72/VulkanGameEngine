using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassEditor
{
    public class RenderPassEditorConsts
    {
        public const string RenderPassBasePath = $@"..\..\..\RenderPass\";
        public const string PipelineBasePath = $@"..\..\..\Pipelines\";

        public const string DefaultColorAttachmentDescriptionModel = $@"{RenderPassBasePath}AttachmentDescription\DefaultColorAttachment.json";
        public const string DefaultDepthAttachmentDescriptionModel = $@"{RenderPassBasePath}AttachmentDescription\DefaultDepthAttachment.json";
        public const string DefaultCreateColorImageInfo = $@"{RenderPassBasePath}CreateImageInfo\DefaultColorTextureCreateInfo.json";
        public const string DefaultCreateDepthImageInfo = $@"{RenderPassBasePath}CreateImageInfo\DefaultDepthTextureCreateInfo.json";
        public const string DefaultColorSamplerCreateInfo = $@"{RenderPassBasePath}CreateSamplerInfo\DefaultColorSampleCreateInfo.json";
        public const string DefaultDepthSamplerCreateInfo = $@"{RenderPassBasePath}CreateSamplerInfo\DefaultDepthSampleCreateInfo.json";
        public const string DefaultSubpassDependencyModel = $@"{RenderPassBasePath}SubPassDependencies\DefaultSubpassDependency.json";

        public const string AttachmentDescriptionModelPath = $@"{RenderPassBasePath}AttachmentDescription\";
        public const string CreateImageInfoPath = $@"{RenderPassBasePath}CreateImageInfo\";
        public const string SamplerCreateInfoPath = $@"{RenderPassBasePath}SamplerCreateInfo\";
        public const string SubpassDependencyModelPath = $@"{RenderPassBasePath}SamplerCreateInfo\";

        public const string Default2DRenderPass = $@"{RenderPassBasePath}Default2DRenderPass.json";
        public const string Default3DRenderPass = $@"{RenderPassBasePath}Default3DRenderPass.json";

        public const string Default2DPipeline = $@"{PipelineBasePath}Default2DPipeline.json";
        public const string Default3DPipeline = $@"{PipelineBasePath}Default3DPipeline.json";
    }
}
