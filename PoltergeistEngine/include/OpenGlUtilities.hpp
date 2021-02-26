#ifndef POLTERGEIST_OPENGLUTILITIES_HPP
#define POLTERGEIST_OPENGLUTILITIES_HPP

#include <cstdint>
#include <type_traits>
#include <glad/glad.h>

template<typename T>
int32_t GetOpenGlType()
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

#endif
