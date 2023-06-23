using lln.ChuDaDi_MainLogic.Utils;
using UnityEngine;

namespace lln.ChuDaDi_MainLogic.rules
{

    public interface Rule
    {
        bool validate(CardGroup currentGroup , CardGroup group);
    }
}