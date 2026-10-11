using UnityEngine;

/// <summary>
/// 四元数旋转变换演示（课程：04 四元数编程）
/// 演示内容：
///   1. 轴-角旋转：Quaternion.AngleAxis(角度, 轴) 构造四元数
///   2. Slerp 球面插值：从当前朝向平滑旋转到目标朝向
///   3. 实时显示四元数 (x,y,z,w)、旋转轴、旋转角、对应欧拉角
/// 用法：挂到任意 3D 物体上，按 [空格] 触发 Slerp 动画。
/// </summary>
public class QuaternionRotationDemo : MonoBehaviour
{
    [Header("目标旋转（轴-角式输入）")]
    public Vector3 rotationAxis = new Vector3(0, 1, 0);   // 旋转轴
    [Range(0f, 360f)] public float rotationAngle = 90f;   // 旋转角度（度）

    [Header("插值动画")]
    public bool useSlerp = true;      // true=四元数球面插值；false=线性插值
    public float slerpSpeed = 1.5f;   // 插值速度（越大越快）

    [Header("自动旋转演示")]
    public bool autoRotate = false;   // 勾选后持续绕轴旋转
    public float autoSpeed = 30f;     // 每秒旋转角度

    private Quaternion _startRotation;  // 起始朝向
    private Quaternion _targetRotation; // 目标朝向
    private float _t = 0f;              // 插值进度 0~1
    private bool _isAnimating = false;
    private float _autoAngle = 0f;

    void Start()
    {
        // 记录当前朝向作为插值起点
        _startRotation = transform.rotation;
        // 核心 API：用 轴 + 角度 构造一个四元数
        _targetRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis.normalized);

        Debug.Log("起始四元数: " + _startRotation.ToString("F4"));
        Debug.Log("目标四元数: " + _targetRotation.ToString("F4"));
    }

    void Update()
    {
        // 模式一：自动绕轴旋转
        if (autoRotate)
        {
            _autoAngle += autoSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(_autoAngle, rotationAxis.normalized);
            return;
        }

        // 按空格：从当前朝向插值到目标朝向
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _t = 0f;
            _startRotation = transform.rotation;
            _targetRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis.normalized);
            _isAnimating = true;
        }

        if (_isAnimating)
        {
            _t += Time.deltaTime * slerpSpeed;
            _t = Mathf.Clamp01(_t);

            if (useSlerp)
                transform.rotation = Quaternion.Slerp(_startRotation, _targetRotation, _t);
            else
                transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, _t);

            if (_t >= 1f) _isAnimating = false;
        }
    }

    // 选中物体时，在场景里用黄线画出旋转轴，方便直观看到
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 axis = rotationAxis.normalized;
        Gizmos.DrawLine(transform.position - axis * 2f, transform.position + axis * 2f);
    }

    // 屏幕左上角显示实时数据（IMGUI，无需额外配置）
    void OnGUI()
    {
        Quaternion q = transform.rotation;
        Vector3 euler = q.eulerAngles;                 // 四元数 -> 欧拉角
        q.ToAngleAxis(out float ang, out Vector3 ax);  // 四元数 -> 轴 + 角

        GUI.Box(new Rect(10, 10, 400, 200), "四元数旋转演示 Quaternion Rotation Demo");
        GUI.Label(new Rect(20, 38, 380, 20),
            "Quaternion (x,y,z,w) = (" + F(q.x) + ", " + F(q.y) + ", " + F(q.z) + ", " + F(q.w) + ")");
        GUI.Label(new Rect(20, 62, 380, 20),
            "旋转轴 Axis        = (" + F(ax.x) + ", " + F(ax.y) + ", " + F(ax.z) + ")");
        GUI.Label(new Rect(20, 86, 380, 20),
            "旋转角 Angle       = " + F(ang) + "°");
        GUI.Label(new Rect(20, 110, 380, 20),
            "欧拉角 Euler       = (" + F(euler.x) + ", " + F(euler.y) + ", " + F(euler.z) + ")°");
        GUI.Label(new Rect(20, 134, 380, 20),
            "插值方式           = " + (useSlerp ? "Slerp（球面插值）" : "Lerp（线性插值）"));
        GUI.Label(new Rect(20, 158, 380, 20),
            "操作               = 按 [空格] 从当前朝向 Slerp 到目标朝向");
    }

    // 保留 3 位小数，方便阅读
    string F(float v) => v.ToString("0.000");
}
//（注：内容由AI生成）
