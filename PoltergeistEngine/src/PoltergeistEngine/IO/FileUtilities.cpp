#include "PoltergeistEngine/IO/FileUtilities.hpp"
#ifdef WIN32
#include "PoltergeistEngine/Encoding/EncodingUtilities.hpp"
#endif

FILE* OpenFile(const char* filename, const char* mode)
{
#ifdef WIN32
	const auto* wideFileName = ConvertUtf8ToUtf16(filename);
	const auto* wideMode = ConvertUtf8ToUtf16(mode);

	std::wstring openMode(wideMode);
	openMode += L", ccs=UTF-8";

	FILE* file = _wfopen(wideFileName, openMode.c_str());

	delete[] wideFileName;
	delete[] wideMode;

	return file;
#else
	return fopen(filename, mode);
#endif
}
