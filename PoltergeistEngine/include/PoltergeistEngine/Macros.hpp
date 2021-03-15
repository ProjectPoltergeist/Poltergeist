#ifndef POLTERGEIST_MACROS_HPP
#define POLTERGEIST_MACROS_HPP

#ifdef POLTERGEIST_ENGINE_EXPORTS
#endif
#if defined _WIN32 || defined WIN32 || defined __CYGWIN__
#ifdef _MSC_VER
#define POLTERGEIST_PUBLIC __declspec(dllexport)
#else
#define POLTERGEIST_PUBLIC __attribute__ ((dllexport))
#endif
#define POLTERGEIST_INTERNAL
#else
#if __clang_major__ >= 3 || __GNUC__ >= 4
#define POLTERGEIST_PUBLIC __attribute__ ((visibility ("default")))
#define POLTERGEIST_INTERNAL  __attribute__ ((visibility ("hidden")))
#else
#define POLTERGEIST_PUBLIC
#define POLTERGEIST_INTERNAL
#endif
#endif

#endif
