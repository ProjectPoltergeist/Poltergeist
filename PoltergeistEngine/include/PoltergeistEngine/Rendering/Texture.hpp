#ifndef POLTERGEIST_TEXTURE_HPP
#define POLTERGEIST_TEXTURE_HPP

#include <cstdint>
#include <filesystem>

class Texture
{
private:
	uint32_t m_textureId;
	uint8_t m_slot;

	Texture(uint32_t textureId, uint8_t slot) noexcept;
public:
	~Texture() noexcept;

	[[nodiscard]] static std::shared_ptr<Texture> CreateEmpty(size_t width, size_t height, uint8_t slot);
	[[nodiscard]] static std::shared_ptr<Texture> CreateFromFile(const std::filesystem::path& texturePath, uint8_t slot);

	void Bind() const noexcept;
	void Unbind() const noexcept;

	uint32_t GetId() const noexcept;
	uint8_t GetSlot() const noexcept;
};

#endif
