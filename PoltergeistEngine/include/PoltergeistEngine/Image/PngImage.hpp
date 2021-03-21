#ifndef POLTERGEIST_PNGIMAGE_HPP
#define POLTERGEIST_PNGIMAGE_HPP

#include <cstdint>
#include <cstdio>
#include "PoltergeistEngine/Image/ImageLoader.hpp"

class PngImage : public ImageLoader
{
public:
	static bool IsValidFormat(FILE* file);
	void LoadImage(FILE* file) override;
};

#endif
