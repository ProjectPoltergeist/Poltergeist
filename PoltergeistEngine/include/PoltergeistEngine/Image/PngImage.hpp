#ifndef POLTERGEIST_PNGIMAGE_HPP
#define POLTERGEIST_PNGIMAGE_HPP

#include "PoltergeistEngine/Image/Image.hpp"

static class PngImage : public Image
{
public:
	[[nodiscard]] static std::shared_ptr<PngImage> LoadFromFile(FILE* file);
	[[nodiscard]] static bool IsValidHeader(FILE* file);
};

#endif
