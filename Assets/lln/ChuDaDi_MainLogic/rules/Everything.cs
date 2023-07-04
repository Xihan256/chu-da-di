using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class Everything : Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            return true;
        }
    }
}