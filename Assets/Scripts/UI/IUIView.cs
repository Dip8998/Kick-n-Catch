namespace KNC.UI
{
    public interface IUIView
    {
        void EnableView();
        void DisableView();
        void SetController(IUIController controller);
    }
}
