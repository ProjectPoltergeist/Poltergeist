#include "VertexBuffer.hpp"
#include "OpenGlUtilities.hpp"

VertexBuffer::VertexBuffer(const void* vertices, const size_t size, const VertexBufferLayout& layout) noexcept
{
	glGenBuffers(1, &m_vertexBufferId);
	Bind();
	glBufferData(GL_ARRAY_BUFFER, size, vertices, GL_STATIC_DRAW);

	size_t offset = 0;

	for (size_t i = 0; i < layout.GetElements().size(); i++)
	{
		auto& element = layout.GetElements()[i];

		glVertexAttribPointer(i, element.GetCount(), element.GetType(), GL_FALSE, layout.GetStride(), reinterpret_cast<void*>(offset));
		glEnableVertexAttribArray(i);

		offset += element.GetCount() * GetOpenGlTypeSize(element.GetType());
	}

	Unbind();
}

VertexBuffer::~VertexBuffer() noexcept
{
	glDeleteBuffers(1, &m_vertexBufferId);
}

void VertexBuffer::Bind() const noexcept
{
	glBindBuffer(GL_ARRAY_BUFFER, m_vertexBufferId);
}

void VertexBuffer::Unbind() const noexcept
{
	glBindBuffer(GL_ARRAY_BUFFER, m_vertexBufferId);
}
