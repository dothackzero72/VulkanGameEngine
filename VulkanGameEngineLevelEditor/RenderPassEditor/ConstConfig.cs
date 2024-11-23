using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.RenderPassEditor
{
    public static class ConstConfig
    {
        public static string RenderPassBasePath => ConfigurationManager.AppSettings["RenderPassBasePath"];
        public static string PipelineBasePath => ConfigurationManager.AppSettings["PipelineBasePath"];
        public static string GameObjectComponentDLLPath => ConfigurationManager.AppSettings["GameObjectComponentDLLPath"];

        public static string DefaultColorAttachmentDescriptionModel =>
            $"{RenderPassBasePath}AttachmentDescription\\DefaultColorAttachment.json";

        public static string DefaultDepthAttachmentDescriptionModel =>
            $"{RenderPassBasePath}AttachmentDescription\\DefaultDepthAttachment.json";

        public static string DefaultCreateColorImageInfo =>
            $"{RenderPassBasePath}CreateImageInfo\\DefaultColorTextureCreateInfo.json";

        public static string DefaultCreateDepthImageInfo =>
            $"{RenderPassBasePath}CreateImageInfo\\DefaultDepthTextureCreateInfo.json";

        public static string DefaultColorSamplerCreateInfo =>
            $"{RenderPassBasePath}CreateSamplerInfo\\DefaultColorSampleCreateInfo.json";

        public static string DefaultDepthSamplerCreateInfo =>
            $"{RenderPassBasePath}CreateSamplerInfo\\DefaultDepthSampleCreateInfo.json";

        public static string DefaultSubpassDependencyModel =>
            $"{RenderPassBasePath}SubPassDependencies\\DefaultSubpassDependency.json";

        public static string AttachmentDescriptionModelPath =>
            $"{RenderPassBasePath}AttachmentDescription\\";

        public static string CreateImageInfoPath =>
            $"{RenderPassBasePath}CreateImageInfo\\";

        public static string SamplerCreateInfoPath =>
            $"{RenderPassBasePath}SamplerCreateInfo\\";

        public static string SubpassDependencyModelPath =>
            $"{RenderPassBasePath}SamplerCreateInfo\\";

        public static string Default2DRenderPass =>
            $"{RenderPassBasePath}Default2DRenderPass.json";

        public static string Default3DRenderPass =>
            $"{RenderPassBasePath}Default3DRenderPass.json";

        public static string Default2DPipeline =>
            $"{PipelineBasePath}Default2DPipeline.json";

        public static string Default3DPipeline =>
            $"{PipelineBasePath}Default3DPipeline.json";
    }
}
