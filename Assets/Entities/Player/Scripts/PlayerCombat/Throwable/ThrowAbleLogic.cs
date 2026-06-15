using System.Collections.Generic;
using UnityEngine;
public class ThrowAbleLogic : MonoBehaviour
{
    [SerializeField] SmokeNade Smoke;
    [SerializeField] public int SmokeCount;
    [SerializeField] DistractionObject Distraction;
    [SerializeField] public int DistractionCount;
    [SerializeField] FlashNade Flash;
    [SerializeField] public int FlashCount;

    Dictionary<int, BaseNade> NadeMap;
    Dictionary<int, int> NadeCount;

    public int MaxNadeCount = 2;
    int CurrentIndex = 1;

    private void Start()
    {
        NadeMap = new Dictionary<int, BaseNade>
        {
            { 1, Flash },
            { 2, Smoke },
            { 3, Distraction }
        };

        NadeCount = new Dictionary<int, int>
        {
            { 1, FlashCount },
            { 2, SmokeCount },
            { 3, DistractionCount }
        };
    }

    public void IncreaseSmoke(int amount) => NadeCount[2] += amount;
    public void IncreaseFlash(int amount) => NadeCount[1] += amount;
    public void IncreaseDistraction(int amount) => NadeCount[3] += amount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) CurrentIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) CurrentIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) CurrentIndex = 3;
    }

    public int GetIndex() => CurrentIndex;

    public int GetCount(int index) => NadeCount[index];

    public bool CanThrow() => NadeCount[CurrentIndex] > 0;

    public void ThrowNadeLong()
    {
        if (!CanThrow()) return;
        Instantiate(NadeMap[CurrentIndex], transform.position, transform.rotation);
        NadeCount[CurrentIndex]--;
    }
}