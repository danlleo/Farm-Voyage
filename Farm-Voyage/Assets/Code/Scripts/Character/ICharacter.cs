using Farm.Plants;

namespace Character
{
    public interface ICharacter
    {
        public void Accept(IPlantVisitor plantVisitor);
    }
}