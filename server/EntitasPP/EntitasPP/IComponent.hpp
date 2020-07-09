#pragma once

namespace EntitasPP
{
	class IComponent
	{
		friend class Entity;

	protected:
		IComponent() = default;
	};
}