using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy _goblinPrefab;
    [SerializeField] private Enemy _skeletonPrefab;

    private Enemy _enemy;

    public Enemy SpawnEnemy(LevelEnemyInfo levelEnemyInfo)
    {
        switch(levelEnemyInfo.enemyType)
        {
            case EnemyType.Sceletone:
                _enemy = Instantiate(_skeletonPrefab, levelEnemyInfo.position, Quaternion.identity, transform);
                _enemy.SetCharacteristics(levelEnemyInfo.characteristics);
                return _enemy;
            case EnemyType.Goblin:
                _enemy = Instantiate(_goblinPrefab, levelEnemyInfo.position, Quaternion.identity, transform);
                _enemy.SetCharacteristics(levelEnemyInfo.characteristics);
                return _enemy;
            default: 
                return null;
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
