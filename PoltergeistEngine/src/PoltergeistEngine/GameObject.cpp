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

bool GameObject::operator ==(const GameObject& other) noexcept
{
	return &m_registry == &other.m_registry && m_entityId == other.m_entityId;
}

GameObject GameObject::Create(entt::registry& registry) noexcept
{
	return GameObject(registry, registry.create());
}

void GameObject::DestroyEntity() const noexcept
{
	if (m_entityId != entt::null)
	{
		m_registry.destroy(m_entityId);
	}
}
