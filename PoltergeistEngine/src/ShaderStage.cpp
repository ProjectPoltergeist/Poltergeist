#include "ShaderStage.hpp"

ShaderStage::ShaderStage(uint32_t shaderStageId) noexcept
{
    m_shaderStageId = shaderStageId;
}

ShaderStage::~ShaderStage() noexcept
{
    glDeleteShader(m_shaderStageId);
}

std::optional<ShaderStage> ShaderStage::Create(ShaderStageType shaderStageType, const std::filesystem::path &shaderStageFilePath) noexcept
{
    std::optional<std::string> shaderStageSource = GetFileContent(shaderStageFilePath);

    if (!shaderStageSource)
    {
        std::cout << "Failed to read the file\n";

        return {};
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

        return {};
    }

    return std::make_optional<ShaderStage>(shaderStageId);
}

uint32_t ShaderStage::GetId() const noexcept
{
    return m_shaderStageId;
}
