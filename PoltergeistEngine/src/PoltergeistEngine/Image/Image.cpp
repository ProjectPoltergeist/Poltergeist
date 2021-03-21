#include "PoltergeistEngine/Image/Image.hpp"
#include "PoltergeistEngine/Image/PngImage.hpp"
#include "PoltergeistEngine/Image/JpegImage.hpp"
#include "PoltergeistEngine/IO/FileUtilities.hpp"
#include "PoltergeistEngine/Image/ImageLoader.hpp"
#include <cstdio>

Image::Image(const std::filesystem::path &imagePath)
{
	FILE* file = OpenFile(imagePath.generic_string().c_str(), "rb");

	if (!file)
		throw std::runtime_error("Couldn't open the file");

	ImageLoader* loader = ImageLoader::GetLoaderForFormat(file);
	if(!loader)
		throw std::runtime_error("Unsupported Format!");
	loader->LoadImage(file);

	m_width = loader->width;
	m_height = loader->height;
	m_data = loader->data;

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
