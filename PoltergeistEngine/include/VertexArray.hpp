#ifndef POLTERGEIST_VERTEXARRAY_HPP
#define POLTERGEIST_VERTEXARRAY_HPP

#include <cstdint>
#include <glad/glad.h>

class VertexArray
{
private:
    uint32_t m_vertexArrayId;
public:
    VertexArray() noexcept;
    ~VertexArray() noexcept;

    void Bind() const noexcept;
    void Unbind() const noexcept;
};

#endif
