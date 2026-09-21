using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float targetY = 0f;
    public float perfectThreshold = 0.3f;
    public float okeyThreshold = 0.7f;
    public float normalThreshold = 1.2f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            float distance = Mathf.Abs(transform.position.y - targetY);

            if (distance <= normalThreshold)
            {
                HitType hit = GetHitType(distance);
                
                Debug.Log($"Hit: {hit} | Abstand: {distance}");

                Destroy(gameObject);
            }
        }

        if (transform.position.y <= -10f)
        {
            Debug.Log("Hit: Missed (zu weit unten)");

            Destroy(gameObject);
        }
    }

    private HitType GetHitType(float distance)
    {
        if (distance <= perfectThreshold) return HitType.Perfect;
        if (distance <= okeyThreshold) return HitType.Okey;
        return HitType.Normal;
    }
}