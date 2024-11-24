using System.Threading.Tasks;

namespace GameMain
{
    public interface IManager
    {
        bool Inited { get; }

        Task OnEnter();
        void OnExit();
        void OnInitEnd();
    }
}