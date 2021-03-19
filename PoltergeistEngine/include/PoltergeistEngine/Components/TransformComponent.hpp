#ifndef POLTERGEIST_TRANSFORMCOMPONENT_HPP
#define POLTERGEIST_TRANSFORMCOMPONENT_HPP

#include <glm/glm.hpp>

class TransformComponent
{
public:
	glm::vec2 m_position;
	float m_rotation;
	glm::vec2 m_scale;

	TransformComponent(glm::vec2 position, float rotation, glm::vec2 scale) noexcept
	{
		m_position = position;
		m_rotation = rotation;
		m_scale = scale;
	}
};

#endif
