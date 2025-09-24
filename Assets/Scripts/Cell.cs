using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;
    public int age;
    private int Amax;
    private SpriteRenderer sr;

    // Inicializa la celda
    public void Init(bool startAlive, int maxAge, Sprite defaultSprite = null)
    {
        // Intenta obtener el SpriteRenderer existente
        sr = GetComponent<SpriteRenderer>();

        // Si no existe, lo agrega y asigna el sprite por defecto
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
            if (defaultSprite != null)
            {
                sr.sprite = defaultSprite;
            }
        }

        alive = startAlive;
        age = startAlive ? 1 : 0;
        Amax = maxAge;

        UpdateColor();
    }

    // Actualiza el estado de la celda
    public void SetState(bool newAlive, int newAge)
    {
        alive = newAlive;
        age = newAlive ? Mathf.Max(newAge, 1) : 0;
        UpdateColor();
    }

   
    private void UpdateColor()
    {
        if (!alive)
        {
            sr.color = Color.black; // Celda muerta
        }
        else
        {
            float t = Mathf.Clamp01((float)age / Amax);
            sr.color = Color.Lerp(Color.green, Color.red, t); // Verde = joven, rojo = vieja
        }
    }
}


