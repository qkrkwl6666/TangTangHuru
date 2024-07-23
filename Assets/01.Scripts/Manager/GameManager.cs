public class GameManager : Singleton<GameManager>
{
    public int CurrentStage { get; private set; } = 1;

    public void ChangeStage(int stage)
    {
        CurrentStage = stage;
    }
}
