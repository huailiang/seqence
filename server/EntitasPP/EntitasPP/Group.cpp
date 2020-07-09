#include "Group.hpp"
#include "Matcher.hpp"
#include "GroupObserver.hpp"
#include <algorithm>

namespace EntitasPP
{
	Group::Group(const Matcher& matcher) : mMatcher(matcher)
	{
	}

	auto Group::Count() const -> const unsigned int
	{
		return mEntities.size();
	}

	auto Group::GetEntities() -> std::vector<EntityPtr>
	{
		if (mEntitiesCache.empty() && !mEntities.empty())
		{
			mEntitiesCache = std::vector<EntityPtr>(mEntities.begin(), mEntities.end());
		}

		return mEntitiesCache;
	}

	auto Group::GetSingleEntity() const -> EntityPtr
	{
		auto count = Count();

		if (count == 1)
		{
			return *(mEntities.begin());
		}
		else if (count == 0)
		{
			return nullptr;
		}
		else
		{
			throw std::runtime_error("Error, cannot get the single entity from group. Group contains more than one entity.");
		}

		return nullptr;
	}

	bool Group::ContainsEntity(const EntityPtr& entity) const
	{
		return std::find(mEntities.begin(), mEntities.end(), entity) != mEntities.end();
	}

	auto Group::GetMatcher() const -> Matcher
	{
		return mMatcher;
	}

	auto Group::CreateObserver(const GroupEventType eventType) -> std::shared_ptr<GroupObserver>
	{
		return std::shared_ptr<GroupObserver>(new GroupObserver(mInstance.lock(), eventType));
	}

	void Group::SetInstance(std::shared_ptr<Group> instance)
	{
		mInstance = std::weak_ptr<Group>(instance);
	}

	auto Group::HandleEntity(EntityPtr entity) -> GroupChanged*
	{
		return mMatcher.Matches(entity) ? AddEntity(entity) : RemoveEntity(entity);
	}

	void Group::HandleEntitySilently(EntityPtr entity)
	{
		if (mMatcher.Matches(entity))
		{
			AddEntitySilently(entity);
		}
		else
		{
			RemoveEntitySilently(entity);
		}
	}

	void Group::HandleEntity(EntityPtr entity, ComponentId index, IComponent* component)
	{
		if (mMatcher.Matches(entity))
		{
			AddEntity(entity, index, component);
		}
		else
		{
			RemoveEntity(entity, index, component);
		}
	}

	void Group::UpdateEntity(EntityPtr entity, ComponentId index, IComponent* previousComponent, IComponent* newComponent)
	{
		if (ContainsEntity(entity))
		{
			OnEntityRemoved(mInstance.lock(), entity, index, previousComponent);
			OnEntityAdded(mInstance.lock(), entity, index, newComponent);
			OnEntityUpdated(mInstance.lock(), entity, index, previousComponent, newComponent);
		}
	}

	void Group::RemoveAllEventHandlers()
	{
		OnEntityAdded.Clear();
		OnEntityRemoved.Clear();
		OnEntityUpdated.Clear();
	}

	bool Group::AddEntitySilently(EntityPtr entity)
	{
		if (mEntities.insert(entity).second)
		{
			mEntitiesCache.clear();
			return true;
		}

		return false;
	}

	void Group::AddEntity(EntityPtr entity, ComponentId index, IComponent* component)
	{
		if (AddEntitySilently(entity))
		{
			OnEntityAdded(mInstance.lock(), entity, index, component);
		}
	}

	auto Group::AddEntity(EntityPtr entity) -> GroupChanged*
	{
		return AddEntitySilently(entity) ? &OnEntityAdded : nullptr;
	}

	bool Group::RemoveEntitySilently(EntityPtr entity)
	{
		if (mEntities.erase(entity))
		{
			mEntitiesCache.clear();
			return true;
		}

		return false;
	}

	void Group::RemoveEntity(EntityPtr entity, ComponentId index, IComponent* component)
	{
		if (RemoveEntitySilently(entity))
		{
			OnEntityRemoved(mInstance.lock(), entity, index, component);
		}
	}

	auto Group::RemoveEntity(EntityPtr entity) -> GroupChanged*
	{
		return RemoveEntitySilently(entity) ? &OnEntityRemoved : nullptr;
	}
}