using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;//Tốc độ di chuyển của nhân vật.
    [SerializeField] private float jumpPower;//Lực nhảy

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; // Khoảng thời gian nhân vật có thể nhảy ngay sau khi rời khỏi mặt đất 
    private float coyoteCounter; //Bộ đếm theo dõi thời gian đã trôi qua kể từ khi nhân vật rời mặt đất

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;//Số lần nhảy bổ sung mà nhân vật có thể thực hiện khi đang trên không
    private int jumpCounter;//Bộ đếm để theo dõi số lần nhảy còn lại

    //Lực nhảy ngang và nhảy dọc khi nhân vật thực hiện nhảy từ tường
    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY; 

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;//Lớp dùng để xác định mặt đất.
    [SerializeField] private LayerMask wallLayer;//Lớp dùng để xác định tường

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;//Âm thanh khi nhảy

    private Rigidbody2D body;//Tham chiếu đến thành phần Rigidbody2D của nhân vật.
    private Animator anim;//Tham chiến đến Animator dùng để điều khiển hoạt cảnh của nhân vật
    private BoxCollider2D boxCollider;//Tham chiếu đến BoxCollider2D, được dùng để xác định vùng va chạm (collision) của nhân vật.
    private float wallJumpCooldown;//Biến này lưu trữ thời gian hồi chiêu cho hành động nhảy từ tường
    private float horizontalInput;//Biến này lưu trữ giá trị đầu vào ngang (hướng trái/phải) từ người chơi

    private Vector3 originalScale; // Biến lưu kích thước ban đầu của Player
    //Lấy tham chiếu đến Rigidbody2D, Animator, và BoxCollider2D của nhân vật để sử dụng sau này.
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale; // Lưu kích thước ban đầu khi khởi tạo
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");//Lấy giá trị đầu vào của người chơi (di chuyển sang trái hoặc phải)
        //Nhân vật sẽ lật mặt tùy theo hướng di chuyển.
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        //Đặt các tham số Animation để hiển thị các trạng thái như chạy và chạm đất.
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Kiểm tra xem người chơi có nhấn phím Space để nhảy hay không.
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Điều chỉnh chiều cao nhảy. Nếu nhả phím Space giữa lúc nhảy, chiều cao nhảy sẽ bị giảm đi một nửa.
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        //Nếu nhân vật chạm tường, trọng lực được vô hiệu hóa và nhân vật không thể rơi xuống cho đến khi nhảy khỏi tường.
        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;// điều chỉnh độ mạnh của trọng lực tác động lên vật thể
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);//Cập nhật vận tốc (velocity) của nhân vật.

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Nếu nhân vật đang đứng trên mặt đất, biến coyoteCounter được đặt lại về giá trị coyoteTime.
                jumpCounter = extraJumps; //Đặt lại jumpCounter về số lượng nhảy bổ sung (extraJumps) cho phép nhân vật thực hiện thêm các lần nhảy (nếu có) khi đang đứng trên mặt đất.
            }
            else
                coyoteCounter -= Time.deltaTime; //Nếu nhân vật không đứng trên mặt đất, coyoteCounter sẽ được giảm dần theo thời gian (Time.deltaTime).
        }
    }
    //Kiểm tra nếu còn thời gian Coyote Time hoặc số lần nhảy phụ thì cho phép nhân vật nhảy. Nếu nhân vật đang trên tường, gọi WallJump().
    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;//Điều kiện này kiểm tra xem nhân vật có đủ điều kiện để nhảy hay không

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())//Kiểm tra xem nhân vật có đang chạm vào tường hay không.
            WallJump();//Nếu đang chạm tường, gọi phương thức WallJump() để thực hiện nhảy từ tường
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);//Nếu nhân vật đang đứng trên mặt đất, áp dụng lực nhảy với vận tốc theo phương Y
            else//Nếu không đứng trên mặt đất, kiểm tra xem nhân vật có thể nhảy hay không dựa vào coyoteCounter
            {
           
                if (coyoteCounter > 0)//Nếu coyoteCounter lớn hơn 0, cho phép nhảy bình thường.
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else//Kiểm tra xem nhân vật còn nhảy bổ sung hay không (jumpCounter > 0).
                {
                    if (jumpCounter > 0) 
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);//Nếu có, cho phép nhân vật thực hiện nhảy bổ sung và giảm số lượng nhảy (jumpCounter--).
                        jumpCounter--;
                    }
                }
            }

            //Đặt lại coyoteCounter về 0 để đảm bảo nhân vật không thể thực hiện nhảy thêm sau khi đã nhảy, trừ khi lại chạm đất.
            coyoteCounter = 0;
        }
    }
    //Nhân vật sẽ nhảy khỏi tường theo hướng ngược lại với lực nhảy ngang và dọc được xác định bởi wallJumpX và wallJumpY.
    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    //Kiểm tra xem nhân vật có đang đứng trên mặt đất hay không.
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    //Kiểm tra xem nhân vật có đang chạm tường hay không
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    //Kiểm tra xem nhân vật có thể tấn công (đứng yên, không trên tường, và chạm đất) hay không.
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}