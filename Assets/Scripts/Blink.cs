using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{ 
    [SerializeField]  [Range(0f, 1f)] float lerpTime = 0.04f;
    [NonSerialized] public  float RefreshRate;
    [NonSerialized] public  Material objMaterial;

    public Color lightColor;
    public Color darkColor;
    [NonSerialized] Color _origColor;

    [NonSerialized] private Coroutine _coroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;
        // objMaterial = GetComponent<Material>();
        RefreshRate = 0.01f;
        _origColor = objMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRoutine()
    {
        if (objMaterial == null)
            return;

        _coroutine = StartCoroutine(Routine());
    }
    
    public IEnumerator Routine()
    {
        Color firstColor = lightColor;
        Color secondColor = darkColor;
        while (true)
        {
            float t = 0;
            while (t < 1)
            {
                Color newColor = Color.Lerp(firstColor, secondColor, t);
                t += lerpTime;
                objMaterial.color = newColor;
                yield return new WaitForSeconds(RefreshRate);
            }

            Color temp = firstColor;
            firstColor = secondColor;
            secondColor = temp;
            yield return null;
        }
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        objMaterial.color = _origColor;
    }
 
}
