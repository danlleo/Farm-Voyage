using System.ComponentModel;

namespace Common
{
    public interface IVisitor
    {
        public void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}