cmake_minimum_required(VERSION 3.2)

project(Poltergeist)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_C_STANDARD 17)

# MSVC settings
if (MSVC)
	add_compile_options("/utf-8")
endif (MSVC)

# Windows settings
if (WIN32)
	add_definitions(-DUNICODE -D_UNICODE)
	add_definitions(-DWIN32_LEAN_AND_MEAN)
	add_definitions(-DNTDDI_VERSION=NTDDI_WIN7 -DWINVER=_WIN32_WINNT_WIN7 -D_WIN32_WINNT=_WIN32_WINNT_WIN7)
endif (WIN32)

add_subdirectory("PoltergeistEngine")
add_subdirectory("PoltergeistEditor")
add_subdirectory("PoltergeistSandbox")
