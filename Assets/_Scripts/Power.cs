using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [Header("Powers")]
    public bool air;
    public bool earth;
    public bool fire;
    public bool lightning;
    public bool water;

    public CardType cardType;

    public void Init(CardType c)
    {
        air = false;
        earth = false;
        fire = false;
        lightning = false;
        water = false;

        cardType = c;

        switch (c)
        {
            case CardType.WaterP:
                water = true;
                break;
            case CardType.EarthP:
                earth = true;
                break;
            case CardType.FireP:
                fire = true;
                break;
            case CardType.AirP:
                air = true;
                break;
            case CardType.LightningP:
                lightning = true;
                break;


            case CardType.WaterFireP:
                water = true;
                fire = true;
                break;
            case CardType.WaterAirP:
                water = true;
                air = true;
                break;
            case CardType.WaterLightningP:
                water = true;
                lightning = true;
                break;
            case CardType.WaterEarthP:
                water = true;
                earth = true;
                break;
            case CardType.FireAirP:
                air = true;
                fire = true;
                break;
            case CardType.FireLightningP:
                lightning = true;
                fire = true;
                break;
            case CardType.FireEarthP:
                earth = true;
                fire = true;
                break;
            case CardType.AirLightningP:
                air = true;
                lightning = true;
                break;
            case CardType.AirEarthP:
                air = true;
                earth = true;
                break;
            case CardType.EarthLightningP:
                earth = true;
                lightning = true;
                break;


            case CardType.WaterFireAirP:
                water = true;
                fire = true;
                air = true;
                break;
            case CardType.LightningFireWaterP:
                lightning = true;
                fire = true;
                water = true;
                break;
            case CardType.WaterFireEarthP:
                water = true;
                fire = true;
                earth = true;
                break;
            case CardType.WaterAirLightningP:
                water = true;
                air = true;
                lightning = true;
                break;
            case CardType.WaterAirEarthP:
                water = true;
                air = true;
                earth = true;
                break;
            case CardType.LightningWaterEarthP:
                lightning = true;
                water = true;
                earth = true;
                break;
            case CardType.LightningFireAirP:
                lightning = true;
                fire = true;
                air = true;
                break;
            case CardType.FireAirEarthP:
                fire = true;
                air = true;
                earth = true;
                break;
            case CardType.LightningFireEarthP:
                lightning = true;
                fire = true;
                earth = true;
                break;
            case CardType.LightningAirEarthP:
                lightning = true;
                air = true;
                earth = true;
                break;


            case CardType.MegaPowerP:
                lightning = true;
                air = true;
                earth = true;
                water = true;
                fire = true;
                break;
        }
    }
}
