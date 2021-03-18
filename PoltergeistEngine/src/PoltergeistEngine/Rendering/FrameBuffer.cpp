#include <stdexcept>
#include "PoltergeistEngine/Rendering/FrameBuffer.hpp"

FrameBuffer::FrameBuffer(uint32_t width, uint32_t height)
{
	m_width = width;
	m_height = height;

	glGenFramebuffers(1, &m_frameBufferId);

	Bind();

	m_textureAttachment = Texture::CreateEmpty(width, height, 1);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, m_textureAttachment->GetId(), 0);

	glGenRenderbuffers(1, &m_depthAndStencilAttachmentId);
	glBindRenderbuffer(GL_RENDERBUFFER, m_depthAndStencilAttachmentId);
	glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH24_STENCIL8, width, height);
	glBindRenderbuffer(GL_RENDERBUFFER, 0);

	glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT, GL_RENDERBUFFER, m_depthAndStencilAttachmentId);

	if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE) {
		throw std::runtime_error("Failed to create a frame buffer");
	}

	Unbind();
}

FrameBuffer::~FrameBuffer() noexcept
{
	glDeleteFramebuffers(1, &m_frameBufferId);
}

void FrameBuffer::Bind() const noexcept
{
	glBindFramebuffer(GL_FRAMEBUFFER, m_frameBufferId);
	glViewport(0, 0, m_width, m_height);
}

void FrameBuffer::Unbind() const noexcept
{
	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

std::shared_ptr<Texture> FrameBuffer::GetTextureAttachment() const noexcept
{
	return m_textureAttachment;
}
