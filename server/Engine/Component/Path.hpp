#include "../Util/vector3.hpp"
#include "../Util/util.hpp"

namespace Entitas
{

	class Path : public IComponent {
        
	public:
		void Reset(const char* name) {
            util::LoadPath(name, cnt, time, pos, rot);
		}
        
        vector3 Sample(float t)
        {
            if (t<time[0]) {
                return pos[0];
            }
            else if(t>time[cnt-1])
            {
                return pos[cnt-1];
            }
            else
            {
                for(int i=0;i<cnt-1;i++)
                {
                    if(time[i]<t && time[i+1]>t)
                    {
                        float p = (time[i+1]-t)/(time[i+1]-time[i]);
                        return util::lerp(pos[i], pos[i+1], p);
                    }
                }
            }
            return vector3(0,0,0);
        }

        size_t cnt;
        float* time;
		vector3* pos;
        float* rot;
	};


}
