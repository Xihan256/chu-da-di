using lln.ChuDaDi_MainLogic.Utils;

namespace lln.ChuDaDi_MainLogic.rules
{
    public class FourWithSingle: Rule
    {
        public bool validate(CardGroup currentGroup, CardGroup group){
            if (group.type != currentGroup.type || group.size != currentGroup.size){
                return false;
            }

            int groupFour = four(group);
            int currentGroupFour = four(currentGroup);

            return groupFour > currentGroupFour;
        }

        private int four(CardGroup group){
            bool isIn4 = false;
            int point = group.cards[0].point;
            for (int i = 1; i < group.cards.Count; i++){
                if (group.cards[i].point == point){
                    isIn4 = true;
                    break;
                }
            }

            if (!isIn4){
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