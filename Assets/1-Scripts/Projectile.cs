using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
enum ProjectileType
{
    arrow,
    spear
}

public class Projectile : MonoBehaviour
{
    public GameObject projectileOnWall;
    private Vector3 dir;
    public float multipler;
    public Vector3 ofset;
    [SerializeField] ProjectileType projectileType;
    public void SetDirection(Vector3 arrowDir)
    {
        dir = arrowDir;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        if(!collider.CompareTag("Player"))
        {
            AudioController.Instance.PlayAudio(AudioType.ArrowHit);

            GameObject arrowOnWallref = Instantiate(projectileOnWall, gameObject.transform.position + dir.normalized * multipler + ofset, gameObject.transform.rotation,WaveController.Instance.trash.transform);
            arrowOnWallref.transform.rotation = Quaternion.Euler(arrowOnWallref.transform.rotation.eulerAngles.x, arrowOnWallref.transform.rotation.eulerAngles.y, 180 +arrowOnWallref.transform.rotation.eulerAngles.z);
            arrowOnWallref.transform.localScale = gameObject.transform.localScale;
            //Destroy(arrowOnWallref, 20f);
        }
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject,0.2f);
    }
}
