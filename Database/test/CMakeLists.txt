cmake_minimum_required(VERSION 3.18)

set(CMAKE_CXX_STANDARD 11)
project(DatabaseTest)
set(BUNDLE OFF)
set(SOURCES main.cpp)
add_subdirectory(../ database)
add_subdirectory(../lib/googletest googletest)

add_executable(DatabaseTest ${SOURCES})
target_link_libraries(DatabaseTest Database gtest)
target_include_directories(DatabaseTest
                           PRIVATE ../lib/googletest/googletest/include)
