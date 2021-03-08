#include "Image.hpp"
#include "FileUtilities.hpp"
#include <cstdio>
#include <png.h>

Image::Image(const std::filesystem::path &imagePath)
{
	FILE* file = OpenFile(imagePath.generic_string().c_str(), "rb");

	if (!file)
		throw std::runtime_error("Couldn't open the file");

	uint8_t header[8];
	fread(header, 1, 8, file);
	fseek(file, -8, SEEK_CUR);

	if (png_sig_cmp(header, 0, 8) != 0)
		throw std::runtime_error("Unsupported format");

	png_structp internalState = png_create_read_struct(PNG_LIBPNG_VER_STRING, nullptr, nullptr, nullptr);

	png_set_user_limits(internalState, 0x7fffffff, 0x7fffffff);

	png_infop imageInfo = png_create_info_struct(internalState);

	png_init_io(internalState, file);
	png_read_info(internalState, imageInfo);
	png_set_palette_to_rgb(internalState);

	m_width = png_get_image_width(internalState, imageInfo);
	m_height = png_get_image_height(internalState, imageInfo);

	png_read_update_info(internalState, imageInfo);

	png_bytepp rows = new png_bytep[m_height];

	for (size_t row = 0; row < m_height; row++)
	{
		rows[row] = new png_byte[m_width * 3];
	}

	png_read_image(internalState, rows);
	png_read_end(internalState, imageInfo);

	m_data = new uint8_t[m_width * m_height * 3];

	for (uint32_t y = 0; y < m_height; y++)
	{
		for (uint32_t x = 0; x < m_width * 3; x++)
		{
			m_data[y * m_width * 3 + x] = rows[y][x];
		}
	}

	delete[] rows;

	png_destroy_read_struct(&internalState, &imageInfo, nullptr);
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
