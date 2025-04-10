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
        public static string AssetBasePass => ConfigurationManager.AppSettings["AssetBasePath"];
        public static string MaterialBasePath => ConfigurationManager.AppSettings["MaterialBasePath"];
        public static string TextureBasePath => ConfigurationManager.AppSettings["TextureBasePath"];
        public static string LevelBasePath => ConfigurationManager.AppSettings["LevelBasePath"];
        public static string RenderPassBasePath => ConfigurationManager.AppSettings["RenderPassBasePath"];
        public static string PipelineBasePath => ConfigurationManager.AppSettings["PipelineBasePath"];
        public static string GameObjectComponentDLLPath => ConfigurationManager.AppSettings["GameObjectComponentDLLPath"];

        public static string DefaultColorAttachmentDescriptionModel =>
            $"AttachmentDescription\\DefaultColorAttachment.json";

        public static string DefaultDepthAttachmentDescriptionModel =>
            $"AttachmentDescription\\DefaultDepthAttachment.json";

        public static string DefaultCreateColorImageInfo =>
            $"CreateImageInfo\\DefaultColorTextureCreateInfo.json";

        public static string DefaultCreateDepthImageInfo =>
            $"CreateImageInfo\\DefaultDepthTextureCreateInfo.json";

        public static string DefaultColorSamplerCreateInfo =>
            $"CreateSamplerInfo\\DefaultColorSampleCreateInfo.json";

        public static string DefaultDepthSamplerCreateInfo =>
            $"CreateSamplerInfo\\DefaultDepthSampleCreateInfo.json";

        public static string DefaultSubpassDependencyModel =>
            $"SubPassDependencies\\DefaultSubpassDependency.json";

        public static string AttachmentDescriptionModelPath =>
            $"AttachmentDescription\\";

        public static string CreateImageInfoPath =>
            $"CreateImageInfo\\";

        public static string SamplerCreateInfoPath =>
            $"SamplerCreateInfo\\";

        public static string SubpassDependencyModelPath =>
            $"SamplerCreateInfo\\";

        public static string Default2DRenderPass =>
            $"Default2DRenderPass.json";

        public static string FrameBufferRenderPass =>
    $"Default2DRenderPass.json";

        public static string Default3DRenderPass =>
            $"Default3DRenderPass.json";

        public static string Default2DPipeline =>
            $"Default2DPipeline.json";
        public static string FrameBufferPipeline =>
    $"FrameBufferPipeline.json";
        public static string Default3DPipeline =>
            $"Default3DPipeline.json";
    }
}
