#ifndef POLTERGEIST_PNGIMAGE_HPP
#define POLTERGEIST_PNGIMAGE_HPP

#include <cstdint>
#include <cstdio>

static class PngImage
{
public:
	static bool IsValidFormat(FILE* file);
	static void LoadImageFromFile(FILE* file, uint32_t& width, uint32_t& height, uint8_t*& data);
};

#endif
