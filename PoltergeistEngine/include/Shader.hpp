#ifndef POLTERGEIST_SHADER_HPP
#define POLTERGEIST_SHADER_HPP

#include <cstdint>
#include <filesystem>
#include <iostream>
#include <optional>
#include <glad/glad.h>
#include "ShaderStage.hpp"
#include "ShaderStageType.hpp"

class Shader
{
private:
    uint32_t m_shaderId;
public:
    explicit Shader(uint32_t shaderId) noexcept;
    ~Shader() noexcept;

    [[nodiscard]] static std::optional<Shader> Create(const std::filesystem::path& vertexShaderFilePath, const std::filesystem::path& fragmentShaderFilePath) noexcept;

    void Bind() const noexcept;
    void Unbind() const noexcept;

    void SetUniform(const std::string& uniformName, int32_t value) const noexcept;
};

#endif
