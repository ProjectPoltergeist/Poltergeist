cmake_minimum_required(VERSION 3.1)

project(Poltergeist)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_C_STANDARD 17)

add_subdirectory("PoltergeistEngine")
add_subdirectory("PoltergeistEditor")
add_subdirectory("PoltergeistSandbox")
