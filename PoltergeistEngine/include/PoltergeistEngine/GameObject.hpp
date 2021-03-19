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
	T& AddComponent(Arguments&&... arguments);

	template<typename T>
	void RemoveComponent();

	template<typename T>
	bool HasComponent() noexcept;

	template<typename T>
	T& GetComponent();
private:
	void DestroyEntity() const noexcept;
};

#endif
