#ifndef POLTERGEIST_VERTEX_HPP
#define POLTERGEIST_VERTEX_HPP

#include <glm/glm.hpp>

struct Vertex
{
    glm::vec2 m_position;
    glm::vec4 m_color;
    glm::vec2 m_textureCoordinate;

    Vertex(glm::vec2 position, glm::vec4 color, glm::vec2 textureCoordinate) noexcept
    {
        m_position = position;
        m_color = color;
        m_textureCoordinate = textureCoordinate;
    }
};

#endif
