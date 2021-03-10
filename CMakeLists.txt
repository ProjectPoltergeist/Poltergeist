cmake_minimum_required(VERSION 3.1)

project(Poltergeist)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_C_STANDARD 17)

# Windows Unicode handling settings
add_compile_options("$<$<C_COMPILER_ID:MSVC>:/utf-8>")
add_compile_options("$<$<CXX_COMPILER_ID:MSVC>:/utf-8>")
add_definitions(-DUNICODE -D_UNICODE)

add_subdirectory("PoltergeistEngine")
add_subdirectory("PoltergeistEditor")
add_subdirectory("PoltergeistSandbox")
