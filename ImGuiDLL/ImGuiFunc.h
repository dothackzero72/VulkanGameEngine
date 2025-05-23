//#pragma once
//#include <vulkan/vulkan_core.h>
//#include <glfw/include/GLFW/glfw3.h>
//#include <ImGui/backends/imgui_impl_glfw.h>
//#include <ImGui/imgui.h>
//#include <imgui/backends/imgui_impl_vulkan.h>
//#include <vector>
//
//#include "DLL.h"
//
//
//struct ImGuiRenderer
//{
//	VkRenderPass RenderPass = VK_NULL_HANDLE;
//	VkDescriptorPool ImGuiDescriptorPool = VK_NULL_HANDLE;
//	VkCommandBuffer ImGuiCommandBuffer = VK_NULL_HANDLE;
//	std::vector<VkFramebuffer> SwapChainFramebuffers;
//};
//
//DLL_EXPORT ImGuiRenderer ImGui_StartUp(const RendererState& rendererState);
//DLL_EXPORT void ImGui_StartFrame();
//DLL_EXPORT void ImGui_EndFrame();
//DLL_EXPORT VkCommandBuffer ImGui_Draw(const RendererState& rendererState, ImGuiRenderer& imGui);
//
//VkRenderPass ImGui_CreateRenderPass(const RendererState& rendererState);
//std::vector<VkFramebuffer> ImGui_CreateRendererFramebuffers(const RendererState& rendererState, const VkRenderPass& renderPass);
//void ImGui_VkResult(VkResult err);
