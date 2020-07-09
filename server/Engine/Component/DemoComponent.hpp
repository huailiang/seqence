#include "../Entitas/Pool.hpp"
#include <iostream>
#include <string>

namespace Entitas
{
	using namespace std;

	class DemoComponent : public IComponent {
	public:
		void Reset(const std::string& name1, const std::string& name2) {
			std::cout << "Created new entity: " << name1 << "," << name2 << std::endl;
		}
	};


}