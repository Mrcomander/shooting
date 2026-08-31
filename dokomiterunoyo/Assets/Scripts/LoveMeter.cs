using UnityEngine;
using System.Collections;

public class LoveMeter : MonoBehaviour
{
    [Header("ラブメーター")]
    public int InitialLove = 50;
    public int currentLove;
    public int maxLove = 100;

    void Start()
    {
        currentLove = InitialLove;
    }






    // 即時ダメージ
    public void LoveTakeDamage(int damage)
    {
        currentLove -= damage;

        if (currentLove <= 0)
        {
            currentLove = 0;
            Die();
        }

        Debug.Log("現在ラブ：" + currentLove);
    }

    // 継続ダメージ（例：1秒ごとに10ダメージを5秒間）
    public void LoveTakeContinuousDamage(int damagePerTick, float intervalSeconds, int totalSeconds)
    {
        StartCoroutine(ContinuousDamageRoutine(damagePerTick, intervalSeconds, totalSeconds));
    }
    //継続処理
    private IEnumerator ContinuousDamageRoutine(int damagePerTick, float intervalSeconds, int totalSeconds)
    {
        int tickCount = Mathf.FloorToInt(totalSeconds / intervalSeconds);

        for (int i = 0; i < tickCount; i++)
        {
            currentLove -= damagePerTick;

            if (currentLove <= 0)
            {
                currentLove = 0;
                Die();
                yield break; // 死亡したら継続ダメージ終了
            }

            Debug.Log("現在ラブ：" + currentLove);

            yield return new WaitForSeconds(intervalSeconds);
        }
    }







    // 即時回復
    public void LoveHeal(int amount)
    {
        currentLove += amount;

        if (currentLove > maxLove)
        {
            currentLove = maxLove;
            LoveDie();
        }    


        Debug.Log("現在ラブ：" + currentLove);
    }

    //継続回復
    public void LoveHealContinuous(int healPerTick, float intervalSeconds, int totalSeconds)
    {
        StartCoroutine(ContinuousHealRoutine(healPerTick, intervalSeconds, totalSeconds));
    }
    //継続処理
    private IEnumerator ContinuousHealRoutine(int healPerTick, float intervalSeconds, int totalSeconds)
    {
        int tickCount = Mathf.FloorToInt(totalSeconds / intervalSeconds);

        for (int i = 0; i < tickCount; i++)
        {
            currentLove += healPerTick;

            if (currentLove > maxLove)
            {
                currentLove = maxLove;
                LoveDie();
                yield break;
            }

            Debug.Log("現在ラブ：" + currentLove);

            yield return new WaitForSeconds(intervalSeconds);
        }
    }


    //好感度MAX
    void LoveDie()
    {
        Debug.Log(gameObject.name + " は恋におちた");
        Destroy(gameObject);
    }

    // 死亡処理
    void Die()
    {
        Debug.Log(gameObject.name + " は振られた！");
        Destroy(gameObject);
    }
}
