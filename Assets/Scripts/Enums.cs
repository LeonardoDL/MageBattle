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
    Eclipse
}

public enum SlotsOnBoard
{
    ElementPlayer,
    ElementEnemy,
    ElementPlayerPortal,
    ElementEnemyPortal,
    EffectPlayer,
    EffectEnemy,
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
    ClearPhase,
    EndGame
}

public enum WinCondition
{
    Victory,
    Loss,
    Draw
}