using UnityEngine;

public enum EScreenEdge
{
    Top,
    Bottom,
    Left,
    Right,
    Random
}

public static class ScreenEdgeUtility
{
    private static Camera _mainCamera;
    private static bool _isInitialized;

    private static Vector2 _topLeft;
    private static Vector2 _topRight;
    private static Vector2 _bottomLeft;
    private static Vector2 _bottomRight;
    private static Vector2 _center;
    private static float _screenWidth;
    private static float _screenHeight;

    public static Vector2 TopLeft { get { EnsureInitialized(); return _topLeft; } }
    public static Vector2 TopRight { get { EnsureInitialized(); return _topRight; } }
    public static Vector2 BottomLeft { get { EnsureInitialized(); return _bottomLeft; } }
    public static Vector2 BottomRight { get { EnsureInitialized(); return _bottomRight; } }
    public static Vector2 Center { get { EnsureInitialized(); return _center; } }
    public static float ScreenWidth { get { EnsureInitialized(); return _screenWidth; } }
    public static float ScreenHeight { get { EnsureInitialized(); return _screenHeight; } }

    private static void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
    }

    private static void Initialize()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null) return;

        float nearClipPlane = _mainCamera.nearClipPlane;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        _topLeft = _mainCamera.ScreenToWorldPoint(new Vector3(0f, screenHeight, nearClipPlane));
        _topRight = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, nearClipPlane));
        _bottomLeft = _mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, nearClipPlane));
        _bottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, 0f, nearClipPlane));
        _center = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, screenHeight / 2f, nearClipPlane));
        _screenWidth = _topRight.x - _topLeft.x;
        _screenHeight = _topLeft.y - _bottomLeft.y;

        _isInitialized = true;
    }

    /// <summary>
    /// 랜덤 Edge에서 반대편 랜덤 위치로
    /// </summary>
    public static (Vector2 startPos, Vector2 endPos) GetRandomEdgePositions(float offset = 0.1f)
    {
        EnsureInitialized();

        EScreenEdge edge = (EScreenEdge)Random.Range(0, 4);
        return GetEdgeToOpposite(edge, offset);
    }

    /// <summary>
    /// 특정 Edge에서 반대편 랜덤 위치로
    /// </summary>
    public static (Vector2 startPos, Vector2 endPos) GetEdgeToOpposite(EScreenEdge edge, float offset = 0.1f)
    {
        EnsureInitialized();

        if (edge == EScreenEdge.Random)
        {
            edge = (EScreenEdge)Random.Range(0, 4);
        }

        return edge switch
        {
            EScreenEdge.Top => (
                new Vector2(Random.Range(_topLeft.x, _topRight.x), _topLeft.y + _screenHeight * offset),
                new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y - _screenHeight * offset)),
            EScreenEdge.Bottom => (
                new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y - _screenHeight * offset),
                new Vector2(Random.Range(_topLeft.x, _topRight.x), _topLeft.y + _screenHeight * offset)),
            EScreenEdge.Left => (
                new Vector2(_topLeft.x - _screenWidth * offset, Random.Range(_bottomLeft.y, _topLeft.y)),
                new Vector2(_topRight.x + _screenWidth * offset, Random.Range(_bottomRight.y, _topRight.y))),
            EScreenEdge.Right => (
                new Vector2(_topRight.x + _screenWidth * offset, Random.Range(_bottomRight.y, _topRight.y)),
                new Vector2(_topLeft.x - _screenWidth * offset, Random.Range(_bottomLeft.y, _topLeft.y))),
            _ => (Vector2.zero, Vector2.zero)
        };
    }

    /// <summary>
    /// 특정 Edge에서 중앙으로
    /// </summary>
    public static (Vector2 startPos, Vector2 endPos) GetEdgeToCenter(EScreenEdge edge, float offset = 0.1f)
    {
        EnsureInitialized();

        if (edge == EScreenEdge.Random)
        {
            edge = (EScreenEdge)Random.Range(0, 4);
        }

        Vector2 startPos = GetRandomPointOnEdge(edge, offset);
        return (startPos, _center);
    }

    /// <summary>
    /// 특정 Edge의 랜덤 위치 반환
    /// </summary>
    public static Vector2 GetRandomPointOnEdge(EScreenEdge edge, float offset = 0.1f)
    {
        EnsureInitialized();

        if (edge == EScreenEdge.Random)
        {
            edge = (EScreenEdge)Random.Range(0, 4);
        }

        return edge switch
        {
            EScreenEdge.Top => new Vector2(Random.Range(_topLeft.x, _topRight.x), _topLeft.y + _screenHeight * offset),
            EScreenEdge.Bottom => new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y - _screenHeight * offset),
            EScreenEdge.Left => new Vector2(_topLeft.x - _screenWidth * offset, Random.Range(_bottomLeft.y, _topLeft.y)),
            EScreenEdge.Right => new Vector2(_topRight.x + _screenWidth * offset, Random.Range(_bottomRight.y, _topRight.y)),
            _ => Vector2.zero
        };
    }
}
