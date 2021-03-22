#include "PoltergeistEngine/Image/PngImage.hpp"
#include <png.h>

bool PngImage::IsValidHeader(FILE* file)
{
	uint8_t header[8];
	fread(header, 1, 8, file);
	fseek(file, -8, SEEK_CUR);

	return png_sig_cmp(header, 0, 8) == 0;
}

std::shared_ptr<PngImage> PngImage::LoadFromFile(FILE* file)
{
	std::shared_ptr<PngImage> result = std::make_shared<PngImage>();
	png_structp internalState = png_create_read_struct(PNG_LIBPNG_VER_STRING, nullptr, nullptr, nullptr);

	png_set_user_limits(internalState, 0x7fffffff, 0x7fffffff);

	png_infop imageInfo = png_create_info_struct(internalState);

	png_init_io(internalState, file);
	png_read_info(internalState, imageInfo);
	png_set_palette_to_rgb(internalState);

	result->m_width = png_get_image_width(internalState, imageInfo);
	result->m_height = png_get_image_height(internalState, imageInfo);

	png_read_update_info(internalState, imageInfo);

	png_bytepp rows = new png_bytep[result->m_height];

	for (size_t row = 0; row < result->m_height; row++)
	{
		rows[row] = new png_byte[result->m_width * 3];
	}

	png_read_image(internalState, rows);
	png_read_end(internalState, imageInfo);

	result->m_data = new uint8_t[result->m_width * result->m_height * 3];

	for (uint32_t y = 0; y < result->m_height; y++)
	{
		for (uint32_t x = 0; x < result->m_width * 3; x++)
		{
			(result->m_data)[y * result->m_width * 3 + x] = rows[y][x];
		}
	}

	delete[] rows;

	png_destroy_read_struct(&internalState, &imageInfo, nullptr);
	return result;
}
