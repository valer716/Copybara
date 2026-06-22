using System.Collections;
using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameplayPlayModeTests
{

    // teszteli a coinfelszedést, nézi, hogy a coins változó tényleg növekszik-e
    [UnityTest]
    public IEnumerator CoinTrigger_IncrementsGameManagerCoins()
    {
        var gameManagerObject = new GameObject("GameManager");
        var gameManagerType = FindTypeByName("GameManager");
        var gameManager = gameManagerObject.AddComponent(gameManagerType);

        var coinObject = new GameObject("Coin");
        var coinType = FindTypeByName("Coin");
        var coin = coinObject.AddComponent(coinType);

        var triggerSource = new GameObject("TriggerSource");
        var triggerCollider = triggerSource.AddComponent<BoxCollider2D>();

        yield return null;

        coin.SendMessage("OnTriggerEnter2D", triggerCollider);
        yield return null;

        Assert.AreEqual(1, GetPublicFieldValue<int>(gameManager, "coins"));

        UnityEngine.Object.Destroy(gameManagerObject);
        UnityEngine.Object.Destroy(triggerSource);
    }

    [UnityTest]
    //csúzli felszedést teszteli, figyeli a holdingSlingshot változót, meg a játékos sprite-ját, hogy megváltozik-e 
    public IEnumerator SlingshotPickup_ArmsPlayerAndSetsHoldingFlag()
    {
        var playerObject = new GameObject("Player");
        var spriteRenderer = playerObject.AddComponent<SpriteRenderer>();
        playerObject.AddComponent<Rigidbody2D>();
        var playerType = FindTypeByName("Player");
        var player = playerObject.AddComponent(playerType);

        var texture = new Texture2D(4, 4);
        var armedSprite = Sprite.Create(texture, new Rect(0f, 0f, 4f, 4f), new Vector2(0.5f, 0.5f));
        SetPrivateField(player, "armedCapybara", armedSprite);

        var slingshotObject = new GameObject("Slingshot");
        var slingshotType = FindTypeByName("Slingshot");
        var slingshot = slingshotObject.AddComponent(slingshotType);

        var triggerSource = new GameObject("TriggerSource");
        var triggerCollider = triggerSource.AddComponent<BoxCollider2D>();

        yield return null;

        slingshot.SendMessage("OnTriggerEnter2D", triggerCollider);
        yield return null;

        var holdingSlingshot = (bool)GetPrivateField(player, "holdingSlingshot");

        Assert.IsTrue(holdingSlingshot);
        Assert.AreEqual(armedSprite, spriteRenderer.sprite);

        UnityEngine.Object.Destroy(playerObject);
        UnityEngine.Object.Destroy(triggerSource);
        UnityEngine.Object.Destroy(texture);
    }

    [UnityTest]
    //azt teszteli hogy a bodylslam-kor keletkezett kör el e pusztítja magát megfelelő időn belül
    public IEnumerator SlamCircle_DestroysItselfAfterTimer()
    {
        var slamCircleObject = new GameObject("SlamCircle");
        var slamCircleType = FindTypeByName("SlamCircle");
        var slamCircle = slamCircleObject.AddComponent(slamCircleType);
        SetPublicFieldValue(slamCircle, "timer", 0.05f);

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(slamCircleObject == null);
    }

    private static void SetPrivateField(object target, string fieldName, object value)
    {
        var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(fieldInfo, $"Field '{fieldName}' was not found.");
        fieldInfo.SetValue(target, value);
    }

    private static void SetPublicFieldValue(object target, string fieldName, object value)
    {
        var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        Assert.IsNotNull(fieldInfo, $"Field '{fieldName}' was not found.");
        fieldInfo.SetValue(target, value);
    }

    private static T GetPublicFieldValue<T>(object target, string fieldName)
    {
        var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
        Assert.IsNotNull(fieldInfo, $"Field '{fieldName}' was not found.");
        return (T)fieldInfo.GetValue(target);
    }

    private static object GetPrivateField(object target, string fieldName)
    {
        var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(fieldInfo, $"Field '{fieldName}' was not found.");
        return fieldInfo.GetValue(target);
    }

    private static Type FindTypeByName(string typeName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(typeName) ?? assembly.GetType($"{typeName}");
            if (type != null)
            {
                return type;
            }
        }

        Assert.Fail($"Type '{typeName}' was not found in loaded assemblies.");
        return null;
    }
}
