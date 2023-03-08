using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DarkShrine
{
    int position;
    float aggro;
    float aggressiveness;
    float power;
    float powerCreep;

    public DarkShrine(int position, float aggressiveness, float power, float powerCreep)
    {
        this.position = position;
        this.aggressiveness = aggressiveness;
        this.power = power;
        this.powerCreep = powerCreep;
        this.aggro = 20;
        Vector2Int vPos = Grid._instance.GetPosition(position);
        PlaceTiles._instance.overlayMap.SetTile(new Vector3Int(vPos.x, vPos.y, 1), PlaceTiles._instance.buildingTiles[250]);
        EventHub.OnTick += Update;
        Debug.Log("DarkShrine Created");
    }
    public static bool MADEFIRSTUNIT;
    static int[] allowedUnits = {0, 1, 2, 3};
    public void Update()
    {
        if (!MADEFIRSTUNIT) return;
        power += powerCreep * Time.deltaTime;
        Debug.Log(power + " | " + aggro);
        if (Random.Range(0f, 100f) < aggro)
        {
            aggro = 0;

            List<Group> army = new List<Group>();
            int lowestSpeed = int.MaxValue;

            int unitAmount = Random.Range(0, allowedUnits.Length);
            allowedUnits.OrderBy(x => Random.Range(0, allowedUnits.Length));
            for (int i = 0; i < unitAmount; ++i)
            {
                int count = (int)Random.Range(0, power);
                if (count < 1) count = 1;
                Group unitGroup = new Group(count, allowedUnits[i]);
                int speed = (int)UnitDefinition.I[unitGroup.unitId].speed;
                if (speed < lowestSpeed) lowestSpeed = speed;
                army.Add(unitGroup);
            }

            Debug.Log("DarkShrine launching attack");
            int travelTime = (int)Vector2Int.Distance(Grid._instance.GetPosition(position), Grid._instance.GetPosition(LocalData.SelfPlayer.cityLocation)) * lowestSpeed;
            travelTime /= 2;
            ScheduledAttackEvent attackEvent = new ScheduledAttackEvent(travelTime, army, LocalData.SelfPlayer.cityLocation, position, 666);
        }
        else
        {
            aggro += aggressiveness * Time.deltaTime;
        }
    }
}
