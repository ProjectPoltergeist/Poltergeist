#ifndef POLTERGEIST_SCENE_HPP
#define POLTERGEIST_SCENE_HPP

#include <vector>
#include "GameObject.hpp"

class Scene
{
private:
	std::vector<GameObject> m_gameObjects;
public:
	GameObject& CreateGameObject() noexcept;

	std::vector<GameObject>& GetGameObjects() noexcept;
};

#endif
