/*Used from:
YouTube Channel: ChiliTomatoNoodle. (n.d.). Retrieved 8/21/24, from https://www.youtube.com/@ChiliTomatoNoodle
GitHub Repository: planetchili/Chil. (n.d.). Retrieved 8/21/24, from https://github.com/planetchili/Chil */

#include "ChiliSingletons.h"

namespace chil::ioc
{
    Singletons& Sing()
    {
        static Singletons sing;
        return sing;
    }
}