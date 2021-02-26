#ifndef POLTERGEIST_VERTEXBUFFERLAYOUTELEMENT_HPP
#define POLTERGEIST_VERTEXBUFFERLAYOUTELEMENT_HPP

#include <cstdint>

class VertexBufferLayoutElement
{
private:
    int32_t m_type;
    size_t m_count;
public:
    VertexBufferLayoutElement(int32_t type, size_t count) : m_type(type), m_count(count) {}

    [[nodiscard]] int32_t GetType() const noexcept
    {
        return m_type;
    }

    [[nodiscard]] size_t GetCount() const noexcept
    {
        return m_count;
    }
};

#endif
