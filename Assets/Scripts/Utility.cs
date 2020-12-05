using UnityEngine;


public class Utility
{

    private static float minX = -14f;
    private static float maxX = 15f;
    private static float minY = -8f;
    private static float maxY = 6f;


    public static Vector2 RandomVector2D()
    {
        float randX = Random.Range(minX, maxX);
        float randY = Random.Range(minY, maxY);
        return new Vector2(randX, randY);
    }


    public static bool IsWithinBoundary(Vector2 pos)
    {
        if (pos.x > maxX || pos.x < minX ||
            pos.y > maxY || pos.y < minY)
            return false;

        else return true;

    }

    


}