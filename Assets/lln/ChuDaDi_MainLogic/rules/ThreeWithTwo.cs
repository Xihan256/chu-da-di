using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class ThreeWithTwo: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type == CardGroup.TONGHUASHUN || group.type == CardGroup.FOUR_WITH_SINGLE){
                return true;
            }
            
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }
            
            int groupThree = three(group);
            int currentGroupThree = three(currentGroup);

            return group.cards[groupThree].greaterThan(currentGroup.cards[currentGroupThree]);
        }
        
        private int three(CardGroup group){
            int index = 0;
            int count = 1;

            for (int i = 1; i < group.cards.Count; i++){
                if (group.cards[i].point == group.cards[0].point){
                    count++;
                }
            }
            
            if (count != 3){
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