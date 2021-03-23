#ifndef POLTERGEIST_GAMEOBJECT_HPP
#define POLTERGEIST_GAMEOBJECT_HPP

#include <entt/entt.hpp>

class GameObject
{
private:
	entt::registry& m_registry;
	entt::entity m_entityId;

	GameObject(entt::registry& registry, entt::entity entityId) noexcept;
public:
	GameObject(const GameObject& other) noexcept = delete;
	GameObject(GameObject&& other) noexcept;

	~GameObject() noexcept;

	GameObject& operator =(const GameObject& other) noexcept = delete;
	GameObject& operator =(GameObject&& other) noexcept;

	static GameObject Create(entt::registry& registry) noexcept;

	template<typename T, typename... Arguments>
	T& AddComponent(Arguments&&... arguments)
	{
		if (HasComponent<T>())
		{
			throw std::runtime_error("Entity already has component");
		}

		return m_registry.emplace<T>(m_entityId, std::forward<Arguments>(arguments)...);
	}

	template<typename T>
	void RemoveComponent()
	{
		if (!HasComponent<T>())
		{
			throw std::runtime_error("Entity doesn't have component");
		}

		m_registry.remove<T>(m_entityId);
	}

	template<typename T>
	bool HasComponent() const noexcept
	{
		return m_registry.has<T>(m_entityId);
	}

	template<typename T>
	T& GetComponent()
	{
		if (!HasComponent<T>())
		{
			throw std::runtime_error("Entity doesn't have component");
		}

		return m_registry.get<T>(m_entityId);
	}

	template<typename T>
	const T& GetComponent() const
	{
		if (!HasComponent<T>())
		{
			throw std::runtime_error("Entity doesn't have component");
		}

		return m_registry.get<T>(m_entityId);
	}
private:
	void DestroyEntity() const noexcept;
};

#endif
