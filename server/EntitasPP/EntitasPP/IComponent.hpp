#pragma once

namespace Entitas
{
	class IComponent
	{
		friend class Entity;

	protected:
		IComponent() = default;
	};
}