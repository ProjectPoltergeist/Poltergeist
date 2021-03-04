#ifndef POLTERGEIST_TEXTURE_HPP
#define POLTERGEIST_TEXTURE_HPP

#include <cstdint>
#include <filesystem>

class Texture
{
private:
    uint32_t m_textureId;
    uint8_t m_slot;
public:
    Texture(const std::filesystem::path& texturePath, uint8_t slot);
    ~Texture() noexcept;

    void Bind() const noexcept;
    void Unbind() const noexcept;
};

#endif
