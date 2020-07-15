#pragma once

#include "Entity.hpp"
#include "Group.hpp"
#include <unordered_map>
#include <map>

namespace Entitas
{
	class ISystem;

	class Pool
	{
	public:
		Pool(const unsigned int startCreationIndex = 1);
		~Pool();

		auto CreateEntity()->EntityPtr;
		bool HasEntity(const EntityPtr& entity) const;
		void DestroyEntity(EntityPtr entity);
		void DestroyAllEntities();

		auto GetEntities()->std::vector<EntityPtr>;
		auto GetEntities(const Matcher matcher)->std::vector<EntityPtr>;
		auto GetGroup(Matcher matcher)->std::shared_ptr<Group>;

		void ClearGroups();
		void ResetCreationIndex();
		void ClearComponentPool(const ComponentId index);
		void ClearComponentPools();
		void Reset();

		auto GetEntityCount() const -> size_t;
		auto GetReusableEntitiesCount() const -> size_t;
		auto GetRetainedEntitiesCount() const -> size_t;

		auto CreateSystem(std::shared_ptr<ISystem> system)->std::shared_ptr<ISystem>;
		template <typename T> inline auto CreateSystem()->std::shared_ptr<ISystem>;

		using PoolChanged = Delegate<void(Pool* pool, EntityPtr entity)>;
		using GroupChanged = Delegate<void(Pool* pool, std::shared_ptr<Group> group)>;

		PoolChanged OnEntityCreated;
		PoolChanged OnEntityWillBeDestroyed;
		PoolChanged OnEntityDestroyed;
		GroupChanged OnGroupCreated;
		GroupChanged OnGroupCleared;

	private:
		void UpdateGroupsComponentAddedOrRemoved(EntityPtr entity, ComponentId index, IComponent* component);
		void UpdateGroupsComponentReplaced(EntityPtr entity, ComponentId index, IComponent* previousComponent, IComponent* newComponent);
		void OnEntityReleased(Entity* entity);

		unsigned int mCreationIndex;
		std::unordered_set<EntityPtr> mEntities;
		std::unordered_map<Matcher, std::shared_ptr<Group>> mGroups;
		std::stack<Entity*> mReusableEntities;
		std::unordered_set<Entity*> mRetainedEntities;

		std::map<ComponentId, std::stack<IComponent*>> mComponentPools;
		std::map<ComponentId, std::vector<std::weak_ptr<Group>>> mGroupsForIndex;

		std::vector<EntityPtr> mEntitiesCache;
		std::function<void(Entity*)> mOnEntityReleasedCache;
		std::function<void(EntityPtr, ComponentId, IComponent*)> mOnComponentAddedCache;
		std::function<void(EntityPtr, ComponentId, IComponent*)> mOnComponentRemovedCache;
		std::function<void(EntityPtr, ComponentId, IComponent*, IComponent*)> mOnComponentReplacedCache;
	};

	template <typename T>
	auto Pool::CreateSystem() -> std::shared_ptr<ISystem>
	{
		return CreateSystem(std::dynamic_pointer_cast<ISystem>(std::shared_ptr<T>(new T())));
	}
}
