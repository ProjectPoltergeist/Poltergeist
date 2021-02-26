#include <iostream>
#include <memory>
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <VertexArray.hpp>
#include <IndexBuffer.hpp>

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

        if (window.get() == nullptr) {
            glfwTerminate();
            return -1;
        }

        glfwMakeContextCurrent(window.get());

        if (!gladLoadGLLoader(reinterpret_cast<GLADloadproc>(glfwGetProcAddress))) {
            glfwTerminate();
            return -2;
        }

        VertexArray vertexArray;
        vertexArray.Bind();

        float vertices[] = {
                 0.5f,  0.5f, 0.0f,
                 0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f
        };

        uint32_t vertexBufferId;
        glGenBuffers(1, &vertexBufferId);
        glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId);
        glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), &vertices, GL_STATIC_DRAW);
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), nullptr);
        glEnableVertexAttribArray(0);
        glBindBuffer(GL_ARRAY_BUFFER, 0);

        uint32_t indices[] = {
                0, 1, 3,
                1, 2, 3
        };

        IndexBuffer indexBuffer(indices, 6);
        indexBuffer.Bind();

        while (!glfwWindowShouldClose(window.get())) {
            glfwPollEvents();
            glClear(GL_COLOR_BUFFER_BIT);
            glClearColor(0.3f, 0.3f, 0.3f, 1.0f);
            glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, nullptr);
            glfwSwapBuffers(window.get());
        }

        indexBuffer.Unbind();
        vertexArray.Unbind();

        glDeleteBuffers(1, &vertexBufferId);
    }

    glfwTerminate();

    return 0;
}
