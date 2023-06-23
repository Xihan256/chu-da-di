using lln.ChuDaDi_MainLogic.cardLogic;
using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class Three: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }
            
            if (group.cards[0].point > currentGroup.cards[0].point){
                return true;
            } else{
                Card groupMax = Cards.findMax(group.cards);
                Card currentGroupMax = Cards.findMax(currentGroup.cards);

                return groupMax.greaterThan(currentGroupMax);
            }
            
        }
    }
}