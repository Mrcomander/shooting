using UnityEngine;
using System.Collections;

public class TestCaller : MonoBehaviour
{
    public PlayerHealth player;
    public LoveMeter loveMeter;

    void Update()
    {
        //プレイヤーHP
        // Aキーで即時ダメージ
        if (Input.GetKeyDown(KeyCode.A))
        {
            player.TakeDamage(50);
        }

        // Sキーで継続ダメージ（1秒ごとに10ダメージを5秒間）
        if (Input.GetKeyDown(KeyCode.S))
        {
            player.TakeContinuousDamage(10, 1f, 5);
        }

        // Dキーで継続回復（1秒ごとに20回復を5秒間）
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.HealContinuous(20, 1f, 5);
        }
        //Fキーで即回復
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Heal(50);
        }


        //ラブメーター
        // Zキーで即時ダメージ
        if (Input.GetKeyDown(KeyCode.Z))
        {
            loveMeter.LoveTakeDamage(25);
        }

        // Xキーで継続ダメージ（1秒ごとに1ダメージを5秒間）
        if (Input.GetKeyDown(KeyCode.X))
        {
            loveMeter.LoveTakeContinuousDamage(1, 1f, 5);
        }

        // Cキーで継続回復（1秒ごとに1回復を5秒間）
        if (Input.GetKeyDown(KeyCode.C))
        {
            loveMeter.LoveHealContinuous(1, 1f, 5);
        }
        //Vキーで即回復
        if (Input.GetKeyDown(KeyCode.V))
        {
            loveMeter.LoveHeal(25);
        }
    }
}

