using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Skill", menuName = "Summon Skill/Seeking Spell shots")]
public class SeekingSpellshots : Skills
{
    [SerializeField] GameObject skillPrefabs;
    [SerializeField] float speed;
    [SerializeField] int missileCount = 5;
    [SerializeField] float sizeAttack;
    [SerializeField] LayerMask layerMask;
    public override void Activate()
    {
        PlayerController.Instance.StartCoroutine(FireMissiles());
    }

    IEnumerator FireMissiles()
    {
        // Find enemies
        List<GameObject> enemies = FindClosestEnemies(PlayerController.Instance.transform.position);

        // Determine targets for missiles
        List<GameObject> targets = SelectTargets(enemies);

        // Fire missiles
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                GameObject missile = Instantiate(skillPrefabs, PlayerController.Instance.transform.position, Quaternion.identity);
                HomingMissile homingMissile = missile.GetComponent<HomingMissile>();
                if (homingMissile != null)
                {
                    homingMissile.SetTarget(target.transform, speed);
                }
            }
        }

        yield return null;
    }

    private List<GameObject> FindClosestEnemies(Vector3 playerPosition)
    {
        int i = 0;
        Collider2D[] objectToHit = Physics2D.OverlapCircleAll(PlayerController.Instance.transform.position, sizeAttack, layerMask, 0);
        GameObject[] enemies = new GameObject[objectToHit.Length];
        foreach (Collider2D obj in objectToHit)
        {
            enemies[i] = obj.gameObject;
            i++;
        }
        List<GameObject> sortedEnemies = enemies.OrderBy(e => Vector3.Distance(playerPosition, e.transform.position)).ToList();
        return sortedEnemies;
    }

    private List<GameObject> SelectTargets(List<GameObject> enemies)
    {
        List<GameObject> targets = new List<GameObject>();

        if (enemies.Count >= missileCount)
        {
            targets = enemies.Take(missileCount).ToList();
        }
        else if (enemies.Count > 1 && enemies.Count < missileCount)
        {
            int missilesPerEnemy = missileCount / enemies.Count;
            int extraMissiles = missileCount % enemies.Count;

            foreach (var enemy in enemies)
            {
                for (int i = 0; i < missilesPerEnemy; i++)
                {
                    targets.Add(enemy);
                }
            }

            for (int i = 0; i < extraMissiles; i++)
            {
                targets.Add(enemies[i]);
            }
        }
        else
        {
            targets.AddRange(enemies);
        }

        return targets;
    }
}
