using UnityEngine;

/// <summary>
/// 简化版触发器生成小球脚本
/// 适合初学者使用
/// </summary>
public class SimpleBallSpawner : MonoBehaviour
{
    [Header("基本设置")]
    public GameObject ballPrefab;           // 小球预制体
    public Transform spawnPoint;            // 生成位置
    public bool onlyTriggerOnce = true;     // 是否只触发一次

    private bool hasSpawned = false;        // 是否已经生成过

    void Start()
    {
        // 如果没有指定生成点，使用当前位置
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    // 当有对象进入触发器时调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查是否为玩家
        if (other.CompareTag("Player"))
        {
            // 检查是否已经生成过
            if (onlyTriggerOnce && hasSpawned)
                return;

            // 生成小球
            if (ballPrefab != null && spawnPoint != null)
            {
                Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
                hasSpawned = true;

                //Debug.Log("小球已生成！");
            }
        }
    }
}