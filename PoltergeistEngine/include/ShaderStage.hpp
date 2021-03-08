#ifndef POLTERGEIST_SHADERSTAGE_HPP
#define POLTERGEIST_SHADERSTAGE_HPP

#include <cstdint>
#include <filesystem>
#include <iostream>
#include <glad/glad.h>
#include "FileUtilities.hpp"
#include "ShaderStageType.hpp"

class ShaderStage
{
private:
	uint32_t m_shaderStageId;

	explicit ShaderStage(uint32_t shaderStageId) noexcept;
public:
	~ShaderStage() noexcept;

	[[nodiscard]] static ShaderStage Create(ShaderStageType shaderStageType, const std::filesystem::path& shaderStageFilePath);

	uint32_t GetId() const noexcept;
};

#endif
