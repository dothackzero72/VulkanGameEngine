//#include "TextureDLL.h"
//
//Texture Texture_LoadTexture_DLL(const RendererStateCS& rendererStateCS, const char* jsonString)
//{
//	RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
//	return Texture_LoadTexture(renderState, jsonString);
//}
//
//Texture Texture_CreateTexture_DLL(const RendererStateCS& rendererStateCS, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo)
//{
//	RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
//	return Texture_CreateTexture(renderState, imageType, createImageInfo, samplerCreateInfo);
//}
//
//Texture Texture_CreateTexture_DLL(const RendererStateCS& rendererStateCS, const String& texturePath, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
//{
//	RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
//	return Texture_CreateTexture(renderState, texturePath, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
//}
//
//Texture Texture_CreateTexture_DLL(const RendererStateCS& rendererStateCS, Pixel& clearColor, VkImageAspectFlags imageType, VkImageCreateInfo& createImageInfo, VkSamplerCreateInfo& samplerCreateInfo, bool useMipMaps)
//{
//	RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
//	return Texture_CreateTexture(renderState, clearColor, imageType, createImageInfo, samplerCreateInfo, useMipMaps);
//}
//
//void Texture_UpdateTextureSize_DLL(const RendererStateCS& rendererStateCS, Texture& texture, VkImageAspectFlags imageType, vec2& TextureResolution)
//{
//	RendererState renderState = Renderer_RendererStateCStoCPP(rendererStateCS);
//}
//
//void Texture_UpdateTextureBufferIndex_DLL(Texture& texture, uint32 bufferIndex)
//{
//	Texture_UpdateTextureBufferIndex_DLL(texture, bufferIndex);
//}
//
//void Texture_GetTexturePropertiesBuffer_DLL(Texture& texture, Vector<VkDescriptorImageInfo>& textureDescriptorList)
//{
//	Texture_GetTexturePropertiesBuffer(texture, textureDescriptorList);
//}