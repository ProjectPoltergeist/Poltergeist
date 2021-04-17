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
    ErrorManager* errorManager = reinterpret_cast<ErrorManager*>(decompressInfo->err);
    (*decompressInfo->err->output_message) (decompressInfo);
    longjmp(errorManager->setJumpBuffer, 1);
}

bool JpegImage::IsValidHeader(FILE* file)
{
	uint8_t header[2];
	size_t readSize = fread(&header, 1, 2, file);
	fseek(file, -readSize, SEEK_CUR);

	if (readSize != 2)
		return false;

	return header[0] == 0xFF && header[1] == 0xD8;
}

std::shared_ptr<JpegImage> JpegImage::LoadFromFile(FILE* file)
{
	jpeg_decompress_struct decompressInfo;
	decompressInfo.out_color_space = JCS_RGB;
	ErrorManager errorManager;
	decompressInfo.err = jpeg_std_error(&errorManager.publicErrorManager);
	errorManager.publicErrorManager.error_exit = ErrorExit;
	if (setjmp(errorManager.setJumpBuffer))
	{
		jpeg_destroy_decompress(&decompressInfo);
		throw std::runtime_error("Initializing decompress error");
	}

	jpeg_create_decompress(&decompressInfo);
	jpeg_stdio_src(&decompressInfo, file);
	jpeg_read_header(&decompressInfo, true);
	jpeg_start_decompress(&decompressInfo);

	int rowLength = decompressInfo.output_width * decompressInfo.output_components;
	JSAMPARRAY pixelBuffer;
	pixelBuffer = (*decompressInfo.mem->alloc_sarray)
		(reinterpret_cast<j_common_ptr>(&decompressInfo), JPOOL_IMAGE, rowLength, 1);

	std::shared_ptr<JpegImage> decompressResult = std::make_shared<JpegImage>();
	decompressResult->m_data = new uint8_t[decompressInfo.output_width * decompressInfo.output_height * 3];
	size_t dataOffset = 0;
	while (decompressInfo.output_scanline < decompressInfo.output_height)
	{
		jpeg_read_scanlines(&decompressInfo, pixelBuffer, 1);
		memcpy(decompressResult->m_data + dataOffset, pixelBuffer[0], rowLength);
		dataOffset += rowLength;
	}

	jpeg_finish_decompress(&decompressInfo);
	jpeg_destroy_decompress(&decompressInfo);

	if (errorManager.publicErrorManager.num_warnings > 0)
		throw std::runtime_error("Decompressing error");

	decompressResult->m_width = decompressInfo.output_width;
	decompressResult->m_height = decompressInfo.output_height;

	return decompressResult;
}
