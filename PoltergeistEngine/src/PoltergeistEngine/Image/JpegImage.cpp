#include "PoltergeistEngine/Image/JpegImage.hpp"
#include <jpeglib.h>
#include <iostream>
#include <csetjmp>
#include <cstring>

struct ErrorManager
{
    jpeg_error_mgr publicErrorManager;
    jmp_buf setJumpBuffer;
};

void ErrorExit(j_common_ptr decompressInfo)
{
    ErrorManager* error = reinterpret_cast<ErrorManager*>(decompressInfo->err);
    (*decompressInfo->err->output_message) (decompressInfo);
    longjmp(error->setJumpBuffer, 1);
}

bool JpegImage::IsValidHeader(FILE* file)
{
	uint16_t header;
	fread(&header, 1, 2, file);
	fseek(file, -2, SEEK_CUR);
	return header == 0xD8FF;
}

std::shared_ptr<JpegImage> JpegImage::LoadFromFile(FILE* file)
{
	jpeg_decompress_struct decompressInfo;
	ErrorManager error;
	decompressInfo.err = jpeg_std_error(&error.publicErrorManager);
	error.publicErrorManager.error_exit = ErrorExit;
	if (setjmp(error.setJumpBuffer))
	{
		jpeg_destroy_decompress(&decompressInfo);
		throw std::runtime_error("Initalizing decompress error");
	}

	jpeg_create_decompress(&decompressInfo);
	jpeg_stdio_src(&decompressInfo, file);
	jpeg_read_header(&decompressInfo, true);
	jpeg_start_decompress(&decompressInfo);

	int rowLength = decompressInfo.output_width * decompressInfo.output_components;
	JSAMPARRAY pixelBuffer;
	pixelBuffer = (*decompressInfo.mem->alloc_sarray)
		(reinterpret_cast<j_common_ptr>(&decompressInfo), JPOOL_IMAGE, rowLength, 1);

	std::shared_ptr<JpegImage> result = std::make_shared<JpegImage>();
	result->m_data = new uint8_t[decompressInfo.output_width * decompressInfo.output_height * 3];
	size_t dataOffset = 0;
	while (decompressInfo.output_scanline < decompressInfo.output_height)
	{
		jpeg_read_scanlines(&decompressInfo, pixelBuffer, 1);
		memcpy(result->m_data + dataOffset, pixelBuffer[0], rowLength);
		dataOffset += rowLength;
	}

	jpeg_finish_decompress(&decompressInfo);
	jpeg_destroy_decompress(&decompressInfo);

	if (error.publicErrorManager.num_warnings > 0)
		throw std::runtime_error("Decompressing error");

	result->m_width = decompressInfo.output_width;
	result->m_height = decompressInfo.output_height;
}
