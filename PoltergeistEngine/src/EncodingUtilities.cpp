#include "EncodingUtilities.hpp"
#include <stdexcept>
#ifdef WIN32
#include <Windows.h>
#endif

#ifdef WIN32
wchar_t* ConvertUtf8ToUtf16(const char* data)
{
	const size_t dataLength = strlen(data);
	int length = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, data, dataLength, nullptr, 0);
	if (FAILED(length))
		throw std::runtime_error("Encoding conversion error");
	auto* result = new wchar_t[length + 1];
	length = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS, data, dataLength, result, length);
	if (FAILED(length))
	{
		delete[] result;
		throw std::runtime_error("Encoding conversion error");
	}
	result[length] = L'\0';
	return result;
}

char* ConvertUtf16ToUtf8(const wchar_t* data)
{
	const size_t dataLength = wcslen(data);
	int length = WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, data, dataLength, nullptr, 0, nullptr, nullptr);
	if (FAILED(length))
		throw std::runtime_error("Encoding conversion error");
	auto* result = new char[length + 1];
	length = WideCharToMultiByte(CP_UTF8, WC_ERR_INVALID_CHARS, data, dataLength, result, length, nullptr, nullptr);
	if (FAILED(length))
	{
		delete[] result;
		throw std::runtime_error("Encoding conversion error");
	}
	result[length] = '\0';
	return result;
}
#endif
