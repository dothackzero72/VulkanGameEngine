using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
