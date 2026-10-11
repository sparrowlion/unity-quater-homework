using UnityEngine;

/// <summary>
/// 万向锁（Gimbal Lock）对比演示 —— 说明"为什么用四元数"。
/// 欧拉角用三个互相垂直的轴逐次旋转，当中间轴转到 ±90° 时，
/// 两个旋转轴会重合，导致失去一个自由度，表现为主轴"卡住"或突然反转。
///
/// 用法：挂到一个 3D 物体上，用键盘逐轴旋转观察现象。
///   Q = 绕 X 轴  W = 绕 Y 轴  E = 绕 Z 轴
/// 提示：把 X 转到 90° 左右，再去转 Y 或 Z，就能看到万向锁现象。
/// </summary>
public class EulerGimbalLockDemo : MonoBehaviour
{
    public float speed = 30f;   // 每帧旋转速度（度/秒）
    private Vector3 _euler;     // 累加的欧拉角

    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) _euler.x += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) _euler.y += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) _euler.z += speed * Time.deltaTime;

        // 用欧拉角构造旋转（这是会出万向锁的方式）
        transform.rotation = Quaternion.Euler(_euler);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 520, 20),
            "欧拉角模式（Euler）: Q=绕X轴  W=绕Y轴  E=绕Z轴");
        GUI.Label(new Rect(10, 32, 520, 20),
            "当前欧拉角 = " + _euler);
        GUI.Label(new Rect(10, 56, 540, 60),
            "把 X 转到 90° 附近，再转 Y 或 Z，会看到物体轴'卡住'或突然反转 —— \n这就是万向锁(Gimbal Lock)。四元数(Slerp)旋转不会出现这个问题。");
    }
}
//（注：内容由AI生成）
