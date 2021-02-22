add_executable(PoltergeistSandbox src/PoltergeistSandbox.cpp)
target_include_directories(PoltergeistSandbox PUBLIC "${CMAKE_CURRENT_SOURCE_DIR}/include/")
target_link_libraries(PoltergeistSandbox PUBLIC Poltergeist::PoltergeistEngine)
add_executable(Poltergeist::PoltergeistSandbox ALIAS PoltergeistSandbox)
