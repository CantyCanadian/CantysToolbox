namespace Canty.Managers
{
    public interface IPoolComponent
    {
        void Initialize();
        void OnTrigger();
        void OnDiscard();
    }
}