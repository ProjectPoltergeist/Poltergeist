#ifndef POLTERGEIST_VERTEXBUFFERLAYOUT_HPP
#define POLTERGEIST_VERTEXBUFFERLAYOUT_HPP

#include <vector>
#include "VertexBufferLayoutElement.hpp"
#include "OpenGlUtilities.hpp"

class VertexBufferLayout
{
private:
	std::vector<VertexBufferLayoutElement> m_elements;
	size_t m_stride = 0;
public:
	template<typename T>
	void AddElement(size_t count) noexcept
	{
		m_elements.emplace_back(GetOpenGlType<T>(), count);
		m_stride += sizeof(T) * count;
	}

	[[nodiscard]] const std::vector<VertexBufferLayoutElement>& GetElements() const noexcept
	{
		return m_elements;
	}

	[[nodiscard]] const size_t GetStride() const noexcept
	{
		return m_stride;
	}
};

#endif
