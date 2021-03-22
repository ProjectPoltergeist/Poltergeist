#ifndef POLTERGEIST_JPEGIMAGE_HPP
#define POLTERGEIST_JPEGIMAGE_HPP

#include <cstdint>
#include <cstdio>

static class JpegImage
{
public:
	static bool IsValidFormat(FILE* file);
	static void LoadImageFromFile(FILE* file, uint32_t& width, uint32_t& height, uint8_t*& data);
};

#endif
