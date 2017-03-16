using UnityEngine;
using System.Collections;

public class TB_MapInfo
{
    public int mMapNo;
    public int mEnableBattle;
    public int[] mArrRegenMostersPer = new int[3];
    public int[] mArrRegenMosters = new int[3];
    public int mMonLv;

    public int SumMonsterPer()
    {
        int iSum = 0;
        for (int i = 0; i < mArrRegenMostersPer.Length; ++i)
        {
            iSum += mArrRegenMostersPer[i];
        }

        return iSum;
    }
}

