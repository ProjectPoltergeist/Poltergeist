#ifndef POLTERGEIST_IMAGE_HPP
#define POLTERGEIST_IMAGE_HPP

#include <cstdint>
#include <filesystem>

class Image
{
protected:
	uint32_t m_width = 0, m_height = 0;
	uint8_t* m_data = nullptr;
public:
	[[nodiscard]] uint32_t GetWidth() const noexcept;
	[[nodiscard]] uint32_t GetHeight() const noexcept;
	[[nodiscard]] uint8_t* GetData() const noexcept;
};

#endif
