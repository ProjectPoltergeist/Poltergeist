#ifndef POLTERGEIST_FILEUTILITIES_HPP
#define POLTERGEIST_FILEUTILITIES_HPP

#include <filesystem>
#include <fstream>
#include <optional>

FILE* OpenFile(const char* filename, const char* mode);

[[nodiscard]] static std::optional<std::string> GetFileContent(const std::filesystem::path& filePath)
{
	std::ifstream fileStream(filePath);

	if (fileStream.good())
	{
		auto result = std::make_optional<std::string>();

		fileStream.seekg(0, std::ios::end);
		result->resize(fileStream.tellg());
		fileStream.seekg(0, std::ios::beg);
		fileStream.read(result->data(), result->size());

		return result;
	}

	return {};
}

#endif
