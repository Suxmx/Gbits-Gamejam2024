namespace GameMain
{
    public interface IManager
    {
        bool Inited { get; }

        void OnEnter();
        void OnExit();
        void OnInitEnd();
    }
}