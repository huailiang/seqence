#include "engine.hpp"
#include "log.hpp"
#include "EngineInfo.hpp"
#include "Entitas/Matcher.hpp"
#include "System/MoveSystem.hpp"
#include "System/EntitySystem.hpp"
#include "Conf/PathSystem.hpp"
#include "Conf/SkillSystem.hpp"
#include "Util/util.hpp"

namespace Entitas
{
	SystemContainer *systems;
	Pool* pool;

	int EngineInfo::frameRate;
	int EngineInfo::frameCount;
	float EngineInfo::time;
	float EngineInfo::delta;

	SyncPos posDelegate;
	SyncRole roleDelegate;
	SyncPlay  playDelegate;

	/*
	 * 第一个参数是fps
	 * 第二个参数是asset目录（资源目录）
	 */
	void EngineInitial(int rate, const char* assets)
	{
		InitLogger("debug.log", "warn.log", "error.log");
		LOG("engine initial");
		EngineInfo::frameRate = rate;
		EngineInfo::frameCount = 0;
		EngineInfo::time = 0;
		EngineInfo::assetPath = assets;

		systems = new SystemContainer();
		pool = new Pool();

		systems->Add(pool->CreateSystem<MoveSystem>());
		systems->Add(pool->CreateSystem<EntitySystem>());
		systems->Initialize();
	}


	void EngineUpdate(float delta)
	{
		//printf("engine update %.3f \n", delta);
		if (systems)
		{
			systems->Execute();
		}
		EngineInfo::frameCount++;
		EngineInfo::delta = delta;
		EngineInfo::time += delta;
	}

	void EngineNotify(unsigned int id, unsigned char* buffer, int len)
	{
		// recv buffer from server
	}

	void EngineDestroy()
	{
		delete systems;
		delete pool;
		printf("engine quit");
	}

}
