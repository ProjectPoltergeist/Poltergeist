#ifndef POLTERGEIST_IMAGE_HPP
#define POLTERGEIST_IMAGE_HPP

#include <cstdint>
#include <filesystem>

class Image
{
private:
	uint32_t m_width = 0, m_height = 0;
	uint8_t* m_data = nullptr;
public:
	explicit Image(const std::filesystem::path &imagePath);
	~Image() noexcept;

	uint32_t GetWidth() const noexcept;
	uint32_t GetHeight() const noexcept;
	uint8_t* GetData() const noexcept;
};

#endif
