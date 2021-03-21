#ifndef POLTERGEIST_JPEGIMAGE_HPP
#define POLTERGEIST_JPEGIMAGE_HPP

#include <cstdint>
#include <cstdio>
#include "PoltergeistEngine/Image/ImageLoader.hpp"

class JpegImage : public ImageLoader
{
public:
	static bool IsValidFormat(FILE* file);
	void LoadImage(FILE* file) override;
};

#endif
