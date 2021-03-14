#include "PoltergeistEngine/Rendering/Texture.hpp"
#include "PoltergeistEngine/Image/Image.hpp"
#include <glad/glad.h>

Texture::Texture(uint32_t textureId, uint8_t slot) noexcept
{
	m_textureId = textureId;
	m_slot = slot;
}

Texture::~Texture() noexcept
{
	glDeleteTextures(1, &m_textureId);
}

std::shared_ptr<Texture> Texture::CreateEmpty(size_t width, size_t height, uint8_t slot)
{
	uint32_t textureId;
	glGenTextures(1, &textureId);

	std::shared_ptr<Texture> texture(new Texture(textureId, slot));

	glBindTexture(GL_TEXTURE_2D, textureId);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, nullptr);

	texture->Unbind();

	return texture;
}

std::shared_ptr<Texture> Texture::CreateFromFile(const std::filesystem::path &texturePath, uint8_t slot)
{
	uint32_t textureId;
	glGenTextures(1, &textureId);

	std::shared_ptr<Texture> texture(new Texture(textureId, slot));

	glBindTexture(GL_TEXTURE_2D, textureId);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);

	Image image(texturePath);

	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, image.GetWidth(), image.GetHeight(), 0, GL_RGB, GL_UNSIGNED_BYTE, image.GetData());

	texture->Unbind();

	return texture;
}

void Texture::Bind() const noexcept
{
	glActiveTexture(GL_TEXTURE0 + m_slot);
	glBindTexture(GL_TEXTURE_2D, m_textureId);
}

void Texture::Unbind() const noexcept
{
	glBindTexture(GL_TEXTURE_2D, 0);
}

uint32_t Texture::GetId() const noexcept
{
	return m_textureId;
}

uint8_t Texture::GetSlot() const noexcept
{
	return m_slot;
}
