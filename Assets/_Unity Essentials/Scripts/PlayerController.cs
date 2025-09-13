using UnityEngine;

//玩家移动和旋转
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;//移动速度
    public float rotationspeed = 120.0f;//旋转速度
    public float jumpForce = 5.0f;//跳跃高度

    private Rigidbody rb;//引用玩家刚体

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    //处理移动和旋转逻辑
    private void FixedUpdate()
    {
        //移动逻辑
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        //旋转逻辑
        float turn = Input.GetAxis("Horizontal") * rotationspeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}