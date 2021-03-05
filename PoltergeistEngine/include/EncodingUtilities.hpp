#ifndef POLTERGEIST_ENCODINGUTILITIES_HPP
#define POLTERGEIST_ENCODINGUTILITIES_HPP

#ifdef WIN32
wchar_t* ConvertUtf8ToUtf16(const char* data);
char* ConvertUtf16ToUtf8(const wchar_t* data);
#endif

#endif
