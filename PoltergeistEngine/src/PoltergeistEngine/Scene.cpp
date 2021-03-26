#include "PoltergeistEngine/Scene.hpp"

GameObject& Scene::CreateGameObject() noexcept
{
	m_gameObjects.push_back(std::move(GameObject::Create(m_registry)));

	return m_gameObjects[m_gameObjects.size() - 1];
}

void Scene::RemoveGameObject(GameObject& gameObject) noexcept
{
	m_gameObjects.erase(std::find(m_gameObjects.begin(), m_gameObjects.end(), gameObject));
}

std::vector<GameObject>& Scene::GetGameObjects() noexcept
{
	return m_gameObjects;
}
