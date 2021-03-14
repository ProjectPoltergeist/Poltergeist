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
public:
	FrameBuffer(size_t width, size_t height);
	~FrameBuffer() noexcept;

	void Bind() const noexcept;
	void Unbind() const noexcept;

	std::shared_ptr<Texture> GetTextureAttachment() const noexcept;
};

#endif
