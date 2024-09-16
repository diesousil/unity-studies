using UnityEngine;

public class Movement:MonoBehaviour
{
    private const float grade = Mathf.PI / 360;
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.x -= grade;
            this.transform.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.x += grade;
            this.transform.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.y-= grade;
            this.transform.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.R))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.y+= grade;
            this.transform.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.T))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.z-= grade;
            this.transform.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            Quaternion rotation = this.transform.rotation;
            rotation.z+= grade;
            this.transform.rotation = rotation;
        }



        if (Input.GetKey(KeyCode.Z))
        {
            Vector3 position = this.transform.position;
            position.z--;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.X))
        {
            Vector3 position = this.transform.position;
            position.z++;
            this.transform.position = position;
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 position = this.transform.position;
            position.x--;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 position = this.transform.position;
            position.x++;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 position = this.transform.position;
            position.y++;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 position = this.transform.position;
            position.y--;
            this.transform.position = position;
        }

    }

}