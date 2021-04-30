#ifndef POLTERGEIST_JPEGIMAGE_HPP
#define POLTERGEIST_JPEGIMAGE_HPP

#include "PoltergeistEngine/Image/Image.hpp"

class JpegImage : public Image
{
public:
	[[nodiscard]] static std::shared_ptr<JpegImage> LoadFromFile(FILE* file);
	[[nodiscard]] static bool IsValidHeader(FILE* file);
};

#endif
