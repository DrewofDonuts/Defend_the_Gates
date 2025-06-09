namespace Etheral
{
    public enum TargetType
    {
        None = 0,
        Fellowship = 1,
        AI = 2,
        Gate = 3,
        Tower = 4,
        Enemy = 5,
    }


    public enum FeedbackType
    {
        Light,
        Medium,
        Heavy,
        None
    }

    public enum SongPart
    {
        Intro = 0,
        Loop = 1,
        Outro = 2,
    }

    public enum TrackType
    {
        Main = 0,
        Drums = 1,
        Bass = 2,
        Guitar = 3,
        Vocals = 4,
        Synth = 5,
        Horns = 6,
        Strings = 7,
    }


    public enum ClimbActionType
    {
        GroundToLedge = 0,
        Hanging = 1,
        Left = 2,
        Right = 3,
        Up = 4,
        Down = 5,
        ShimmyLeft = 6,
        ShimmyRight = 7,
        SwingFromGronud = 8,
        SwingToGround = 9,
        SwingToSwing = 10,
    }

    public enum ConnectionType
    {
        DynoJump = 0,
        Leap = 1,
        LeapBig = 2,
        LeapToClimb = 3,
        Shimmy = 4,
        SwingFromGround = 6,
        SwingToGround = 7,
        SwingToSwing = 5,
    }

    public enum ClimbDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum PlayerControlTypes
    {
        DialogueState = 1,
        KnockedDownState = 3,
        MovementState = 0,
        UIState = 2,
    }


    public enum AudioType
    {
        impact = 0,
        woosh = 1,
        death = 2,
        spell = 3,
        spellImpact = 4,
        none = 5,
        spawn = 6,
        cooldown = 7,
        pickup = 8,
        block = 9,
        rangedWeapon = 10,
        attackEmote = 11,
        taunt = 12,
    }

    public enum Affiliation
    {
        Enemy,
        Fellowship,
        Neutral
    }

    public enum InputType
    {
        playStation,
        nintendoSwitch,
        xbox,
        keyboard,
        gamePad
    }
}