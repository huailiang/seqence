#include "SystemContainer.hpp"
#include "ReactiveSystem.hpp"
#include <memory>

namespace EntitasPP
{
	auto SystemContainer::Add(std::shared_ptr<ISystem> system) -> SystemContainer*
	{
		if (std::dynamic_pointer_cast<ReactiveSystem>(system) != nullptr)
		{
			if (std::dynamic_pointer_cast<IInitializeSystem>((std::dynamic_pointer_cast<ReactiveSystem>(system))->GetSubsystem()) != nullptr)
			{
				mInitializeSystems.push_back(std::dynamic_pointer_cast<IInitializeSystem>((std::dynamic_pointer_cast<ReactiveSystem>(system))->GetSubsystem()));
			}
		}
		else
		{
			if (std::dynamic_pointer_cast<IInitializeSystem>(system) != nullptr)
			{
				mInitializeSystems.push_back(std::dynamic_pointer_cast<IInitializeSystem>(system));
			}
		}

		if (std::dynamic_pointer_cast<IExecuteSystem>(system) != nullptr)
		{
			mExecuteSystems.push_back(std::dynamic_pointer_cast<IExecuteSystem>(system));
		}

		if (std::dynamic_pointer_cast<IFixedExecuteSystem>(system) != nullptr)
		{
			mFixedExecuteSystems.push_back(std::dynamic_pointer_cast<IFixedExecuteSystem>(system));
		}

		return this;
	}

	void SystemContainer::Initialize()
	{
		for (const auto &system : mInitializeSystems)
		{
			system->Initialize();
		}
	}

	void SystemContainer::Execute()
	{
		for (const auto &system : mExecuteSystems)
		{
			system->Execute();
		}
	}

	void SystemContainer::FixedExecute()
	{
		for (const auto &system : mFixedExecuteSystems)
		{
			system->FixedExecute();
		}
	}

	void SystemContainer::ActivateReactiveSystems()
	{
		for (const auto &system : mExecuteSystems)
		{
			if (std::dynamic_pointer_cast<ReactiveSystem>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<ReactiveSystem>(system))->Activate();
			}

			if (std::dynamic_pointer_cast<SystemContainer>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<SystemContainer>(system))->ActivateReactiveSystems();
			}
		}
	}

	void SystemContainer::DeactivateReactiveSystems()
	{
		for (const auto &system : mExecuteSystems)
		{
			if (std::dynamic_pointer_cast<ReactiveSystem>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<ReactiveSystem>(system))->Deactivate();
			}

			if (std::dynamic_pointer_cast<SystemContainer>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<SystemContainer>(system))->DeactivateReactiveSystems();
			}
		}
	}

	void SystemContainer::ClearReactiveSystems()
	{
		for (const auto &system : mExecuteSystems)
		{
			if (std::dynamic_pointer_cast<ReactiveSystem>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<ReactiveSystem>(system))->Clear();
			}

			if (std::dynamic_pointer_cast<SystemContainer>(system) != nullptr)
			{
				(std::dynamic_pointer_cast<SystemContainer>(system))->ClearReactiveSystems();
			}
		}
	}
}