using UnityEngine;
public class Player: MonoBehaviour
{
    public enum CombatState { idle, shoot, reload, ADS, Pull }
    public CombatState CurrentCombatState;
}