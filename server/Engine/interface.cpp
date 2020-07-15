#include "interface.hpp"
#include "engine.hpp"


namespace Entitas
{
	extern "C"
	{

		// c# -> c++
		void InitNative(int rate, const char* assets,
			SyncPos pos, SyncRole role, SyncPlay play, BroadCast broad)
		{
			posDelegate = pos;
			roleDelegate = role;
			playDelegate = play;
			broadDelegate = broad;

			EngineInitial(rate, assets);
		}

		void NativeRecv(unsigned int id, unsigned char* buffer, int len)
		{
			EngineNotify(id, buffer, len);
		}


		void NativeUpdate(float delta)
		{
			EngineUpdate(delta);
		}

		void NativeDestroy()
		{
			EngineDestroy();
		}

	}
}
