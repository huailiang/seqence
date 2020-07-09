#pragma once

#include "ISystem.hpp"
#include <vector>

namespace Entitas
{
	class SystemContainer : public IInitializeSystem, public IExecuteSystem, public IFixedExecuteSystem
	{
	public:
		SystemContainer() = default;

		auto Add(std::shared_ptr<ISystem> system)->SystemContainer*;
		template <typename T> inline auto Add()->SystemContainer*;

		void Initialize();
		void Execute();
		void FixedExecute();

		void ActivateReactiveSystems();
		void DeactivateReactiveSystems();
		void ClearReactiveSystems();

	private:
		std::vector<std::shared_ptr<IInitializeSystem>> mInitializeSystems;
		std::vector<std::shared_ptr<IExecuteSystem>> mExecuteSystems;
		std::vector<std::shared_ptr<IFixedExecuteSystem>> mFixedExecuteSystems;
	};

	template <typename T>
	auto SystemContainer::Add() -> SystemContainer*
	{
		return Add(std::shared_ptr<T>(new T()));
	}
}
