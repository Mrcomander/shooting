using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("体力")]
    public int maxHP = 1000;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }






    // 即時ダメージ
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        Debug.Log("現在HP：" + currentHP);
    }

    // 継続ダメージ（例：1秒ごとに10ダメージを5秒間）
    public void TakeContinuousDamage(int damagePerTick, float intervalSeconds, int totalSeconds)
    {
        StartCoroutine(ContinuousDamageRoutine(damagePerTick, intervalSeconds, totalSeconds));
    }
    //継続処理
    private IEnumerator ContinuousDamageRoutine(int damagePerTick, float intervalSeconds, int totalSeconds)
    {
        int tickCount = Mathf.FloorToInt(totalSeconds / intervalSeconds);

        for (int i = 0; i < tickCount; i++)
        {
            currentHP -= damagePerTick;

            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
                yield break; // 死亡したら継続ダメージ終了
            }

            Debug.Log("現在HP：" + currentHP);

            yield return new WaitForSeconds(intervalSeconds);
        }
    }







    // 即時回復
    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
            currentHP = maxHP;

        Debug.Log("現在HP：" + currentHP);
    }

    //継続回復
    public void HealContinuous(int healPerTick, float intervalSeconds, int totalSeconds)
    {
        StartCoroutine(ContinuousHealRoutine(healPerTick, intervalSeconds, totalSeconds));
    }
    //継続処理
    private IEnumerator ContinuousHealRoutine(int healPerTick, float intervalSeconds, int totalSeconds)
    {
        int tickCount = Mathf.FloorToInt(totalSeconds / intervalSeconds);

        for (int i = 0; i < tickCount; i++)
        {
            currentHP += healPerTick;

            if (currentHP > maxHP)
                currentHP = maxHP;

            Debug.Log("現在HP：" + currentHP);

            yield return new WaitForSeconds(intervalSeconds);
        }
    }



   

    // 死亡処理
    void Die()
    {
        Debug.Log(gameObject.name + " は倒れた！");
        Destroy(gameObject);
    }
}