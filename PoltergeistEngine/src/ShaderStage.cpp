#include "ShaderStage.hpp"

ShaderStage::ShaderStage(uint32_t shaderStageId) noexcept
{
    m_shaderStageId = shaderStageId;
}

ShaderStage::~ShaderStage() noexcept
{
    glDeleteShader(m_shaderStageId);
}

ShaderStage ShaderStage::Create(ShaderStageType shaderStageType, const std::filesystem::path &shaderStageFilePath)
{
    std::optional<std::string> shaderStageSource = GetFileContent(shaderStageFilePath);

    if (!shaderStageSource)
    {
        throw std::runtime_error("Failed to read the file");
    }

    const char* shaderStageSourceCString = shaderStageSource->data();
    auto shaderStageSourceSize = static_cast<int32_t>(shaderStageSource->size());
    uint32_t shaderStageId = glCreateShader(static_cast<uint32_t>(shaderStageType));

    glShaderSource(shaderStageId, 1, &shaderStageSourceCString, &shaderStageSourceSize);
    glCompileShader(shaderStageId);

    int32_t success = 0;
    glGetShaderiv(shaderStageId, GL_COMPILE_STATUS, &success);

    if (!success)
    {
        constexpr size_t SIZE = 512;

        char infoLog[SIZE];
        glGetShaderInfoLog(shaderStageId, SIZE, nullptr, infoLog);

        std::cout << "Failed to compile the shader stage: " << infoLog << "\n";

        throw std::runtime_error("Failed to compile the shader stage");
    }

    return ShaderStage(shaderStageId);
}

uint32_t ShaderStage::GetId() const noexcept
{
    return m_shaderStageId;
}
