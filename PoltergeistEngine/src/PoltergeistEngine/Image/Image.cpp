#include "PoltergeistEngine/Image/Image.hpp"
#include "PoltergeistEngine/Image/PngImage.hpp"
#include "PoltergeistEngine/Image/JpegImage.hpp"

uint32_t Image::GetWidth() const noexcept
{
	return m_width;
}

uint32_t Image::GetHeight() const noexcept
{
	return m_height;
}

uint8_t* Image::GetData() const noexcept
{
	return m_data;
}
