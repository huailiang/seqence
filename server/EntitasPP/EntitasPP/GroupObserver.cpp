#include "GroupObserver.hpp"
#include "Group.hpp"
#include <functional>

namespace Entitas
{
	GroupObserver::GroupObserver(std::weak_ptr<Group> group, const GroupEventType eventType)
	{
		mGroups.push_back(group);
		mEventTypes.push_back(eventType);
		mAddEntityCache = std::bind(&GroupObserver::AddEntity, this, std::placeholders::_1, std::placeholders::_2, std::placeholders::_3, std::placeholders::_4);
	}

	GroupObserver::GroupObserver(std::vector<std::weak_ptr<Group>> groups, std::vector<GroupEventType> eventTypes)
	{
		mGroups = groups;
		mEventTypes = eventTypes;
		mAddEntityCache = std::bind(&GroupObserver::AddEntity, this, std::placeholders::_1, std::placeholders::_2, std::placeholders::_3, std::placeholders::_4);

		if (groups.size() != eventTypes.size())
		{
			throw std::runtime_error("Error, group and eventType vector counts must be equal");
		}

		Activate();
	}

	GroupObserver::~GroupObserver()
	{
		Deactivate();
	}

	void GroupObserver::Activate()
	{
		for (unsigned int i = 0, groupCount = mGroups.size(); i < groupCount; ++i)
		{
			auto g = mGroups[i].lock();
			auto eventType = mEventTypes[i];

			if (eventType == GroupEventType::OnEntityAdded)
			{
				g->OnEntityAdded -= mAddEntityCache;
				g->OnEntityAdded += mAddEntityCache;
			}
			else if (eventType == GroupEventType::OnEntityRemoved)
			{
				g->OnEntityRemoved -= mAddEntityCache;
				g->OnEntityRemoved += mAddEntityCache;
			}
			else if (eventType == GroupEventType::OnEntityAddedOrRemoved)
			{
				g->OnEntityAdded -= mAddEntityCache;
				g->OnEntityAdded += mAddEntityCache;

				g->OnEntityRemoved -= mAddEntityCache;
				g->OnEntityRemoved += mAddEntityCache;
			}
		}
	}

	void GroupObserver::Deactivate()
	{
		for (unsigned int i = 0, groupCount = mGroups.size(); i < groupCount; ++i)
		{
			if (!mGroups[i].expired())
			{
				auto g = mGroups[i].lock();

				g->OnEntityAdded -= mAddEntityCache;
				g->OnEntityRemoved -= mAddEntityCache;
			}
		}

		ClearCollectedEntities();
	}

	auto GroupObserver::GetCollectedEntities() -> std::unordered_set<EntityPtr>
	{
		return mCollectedEntities;
	}

	void GroupObserver::ClearCollectedEntities()
	{
		mCollectedEntities.clear();
	}

	void GroupObserver::AddEntity(std::shared_ptr<Group> group, EntityPtr entity, ComponentId index, IComponent* component)
	{
		mCollectedEntities.insert(entity);
	}
}
