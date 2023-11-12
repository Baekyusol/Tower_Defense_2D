using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;     // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab;  // �� ü�� slider
    [SerializeField]
    private Transform canvasTransform;  // UI ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float spawnTime;            // �� ���� �ֱ�

    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList;

    public List<Enemy> EnemyList => enemyList;

    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine("SpawnEnemy");        
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);    //�� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();      // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(this, wayPoints);                         // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);     // spawnTime �ð� ���� ���
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ��
        if(type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ü�� - 1
            playerHP.TakeDamage(1);
        }
        else if(type == EnemyDestroyType.kill)
        {
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // �� ü�� ��Ÿ���� slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�.
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1,1,1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �i�ƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI�� �ڽ��� ü�������� ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    
    }
}
