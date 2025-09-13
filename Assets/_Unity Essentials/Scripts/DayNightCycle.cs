using UnityEngine;

/// <summary>
/// 日夜循环控制器 - 通过旋转定向光源模拟一天的时间变化
/// 将此脚本添加到定向光源游戏物体上
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Header("时间设置")]
    [Tooltip("一天的实际秒数")]
    public float dayDurationInSeconds = 120f; // 默认2分钟为一天

    [Header("光照设置")]
    [Tooltip("太阳在天空中的起始角度 (度)")]
    [Range(-90f, 90f)]
    public float sunStartAngle = -90f; // 日出时太阳的角度

    [Tooltip("太阳在天空中的结束角度 (度)")]
    [Range(-90f, 90f)]
    public float sunEndAngle = 90f; // 日落时太阳的角度

    [Header("光强度设置")]
    [Tooltip("夜晚时的光照强度")]
    [Range(0f, 2f)]
    public float nightIntensity = 0.1f;

    [Tooltip("白天时的光照强度")]
    [Range(0f, 3f)]
    public float dayIntensity = 1.2f;

    [Header("光颜色设置")]
    [Tooltip("日出/日落时的光照颜色")]
    public Color sunriseColor = new Color(1f, 0.6f, 0.3f, 1f); // 橙红色

    [Tooltip("正午时的光照颜色")]
    public Color noonColor = new Color(1f, 1f, 0.9f, 1f); // 白色偏暖

    [Tooltip("夜晚时的光照颜色")]
    public Color nightColor = new Color(0.3f, 0.4f, 0.8f, 1f); // 蓝色

    [Header("调试信息")]
    [Tooltip("显示当前时间信息")]
    public bool showDebugInfo = true;

    // 私有变量
    private Light directionalLight;
    private float currentTime = 0f; // 当前时间（0-1之间，0为午夜，0.5为正午）

    /// <summary>
    /// 当前时间（24小时制）
    /// </summary>
    public float CurrentTimeOfDay
    {
        get { return currentTime * 24f; }
    }

    /// <summary>
    /// 是否为白天
    /// </summary>
    public bool IsDayTime
    {
        get { return currentTime > 0.25f && currentTime < 0.75f; }
    }

    private void Start()
    {
        // 获取定向光源组件
        directionalLight = GetComponent<Light>();

        if (directionalLight == null)
        {
            Debug.LogError("DayNightCycle: 未找到Light组件！请将此脚本添加到定向光源上。");
            enabled = false;
            return;
        }

        if (directionalLight.type != LightType.Directional)
        {
            Debug.LogWarning("DayNightCycle: 建议使用定向光源类型以获得最佳效果。");
        }

        // 初始化时间（可以设置为特定时间，这里设置为日出）
        currentTime = 0.25f; // 早上6点
        UpdateSunPosition();
        UpdateLightProperties();
    }

    private void Update()
    {
        // 更新时间
        UpdateTime();

        // 更新太阳位置
        UpdateSunPosition();

        // 更新光照属性
        UpdateLightProperties();

        // 显示调试信息
        if (showDebugInfo)
        {
            ShowDebugInfo();
        }
    }

    /// <summary>
    /// 更新当前时间
    /// </summary>
    private void UpdateTime()
    {
        // 计算时间增量
        float timeIncrement = Time.deltaTime / dayDurationInSeconds;

        // 更新当前时间
        currentTime += timeIncrement;

        // 确保时间在0-1范围内循环
        if (currentTime >= 1f)
        {
            currentTime = 0f;
        }
    }

    /// <summary>
    /// 更新太阳位置（旋转定向光源）
    /// </summary>
    private void UpdateSunPosition()
    {
        // 计算太阳角度（基于当前时间）
        float sunAngle = Mathf.Lerp(sunStartAngle, sunEndAngle, currentTime);

        // 应用旋转（绕X轴旋转模拟太阳轨迹）
        transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
    }

    /// <summary>
    /// 更新光照属性（强度和颜色）
    /// </summary>
    private void UpdateLightProperties()
    {
        // 计算光照强度
        float intensity = CalculateLightIntensity();
        directionalLight.intensity = intensity;

        // 计算光照颜色
        Color lightColor = CalculateLightColor();
        directionalLight.color = lightColor;
    }

    /// <summary>
    /// 计算光照强度
    /// </summary>
    private float CalculateLightIntensity()
    {
        // 使用余弦曲线模拟自然的光照变化
        float normalizedTime = currentTime * 2f * Mathf.PI; // 转换为弧度
        float intensityCurve = Mathf.Cos(normalizedTime); // -1到1的余弦值

        // 将余弦值转换为0-1范围
        float normalizedIntensity = (intensityCurve + 1f) / 2f;

        // 应用最小和最大强度
        return Mathf.Lerp(nightIntensity, dayIntensity, normalizedIntensity);
    }

    /// <summary>
    /// 计算光照颜色
    /// </summary>
    private Color CalculateLightColor()
    {
        Color currentColor;

        if (currentTime < 0.25f) // 午夜到日出 (0:00 - 6:00)
        {
            float t = currentTime / 0.25f;
            currentColor = Color.Lerp(nightColor, sunriseColor, t);
        }
        else if (currentTime < 0.5f) // 日出到正午 (6:00 - 12:00)
        {
            float t = (currentTime - 0.25f) / 0.25f;
            currentColor = Color.Lerp(sunriseColor, noonColor, t);
        }
        else if (currentTime < 0.75f) // 正午到日落 (12:00 - 18:00)
        {
            float t = (currentTime - 0.5f) / 0.25f;
            currentColor = Color.Lerp(noonColor, sunriseColor, t);
        }
        else // 日落到午夜 (18:00 - 24:00)
        {
            float t = (currentTime - 0.75f) / 0.25f;
            currentColor = Color.Lerp(sunriseColor, nightColor, t);
        }

        return currentColor;
    }

    /// <summary>
    /// 显示调试信息
    /// </summary>
    private void ShowDebugInfo()
    {
        if (Application.isEditor)
        {
            float hours = CurrentTimeOfDay;
            int hour = Mathf.FloorToInt(hours);
            int minutes = Mathf.FloorToInt((hours - hour) * 60f);

            string timeString = string.Format("{0:D2}:{1:D2}", hour, minutes);
            string dayNight = IsDayTime ? "白天" : "夜晚";

            // 在Scene视图中显示信息（仅在编辑器中）
            Debug.Log($"当前时间: {timeString} ({dayNight}) - 光照强度: {directionalLight.intensity:F2}");
        }
    }

    /// <summary>
    /// 设置特定时间
    /// </summary>
    /// <param name="hour">小时 (0-23)</param>
    /// <param name="minute">分钟 (0-59)</param>
    public void SetTime(int hour, int minute)
    {
        hour = Mathf.Clamp(hour, 0, 23);
        minute = Mathf.Clamp(minute, 0, 59);

        currentTime = (hour + minute / 60f) / 24f;

        UpdateSunPosition();
        UpdateLightProperties();
    }

    /// <summary>
    /// 暂停/恢复时间流逝
    /// </summary>
    public void PauseTime()
    {
        enabled = !enabled;
    }

    /// <summary>
    /// 重置到特定时间点
    /// </summary>
    public void ResetToMorning()
    {
        SetTime(6, 0); // 早上6点
    }

    public void ResetToNoon()
    {
        SetTime(12, 0); // 正午12点
    }

    public void ResetToEvening()
    {
        SetTime(18, 0); // 傍晚6点
    }

    public void ResetToMidnight()
    {
        SetTime(0, 0); // 午夜12点
    }
}