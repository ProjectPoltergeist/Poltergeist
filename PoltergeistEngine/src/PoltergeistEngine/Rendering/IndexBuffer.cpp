#include "PoltergeistEngine/Rendering/IndexBuffer.hpp"

IndexBuffer::IndexBuffer(const uint32_t* indices, size_t count) noexcept
{
	glGenBuffers(1, &m_indexBufferId);
	Bind();
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint32_t) * count, indices, GL_STATIC_DRAW);
	Unbind();
}

IndexBuffer::~IndexBuffer() noexcept
{
	glDeleteBuffers(1, &m_indexBufferId);
}

void IndexBuffer::Bind() const noexcept
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, m_indexBufferId);
}

void IndexBuffer::Unbind() const noexcept
{
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
}
