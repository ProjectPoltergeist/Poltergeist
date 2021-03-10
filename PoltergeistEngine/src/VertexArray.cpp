#include "VertexArray.hpp"

VertexArray::VertexArray() noexcept
{
    glGenVertexArrays(1, &m_vertexArrayId);
}

VertexArray::~VertexArray() noexcept
{
    glDeleteVertexArrays(1, &m_vertexArrayId);
}

void VertexArray::Bind() const noexcept
{
    glBindVertexArray(m_vertexArrayId);
}

void VertexArray::Unbind() const noexcept
{
    glBindVertexArray(0);
}
