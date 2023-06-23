using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class ThreeWithTwo: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }
            
            int groupThree = three(group);
            int currentGroupThree = three(currentGroup);

            return groupThree > currentGroupThree;
        }
        
        private int three(CardGroup group){
            int count = 1;
            int point = group.cards[0].point;
            for (int i = 1; i < group.cards.Count; i++){
                if (group.cards[i].point == point){
                    count++;
                }
            }

            if (count != 3){
                for (int i = 1; i < group.cards.Count; i++){
                    if (group.cards[i].point != point){
                        point = group.cards[i].point;
                        break;
                    }
                }
            }

            return point;
        }
    }
}