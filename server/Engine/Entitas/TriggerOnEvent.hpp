#pragma once

#include "Matcher.hpp"
#include "GroupEventType.hpp"

namespace Entitas
{
	struct TriggerOnEvent
	{
	public:
		TriggerOnEvent(const Matcher trigger, const GroupEventType eventType)
		{
			this->trigger = trigger;
			this->eventType = eventType;
		}

		Matcher trigger;
		GroupEventType eventType;
	};
}
