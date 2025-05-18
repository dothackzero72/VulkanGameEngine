#pragma once
#include <../External/glfw/include/GLFW/glfw3.h>
#include <ImGui/backends/imgui_impl_glfw.h>
#include <ImGui/imgui.h>
#include <imgui/backends/imgui_impl_vulkan.h>

#include "DLL.h"
#include "CTypedef.h"
#include "VulkanWindow.h"
#include "CoreVulkanRenderer.h"


	void ImGui_StartUp(const RendererState& rendererState, VkDescriptorPool& ImGuiDescriptorPool, VkCommandBuffer& ImGuiCommandBuffer);
	VkRenderPass ImGui_CreateRenderPass(const RendererState& rendererState);
	void ImGui_CreateRendererFramebuffers(const RendererState& rendererState, const VkRenderPass& renderPass, Vector<VkFramebuffer>& frameBuffers);
	void ImGui_VkResult(VkResult err);


