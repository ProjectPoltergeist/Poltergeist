#ifndef POLTERGEIST_FRAMEBUFFER_HPP
#define POLTERGEIST_FRAMEBUFFER_HPP

#include <cstddef>
#include <glad/glad.h>
#include "PoltergeistEngine/Rendering/Texture.hpp"

class FrameBuffer
{
private:
	uint32_t m_frameBufferId;
	std::shared_ptr<Texture> m_textureAttachment;
	uint32_t m_depthAndStencilAttachmentId;
	uint32_t m_width;
	uint32_t m_height;
public:
	FrameBuffer(uint32_t width, uint32_t height);
	~FrameBuffer() noexcept;

	void Bind() const noexcept;
	void Unbind() const noexcept;

	std::shared_ptr<Texture> GetTextureAttachment() const noexcept;
};

#endif
