#include "PoltergeistEngine/Image/ImageLoader.hpp"
#include "PoltergeistEngine/Image/JpegImage.hpp"
#include "PoltergeistEngine/Image/PngImage.hpp"

void ImageLoader::LoadImage(FILE* file) { }

ImageLoader* ImageLoader::GetLoaderForFormat(FILE* file)
{
	if (JpegImage::IsValidFormat(file))
		return new JpegImage();
	if (PngImage::IsValidFormat(file))
		return new PngImage();
}
