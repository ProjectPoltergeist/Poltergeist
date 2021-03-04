#include "Texture.hpp"
#include "Image.hpp"
#include <glad/glad.h>

Texture::Texture(const std::filesystem::path &texturePath, uint8_t slot)
{
    m_slot = slot;

    glGenTextures(1, &m_textureId);
    glBindTexture(GL_TEXTURE_2D, m_textureId);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);

    Image image(texturePath);

    glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, image.GetWidth(), image.GetHeight(), 0, GL_RGB, GL_UNSIGNED_BYTE, image.GetData());

    Unbind();
}

Texture::~Texture() noexcept
{
    glDeleteTextures(1, &m_textureId);
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
