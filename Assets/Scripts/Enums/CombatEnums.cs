namespace Etheral
{

    
    
    public enum PlayerAbilityTypes
    {
        None = 5,
        BlessedGround = 1,
        HolyCharge = 2,
        Purification = 3,
        HolyShield = 4,
        Leap = 0,
        RangedAttack = 6,
        Crusade = 7,
    }
    public enum DamageType
    {
        Melee = 0,
        Projectile = 1,
        Fire = 2,
        Holy = 3
    }

    public enum DamageEffects
    {
        Slow = 0,
        Stun = 1,
    }
    

    public enum EnemyType
    {
        Melee,
        Ranged,
        RangedMelee,
        Caster
    }
    
    public enum CastDirection
    {
        FourDirectional = 0,
        EightDirectional = 1,
        None = 2
    }
    
    public enum StateType
    {
        Attack = 0,
        AttackRanged = 1,
        Block = 2,
        Climbing = 3,
        Chase = 4,
        Dialogue = 5,
        Dead = 6,
        Follow = 7,
        Idle = 8,
        Move = 9,
        NPC = 10,
        Impact = 11,
        Special = 12,
        Strafe = 13,
        Stunned = 14,
        Puzzle = 15,
        KnockedDown = 16,
        Dodge = 17,
        Default = 18,
        Patrol = 19,
        AbsoluteChase = 20,
        JumpOffMeshLink = 21,
        MoveToGate = 22,
    }


}