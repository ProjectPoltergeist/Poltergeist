#ifndef POLTERGEIST_TAGCOMPONENT_HPP
#define POLTERGEIST_TAGCOMPONENT_HPP

#include <string>

class TagComponent
{
public:
	std::string m_tag;

	TagComponent(const std::string& tag) noexcept
	{
		m_tag = tag;
	}
};

#endif
