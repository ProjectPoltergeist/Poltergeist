#include "PoltergeistEngine/Image/JpegImage.hpp"
#include <jpeglib.h>
#include <iostream>
#include <csetjmp>

struct my_error_mgr
{
    jpeg_error_mgr publicErrorManager;
    jmp_buf setJmpBuffer;
};

void my_error_exit(j_common_ptr cinfo)
{
    my_error_mgr* error = (my_error_mgr*)cinfo->err;
    (*cinfo->err->output_message) (cinfo);
    longjmp(error->setJmpBuffer, 1);
}

bool JpegImage::IsValidFormat(FILE* file)
{
	uint16_t header;
	fread(&header, 1, 2, file);
	fseek(file, -2, SEEK_CUR);
	return header == 0xD8FF;
}

void JpegImage::LoadImage(FILE* file)
{
    my_error_mgr errorManager;
    jpeg_decompress_struct decompressInfo;
    JSAMPARRAY pixelBuffor;

    decompressInfo.err = jpeg_std_error(&errorManager.publicErrorManager);
    errorManager.publicErrorManager.error_exit = my_error_exit;
    if (setjmp(errorManager.setJmpBuffer))
    {
        jpeg_destroy_decompress(&decompressInfo);
        throw std::runtime_error("jpg:jmp error");
    }

    jpeg_create_decompress(&decompressInfo);
    jpeg_stdio_src(&decompressInfo, file);
    jpeg_read_header(&decompressInfo, TRUE);
    jpeg_start_decompress(&decompressInfo);

    int rowLength = decompressInfo.output_width * 3;
    pixelBuffor = (*decompressInfo.mem->alloc_sarray)
        ((j_common_ptr)&decompressInfo, JPOOL_IMAGE, rowLength, 1);

    data = new uint8_t[rowLength * decompressInfo.output_height];
    size_t data_offest = 0;
    while (decompressInfo.output_scanline < decompressInfo.output_height)
    {
        jpeg_read_scanlines(&decompressInfo, pixelBuffor, 1);
        memcpy(data + data_offest, pixelBuffor[0], rowLength);
        data_offest += rowLength; //wiem że tutaj też teoretycznie da sie odwrćóić ale próowałem na wszystkie znane mi oraz znalezione w sieci sposoby i wywala mem error przy finishowaniu
    }

	int8_t flipBuffer = 0;
	for (unsigned int i = 0; i < (decompressInfo.output_height)/2; i++)
	{
		for (unsigned int x = 0; x < rowLength; x++)
		{
			flipBuffer = data[i * rowLength + x];
			data[i * rowLength + x] = data[(decompressInfo.output_height - i - 1) * rowLength + x];
			data[(decompressInfo.output_height - i - 1) * rowLength + x] = flipBuffer;
		}		
	}

    jpeg_finish_decompress(&decompressInfo);
    jpeg_destroy_decompress(&decompressInfo);

    if (errorManager.publicErrorManager.num_warnings) throw std::runtime_error("Decompressing error");

    width = decompressInfo.output_width;
    height = decompressInfo.output_height;
}
