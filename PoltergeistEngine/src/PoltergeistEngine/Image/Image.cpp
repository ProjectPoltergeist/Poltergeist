#include "PoltergeistEngine/Image/Image.hpp"
#include "PoltergeistEngine/Image/PngImage.hpp"
#include "PoltergeistEngine/Image/JpegImage.hpp"
#include "PoltergeistEngine/IO/FileUtilities.hpp"
#include <cstdio>

Image::Image(const std::filesystem::path &imagePath)
{
	FILE* file = OpenFile(imagePath.generic_string().c_str(), "rb");

	if (!file)
		throw std::runtime_error("Couldn't open the file");

	if (PngImage::IsValidFormat(file))
		PngImage::LoadImageFromFile(file, m_width, m_height, m_data);
	else if (JpegImage::IsValidFormat(file))
		JpegImage::LoadImageFromFile(file, m_width, m_height, m_data);
	else
		throw std::runtime_error("Unsupported format");

	fclose(file);
}

Image::~Image() noexcept
{
	delete[] m_data;
}

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
