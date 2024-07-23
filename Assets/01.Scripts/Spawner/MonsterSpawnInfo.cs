public class MonsterSpawnInfo
{
    public int MonsterId { get; private set; }
    public int MonsterCount { get; private set; }
    public int MonsterDuration { get; private set; }
    public int SpawnType { get; private set; }

    private float time;

    public bool IsValid
    {
        get { return MonsterId != -1; }
    }

    public bool IsSpawn
    {
        get 
        { 
            if(MonsterId != -1 && time >= MonsterDuration)
            {
                time = 0f;
                return true;
            }

            return false;
        }
    }
    public MonsterSpawnInfo()
    {
        MonsterId = -1;
        MonsterCount = -1;
        MonsterDuration = -1;
        SpawnType = -1;
    }

    public void SetSpawnData(int monsterId, int monsterCount, int monsterDuration, int spawnType)
    {
        this.MonsterId = monsterId;
        this.MonsterCount = monsterCount;
        this.MonsterDuration = monsterDuration;
        this.SpawnType = spawnType;
    }

    public void Update(float deltaTime)
    {
        time += deltaTime;
    }

}
