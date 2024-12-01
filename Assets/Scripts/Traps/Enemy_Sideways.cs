//Trap này chuyển động qua lại trong 1 phạm vi nhất định( dùng cho Saw)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movementDistance;// Khoảng cách di chuyển từ vị trí ban đầu
    [SerializeField] private float speed;//Tốc độ di chuyển
    [SerializeField] private float damage;// Lượng sát thương gây ra
    private bool movingLeft;// Xác định hướng di chuyển hiện tại
    private float leftEdge;// Biên giới bên trái không di chuyển qua
    private float rightEdge;// Biên giới bên phải không di chuyển qua

    private void Awake()
    {
        // Xác định giới hạn di chuyển của kẻ địch từ vị trí ban đầu
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Kiểm tra nếu kẻ địch đang di chuyển sang trái
        if (movingLeft)
        {
            // Nếu vị trí hiện tại của kẻ địch vẫn còn lớn hơn biên giới bên trái, tiếp tục di chuyển sang trái
            if (transform.position.x > leftEdge)
            {
                // Di chuyển đối tượng theo chiều x với tốc độ đã chỉ định
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;// Nếu đã tới giới hạn trái, đổi hướng sang phải
        }
        else
        {
            // Nếu vị trí hiện tại của kẻ địch vẫn còn nhỏ hơn biên giới bên phải, tiếp tục di chuyển sang phải
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;// Nếu đã tới giới hạn phải, đổi hướng sang trái
        }
    }
    // Phương thức được gọi khi có đối tượng khác va chạm với kẻ địch
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")//Nếu va chạm là người chơi
        {
            collision.GetComponent<Health>().TakeDamage(damage);// Gây sát thương
        }
    }
}
