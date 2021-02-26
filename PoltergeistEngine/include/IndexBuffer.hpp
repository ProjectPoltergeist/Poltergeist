#ifndef POLTERGEIST_INDEXBUFFER_HPP
#define POLTERGEIST_INDEXBUFFER_HPP

#include <cstdint>
#include <glad/glad.h>

class IndexBuffer
{
private:
    uint32_t m_indexBufferId;
public:
    IndexBuffer(const uint32_t* indices, size_t count) noexcept;
    ~IndexBuffer() noexcept;

    void Bind() const noexcept;
    void Unbind() const noexcept;
};

#endif
