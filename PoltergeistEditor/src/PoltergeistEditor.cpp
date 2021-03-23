#include <iostream>
#include <memory>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <imgui.h>
#include <imgui_impl_glfw.h>
#include <imgui_impl_opengl3.h>
#include <PoltergeistEngine/Components/TagComponent.hpp>
#include <PoltergeistEngine/Components/TransformComponent.hpp>
#include <PoltergeistEngine/Rendering/Texture.hpp>
#include <PoltergeistEngine/Rendering/FrameBuffer.hpp>
#include <PoltergeistEngine/Rendering/Renderer.hpp>
#include <PoltergeistEngine/GameObject.hpp>
#include <PoltergeistEngine/Macros.hpp>
#include <PoltergeistEngine/Scene.hpp>
#ifdef WIN32
#include <Windows.h>
#endif

extern "C"
{
	POLTERGEIST_PUBLIC uint32_t NvOptimusEnablement = 1;
	POLTERGEIST_PUBLIC int32_t AmdPowerXpressRequestHighPerformance = 1;
}

void OnWindowSizeUpdate(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

class Editor
{

};

int main()
{
#ifdef WIN32
	SetConsoleOutputCP(CP_UTF8);
	setvbuf(stdout, nullptr, _IOFBF, 1024);
#endif

	std::cout << "Hello editor!" << std::endl;

	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLFW_TRUE);

	{
		std::unique_ptr<GLFWwindow, decltype(glfwDestroyWindow)*> window {
				glfwCreateWindow(1280, 720, "Poltergeist Editor", nullptr, nullptr),
				&glfwDestroyWindow
		};

		if (window.get() == nullptr)
		{
			glfwTerminate();
			return -1;
		}

		glfwMakeContextCurrent(window.get());
		glfwSetWindowSizeCallback(window.get(), OnWindowSizeUpdate);

		if (!gladLoadGLLoader(reinterpret_cast<GLADloadproc>(glfwGetProcAddress)))
		{
			glfwTerminate();
			return -2;
		}

		IMGUI_CHECKVERSION();

		ImGui::CreateContext();
		ImGui::StyleColorsDark();
		ImGui_ImplGlfw_InitForOpenGL(window.get(), true);
		ImGui_ImplOpenGL3_Init();

		std::shared_ptr<Texture> texture = Texture::CreateFromFile("texture.png", 1);
		std::shared_ptr<Renderer> renderer = Renderer::Create();

		FrameBuffer frameBuffer(800, 600);

		Scene defaultScene;

		GameObject& gameObject1 = defaultScene.CreateGameObject();
		gameObject1.AddComponent<TagComponent>("First square");
		gameObject1.AddComponent<TransformComponent>(glm::vec2(0.0f, 0.0f), 0.0f, glm::vec2(0.25f, 0.25f));

		GameObject& gameObject2 = defaultScene.CreateGameObject();
		gameObject2.AddComponent<TagComponent>("Second square");
		gameObject2.AddComponent<TransformComponent>(glm::vec2(-1.0f, -1.0f), 0.0f, glm::vec2(0.5f, 0.5f));

		while (!glfwWindowShouldClose(window.get()))
		{
			glfwPollEvents();
			
			ImGui_ImplOpenGL3_NewFrame();
			ImGui_ImplGlfw_NewFrame();
			ImGui::NewFrame();

			ImGui::Begin("Scene Hierarchy", nullptr, ImGuiWindowFlags_NoCollapse);

			size_t currentNodeIndex = 0;

			for (const GameObject& gameObject : defaultScene.GetGameObjects())
			{
				const char* nodeName = gameObject.HasComponent<TagComponent>()
				        ? gameObject.GetComponent<TagComponent>().m_tag.c_str()
				        : "Object";

				bool open = ImGui::TreeNodeEx(reinterpret_cast<void*>(currentNodeIndex), ImGuiTreeNodeFlags_OpenOnArrow, nodeName);

				if (open)
				{
					ImGui::TreePop();
				}

				currentNodeIndex++;
			}

			ImGui::End();

			ImGui::PushStyleVar(ImGuiStyleVar_WindowPadding, ImVec2{ 0, 0 });
			ImGui::Begin("Scene", nullptr, ImGuiWindowFlags_AlwaysAutoResize | ImGuiWindowFlags_NoCollapse);

			renderer->BeginRenderPass(frameBuffer);
			renderer->Clear(glm::vec3(1.0f, 1.0f, 1.0f));

			for (GameObject& gameObject : defaultScene.GetGameObjects())
			{
				if (gameObject.HasComponent<TransformComponent>())
				{
					TransformComponent& transform = gameObject.GetComponent<TransformComponent>();

					renderer->DrawQuad(transform.m_position, transform.m_rotation, transform.m_scale, glm::vec3(1.0, 0.0, 0.0));
				}
			}

			renderer->EndRenderPass();

			ImGui::Image(reinterpret_cast<void*>(frameBuffer.GetTextureAttachment()->GetId()), ImVec2(800, 600), ImVec2(0, 1), ImVec2(1, 0));

			ImGui::End();
			ImGui::PopStyleVar();

			ImGui::Render();

			ImVec4 backgroundColor(1.0f, 1.0f, 1.0f, 1.0f);
			glClearColor(backgroundColor.x, backgroundColor.y, backgroundColor.z, backgroundColor.w);
			glClear(GL_COLOR_BUFFER_BIT);

			ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());

			glfwSwapBuffers(window.get());
		}
	}

	ImGui_ImplOpenGL3_Shutdown();
	ImGui_ImplGlfw_Shutdown();
	ImGui::DestroyContext();

	glfwTerminate();

	return 0;
}
