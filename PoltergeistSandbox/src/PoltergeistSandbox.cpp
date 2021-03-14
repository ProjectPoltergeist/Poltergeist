#include <iostream>
#include <memory>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <PoltergeistEngine/Rendering/Texture.hpp>
#include <PoltergeistEngine/Rendering/Renderer.hpp>
#ifdef WIN32
#include <Windows.h>
#include <PoltergeistEngine/Rendering/FrameBuffer.hpp>

#endif

extern "C"
{
	__declspec(dllexport) uint32_t NvOptimusEnablement = 1;
	__declspec(dllexport) int32_t AmdPowerXpressRequestHighPerformance = 1;
}

void OnWindowSizeUpdate(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

int main()
{
#ifdef WIN32
	SetConsoleOutputCP(CP_UTF8);
	setvbuf(stdout, nullptr, _IOFBF, 1024);
#endif

	std::cout << "Hello sandbox!" << std::endl;

	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLFW_TRUE);

	{
		std::unique_ptr<GLFWwindow, decltype(glfwDestroyWindow)*> window {
				glfwCreateWindow(800, 600, "Poltergeist Sandbox", nullptr, nullptr),
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

		std::shared_ptr<Texture> texture = Texture::CreateFromFile("texture.png", 1);
		std::shared_ptr<Renderer> renderer = Renderer::Create();

		FrameBuffer frameBuffer(800, 600);

		while (!glfwWindowShouldClose(window.get()))
		{
			glfwPollEvents();

			renderer->BeginRenderPass(frameBuffer);

			renderer->Clear(glm::vec3(0.3f, 0.3f, 0.3f));
			renderer->DrawQuad(glm::vec2(-0.70f, 0.0f), 0.0f, glm::vec2(0.25f, 0.25f), glm::vec3(1.0, 0.0, 0.0));
			renderer->DrawQuad(glm::vec2(0.25f, 0.25f), 45.0f, glm::vec2(0.50f, 0.50f), texture);

			renderer->EndRenderPass();

			renderer->BeginRenderPass();

			renderer->Clear(glm::vec3(0.3f, 0.3f, 0.3f));
			renderer->DrawQuad(glm::vec2(0.0f, 0.0f), 0.0f, glm::vec2(0.5f, 0.5f), frameBuffer.GetTextureAttachment());

			renderer->EndRenderPass();

			glfwSwapBuffers(window.get());
		}
	}

	glfwTerminate();

	return 0;
}
