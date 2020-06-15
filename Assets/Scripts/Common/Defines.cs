using UnityEngine;

public delegate void DefaultHandler();
public delegate void StringHandler(string strValue);
public delegate void BoolHandler(bool bValue);
public delegate void IntHandler(int nValue);
public delegate void FloatHandler(float fValue);
public delegate void Vector2Handler(Vector2 vec2Value);
public delegate void Vector3Handler(Vector3 vec3Value);
public delegate void CollisionHandler(Collider collider, Collision collision);
public delegate void ColliderHandler(Collider collider1, Collider collider2);

namespace Define
{
    namespace ResourcePath
    {
        public class HealthBar
        {
            public const string PLAYER = "UI/PlayerHealthBar";
        }

        public class UI
        {
            public const string FLOATING_ITEM = "UI/FloatingItem";
            public const string FLOATING_GET_MONEY = "UI/FloatingGetMoney";
            public const string EMOTION_EXPRESSION_VIEWER = "UI/EmotionExpressionViewer";
        }
    }

    public class PlayFab
    {
        public const string PrimaryCatalogName = "1.0";

        public class CurrencyCode
        {
            public const string Gold = "GD";
            public const string Diamond = "DM";
        }

        public class StoreID
        {
            public const string LobbyStoreID = "LobbyStore";
        }
    }
}
