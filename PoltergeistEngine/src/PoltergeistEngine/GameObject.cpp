#include <stdexcept>
#include "PoltergeistEngine/GameObject.hpp"

GameObject::GameObject(entt::registry& registry, entt::entity entityId) noexcept : m_registry(registry)
{
	m_entityId = entityId;
}

GameObject::GameObject(GameObject&& other) noexcept : m_registry(other.m_registry)
{
	m_entityId = other.m_entityId;
	other.m_entityId = entt::null;
}

GameObject::~GameObject() noexcept
{
	DestroyEntity();
}

GameObject& GameObject::operator =(GameObject&& other) noexcept
{
	DestroyEntity();

	m_registry = std::move(other.m_registry);
	m_entityId = other.m_entityId;
	other.m_entityId = entt::null;

	return *this;
}

GameObject GameObject::Create(entt::registry& registry) noexcept
{
	return GameObject(registry, registry.create());
}

template<typename T, typename... Arguments>
T& GameObject::AddComponent(Arguments&&... arguments)
{
	if (HasComponent<T>())
	{
		throw std::runtime_error("Entity already has component");
	}

	return m_registry.emplace<T>(m_entityId, std::forward<Arguments>(arguments)...);
}

template<typename T>
void GameObject::RemoveComponent()
{
	if (!HasComponent<T>())
	{
		throw std::runtime_error("Entity doesn't have component");
	}

	m_registry.remove<T>(m_entityId);
}

template<typename T>
bool GameObject::HasComponent() noexcept
{
	return m_registry.has<T>(m_entityId);
}

template<typename T>
T& GameObject::GetComponent()
{
	if (!HasComponent<T>())
	{
		throw std::runtime_error("Entity doesn't have component");
	}

	return m_registry.get<T>(m_entityId);
}

void GameObject::DestroyEntity() const noexcept
{
	if (m_entityId != entt::null)
	{
		m_registry.destroy(m_entityId);
	}
}
