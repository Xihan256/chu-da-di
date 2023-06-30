using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class ShunZi: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type == CardGroup.TONGHUASHUN || group.type == CardGroup.THREE_WITH_PAIR ||
                group.type == CardGroup.FOUR_WITH_SINGLE || group.type == CardGroup.TONGHUA){
                return true;
            }
            
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }
            
            Card groupMax = Cards.findMax(group.cards);
            Card currentGroupMax = Cards.findMax(currentGroup.cards);

            return groupMax.greaterThan(currentGroupMax);
        }
        
    }
}