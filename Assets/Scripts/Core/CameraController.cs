using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room Camera: Camera di chuyển đến vị trí phòng tiếp theo
    [SerializeField] private float speed;//Tốc độ khi di chuyển giữa các phòng
    private float currentPosX;//Vị trí X mà camera sẽ duy chuyển khi đổi phòng
    private Vector3 velocity = Vector3.zero;//velocity: Biến dùng để lưu trữ vận tốc cho hàm SmoothDamp (làm mượt chuyển động camera giữa các phòng).
    //Follow player: Camera duy trì một khoảng cách phía trước player khi di chuyển
    [SerializeField] private Transform player;//Tham chiếu đến đối tượng Player mà camera sẽ theo dõi
    [SerializeField] private float aheadDistance;//Khoảng cách phía trước người chơi mà camera sẽ duy trì(trục X).
    [SerializeField] private float cameraSpeed;//Tốc độ mà camera điều chỉnh vị trí khi theo dõi người chơi
    private float lookAhead;//Xác định khoảng cách mà camera sẽ nhìn trước người chơi dựa trên chuyển động của người chơi.
                            //Là giá trị được điều chỉnh liên tục theo thời gian để đạt được khoảng cách đó một cách mượt mà, tạo ra trải nghiệm camera chuyển động nhẹ nhàng hơn khi theo dõi người chơi.


    private void Update()
    {
        //Room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity,speed);

        //follow player: Dòng mã này di chuyển camera để theo dõi người chơi trên trục X, giữ nguyên vị trí Y và Z. Vị trí X của camera là vị trí của người chơi cộng với khoảng cách "look ahead".
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);

        //Dòng mã này điều chỉnh dần khoảng cách "look ahead" bằng cách sử dụng Mathf.Lerp.
        //Hàm này nội suy giữa giá trị hiện tại của lookAhead và giá trị mục tiêu (aheadDistance * player.localScale.x), tùy thuộc vào kích thước và hướng di chuyển của người chơi.
        //Điều này đảm bảo camera theo dõi người chơi một cách mượt mà mà không thay đổi vị trí đột ngột.
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);

    }

    //Hàm này được sử dụng để di chuyển camera đến phòng mới bằng cách cập nhật currentPosX với vị trí X của phòng mới.
    //Khi người chơi bước vào phòng mới, hàm này sẽ được gọi, cho phép camera chuyển tiếp một cách mượt mà đến phòng mới.
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
