#ifndef POLTERGEIST_SHADERSTAGE_HPP
#define POLTERGEIST_SHADERSTAGE_HPP

#include <cstdint>
#include <filesystem>
#include <iostream>
#include <optional>
#include <glad/glad.h>
#include "FileUtilities.hpp"
#include "ShaderStageType.hpp"

class ShaderStage
{
private:
    uint32_t m_shaderStageId;
public:
    explicit ShaderStage(uint32_t shaderStageId) noexcept;
    ~ShaderStage() noexcept;

    [[nodiscard]] static std::optional<ShaderStage> Create(ShaderStageType shaderStageType, const std::filesystem::path& shaderStageFilePath) noexcept;

    uint32_t GetId() const noexcept;
};

#endif
