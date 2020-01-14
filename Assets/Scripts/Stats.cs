using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public static int STAT_MIN = 0;
    public static int STAT_MAX = 100;

    public enum Type
    {
        Attack,
        Defense,
        HP,
        SpAtk,
        SpDef,
        Speed,
    }

    private SingleStat atkStat;
    private SingleStat defStat;
    private SingleStat hpStat;
    private SingleStat spaStat;
    private SingleStat spdStat;
    private SingleStat speedStat;

    public Stats(int hpStatAmt, int atkStatAmt,int defStatAmt, int speedStatAmt, int spdStatAmt, int spaStatAmt)
    {
        hpStat = new SingleStat(hpStatAmt);
        atkStat = new SingleStat(atkStatAmt);
        defStat = new SingleStat(defStatAmt);
        speedStat = new SingleStat(speedStatAmt);
        spdStat = new SingleStat(spdStatAmt);
        spaStat = new SingleStat(spaStatAmt);
    }

    private SingleStat GetSingleStat(Type statType)
    {
        switch (statType)
        {
            default:
            case Type.Attack: return atkStat;
            case Type.Defense: return defStat;
            case Type.HP: return hpStat;
            case Type.SpAtk: return spaStat;
            case Type.SpDef: return spdStat;
            case Type.Speed: return speedStat;
        }
    }
    public void SetStat(Type statType,int Stat)
    {
        GetSingleStat(statType).SetStat(Stat);
    }

    public int GetStat(Type statType)
    {
        return GetSingleStat(statType).GetStat();
    }

    public float GetStatNorm(Type statType)
    {
        return GetSingleStat(statType).GetStatNorm();
    }
    private class SingleStat
    {
        private int stat;

        public SingleStat(int statAmount)
        {
            SetStat(statAmount);
        }
        public void SetStat(int Stat)
        {
            stat = Mathf.Clamp(Stat, STAT_MIN, STAT_MAX);
        }

        public int GetStat()
        {
            return stat;
        }

        public float GetStatNorm()
        {
            return (float)stat / STAT_MAX;
        }
    }
}
