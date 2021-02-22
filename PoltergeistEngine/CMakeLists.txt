add_library(PoltergeistEngine STATIC src/PoltergeistEngine.cpp)
target_include_directories(PoltergeistEngine PUBLIC "${CMAKE_CURRENT_SOURCE_DIR}/include/")
add_library(Poltergeist::PoltergeistEngine ALIAS PoltergeistEngine)
