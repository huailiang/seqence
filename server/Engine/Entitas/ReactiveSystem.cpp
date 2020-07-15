#include "ReactiveSystem.hpp"
#include "Pool.hpp"
#include "TriggerOnEvent.hpp"

namespace Entitas
{
	ReactiveSystem::ReactiveSystem(Pool* pool, std::shared_ptr<IReactiveSystem> subsystem) :
		ReactiveSystem(pool, subsystem, std::vector<TriggerOnEvent>(1, *subsystem->trigger))
	{
	}

	ReactiveSystem::ReactiveSystem(Pool* pool, std::shared_ptr<IMultiReactiveSystem> subsystem) :
		ReactiveSystem(pool, subsystem, subsystem->triggers)
	{
	}

	ReactiveSystem::ReactiveSystem(Pool* pool, std::shared_ptr<IReactiveExecuteSystem> subsystem, std::vector<TriggerOnEvent> triggers)
	{
		mSubsystem = subsystem;

		if (std::dynamic_pointer_cast<IEnsureComponents>(subsystem) != nullptr)
		{
			mEnsureComponents = (std::dynamic_pointer_cast<IEnsureComponents>(subsystem))->ensureComponents;
		}

		if (std::dynamic_pointer_cast<IExcludeComponents>(subsystem) != nullptr)
		{
			mExcludeComponents = (std::dynamic_pointer_cast<IExcludeComponents>(subsystem))->excludeComponents;
		}

		if (std::dynamic_pointer_cast<IClearReactiveSystem>(subsystem) != nullptr)
		{
			mClearAfterExecute = true;
		}

		size_t triggersLength = triggers.size();
		auto groups = std::vector<std::weak_ptr<Group>>(triggersLength);
		auto eventTypes = std::vector<GroupEventType>(triggersLength);

		for (unsigned int i = 0; i < triggersLength; ++i)
		{
			auto trigger = triggers[i];
			groups[i] = pool->GetGroup(trigger.trigger);
			eventTypes[i] = trigger.eventType;
		}

		mObserver = new GroupObserver(groups, eventTypes);
	}

	ReactiveSystem::~ReactiveSystem()
	{
		Deactivate();
		delete mObserver;
	}

	auto ReactiveSystem::GetSubsystem() const -> std::shared_ptr<IReactiveExecuteSystem>
	{
		return mSubsystem;
	}

	void ReactiveSystem::Activate()
	{
		mObserver->Activate();
	}

	void ReactiveSystem::Deactivate()
	{
		mObserver->Deactivate();
	}

	void ReactiveSystem::Clear()
	{
		mObserver->ClearCollectedEntities();
	}

	void ReactiveSystem::Execute()
	{
		if (mObserver->GetCollectedEntities().size() != 0)
		{
			if (!mEnsureComponents.IsEmpty())
			{
				if (!mExcludeComponents.IsEmpty())
				{
					for (const auto &e : mObserver->GetCollectedEntities())
					{
						if (mEnsureComponents.Matches(e) && !mExcludeComponents.Matches(e))
						{
							mEntityBuffer.push_back(e);
						}
					}
				}
				else
				{
					for (const auto &e : mObserver->GetCollectedEntities())
					{
						if (mEnsureComponents.Matches(e))
						{
							mEntityBuffer.push_back(e);
						}
					}
				}
			}
			else if (!mExcludeComponents.IsEmpty())
			{
				for (const auto &e : mObserver->GetCollectedEntities())
				{
					if (!mExcludeComponents.Matches(e))
					{
						mEntityBuffer.push_back(e);
					}
				}
			}
			else
			{
				for (const auto &e : mObserver->GetCollectedEntities())
				{
					mEntityBuffer.push_back(e);
				}
			}

			mObserver->ClearCollectedEntities();

			if (mEntityBuffer.size() != 0)
			{
				mSubsystem->Execute(mEntityBuffer);
				mEntityBuffer.clear();

				if (mClearAfterExecute)
				{
					mObserver->ClearCollectedEntities();
				}
			}
		}
	}
}
