using AutoMapper;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VkSamplerCreateInfoModel, VkSamplerCreateInfoDLL>();
            CreateMap<VkSamplerCreateInfoDLL, VkSamplerCreateInfoModel>();
        }
    }
}
