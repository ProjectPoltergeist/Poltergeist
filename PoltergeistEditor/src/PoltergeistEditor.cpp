#include <iostream>
#include <memory>
#include <glad/glad.h>
#include <GLFW/glfw3.h>

int main() {
	std::cout << "Hello editor!\n";

    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLFW_TRUE);
    glfwWindowHint(GLFW_RESIZABLE, GLFW_FALSE);

    {
        std::unique_ptr<GLFWwindow, decltype(glfwDestroyWindow)*> window {
                glfwCreateWindow(800, 600, "Poltergeist Editor", nullptr, nullptr),
                &glfwDestroyWindow
        };

        if (window.get() == nullptr)
        {
            glfwTerminate();
            return -1;
        }

        glfwMakeContextCurrent(window.get());

        if (!gladLoadGLLoader(reinterpret_cast<GLADloadproc>(glfwGetProcAddress)))
        {
            glfwTerminate();
            return -2;
        }

        while (!glfwWindowShouldClose(window.get()))
        {
            glfwPollEvents();
            glfwSwapBuffers(window.get());
        }
    }

    glfwTerminate();

	return 0;
}
