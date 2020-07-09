#pragma once

#include "IComponent.hpp"
#include <vector>

namespace Entitas
{
	typedef unsigned int ComponentId;
	typedef std::vector<ComponentId> ComponentIdList;

	struct ComponentTypeId
	{
	public:
		template<typename T>
		static const ComponentId Get()
		{
			static_assert((std::is_base_of<IComponent, T>::value && !std::is_same<IComponent, T>::value),
				"Class type must be derived from IComponent");

			static ComponentId id = mCounter++;
			return id;
		}

		static unsigned int Count()
		{
			return mCounter;
		}

	private:
		static unsigned int mCounter;
	};
}
