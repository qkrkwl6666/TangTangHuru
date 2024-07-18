public interface IBossSkill
{
    public int SkillCount { get; set; }
    public bool IsChange { get; set; } 
    public float SkillRate { get; set; }
    public float DamageFactor { get; set; }

    public void Initialize(BossSkillData bossSkillData, float damage);
    public void Activate();
    public void DeActivate();
    public void SkillUpdate(float deltaTime);
}
