using AC;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindRose { Nord, Sud, Est, Ovest, NordEst, NordOvest, SudEst, SudOvest, Idle }
public enum Skin { Università, Pigiama, Sagra, Standard}
public class PlayerController : MonoBehaviour
{
    [SerializeField] List<SkeletonAnimation> skeletons;
    [SerializeField] int halfSideRadiusRange;
    [SerializeField] int sideRadiusRange;
    [SerializeField] Skin skinIniziale;
    WindRose newDirection;

    Player player;
    SkeletonAnimation currentSkeleton;

    Spine.TrackEntry track;
    private void Awake()
    {
        player = GetComponent<Player>();

        SetSkin();

        skeletons.ForEach(x => x.gameObject.SetActive(false));
        currentSkeleton = skeletons[0];
        SetNewDirection(WindRose.Sud);
        SetNewDirection(WindRose.Idle);
        halfSideRadiusRange = Mathf.Abs(halfSideRadiusRange);
        sideRadiusRange = Mathf.Abs(sideRadiusRange);


    }

    private void SetSkin()
    {
        skeletons.ForEach(x => x.initialSkinName = GetInitialSkin());
        if(skinIniziale == Skin.Università)
            skeletons[3].initialSkinName = "università";
    }

    private string GetInitialSkin()
    {
        switch (skinIniziale)
        {
            case Skin.Università:
                return "uni";
            case Skin.Pigiama:
                return "pigiama";
            case Skin.Sagra:
                return "sagra";
            case Skin.Standard:
                return "standard";
            default:
                return "standard";
        }
    }

    private void EnableSkeletonByIndex(int index)
    {
        currentSkeleton.gameObject.SetActive(false);
        skeletons[index].gameObject.SetActive(true);


        skeletons[index].loop = true;

        currentSkeleton = skeletons[index];

    }

    private void Update()
    {
        AnimationController();
    }

    public void AnimationController()
    {

        float currentSpeed = player.GetMoveSpeed();


        int rotation = (int)transform.eulerAngles.y;


        if (currentSpeed > 0)
        {

            if (FindRotation(90, sideRadiusRange, rotation)) //------------ destra
            {
                SetNewDirection(WindRose.Est);
            }
            else if (FindRotation(135, halfSideRadiusRange, rotation)) //------------ destra giù
            {
                SetNewDirection(WindRose.SudEst);
            }
            else if (FindRotation(45, halfSideRadiusRange, rotation)) //------------ destra su
            {
                SetNewDirection(WindRose.NordEst);
            }
            else if (FindRotation(180, sideRadiusRange, rotation)) //------------ giù
            {
                SetNewDirection(WindRose.Sud);
            }
            else if (FindRotation(0, sideRadiusRange, rotation)) //------------ su
            {
                SetNewDirection(WindRose.Nord);
            }
            else if (FindRotation(270, sideRadiusRange, rotation)) //------------ sinistra
            {
                SetNewDirection(WindRose.Ovest);
            }
            else if (FindRotation(225, halfSideRadiusRange, rotation)) //------------ sinistra giù
            {
                SetNewDirection(WindRose.SudOvest);
            }
            else if (FindRotation(315, halfSideRadiusRange, rotation)) //------------ sinistra su
            {
                SetNewDirection(WindRose.NordOvest);
            }
            SetAnimation("Walking");
        }
        else if(newDirection != WindRose.Idle)
        {
            SetNewDirection(WindRose.Idle);

        }

    }

    public void SetAnimation(string animation)
    {
        if(animation == "Sitting")
        {
            StartCoroutine(PlayerSit());
        }
        currentSkeleton.AnimationName = animation;
    }

    private IEnumerator PlayerSit()
    {
        currentSkeleton.AnimationName = "Sitting";
        yield return new WaitForSeconds(0.5f);
        currentSkeleton.AnimationName = "sit";
    }

    private void SetNewDirection(WindRose newDir)
    {
        if (newDirection != newDir)
        {
            newDirection = newDir;


            SetSkeleton();
        }
    }

    private void SetSkeleton()
    {
        switch (newDirection)
        {
            case WindRose.Nord:
                EnableSkeletonByIndex(6);
                break;
            case WindRose.Sud:
                EnableSkeletonByIndex(0);
                break;
            case WindRose.Est:
                EnableSkeletonByIndex(4);
                break;
            case WindRose.Ovest:
                EnableSkeletonByIndex(3);
                break;
            case WindRose.NordEst:
                EnableSkeletonByIndex(7);
                break;
            case WindRose.NordOvest:
                EnableSkeletonByIndex(5);
                break;
            case WindRose.SudEst:
                EnableSkeletonByIndex(2);
                break;
            case WindRose.SudOvest:
                EnableSkeletonByIndex(1);
                break;
            case WindRose.Idle:
                SetAnimation("Idle");
                break;
        }

        print(newDirection);


    }

    bool FindRotation(int angle, int range, int rotation)
    {

        int a, b;
        a = angle - range;
        b = angle + range;

        return rotation > a && rotation < b;
    }

    public void SetInitialSkin(int index)
    {
        skinIniziale = (Skin)index;
        SetSkin();
    }

}
