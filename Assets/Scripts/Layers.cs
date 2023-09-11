public enum GameLayer {
    [LayerName("Default")]
    Default = 0,

    [LayerName("TransparentFX")]
    TransparentFX = 1,

    [LayerName("Ignore Raycast")]
    IgnoreRaycast = 2,

    [LayerName("Water")]
    Water = 4,

    [LayerName("UI")]
    UI = 5,
    
    [LayerName("Ground")]
    Ground = 6,

    [LayerName("Climbing")]
    Climbing = 7,
    
    [LayerName("Player")]
    Player = 8,

    [LayerName("World Confiner")]
    WorldConfiner = 9,

    [LayerName("Enemy")]
    Enemy = 10,

    [LayerName("Bomb")]
    Bomb = 11
}

public class LayerNameAttribute : System.Attribute {
    public string Name { get; private set; }

    public LayerNameAttribute(string name) {
        Name = name;
    }
}

public static class GameLayerExtensions {
    public static string GetName(this GameLayer layer) {
        var type = typeof(GameLayer);
        var memberInfo = type.GetMember(layer.ToString());
        if (memberInfo.Length > 0) {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(LayerNameAttribute), false);
            if (attributes.Length > 0) {
                return ((LayerNameAttribute)attributes[0]).Name;
            }
        }
        return layer.ToString();
    }
}