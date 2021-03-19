#include "PoltergeistEngine/Scene.hpp"

GameObject& Scene::CreateGameObject() noexcept
{
	m_gameObjects.emplace_back();

	return m_gameObjects[m_gameObjects.size() - 1];
}

std::vector<GameObject>& Scene::GetGameObjects() noexcept
{
	return m_gameObjects;
}
