using Character;

namespace Common
{
    public interface IInteractable
    {
        public void Interact(IVisitable initiator);
    }
}