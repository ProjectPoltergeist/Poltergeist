#include "PoltergeistEngine/Image/PngImage.hpp"
#include <png.h>

bool PngImage::IsValidHeader(FILE* file)
{
	uint8_t header[8];
	size_t result = fread(header, 1, 8, file);
	fseek(file, -result, SEEK_CUR);

	if (result != 8)
		return false;

	return png_sig_cmp(header, 0, 8) == 0;
}

std::shared_ptr<PngImage> PngImage::LoadFromFile(FILE* file)
{
	png_structp internalState = png_create_read_struct(PNG_LIBPNG_VER_STRING, nullptr, nullptr, nullptr);

	png_set_user_limits(internalState, 0x7fffffff, 0x7fffffff);

	png_infop imageInfo = png_create_info_struct(internalState);

	png_init_io(internalState, file);
	png_read_info(internalState, imageInfo);
	png_set_palette_to_rgb(internalState);

	std::shared_ptr<PngImage> decompressResult = std::make_shared<PngImage>();
	decompressResult->m_width = png_get_image_width(internalState, imageInfo);
	decompressResult->m_height = png_get_image_height(internalState, imageInfo);

	png_read_update_info(internalState, imageInfo);

	png_bytepp rows = new png_bytep[decompressResult->m_height];

	for (size_t row = 0; row < decompressResult->m_height; row++)
	{
		rows[row] = new png_byte[decompressResult->m_width * 3];
	}

	png_read_image(internalState, rows);
	png_read_end(internalState, imageInfo);

	decompressResult->m_data = new uint8_t[decompressResult->m_width * decompressResult->m_height * 3];

	for (uint32_t y = 0; y < decompressResult->m_height; y++)
	{
		for (uint32_t x = 0; x < decompressResult->m_width * 3; x++)
		{
			decompressResult->m_data[y * decompressResult->m_width * 3 + x] = rows[y][x];
		}
	}

	for (size_t row = 0; row < decompressResult->m_height; row++)
	{
		delete[] rows[row];
	}

	png_destroy_read_struct(&internalState, &imageInfo, nullptr);

	return decompressResult;
}
