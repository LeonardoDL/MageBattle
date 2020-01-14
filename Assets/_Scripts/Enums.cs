using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    None,
    WaterE,
    EarthE,
    FireE,
    AirE,
    LightningE,
    ArcanaE,

    WaterP,
    EarthP,
    FireP,
    AirP,
    LightningP,

    WaterFireP,
    WaterAirP,
    WaterLightningP,
    WaterEarthP,
    FireAirP,
    FireLightningP,
    FireEarthP,
    AirLightningP,
    AirEarthP,
    EarthLightningP,

    WaterFireAirP,
    LightningFireWaterP,
    WaterFireEarthP,
    WaterAirLightningP,
    WaterAirEarthP,
    LightningWaterEarthP,
    LightningFireAirP,
    FireAirEarthP,
    LightningFireEarthP,
    LightningAirEarthP,

    MegaPowerP,

    Intelligence,
    Portal,
    SuperGenius,
    Disintegration,
    BlackHole,
    Eclipse,
    FishingRod,
    Counterspell
}

public enum SlotsOnBoard
{
    ElementPlayer,
    ElementEnemy,
    ElementPlayerPortal,
    ElementEnemyPortal,
    EffectPlayer,
    EffectEnemy,
    Stack,
    Discard,
    VictoryDeckPlayer,
    VictoryDeckEnemy
}

public enum GameState
{
    None,
    DrawPhase,
    PlayerPlayPhase,
    EnemyPlayPhase,
    BattlePhase,
    PlayerEffectPhase,
    EnemyEffectPhase,
    PlayerResponsePhase,
    EnemyResponsePhase,
    PlayerResolutionPhase,
    EnemyResolutionPhase,
    ClearPhase,
    EndGame
}

public enum WinCondition
{
    Victory,
    Loss,
    Draw
}

public enum LastPlayed
{
    None,
    Player,
    Enemy
}

public enum Target
{
    None,
    Player,
    Enemy
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}