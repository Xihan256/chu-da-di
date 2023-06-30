using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class FourWithSingle: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type == CardGroup.TONGHUASHUN){
                return true;
            }
            
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }

            int groupFour = four(group);
            int currentGroupFour = four(currentGroup);

            return group.cards[groupFour].greaterThan(currentGroup.cards[currentGroupFour]);
        }

        private int four(CardGroup group){
            
            int index = 0;
            int count = 1;
            
            for (int i = 1; i < group.cards.Count; i++){
                if (group.cards[i].point == group.cards[0].point){
                    count++;
                }
            }
            
            if (count != 4){
                for (int i = 1; i < group.cards.Count; i++){
                    if (group.cards[i].point != group.cards[0].point){
                        index = i;
                        break;
                    }
                }
            }
            
            return index;
            
        }
    }
}