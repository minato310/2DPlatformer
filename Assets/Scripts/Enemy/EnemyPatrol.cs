//Mô tả hành vi tuần tra của một đối tượng kẻ thù trong game Unity, di chuyển qua lại giữa hai điểm leftEdge và rightEdge.

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    //Đây là hai điểm mà kẻ thù sẽ di chuyển qua lại.
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;// Tham chiếu tới đối tượng kẻ thù
                                             // Transform này sẽ được thay đổi vị trí và kích thước trong quá trình di chuyển.

    [Header("Movement parameters")]
    [SerializeField] private float speed;//Tốc độ của enemy
    private Vector3 initScale;//Kích thước ban đầu của kẻ thù, lưu lại để điều chỉnh khi kẻ thù quay đầu.
    private bool movingLeft;//Biến boolean xác định hướng di chuyển của kẻ thù. Nếu true, kẻ thù sẽ di chuyển về bên trái, nếu false, di chuyển về bên phải.

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;//Thời gian kẻ thù sẽ đứng yên trước khi đổi hướng di chuyển.
    private float idleTimer;//Đếm thời gian cho trạng thái đứng yên

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;//Tham chiếu đến Animator, dùng để kiểm soát các animation của kẻ thù

    private void Awake()//Hàm này được gọi khi đối tượng được khởi tạo.
    {
        initScale = enemy.localScale;//initScale được gán giá trị kích thước ban đầu của kẻ thù để sau này có thể điều chỉnh khi kẻ thù đổi hướng.
    }
    private void OnDisable()//Hàm này được gọi khi đối tượng bị vô hiệu hóa.
    {
        anim.SetBool("moving", false);//Nó dừng animation của kẻ thù bằng cách đặt "moving" thành false.
    }

    //Hàm này quyết định xem kẻ thù sẽ di chuyển về phía bên trái hay phải dựa trên biến movingLeft.
    private void Update()
    {
        //Nếu movingLeft là true, nó kiểm tra xem kẻ thù đã đến leftEdge chưa. Nếu chưa, kẻ thù sẽ di chuyển về bên trái bằng cách gọi MoveInDirection(-1).
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else //Nếu kẻ thù đã đến leftEdge, nó sẽ gọi hàm DirectionChange() để kẻ thù dừng lại và đổi hướng.
                DirectionChange();
        }
        else //Nếu movingLeft là false, kẻ thù sẽ di chuyển về bên phải và cũng sẽ đổi hướng khi đến rightEdge.
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }
    //Khi kẻ thù đến điểm dừng (bên trái hoặc phải), hàm này được gọi để tạm dừng kẻ thù.
    private void DirectionChange()
    {
        anim.SetBool("moving", false); // Dừng animation di chuyển
        idleTimer += Time.deltaTime;// Tăng bộ đếm thời gian idle

        //Nếu idleTimer vượt quá idleDuration, kẻ thù đổi hướng di chuyển bằng cách đảo ngược biến movingLeft.
        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }
    // Hàm này di chuyển kẻ thù theo hướng được chỉ định bởi _direction.
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;//Reset bộ đếm idle mỗi khi kẻ thù di chuyển.
        anim.SetBool("moving", true);// Kích hoạt animation di chuyển

        //Quay mặt kẻ thù về hướng di chuyển. Nếu _direction là -1, kẻ thù quay về bên trái, nếu 1 thì quay về bên phải.
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Cập nhật vị trí của kẻ thù theo tốc độ và hướng di chuyển, sử dụng Time.deltaTime để đảm bảo chuyển động mượt mà theo thời gian thực.
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}