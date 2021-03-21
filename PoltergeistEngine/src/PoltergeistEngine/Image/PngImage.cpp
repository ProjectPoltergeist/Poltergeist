#include "PoltergeistEngine/Image/PngImage.hpp"
#include <png.h>

bool PngImage::IsValidFormat(FILE* file)
{
	uint8_t header[8];
	fread(header, 1, 8, file);
	fseek(file, -8, SEEK_CUR);
	return png_sig_cmp(header, 0, 8) == 0;
}

void PngImage::LoadImage(FILE* file)
{
	png_structp internalState = png_create_read_struct(PNG_LIBPNG_VER_STRING, nullptr, nullptr, nullptr);

	png_set_user_limits(internalState, 0x7fffffff, 0x7fffffff);

	png_infop imageInfo = png_create_info_struct(internalState);

	png_init_io(internalState, file);
	png_read_info(internalState, imageInfo);
	png_set_palette_to_rgb(internalState);

	width = png_get_image_width(internalState, imageInfo);
	height = png_get_image_height(internalState, imageInfo);

	png_read_update_info(internalState, imageInfo);

	png_bytepp rows = new png_bytep[height];

	for (size_t row = 0; row < height; row++)
	{
		rows[row] = new png_byte[width * 3];
	}

	png_read_image(internalState, rows);
	png_read_end(internalState, imageInfo);

	data = new uint8_t[width * height * 3];

	for (uint32_t y = 0; y < height; y++)
	{
		for (uint32_t x = 0; x < width * 3; x++)
		{
			(data)[y * width * 3 + x] = rows[y][x];
		}
	}

	delete[] rows;

	png_destroy_read_struct(&internalState, &imageInfo, nullptr);
}
