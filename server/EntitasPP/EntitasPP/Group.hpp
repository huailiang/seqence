#pragma once

#include "Entity.hpp"
#include "Matcher.hpp"
#include "GroupEventType.hpp"
#include <unordered_set>

namespace EntitasPP
{
	class GroupObserver;

	class Group
	{
		friend class Pool;

	public:
		Group(const Matcher& matcher);
		auto Count() const -> const unsigned int;
		auto GetEntities()->std::vector<EntityPtr>;
		auto GetSingleEntity() const->EntityPtr;
		bool ContainsEntity(const EntityPtr& entity) const;
		auto GetMatcher() const->Matcher;
		auto CreateObserver(const GroupEventType eventType)->std::shared_ptr<GroupObserver>;

		using GroupChanged = Delegate<void(std::shared_ptr<Group> group, EntityPtr entity, ComponentId index, IComponent* component)>;
		using GroupUpdated = Delegate<void(std::shared_ptr<Group> group, EntityPtr entity, ComponentId index, IComponent* previousComponent, IComponent* newComponent)>;

		GroupChanged OnEntityAdded;
		GroupUpdated OnEntityUpdated;
		GroupChanged OnEntityRemoved;

	protected:
		void SetInstance(std::shared_ptr<Group> instance);
		auto HandleEntity(EntityPtr entity)->GroupChanged*;
		void HandleEntitySilently(EntityPtr entity);
		void HandleEntity(EntityPtr entity, ComponentId index, IComponent* component);
		void UpdateEntity(EntityPtr entity, ComponentId index, IComponent* previousComponent, IComponent* newComponent);
		void RemoveAllEventHandlers();

	private:
		bool AddEntitySilently(EntityPtr entity);
		void AddEntity(EntityPtr entity, ComponentId index, IComponent* component);
		auto AddEntity(EntityPtr entity)->GroupChanged*;
		bool RemoveEntitySilently(EntityPtr entity);
		void RemoveEntity(EntityPtr entity, ComponentId index, IComponent* component);
		auto RemoveEntity(EntityPtr entity)->GroupChanged*;

		std::weak_ptr<Group> mInstance;
		Matcher mMatcher;
		std::unordered_set<EntityPtr> mEntities;
		std::vector<EntityPtr> mEntitiesCache;
	};
}
