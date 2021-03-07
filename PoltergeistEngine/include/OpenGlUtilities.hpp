#ifndef POLTERGEIST_OPENGLUTILITIES_HPP
#define POLTERGEIST_OPENGLUTILITIES_HPP

#include <cassert>
#include <cstdint>
#include <type_traits>
#include <glad/glad.h>

template<typename T>
static int32_t GetOpenGlType()
{
    if constexpr(std::is_same_v<T, float>)
    {
        return GL_FLOAT;
    }
    else
    {
        static_assert(false, "Incorrect type");
        return 0;
    }
}

static size_t GetOpenGlTypeSize(uint32_t openGlType)
{
    switch (openGlType)
    {
        case GL_FLOAT: return sizeof(float);
        default:
            assert(false);
            return 0;
    }
}

#endif
