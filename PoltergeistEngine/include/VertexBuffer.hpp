#ifndef POLTERGEIST_VERTEXBUFFER_HPP
#define POLTERGEIST_VERTEXBUFFER_HPP

#include <cstdint>
#include <glad/glad.h>
#include "VertexBufferLayout.hpp"
#include "VertexBufferLayoutElement.hpp"

class VertexBuffer
{
private:
    uint32_t m_vertexBufferId;
public:
    VertexBuffer(const void* vertices, const size_t size, const VertexBufferLayout& layout) noexcept;
    ~VertexBuffer() noexcept;

    void Bind() const noexcept;
    void Unbind() const noexcept;
};

#endif
