using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class Single: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }

            return group.cards[0].greaterThan(currentGroup.cards[0]);
        }
    }
}