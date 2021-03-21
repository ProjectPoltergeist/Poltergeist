#ifndef POLTERGEIST_IMAGELOADER_HPP
#define POLTERGEIST_IMAGELOADER_HPP

#include <cstdio>
#include <cstdint>

class ImageLoader
{
public:
	uint32_t width = 0, height = 0;
	uint8_t* data = nullptr;
	virtual void LoadImage(FILE* file);
	static ImageLoader* GetLoaderForFormat(FILE* file);
};

#endif
