cmake_minimum_required(VERSION 3.15)
cmake_policy(SET CMP0092 NEW)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_C_STANDARD 11)

set(VCPKG_MANIFEST_MODE ON)
set(CMAKE_TOOLCHAIN_FILE ${CMAKE_CURRENT_SOURCE_DIR}/vcpkg/scripts/buildsystems/vcpkg.cmake CACHE STRING "Vcpkg toolchain file")

if (NOT CMAKE_BUILD_TYPE)
	set(CMAKE_BUILD_TYPE Debug)
endif ()

# Force configuration to the specified one
get_property(isMultiConfig GLOBAL PROPERTY GENERATOR_IS_MULTI_CONFIG)
if(isMultiConfig)
	set(CMAKE_CONFIGURATION_TYPES "${CMAKE_BUILD_TYPE}" CACHE STRING "" FORCE)
endif ()

enable_language(C)
enable_language(CXX)

# Warning settings
if (MSVC)
	message("Enabling MSVC warnings")
	add_compile_options(/W4)
else ()
	message("Enabling non-MSVC warnings")
	add_compile_options(-Wall -Wextra -pedantic)
endif ()

# MSVC settings
if (MSVC)
	message("Enabling MSVC UTF-8")
	add_compile_options(/utf-8)
endif ()

# Windows settings
if (WIN32)
	message("Enabling Windows configuration")
	add_definitions(-DUNICODE -D_UNICODE)
	add_definitions(-DWIN32_LEAN_AND_MEAN)
	add_definitions(-DNTDDI_VERSION=NTDDI_WIN7 -DWINVER=_WIN32_WINNT_WIN7 -D_WIN32_WINNT=_WIN32_WINNT_WIN7)
endif ()

# Optimization settings
if (MSVC)
	message("Enabling MSVC optimizations")
	SET(CMAKE_C_FLAGS_RELEASE "${CMAKE_C_FLAGS_RELEASE} /O2 /Oi /Ot /Oy /GL")
	SET(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /O2 /Oi /Ot /Oy /GL")
	if (CMAKE_SIZEOF_VOID_P LESS 8)
		message("Enabling MSVC non 64bit optimizations")
		SET(CMAKE_C_FLAGS_RELEASE "${CMAKE_C_FLAGS_RELEASE} /arch:SSE2")
		SET(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} /arch:SSE2")
	endif ()
	SET(CMAKE_EXE_LINKER_FLAGS_RELEASE "${CMAKE_EXE_LINKER_FLAGS_RELEASE} /LTCG")
else ()
	message("Enabling non-MSVC optimizations")
	SET(CMAKE_C_FLAGS_RELEASE "${CMAKE_C_FLAGS_RELEASE} -O2 -mfpmath=sse -mmmx -msse -msse2")
	SET(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -O2 -mfpmath=sse -mmmx -msse -msse2")
endif ()

project(Poltergeist)

add_subdirectory("PoltergeistEngine")
add_subdirectory("PoltergeistEditor")
add_subdirectory("PoltergeistSandbox")
