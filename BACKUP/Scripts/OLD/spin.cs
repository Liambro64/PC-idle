using UnityEngine;

public class spin : MonoBehaviour
{
    public Vector3 speed = new Vector3(0, 0, 5);
    // Update is called once per frame
    public void Update()
    {
        transform.Rotate(speed);
    }
}
