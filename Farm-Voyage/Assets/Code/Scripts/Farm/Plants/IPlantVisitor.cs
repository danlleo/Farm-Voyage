using Character.Michael;
using Character.Player;

namespace Farm.Plants
{
    public interface IPlantVisitor
    {
        public void Visit(Player player);
        public void Visit(Michael michael);
    }
}