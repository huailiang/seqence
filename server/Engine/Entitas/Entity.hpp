#pragma once

#include "ComponentTypeId.hpp"
#include "Delegate.hpp"
#include <stack>
#include <map>

namespace Entitas
{
	class Entity;
	typedef std::shared_ptr<Entity> EntityPtr;

	class Entity
	{
		friend class Pool;

	public:
		Entity(std::map<ComponentId, std::stack<IComponent*>>* componentPools);

		template <typename T, typename... TArgs> inline auto Add(TArgs&&... args)->EntityPtr;
		template <typename T> inline auto Remove()->EntityPtr;
		template <typename T, typename... TArgs> inline auto Replace(TArgs&&... args)->EntityPtr;
		template <typename T> inline auto Refresh()->EntityPtr;
		template <typename T> inline auto Get() const->T*;
		template <typename T> inline auto Use()->T*;
		template <typename T> inline bool Has() const;

		bool HasComponents(const std::vector<ComponentId>& indices) const;
		bool HasAnyComponent(const std::vector<ComponentId>& indices) const;
		auto GetComponentsCount() const -> size_t;
		void RemoveAllComponents();
		auto GetUuid() const -> const size_t;
		bool IsEnabled();

		bool operator ==(const EntityPtr& right) const;
		bool operator ==(const Entity right) const;

		using EntityChanged = Delegate<void(EntityPtr entity, ComponentId index, IComponent* component)>;
		using ComponentReplaced = Delegate<void(EntityPtr entity, ComponentId index, IComponent* previousComponent, IComponent* newComponent)>;
		using EntityReleased = Delegate<void(Entity* entity)>;

		EntityChanged OnComponentAdded;
		ComponentReplaced OnComponentReplaced;
		EntityChanged OnComponentRemoved;
		EntityReleased OnEntityReleased;

	protected:
		void SetInstance(EntityPtr instance);
		auto AddComponent(const ComponentId index, IComponent* component)->EntityPtr;
		auto RemoveComponent(const ComponentId index)->EntityPtr;
		auto ReplaceComponent(const ComponentId index, IComponent* component)->EntityPtr;
		auto GetComponent(const ComponentId index) const->IComponent*;
		bool HasComponent(const ComponentId index) const;
		void Destroy();

		template <typename T, typename... TArgs> inline auto CreateComponent(TArgs&&... args)->IComponent*;

		unsigned int mUuid{ 0 };
		bool mIsEnabled = true;

	private:
		auto GetComponentPool(const ComponentId index) const->std::stack<IComponent*>*;
		void Replace(const ComponentId index, IComponent* replacement);

		std::weak_ptr<Entity> mInstance;
		std::map<ComponentId, IComponent*> mComponents;
		std::map<ComponentId, std::stack<IComponent*>>* mComponentPools;
	};

	template <typename T, typename... TArgs>
	auto Entity::CreateComponent(TArgs&&... args) -> IComponent*
	{
		std::stack<IComponent*>* componentPool = GetComponentPool(ComponentTypeId::Get<T>());
		IComponent* component = nullptr;

		if (componentPool->size() > 0)
		{
			component = componentPool->top();
			componentPool->pop();
		}
		else
		{
			component = new T();
		}

		(static_cast<T*>(component))->Reset(std::forward<TArgs>(args)...);

		return component;
	}

	template <typename T, typename... TArgs>
	auto Entity::Add(TArgs&&... args) -> EntityPtr
	{
		return AddComponent(ComponentTypeId::Get<T>(), CreateComponent<T>(std::forward<TArgs>(args)...));
	}

	template <typename T>
	auto Entity::Remove() -> EntityPtr
	{
		return RemoveComponent(ComponentTypeId::Get<T>());
	}

	template <typename T, typename... TArgs>
	auto Entity::Replace(TArgs&&... args) -> EntityPtr
	{
		return ReplaceComponent(ComponentTypeId::Get<T>(), CreateComponent<T>(std::forward<TArgs>(args)...));
	}

	template <typename T>
	auto Entity::Refresh() -> EntityPtr
	{
		return ReplaceComponent(ComponentTypeId::Get<T>(), Get<T>());
	}

	template<typename T>
	auto Entity::Get() const -> T*
	{
		return static_cast<T*>(GetComponent(ComponentTypeId::Get<T>()));
	}

	template<typename T>
	auto Entity::Use() -> T*
	{
		Refresh<T>();
		return static_cast<T*>(GetComponent(ComponentTypeId::Get<T>()));
	}

	template <typename T>
	bool Entity::Has() const
	{
		return HasComponent(ComponentTypeId::Get<T>());
	}
}

namespace std
{
	template <>
	struct hash<weak_ptr<Entitas::Entity>>
	{
		std::size_t operator()(const weak_ptr<Entitas::Entity>& ptr) const
		{
			return hash<size_t>()(ptr.lock()->GetUuid());
		}
	};

	bool operator ==(weak_ptr<Entitas::Entity> left, weak_ptr<Entitas::Entity> right);
}
