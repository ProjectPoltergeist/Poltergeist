#ifndef POLTERGEIST_SCENE_HPP
#define POLTERGEIST_SCENE_HPP

#include <vector>
#include <entt/entt.hpp>
#include "GameObject.hpp"

class Scene
{
private:
	entt::registry m_registry;
	std::vector<GameObject> m_gameObjects;
public:
	GameObject& CreateGameObject() noexcept;

	std::vector<GameObject>& GetGameObjects() noexcept;
};

#endif
