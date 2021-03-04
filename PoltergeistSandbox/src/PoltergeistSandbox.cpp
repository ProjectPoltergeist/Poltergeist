#include <iostream>
#include <memory>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <Shader.hpp>
#include <VertexArray.hpp>
#include <VertexBuffer.hpp>
#include <VertexBufferLayout.hpp>
#include <IndexBuffer.hpp>
#include <Texture.hpp>

int main() {
    std::cout << "Hello sandbox!\n";

    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLFW_TRUE);
    glfwWindowHint(GLFW_RESIZABLE, GLFW_FALSE);

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

        if (!gladLoadGLLoader(reinterpret_cast<GLADloadproc>(glfwGetProcAddress)))
        {
            glfwTerminate();
            return -2;
        }

        auto shader = Shader::Create("core.vert", "core.frag");

        if (!shader)
        {
            glfwTerminate();
            return -3;
        }

        shader->Bind();

        VertexArray vertexArray;
        vertexArray.Bind();

        float vertices[] = {
                 0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
                 0.5f, -0.5f, 0.0f, 1.0f, 1.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
                -0.5f,  0.5f, 0.0f, 0.0f, 0.0f
        };

        VertexBufferLayout layout;
        layout.AddElement<float>(3);
        layout.AddElement<float>(2);

        VertexBuffer vertexBuffer(vertices, sizeof(vertices), layout);

        uint32_t indices[] = {
                0, 1, 3,
                1, 2, 3
        };

        IndexBuffer indexBuffer(indices, 6);
        indexBuffer.Bind();

        Texture texture("texture.png", 0);
        texture.Bind();
        shader->SetUniform("u_Texture", 0);

        while (!glfwWindowShouldClose(window.get()))
        {
            glfwPollEvents();
            glClear(GL_COLOR_BUFFER_BIT);
            glClearColor(0.3f, 0.3f, 0.3f, 1.0f);
            glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr);
            glfwSwapBuffers(window.get());
        }

        texture.Unbind();
        indexBuffer.Unbind();
        vertexArray.Unbind();
        shader->Unbind();
    }

    glfwTerminate();

    return 0;
}
