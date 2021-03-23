#include "PoltergeistEngine/Image/Image.hpp"
#include "PoltergeistEngine/Image/PngImage.hpp"
#include "PoltergeistEngine/Image/JpegImage.hpp"
#include "PoltergeistEngine/IO/FileUtilities.hpp"

std::shared_ptr<Image> Image::LoadFromFile(const std::filesystem::path& imagePath)
{
	FILE* file = OpenFile(imagePath.u8string().c_str(), "rb");

	if (!file)
		throw std::runtime_error("Couldn't open the file");

	std::shared_ptr<Image> result;
	if (JpegImage::IsValidHeader(file))
		result = JpegImage::LoadFromFile(file);
	if (PngImage::IsValidHeader(file))
		result = PngImage::LoadFromFile(file);
	else
	{
		fclose(file);
		throw std::runtime_error("Unsupported format!");
	}

	fclose(file);
	return result;
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
