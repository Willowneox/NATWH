using UnityEngine;


// Helper Functions for controlling rates of growth
public static class GrowthFunc
{
    // variable to slow rate of change of fibonacci
    const float k = 0.5f;
    public static int Fibonacci(int n)
    {
        if (n <= 2) return 1;

        float prev2 = 1f;
        float prev1 = 1f;
        float current = 0f;

        for (int i = 3; i <= n; i++)
        {
            current = prev1 + k * prev2;

            prev2 = prev1;
            prev1 = current;
        }

        return (int)Mathf.Floor(current);
    }
}
