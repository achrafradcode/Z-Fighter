using UnityEngine;
using System.Collections;

public class PlayerAIM : MonoBehaviour {
    public Transform Camera;//main camera
    public GameObject Marker;//the allocation of the enemy-target
    public float maxdist = 10;//detection distance

    private GameObject nearest = null;//nearest enemy
    public static bool HasTarget = false;//it used in PlayerShooting (shooting only if player has target)
    private RaycastHit2D hit2;//for the visibility system
    public float mindist;//used in cycle
	void Start () {
        Marker.SetActive(false);//marker deactivation
	}
	

	void Update () {

        mindist = 100;//reset distance the nearest target
        nearest = null;//reset targets

        //making all enemies list
        GameObject[] List;
        List = GameObject.FindGameObjectsWithTag("Enemy");
        //find the nearest Enemy
        foreach (GameObject go in List)
        {
            hit2 = Physics2D.Raycast((Vector2)transform.position, (Vector2)go.transform.position - (Vector2)transform.position, 20, 513);//a ray from the turret to the enemy
            if (hit2.collider != null)
            {
                if (!hit2.collider.gameObject.CompareTag("Wall"))//ray does not touch the walls
                {
                    float tmp2 = Vector3.Distance(transform.position, go.transform.position);
                    if (tmp2 < mindist & tmp2 < maxdist)//if the distance is minimal and is included in the range
                    {
                        mindist = tmp2;
                        nearest = go;
                    }
                    //show rays:
                    //Debug.DrawLine((Vector2)transform.position, (Vector2)go.transform.position);
                } 
                
            }
           
        }

        //rotating to target
        if (nearest != null)//if has target
        {
            HasTarget = true;
            Marker.SetActive(true);//marker activation

            //follow the target
            Vector3 moveDirection = nearest.transform.position - transform.position;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            }
        }
        else//rotating to move direction
        {
            HasTarget = false;
            Marker.SetActive(false);//marker deactivation
            transform.rotation = Quaternion.Euler(0, 0, Player.Move_Angle+90);
        }
	}
    void LateUpdate() {
        if (Marker.activeInHierarchy) Marker.transform.position = nearest.transform.position;//moving marker to target pos
        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -3);//moving camera to player pos
    }
}
