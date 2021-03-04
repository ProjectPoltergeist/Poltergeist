#include "Shader.hpp"

Shader::Shader(uint32_t shaderId) noexcept
{
    m_shaderId = shaderId;
}

Shader::~Shader() noexcept
{
    glDeleteProgram(m_shaderId);
}

std::optional<Shader> Shader::Create(const std::filesystem::path& vertexShaderFilePath, const std::filesystem::path& fragmentShaderFilePath) noexcept
{
    uint32_t shaderId = glCreateProgram();

    {
        auto vertexShaderStage = ShaderStage::Create(ShaderStageType::Vertex, vertexShaderFilePath);

        if (!vertexShaderStage)
        {
            return {};
        }

        auto fragmentShaderStage = ShaderStage::Create(ShaderStageType::Fragment, fragmentShaderFilePath);

        if (!fragmentShaderStage)
        {
            return {};
        }

        glAttachShader(shaderId, vertexShaderStage->GetId());
        glAttachShader(shaderId, fragmentShaderStage->GetId());
        glLinkProgram(shaderId);
        glDetachShader(shaderId, vertexShaderStage->GetId());
        glDetachShader(shaderId, fragmentShaderStage->GetId());
    }

    int32_t success = 0;
    glGetProgramiv(shaderId, GL_LINK_STATUS, &success);

    if (!success)
    {
        constexpr size_t SIZE = 512;

        char info[SIZE];
        glGetProgramInfoLog(shaderId, SIZE, nullptr, info);

        std::cout << "Failed to link the shader: " << info << "\n";

        return {};
    }

    return std::make_optional<Shader>(shaderId);
}

void Shader::Bind() const noexcept
{
    glUseProgram(m_shaderId);
}

void Shader::Unbind() const noexcept
{
    glUseProgram(0);
}

void Shader::SetUniform(const std::string &name, int32_t value) const noexcept
{
    int location = glGetUniformLocation(m_shaderId, name.c_str());

    glUniform1i(location, value);
}
