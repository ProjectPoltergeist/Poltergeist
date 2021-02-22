add_executable(PoltergeistEditor src/PoltergeistEditor.cpp)
target_include_directories(PoltergeistEditor PUBLIC "${CMAKE_CURRENT_SOURCE_DIR}/include/")
target_link_libraries(PoltergeistEditor PUBLIC Poltergeist::PoltergeistEngine)
add_executable(Poltergeist::PoltergeistEditor ALIAS PoltergeistEditor)
