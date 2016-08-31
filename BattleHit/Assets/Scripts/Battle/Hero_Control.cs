using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Hero_Control : MonoBehaviour
{
    public enum eHeroState
    {
        HEROSTATE_IDLE = 0,
        HEROSTATE_WALK,
        HEROSTATE_HIT,
        HEROSTATE_ATT,
        HEROSTATE_CAST,
        HEROSTATE_DIE,
        HEROSTATE_NONE,
    }

    public enum eAttPos
    {
        ATTPOS_NONE = 0,
        ATTPOS_LEFT,
        ATTPOS_RIGHT,
    }

    public readonly int ATTPOS_MAXCOUNT = 2;

    Actor mActor = null;
    eHeroState mHeroState = eHeroState.HEROSTATE_IDLE;

    Guid mHeroUid = new Guid();
    int mHeroNo = 0;
    int mHP = 0;
    int mMaxHP = 0;
    int mAtk = 0;
    int mDef = 0;    
    float mSpeed = 0;
    float mBlowPower = 0;
    float mBlowTolerance = 0;
    string mStResPath = null;

    bool mMyTeam = false;
    bool mIsMove = false;
    bool mIsDie = false;

    Transform mEf_HP = null;

    //Dictionary<Hero_Control, int> mDicAggro = new Dictionary<Hero_Control, int>();
    Hero_Control mTarget = null;
    eAttPos mAttPos = eAttPos.ATTPOS_NONE;
    List<Hero_Control> mListAttPos = new List<Hero_Control>();
    List<SpriteRenderer> mListSR = new List<SpriteRenderer>();
    int mMaxSortingOrderNo = 0;

    int mBlowDir = 0;
    float mBlowTime = 0;
    float mElapsedTime = 0;

    GameObject mHeroObj = null;

    public Guid HeroUid
    {
        set { mHeroUid = value; }
        get { return mHeroUid; }
    }

    public int HeroNo
    {
        set { mHeroNo = value; }
        get { return mHeroNo; }
    }

    public int HP
    {
        set { mHP = value; }
        get { return mHP; }
    }

    public int MaxHP
    {
        set { mMaxHP = value; }
        get { return mMaxHP; }
    }

    public int Atk
    {
        set { mAtk = value; }
        get { return mAtk; }
    }

    public int Def
    {
        set { mDef = value; }
        get { return mDef; }
    }

    public float Speed
    {
        set { mSpeed = value; }
        get { return mSpeed; }
    }

    public float BlowPower
    {
        set { mBlowPower = value; }
        get { return mBlowPower; }
    }

    public float BlowTolerance
    {
        set { mBlowTolerance = value; }
        get { return mBlowTolerance; }
    }

    public string StResPath
    {
        set { mStResPath = value; }
        get { return mStResPath; }
    }

    //public Dictionary<Hero_Control, int> DicAggro
    //{
    //    set { mDicAggro = value; }
    //    get { return mDicAggro; }
    //}

    public Hero_Control Target
    {
        set { mTarget = value; }
        get { return mTarget; }
    }

    public eAttPos AttPos
    {
        set { mAttPos = value; }
        get { return mAttPos; }
    }

    public bool MyTeam
    {
        set { mMyTeam = value; }
        get { return mMyTeam; }
    }

    public GameObject HeroObj
    {
        set { mHeroObj = value; }
        get { return mHeroObj; }
    }

    public eHeroState HeroState
    {
        set { mHeroState = value; }
        get { return mHeroState; }
    }

    public List<SpriteRenderer> ListSR
    {
        set { mListSR = value; }
        get { return mListSR; }
    }

    public int MaxSortingOrderNo
    {
        set { mMaxSortingOrderNo = value; }
        get { return mMaxSortingOrderNo; }
    }
    
    public bool IsDie
    {
        set { mIsDie = value; }
        get { return mIsDie; }
    }

    void Start()
    {
        Transform tObj = transform.FindChild( "Obj" );
        if (tObj != null)
        {
            mActor = tObj.GetComponent<Actor>();

            mEf_HP = tObj.FindChild("ef_HP");
            if (mEf_HP == null)
            {
                Debug.LogError("Not Find ef_HP!");
            }
        }

        SpriteRenderer [] sr = transform.GetComponentsInChildren<SpriteRenderer>();
        if (sr != null && sr.Length > 0)
        {
            mListSR.AddRange(sr);
        }
    }

    void Update()
    {
        MyStateControl();
        //AggroControl();
        BlowUpdate();
        HPGaugePosUpdate();
    }

    
    void BlowUpdate()
    {
        if (mBlowTime > 0)
        {
            mElapsedTime += Time.deltaTime;
            if (mElapsedTime < mBlowTime)
            {
                Vector3 vPos = transform.position;
                if (mBlowDir > 0)
                {
                    vPos.x -= (0.01f - BlowTolerance);
                }
                else
                {
                    vPos.x += (0.01f - BlowTolerance);
                }

                transform.position = vPos;
            }
            else
            {
                mBlowTime = 0;
                mElapsedTime = 0;
            }
        }
    }

    void MyStateControl()
    {
        switch ((int)mHeroState)
        {
            case (int)eHeroState.HEROSTATE_IDLE:
                Battle_Control bc = GameMain.Instance().BattleControl;
                if (bc != null)
                {
                    if (bc.BattleState == Battle_Control.eBattleState.eBattle_Ready)
                    {
                        // 전투 준비 중일 때 시작 위치로 아군이동
                        MoveToBattleReadyPos();
                    }
                    else if (bc.BattleState == Battle_Control.eBattleState.eBattle_Ing)
                    {
                        // 피아 모두 적을 탐색
                        SearchTarget();
                    }
                    else if (bc.BattleState == Battle_Control.eBattleState.eBattle_Win)
                    {
                        // 아군 이겼을 때 포즈 후 다음 스테이지로
                        //MoveToBattleEndPos();
						if (MyTeam) 
						{
							mHeroState = eHeroState.HEROSTATE_IDLE;
						}
                    }
                    else if (bc.BattleState == Battle_Control.eBattleState.eBattle_Lose)
                    {
                        // 적이 이겼을 때 포즈
						if (!MyTeam) 
						{
							mHeroState = eHeroState.HEROSTATE_IDLE;
						}
                    }
                }                
                break;

            case (int)eHeroState.HEROSTATE_WALK:
                Walk();
                break;

            case (int)eHeroState.HEROSTATE_HIT:
                BeHit();
                break;

            case (int)eHeroState.HEROSTATE_ATT:
                AttHero();                
                break;

            case (int)eHeroState.HEROSTATE_CAST:
                break;

            case (int)eHeroState.HEROSTATE_DIE:
                HeroDie();
                break;
        }
    }

    void MoveToBattleReadyPos()
    {
        if (!MyTeam) return;

        Transform tBattleStartTo = GameMain.Instance().BattleControl.BattleStartTo;
        if (tBattleStartTo == null) return;

        FaceTo(tBattleStartTo);
        Vector3 vDir = tBattleStartTo.position - transform.position;
        vDir.Normalize();
        transform.position += vDir * Time.deltaTime * mSpeed * 4f;

        mIsMove = true;
        mActor.PlayAnimation(Actor.AnimationActor.ANI_WALK);
        mActor.SetAnimationSpeed(Actor.AnimationActor.ANI_WALK, 2f);

        float dis = Vector3.Distance(transform.position, tBattleStartTo.position);
        if (dis < 0.01f)
        {
            mActor.SetAnimationSpeed(Actor.AnimationActor.ANI_WALK, 1f);
            GameMain.Instance().BattleControl.BattleState = Battle_Control.eBattleState.eBattle_Ing;
            mHeroState = eHeroState.HEROSTATE_IDLE;
        }
    }

    void MoveToBattleEndPos()
    {
        if (!MyTeam) return;

        NearPath np = FindNearPath();
        if (np == null) return;

        FaceTo(np.mTran);
        Vector3 vDir = np.mTran.position - transform.position;
        vDir.Normalize();
        transform.position += vDir * Time.deltaTime * mSpeed * 4f;

        mIsMove = true;
        mActor.PlayAnimation(Actor.AnimationActor.ANI_WALK);
        mActor.SetAnimationSpeed(Actor.AnimationActor.ANI_WALK, 2f);

        float dis = Vector3.Distance(transform.position, np.mTran.position);
        if (dis < 0.01f)
        {
            np.mIsEntered = true;
            np = NextNearPath();
            if (np != null)
            {
                MoveToBattleEndPos();
            }
            else
            {
                mActor.SetAnimationSpeed(Actor.AnimationActor.ANI_WALK, 1f);
                GameMain.Instance().BattleControl.BattleState = Battle_Control.eBattleState.eBattle_End;
                mHeroState = eHeroState.HEROSTATE_IDLE;
                return;
            }
        }
    }

    NearPath FindNearPath()
    {
        List<NearPath> listEndPos = GameMain.Instance().BattleControl.ListBattleEndPos;

        int iSaveIndex = -1;
        float fSavePos = float.MaxValue;
        for (int i = 0; i < listEndPos.Count; ++i)
        {
            float fTempPos = Vector3.Distance(transform.position, listEndPos[i].mTran.position);
            if (fSavePos > fTempPos && listEndPos[i].mIsEntered == false)
            {
                fSavePos = fTempPos;
                iSaveIndex = i;                
            }
        }

        for (int i = 0; i < iSaveIndex; ++i)
        {
            listEndPos[i].mIsEntered = true;
        }
        
        return listEndPos[iSaveIndex];
    }

    NearPath NextNearPath()
    {
        List<NearPath> listEndPos = GameMain.Instance().BattleControl.ListBattleEndPos;
        for (int i = 0; i < listEndPos.Count; ++i)
        {
            if (listEndPos[i].mIsEntered == false)
            {
                return listEndPos[i];
            }
        }

        return null;
    }

    void HeroDie()
    {
        if (mHeroState == eHeroState.HEROSTATE_NONE) return;

        BattleUI_Control bcUI = UIManager.Instance().GetBattleUI() as BattleUI_Control;
        if (bcUI != null)
        {
            bcUI.DestroyHPGauge(HeroUid);
            IsDie = true;
            GameMain.Instance().BattleControl.CheckEndBattle();
        }

        if (!mActor.PlayAnimation(Actor.AnimationActor.ANI_DIE1))
        {
            StartCoroutine(HeroDeathAlphaFade());
            mHeroState = eHeroState.HEROSTATE_NONE;
        }
    }

    IEnumerator HeroDeathAlphaFade()
    {
        for (int iFadeCount = 7; iFadeCount >= 0; --iFadeCount)
        {
            for (int iAlpha = 10; iAlpha >= 0; --iAlpha)
            {
                for (int i = 0; i < mListSR.Count; ++i)
                {
                    mListSR[i].color = new Color(1f, 1f, 1f, ((float)iAlpha) * 0.1f);
                }

                yield return null;
            }

            float fWaitSeconds = (float)iFadeCount * 0.001f;
            yield return new WaitForSeconds(fWaitSeconds);
        }
    }

    void BeHit()
    {
        if (mActor.IsPlaying(Actor.AnimationActor.ANI_ATT1)) return;

        if (!mActor.PlayAnimation(Actor.AnimationActor.ANI_HIT))
        {
            if (mIsMove)
            {
                mHeroState = eHeroState.HEROSTATE_WALK;
            }
            else
            {
                mHeroState = eHeroState.HEROSTATE_IDLE;
            }
        }
    }

    void AttHero()
    {
        //if (mActor.IsPlaying(Actor.AnimationActor.ANI_IDLE) || mActor.IsPlaying(Actor.AnimationActor.ANI_WALK))
        {
            FaceTo(mTarget);

            if (!mActor.PlayAnimation(Actor.AnimationActor.ANI_ATT1))
            {
                mHeroState = eHeroState.HEROSTATE_IDLE;
            }
        }
    }

    void SearchTarget()
    {
        if (mTarget == null)
        {
            mTarget = GetAggroHero();
            //if (mTarget == null)
            //{                
            //    mActor.PlayAnimation(Actor.AnimationActor.ANI_IDLE, true);
            //}

            mActor.PlayAnimation(Actor.AnimationActor.ANI_IDLE);
        }
        else
        {            
            //if (AttRangeCheck())
            //{
            //    mHeroState = eHeroState.HEROSTATE_ATT;
            //}
            //else
            {
                mHeroState = eHeroState.HEROSTATE_WALK;
            }
        }
    }

    int mCurSkillNo = 0;
    bool AttRangeCheck()
    {
        string stCurSkill = "SkillCol/" + mCurSkillNo.ToString();
        Transform tSkillRange = HeroObj.transform.FindChild(stCurSkill);
        if (tSkillRange != null)
        {
            Collider2D col = tSkillRange.GetComponent<Collider2D>();
            if (col != null)
            {
                Collider2D targetCol = mTarget.HeroObj.GetComponent<Collider2D>();
                if (targetCol != null)
                {
                    return col.bounds.Intersects(targetCol.bounds);
                }
            }
        }

        return false;
    }

    Hero_Control GetAggroHero()
    {
        //int maxAggro = 0;
        //Hero_Control targetHero = null;

        //foreach( var elem in DicAggro)
        //{
        //    if (elem.Key.IsDie) continue;

        //    if (maxAggro < elem.Value)
        //    {
        //        maxAggro = elem.Value;
        //        targetHero = elem.Key;
        //    }
        //}

        //if (targetHero == null) return null;

        Battle_Control bc = GameMain.Instance().BattleControl;
        if (bc == null) return null;
        if (MyTeam)
        {
            return GetNearTargetHero(bc.ListEnemyHeroes);
        }
        else
        {
            return GetNearTargetHero(bc.ListMyHeroes);
        }
    }

    Hero_Control GetNearTargetHero(List<Hero_Control> listHero)
    {
        int iIndex = -1;
        float fSaveDis = float.MaxValue;
        for (int i = 0; i < listHero.Count; ++i)
        {
            float fTemp = Vector3.Distance(transform.position, listHero[i].transform.position);
            if (fSaveDis > fTemp && listHero[i].IsDie == false)
            {
                iIndex = i;
            }
        }

        return listHero[iIndex];
    }

    void Walk()
    {
        if (mTarget == null) return;

        string stTargetPath = null;
        int iCount = 0;
        AttPos = MoveDir(mTarget, ref iCount);
        if (AttPos == eAttPos.ATTPOS_NONE) return;

        if (AttPos == eAttPos.ATTPOS_LEFT)
        {
            stTargetPath = "AttackedPos/Left";
        }
        else if (AttPos == eAttPos.ATTPOS_RIGHT)
        {
            stTargetPath = "AttackedPos/Right";
        }

        Transform tPos = mTarget.transform.Find(stTargetPath);
        if (tPos == null) return;
        Vector3 vPos = tPos.position;
        if (iCount > 0)
        {
            if (mTarget.Target != this)
            {
                vPos += new Vector3(0, 0.15f, 0);
            }
        }

        float dis = Vector3.Distance(transform.position, vPos);
        if (dis < 0.01f)
        {
            FaceTo(mTarget);

            mIsMove = false;

            if (AttRangeCheck())
            {
                mHeroState = eHeroState.HEROSTATE_ATT;
            }
            else
            {
                mHeroState = eHeroState.HEROSTATE_IDLE;
            }
        }
        else
        {
            FaceTo(mTarget);
            Vector3 vDir = vPos - transform.position;
            vDir.Normalize();
            transform.position += vDir * Time.deltaTime * mSpeed * 2f;

            mIsMove = true;
            mActor.PlayAnimation(Actor.AnimationActor.ANI_WALK);
        }
    }    

    void FaceTo(Hero_Control targetHero)
    {
        Vector3 vDir = targetHero.transform.position - transform.position;
        if (vDir.x > 0)
        {
            HeroObj.transform.localRotation = Quaternion.Euler(0, 180, 0);            
        }
        else
        {
            HeroObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FaceTo(Transform targetHero)
    {
        Vector3 vDir = targetHero.position - transform.position;
        if (vDir.x > 0)
        {
            HeroObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            HeroObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    eAttPos MoveDir(Hero_Control targetHero, ref int iCount)
    {
        Battle_Control bc = GameMain.Instance().BattleControl;
        if (bc == null) return eAttPos.ATTPOS_NONE;

        eAttPos eResultPos = eAttPos.ATTPOS_NONE;

        bool bPosLeft = false;
        Vector3 vDir = targetHero.transform.position - transform.position;
        if (vDir.x > 0)
        {
            bPosLeft = true;
        }

        if (MyTeam)
        {
            mListAttPos = bc.ListMyHeroes;
        }
        else
        {
            mListAttPos = bc.ListEnemyHeroes;
        }

        int iLeftAttPosCount = 0;
        int iRightAttPosCount = 0;
        for (int i = 0; i < mListAttPos.Count; ++i)
        {
            if (mListAttPos[i] == this) continue;
            if (mListAttPos[i].AttPos == eAttPos.ATTPOS_NONE) continue;
            if (mListAttPos[i].Target == null) continue;

            if (targetHero.HeroUid.Equals(mListAttPos[i].Target.HeroUid))
            {
                if ((int)mListAttPos[i].AttPos < (int)eAttPos.ATTPOS_RIGHT)
                {
                    iLeftAttPosCount++;
                }
                else
                {
                    iRightAttPosCount++;
                }
            }
        }

        if (bPosLeft)
        {
            if (iLeftAttPosCount == ATTPOS_MAXCOUNT)
            {
                iCount = iRightAttPosCount;
                eResultPos = eAttPos.ATTPOS_RIGHT;
            }
            else
            {
                iCount = iLeftAttPosCount;
                eResultPos = eAttPos.ATTPOS_LEFT;
            }
        }
        else
        {
            if (iRightAttPosCount == ATTPOS_MAXCOUNT)
            {
                iCount = iLeftAttPosCount;
                eResultPos = eAttPos.ATTPOS_LEFT;
            }
            else
            {
                iCount = iRightAttPosCount;
                eResultPos = eAttPos.ATTPOS_RIGHT;
            }
        }

        return eResultPos;
    }

    public void OnBeHit(Hero_Control atthero, int iSkillNo )
    {
        if (IsDie) return;

        if (DamagedHero(atthero, iSkillNo))
        {
            StartCoroutine(DamagedHeroColor(0.1f));

            mHeroState = eHeroState.HEROSTATE_HIT;
        }
    }

    bool DamagedHero(Hero_Control atthero, int iSkillNo)
    {
        // if(immune) return false

        HP -= atthero.Atk;
		BattleUI_Control bcUI = UIManager.Instance().GetBattleUI() as BattleUI_Control;
        if (bcUI == null) return false;

        float amount =  (float)HP / (float)MaxHP;
        bcUI.UpdateHPGauge(mHeroUid, amount);

        GameObject goEfc = EffectManager.Instance().GetEffect(EffectManager.eEffectType.EFFECT_BATTLE_HIT); 
        if (goEfc != null)
        {
            Transform tCen = HeroObj.transform.FindChild("ef_Center");
            if( tCen != null )
            {
                Battle_Control bc = GameMain.Instance().BattleControl;
                Transform tEffect = bc.transform.FindChild("Effect");
                
                goEfc.transform.parent = tEffect; 
                goEfc.transform.position = tCen.position;
                
                ParticleSystem [] pcs = goEfc.GetComponentsInChildren<ParticleSystem>();
                if (pcs != null)
                {
                    for (int i = 0; i < pcs.Length; ++i)
                    {
                        Renderer render = pcs[i].GetComponent<Renderer>();
                        if (render != null)
                        { 
                            render.sortingOrder = MaxSortingOrderNo;
                            render.sortingLayerName = "Hero";
                        }
                    }
                }
            }
        }

        if (HP <= 0)
        {
            mHeroState = eHeroState.HEROSTATE_DIE;
            return false;
        }
        else
        {
            Vector3 vDir = atthero.transform.position - transform.position;
            vDir.Normalize();
            if (vDir.x > 0)
            {
                mBlowDir = 1;
            }
            else
            {
                mBlowDir = -1;
            }

            mBlowTime = atthero.BlowPower;
        }

        return true;
    }

    IEnumerator DamagedHeroColor(float fDelay)
    {
        for (int i = 0; i < mListSR.Count; ++i)
        {
            mListSR[i].color = new Color(1f, 142f/255f, 54f/255f);
        }

        yield return new WaitForSeconds(fDelay);

        for (int i = 0; i < mListSR.Count; ++i)
        {
            mListSR[i].color = Color.white;
        }
    }

    void HPGaugePosUpdate()
    {
        if (IsDie) return;

		BattleUI_Control bcUI = UIManager.Instance().GetBattleUI() as BattleUI_Control;
        if (bcUI == null) return;
        bcUI.UpdatePosHPGauge(mHeroUid, mEf_HP);
    }
}