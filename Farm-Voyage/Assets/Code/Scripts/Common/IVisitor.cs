using Character.Michael;
using Character.Player;

namespace Common
{
    public interface IVisitor
    {
        public void Visit(Player player);
        public void Visit(Michael michael);
    }
}